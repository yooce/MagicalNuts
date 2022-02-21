using MagicalNuts.Primitive;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MagicalNuts.BackTest
{
	/// <summary>
	/// バックテストのコントローラーを表します。
	/// </summary>
	public class Controller
	{
		/// <summary>
		/// バックテストの状態
		/// </summary>
		private BackTestStatus BackTestState = null;

		/// <summary>
		/// バックテスト用ロウソク足の集合の辞書
		/// </summary>
		private Dictionary<string, BackTestCandleCollection> StockCandlesDictionary = null;

		/// <summary>
		/// 手数料計算機
		/// </summary>
		private IFeeCalculator FeeCalculator = null;

		/// <summary>
		/// 非同期でバックテストをします。
		/// </summary>
		/// <typeparam name="T">戻り値の型</typeparam>
		/// <param name="args">バックテストの引数セット</param>
		/// <returns>バックテスト結果</returns>
		public async Task<T> BackTestAsync<T>(Arguments args) where T : BackTestResult, new()
		{
			return await Task.Run(() => BackTest<T>(args));
		}

		/// <summary>
		/// バックテストをします。
		/// </summary>
		/// <param name="args">バックテストの引数セット</param>
		/// <returns>バックテスト結果</returns>
		protected virtual T BackTest<T>(Arguments args) where T : BackTestResult, new()
		{
			StrategyProperties properties = (StrategyProperties)args.Strategy.Properties;

			// メンバー作成
			BackTestState = new BackTestStatus(properties.InitialAssets, args.CurrencyStore, args.CurrencyDigits, properties.Leverage);
			StockCandlesDictionary = args.StockCandles.ToDictionary(candles => candles.Stock.Code);
			FeeCalculator = args.FeeCalculator;

			// バックテスト
			DateTime? prev_dt = null;
			BackTestState.DateTime = args.BeginDateTime;
			while (BackTestState.DateTime <= args.EndDateTime)
			{
				// 銘柄更新
				BackTestState.StockCandles = GetDaysStockCandles(args.StockCandles, BackTestState.DateTime);

				if (BackTestState.StockCandles.Length > 0)
				{
					// 注文処理
					HandleOrders();

					// シグナル取得
					args.Strategy.GetOrders(BackTestState, BackTestState.Orders);
				}

				// 月次手数料
				if (prev_dt != null && BackTestState.DateTime.Month != prev_dt.Value.Month) PayFee(GetMonthlyFee());

				// 年次手数料
				if (prev_dt != null && BackTestState.DateTime.Year != prev_dt.Value.Year) PayFee(GetYearlyFee());

				// 時価資産計算
				BackTestState.MarketAssets = CalculateMarketAssets();

				// 純資産スタンプ
				BackTestState.HistoricalAssetsList.Add(new HistoricalAssets(BackTestState.DateTime, BackTestState.BookAssets
					, BackTestState.MarketAssets, BackTestState.InvestmentAmount));

				Debug.WriteLine(BackTestState.DateTime.ToString());
				prev_dt = BackTestState.DateTime;

				// 次の時間へ
				BackTestState.DateTime = BackTestCandleCollection.GetNextCandleDateTime(prev_dt.Value, args.PeriodInfo);
			}

			// 清算
			Settlement(args.EndDateTime);

			// 結果計算
			T result = GetBackTestResult<T>();

			return result;
		}

		/// <summary>
		/// 指定日時の銘柄ごとの戦略用ロウソク足の集合を取得します。
		/// </summary>
		/// <param name="bt_stock_candles">銘柄ごとのバックテスト用ロウソク足の集合</param>
		/// <param name="dt">日時</param>
		/// <returns>指定日時の銘柄ごとの戦略用ロウソク足の集合</returns>
		private StrategyCandleCollection[] GetDaysStockCandles(BackTestCandleCollection[] bt_stock_candles, DateTime dt)
		{
			List<StrategyCandleCollection> st_stock_candles = new List<StrategyCandleCollection>();
			foreach (BackTestCandleCollection bt_candles in bt_stock_candles)
			{
				// ロウソク足取得
				StrategyCandleCollection st_candles = bt_candles.GetShiftedCandles(dt);

				// ロウソク足が無ければスキップ
				if (st_candles == null) continue;

				// 追加
				st_stock_candles.Add(st_candles);
			}
			return st_stock_candles.ToArray();
		}

		/// <summary>
		/// ポジションをエントリーします。
		/// </summary>
		/// <param name="position">ポジション</param>
		private void EntryPosition(Position position)
		{
			// ここではMarketAssetsの更新はしない

			BackTestState.BookAssets -= position.EntryExecution.Fee;
			BackTestState.NetBalance -= position.EntryExecution.Amount / BackTestState.Leverage + position.EntryExecution.Fee;
			BackTestState.ActivePositions.Add(position);
		}

		/// <summary>
		/// ポジションをイグジットします。
		/// </summary>
		/// <param name="position">ポジション</param>
		private void ExitPosition(Position position)
		{
			// ここではMarketAssetsの更新はしない

			BackTestState.BookAssets += position.Return.Value + position.EntryExecution.Fee; // エントリー時の手数料を足し戻す
			BackTestState.NetBalance += position.ExitExecution.Amount / BackTestState.Leverage - position.ExitExecution.Fee;
			BackTestState.ActivePositions.Remove(position);
			BackTestState.HistoricalPositions.Add(position);
		}

		/// <summary>
		/// 手数料を支払う。
		/// </summary>
		/// <param name="fee">手数料</param>
		private void PayFee(decimal fee)
		{
			// ここではMarketAssetsの更新はしない

			BackTestState.BookAssets -= fee;
			BackTestState.NetBalance -= fee;
		}

		/// <summary>
		/// 決済します。
		/// </summary>
		private void Settlement(DateTime end)
		{
			foreach (Position position in BackTestState.ActiveLongPositions)
			{
				// 銘柄ごとのロウソク足の集合取得
				BackTestCandleCollection candles = StockCandlesDictionary[position.Stock.Code];

				// 価格の決定
				decimal price = candles.GetLatestCandle(end).Close;

				// 為替
				decimal currency = BackTestState.CurrencyStore.GetPrice(position.Stock.MarketType, BackTestState.DateTime);

				// 手仕舞い
				position.Exit(
					end,
					price,
					GetExitExecutionAmount(price, position.Lots, currency),
					GetExitFee(price, position.Lots, currency),
					(end - position.EntryExecution.DateTime).Days);
				ExitPosition(position);
			}

			// 時価資産計算
			BackTestState.MarketAssets = CalculateMarketAssets();
		}

		/// <summary>
		/// 時価資産を取得します。
		/// </summary>
		/// <returns>時価資産</returns>
		private decimal CalculateMarketAssets()
		{
			// 簿価資産
			decimal ma = BackTestState.BookAssets;

			// 保有中のポジション
			foreach (Position position in BackTestState.ActivePositions)
			{
				// 為替
				decimal currency = BackTestState.CurrencyStore.GetPrice(position.Stock.MarketType, BackTestState.DateTime);

				// 銘柄ごとのバックテスト用ロウソク足の集合取得
				BackTestCandleCollection candles = StockCandlesDictionary[position.Stock.Code];

				// 直近の終値取得
				decimal price = candles.GetLatestCandle(BackTestState.DateTime).Close;

				// 損益を加減算
				switch (position.PositionDirection)
				{
					case PositionDirection.Long:
						ma += GetExitExecutionAmount(price, position.Lots, currency) - position.EntryExecution.Amount;
						break;
					case PositionDirection.Short:
						ma += position.EntryExecution.Amount - GetExitExecutionAmount(price, position.Lots, currency);
						break;
				}
			}

			return ma;
		}

		#region 金額処理

		/// <summary>
		/// エントリー時の手数料を取得します。
		/// </summary>
		/// <param name="price">価格</param>
		/// <param name="lots">ロット数</param>
		/// <param name="currency">為替</param>
		/// <returns>エントリー時の手数料</returns>
		private decimal GetEntryFee(decimal price, decimal lots, decimal currency)
		{
			// 切り上げ
			return MathEx.Ceiling(FeeCalculator.GetEntryFee(BackTestState, price, lots, currency), BackTestState.CurrencyDigits);
		}

		/// <summary>
		/// イグジット時の手数料を取得します。
		/// </summary>
		/// <param name="price">価格</param>
		/// <param name="lots">ロット数</param>
		/// <param name="currency">為替</param>
		/// <returns>イグジット時の手数料</returns>
		private decimal GetExitFee(decimal price, decimal lots, decimal currency)
		{
			// 切り上げ
			return MathEx.Ceiling(FeeCalculator.GetExitFee(BackTestState, price, lots, currency), BackTestState.CurrencyDigits);
		}

		/// <summary>
		/// 月次手数料を取得します。
		/// </summary>
		/// <returns>月次手数料</returns>
		private decimal GetMonthlyFee()
		{
			// 切り上げ
			return MathEx.Ceiling(FeeCalculator.GetMonthlyFee(BackTestState), BackTestState.CurrencyDigits);
		}

		/// <summary>
		/// 年次手数料を取得します。
		/// </summary>
		/// <returns>年次手数料</returns>
		private decimal GetYearlyFee()
		{
			// 切り上げ
			return MathEx.Ceiling(FeeCalculator.GetYearlyFee(BackTestState), BackTestState.CurrencyDigits);
		}

		/// <summary>
		/// エントリー時の約定金額を取得します。
		/// </summary>
		/// <param name="price">価格</param>
		/// <param name="lots">ロット数</param>
		/// <param name="currency">為替</param>
		/// <returns>エントリー時の約定金額</returns>
		private decimal GetEntryExecutionAmount(decimal price, decimal lots, decimal currency)
		{
			// 切り上げ
			return MathEx.Ceiling(price * lots * currency, BackTestState.CurrencyDigits);
		}

		/// <summary>
		/// イグジット時の約定金額を取得します。
		/// </summary>
		/// <param name="price">価格</param>
		/// <param name="lots">ロット数</param>
		/// <param name="currency">為替</param>
		/// <returns>イグジット時の約定金額</returns>
		private decimal GetExitExecutionAmount(decimal price, decimal lots, decimal currency)
		{
			// 切り下げ
			return MathEx.Floor(price * lots * currency, BackTestState.CurrencyDigits);
		}

		#endregion

		#region 注文処理

		/// <summary>
		/// 注文を処理します。
		/// </summary>
		private void HandleOrders()
		{
			// キャンセル分と不正注文（ロット数０以下またはイグジット済み）を削除
			for (int i = BackTestState.Orders.Count - 1; i >= 0; i--)
			{
				if (BackTestState.Orders[i].Cancel || BackTestState.Orders[i].Lots <= 0
					|| (BackTestState.Orders[i].PositionToClose != null && BackTestState.Orders[i].PositionToClose.IsExited))
					BackTestState.Orders.RemoveAt(i);
			}

			List<Order> orders = null;

			// 成行売り
			orders = BackTestState.Orders.Where(order => order.OrderType == OrderType.SellMarket).ToList();
			foreach (Order order in orders)
			{
				SellMarket(order);

				// 約定に関係なく削除
				BackTestState.Orders.Remove(order);
			}

			// 成行買い
			orders = BackTestState.Orders.Where(order => order.OrderType == OrderType.BuyMarket).ToList();
			foreach (Order order in orders)
			{
				BuyMarket(order);

				// 約定に関係なく削除
				BackTestState.Orders.Remove(order);
			}

			// 後は順番通り
			orders = BackTestState.Orders.Where(
				order => order.OrderType != OrderType.BuyMarket && order.OrderType != OrderType.SellMarket).ToList();
			foreach (Order order in orders)
			{
				bool ret = false;
				switch (order.OrderType)
				{
					case OrderType.BuyLimit:
						ret = BuyLimit(order);
						break;
					case OrderType.SellLimit:
						ret = SellLimit(order);
						break;
					case OrderType.BuyStop:
						ret = BuyStop(order);
						break;
					case OrderType.SellStop:
						ret = SellStop(order);
						break;
				}

				// 約定したら削除
				if (ret) BackTestState.Orders.Remove(order);
			}
		}

		/// <summary>
		/// 調整済みロット数を取得します。
		/// </summary>
		/// <param name="price">価格</param>
		/// <param name="lots">ロット数</param>
		/// <param name="currency">為替</param>
		/// <param name="unit">単元数</param>
		/// <returns>調整済みロット数</returns>
		private decimal GetAdjustedLots(decimal price, decimal lots, decimal currency, decimal unit)
		{
			while (lots > 0)
			{
				// 清算金額
				decimal clear = GetEntryExecutionAmount(price, lots, currency) + GetEntryFee(price, lots, currency);

				// 資金充足
				if (BackTestState.NetBalance * BackTestState.Leverage >= clear) return lots;

				// 資金不足
				lots -= unit;
			}

			// 買えない
			return 0;
		}

		/// <summary>
		/// 成行買いします。
		/// </summary>
		/// <param name="order">注文</param>
		/// <returns>約定したかどうか</returns>
		private bool BuyMarket(Order order)
		{
			// ロウソク足の集合取得
			StrategyCandleCollection candles = StockCandlesDictionary[order.Stock.Code].GetShiftedCandles(BackTestState.DateTime);
			if (candles == null || candles.Count == 0) return false;

			// 為替
			decimal currency = BackTestState.CurrencyStore.GetPrice(order.Stock.MarketType, BackTestState.DateTime);

			// 価格の決定
			decimal price = candles[0].Open;

			if (order.PositionToClose == null)
			{
				// ロット数の調整
				decimal lots = GetAdjustedLots(price, order.Lots.Value, currency, order.Stock.Unit);
				if (lots == 0) return false;

				// 新規買い
				Position position = new Position(
					order.Stock,
					PositionDirection.Long,
					lots,
					order.Additional,
					BackTestState.DateTime,
					price,
					GetEntryExecutionAmount(price, lots, currency),
					GetEntryFee(price, lots, currency));
				EntryPosition(position);
			}
			else
			{
				// 返済買い
				order.PositionToClose.Exit(
					BackTestState.DateTime,
					price,
					GetExitExecutionAmount(price, order.PositionToClose.Lots, currency),
					GetExitFee(price, order.PositionToClose.Lots, currency),
					(BackTestState.DateTime - order.PositionToClose.EntryDateTime.Value).Days);
				ExitPosition(order.PositionToClose);
			}

			return true;
		}

		/// <summary>
		/// 成行売りします。
		/// </summary>
		/// <param name="order">注文</param>
		/// <returns>約定したかどうか</returns>
		private bool SellMarket(Order order)
		{
			// ロウソク足の集合取得
			StrategyCandleCollection candles = StockCandlesDictionary[order.Stock.Code].GetShiftedCandles(BackTestState.DateTime);
			if (candles == null || candles.Count == 0) return false;

			// 価格の決定
			decimal price = candles[0].Open;

			// 為替
			decimal currency = BackTestState.CurrencyStore.GetPrice(order.Stock.MarketType, BackTestState.DateTime);

			if (order.PositionToClose == null)
			{
				// ロット数の調整
				decimal lots = GetAdjustedLots(price, order.Lots.Value, currency, order.Stock.Unit);
				if (lots == 0) return false;

				// 新規売り
				Position position = new Position(
					order.Stock,
					PositionDirection.Short,
					lots,
					order.Additional,
					BackTestState.DateTime,
					price,
					GetEntryExecutionAmount(price, lots, currency),
					GetEntryFee(price, lots, currency));
				EntryPosition(position);
			}
			else
			{
				// 手仕舞い売り
				order.PositionToClose.Exit(
					BackTestState.DateTime,
					price,
					GetExitExecutionAmount(price, order.PositionToClose.Lots, currency),
					GetExitFee(price, order.PositionToClose.Lots, currency),
					(BackTestState.DateTime - order.PositionToClose.EntryExecution.DateTime).Days);
				ExitPosition(order.PositionToClose);
			}

			return true;
		}

		/// <summary>
		/// 指値買いします。
		/// </summary>
		/// <param name="order">注文</param>
		/// <returns>約定したかどうか</returns>
		private bool BuyLimit(Order order)
		{
			// ロウソク足の集合取得
			StrategyCandleCollection candles = StockCandlesDictionary[order.Stock.Code].GetShiftedCandles(BackTestState.DateTime);
			if (candles == null || candles.Count == 0) return false;

			// 価格の決定
			decimal? price = null;
			if (candles[0].Open <= order.Price.Value) price = candles[0].Open;
			else if (candles[0].Low <= order.Price.Value) price = order.Price;
			if (price == null) return false;

			// 為替
			decimal currency = BackTestState.CurrencyStore.GetPrice(order.Stock.MarketType, BackTestState.DateTime);

			if (order.PositionToClose == null)
			{
				// ロット数の調整
				decimal lots = GetAdjustedLots(price.Value, order.Lots.Value, currency, order.Stock.Unit);
				if (lots == 0) return false;

				// 新規買い
				Position position = new Position(
					order.Stock,
					PositionDirection.Long,
					lots,
					order.Additional,
					BackTestState.DateTime,
					price.Value,
					GetEntryExecutionAmount(price.Value, lots, currency),
					GetEntryFee(price.Value, lots, currency));
				EntryPosition(position);
			}
			else
			{
				// 返済買い
				order.PositionToClose.Exit(
					BackTestState.DateTime,
					price.Value,
					GetExitExecutionAmount(price.Value, order.PositionToClose.Lots, currency),
					GetExitFee(price.Value, order.PositionToClose.Lots, currency),
					(BackTestState.DateTime - order.PositionToClose.EntryDateTime.Value).Days);
				ExitPosition(order.PositionToClose);
			}

			return true;
		}

		/// <summary>
		/// 指値売りします。
		/// </summary>
		/// <param name="order">注文</param>
		/// <returns>約定したかどうか</returns>
		private bool SellLimit(Order order)
		{
			// ロウソク足取得
			StrategyCandleCollection candles = StockCandlesDictionary[order.Stock.Code].GetShiftedCandles(BackTestState.DateTime);
			if (candles == null || candles.Count == 0) return false;

			// 価格の決定
			decimal? price = null;
			if (candles[0].Open >= order.Price.Value) price = candles[0].Open;
			else if (candles[0].High >= order.Price.Value) price = order.Price;
			if (price == null) return false;

			// 為替
			decimal currency = BackTestState.CurrencyStore.GetPrice(order.Stock.MarketType, BackTestState.DateTime);

			if (order.PositionToClose == null)
			{
				// ロット数の調整
				decimal lots = GetAdjustedLots(order.Price.Value, order.Lots.Value, currency, order.Stock.Unit);
				if (lots == 0) return false;

				// 新規売り
				Position position = new Position(
					order.Stock,
					PositionDirection.Short,
					lots,
					order.Additional,
					BackTestState.DateTime,
					price.Value,
					GetEntryExecutionAmount(price.Value, lots, currency),
					GetEntryFee(price.Value, lots, currency));
				EntryPosition(position);
			}
			else
			{
				// 手仕舞い売り
				order.PositionToClose.Exit(
					BackTestState.DateTime,
					price.Value,
					GetExitExecutionAmount(price.Value, order.PositionToClose.Lots, currency),
					GetExitFee(price.Value, order.PositionToClose.Lots, currency),
					(BackTestState.DateTime - order.PositionToClose.EntryDateTime.Value).Days);
				ExitPosition(order.PositionToClose);
			}

			return true;
		}

		/// <summary>
		/// 逆指値買いします。
		/// </summary>
		/// <param name="order">注文</param>
		/// <returns>約定したかどうか</returns>
		private bool BuyStop(Order order)
		{
			// ロウソク足の集合取得
			StrategyCandleCollection candles = StockCandlesDictionary[order.Stock.Code].GetShiftedCandles(BackTestState.DateTime);
			if (candles == null || candles.Count == 0) return false;

			// 価格の決定
			decimal? price = null;
			if (candles[0].Open >= order.Price.Value) price = candles[0].Open;
			else if (candles[0].High >= order.Price.Value) price = order.Price.Value;
			if (price == null) return false;

			// 為替
			decimal currency = BackTestState.CurrencyStore.GetPrice(order.Stock.MarketType, BackTestState.DateTime);

			if (order.PositionToClose == null)
			{
				// ロット数の調整
				decimal lots = GetAdjustedLots(order.Price.Value, order.Lots.Value, currency, order.Stock.Unit);
				if (lots == 0) return false;

				// 新規買い
				Position position = new Position(
					order.Stock,
					PositionDirection.Long,
					lots,
					order.Additional,
					BackTestState.DateTime,
					price.Value,
					GetEntryExecutionAmount(price.Value, lots, currency),
					GetEntryFee(price.Value, lots, currency));
				EntryPosition(position);
			}
			else
			{
				// 返済買い
				order.PositionToClose.Exit(
					BackTestState.DateTime,
					price.Value,
					GetExitExecutionAmount(price.Value, order.PositionToClose.Lots, currency),
					GetExitFee(price.Value, order.PositionToClose.Lots, currency),
					(BackTestState.DateTime - order.PositionToClose.EntryExecution.DateTime).Days);
				ExitPosition(order.PositionToClose);
			}

			return true;
		}

		/// <summary>
		/// 逆指値売りします。
		/// </summary>
		/// <param name="order">注文</param>
		/// <returns>約定したかどうか</returns>
		private bool SellStop(Order order)
		{
			// ロウソク足の集合取得
			StrategyCandleCollection candles = StockCandlesDictionary[order.Stock.Code].GetShiftedCandles(BackTestState.DateTime);
			if (candles == null || candles.Count == 0) return false;

			// 価格の決定
			decimal? price = null;
			if (candles[0].Open <= order.Price.Value) price = candles[0].Open;
			else if (candles[0].Low < order.Price.Value) price = order.Price.Value;
			if (price == null) return false;

			// 為替
			decimal currency = BackTestState.CurrencyStore.GetPrice(order.Stock.MarketType, BackTestState.DateTime);

			if (order.PositionToClose == null)
			{
				// ロット数の調整
				decimal lots = GetAdjustedLots(order.Price.Value, order.Lots.Value, currency, order.Stock.Unit);
				if (lots == 0) return false;

				// 新規売り
				Position position = new Position(
					order.Stock,
					PositionDirection.Short,
					lots,
					order.Additional,
					BackTestState.DateTime,
					price.Value,
					GetEntryExecutionAmount(price.Value, lots, currency),
					GetEntryFee(price.Value, lots,currency));
				EntryPosition(position);
			}
			else
			{
				// 手仕舞い売り
				order.PositionToClose.Exit(
					BackTestState.DateTime,
					price.Value,
					GetExitExecutionAmount(price.Value, order.PositionToClose.Lots, currency),
					GetExitFee(price.Value, order.PositionToClose.Lots, currency),
					(BackTestState.DateTime - order.PositionToClose.EntryExecution.DateTime).Days);
				ExitPosition(order.PositionToClose);
			}

			return true;
		}

		#endregion

		#region バックテスト結果

		/// <summary>
		/// バックテスト結果を取得します。
		/// </summary>
		/// <typeparam name="T">戻り値の型</typeparam>
		/// <returns>バックテスト結果</returns>
		protected virtual T GetBackTestResult<T>() where T : BackTestResult, new()
		{
			// メンバー作成
			T result = new T();
			result.Positions = BackTestState.HistoricalPositions.ToArray();
			result.HistoricalAssetsArray = BackTestState.HistoricalAssetsList.ToArray();

			// 初期資産
			result.InitialAssets = BackTestState.InitialAssets;

			// 追加投資額
			result.AdditionalInvestment = BackTestState.InvestmentAmount - BackTestState.InitialAssets;

			// 勝ちポジションと負けポジション取得
			Position[] win_positions = result.Positions.Where(position => position.IsWin).ToArray();
			Position[] lose_positions = result.Positions.Where(position => position.IsLose).ToArray();

			// 総利益
			result.Profit = win_positions.Select(position => position.Return.Value).Sum();

			// 総損失
			result.Loss = lose_positions.Select(position => position.Return.Value).Sum();

			// 総損益
			result.Return = BackTestState.MarketAssets - BackTestState.InvestmentAmount;

			// 平均利益率
			if (win_positions.Length > 0) result.AverageProfitRate = win_positions.Select(position => position.ReturnRate).Average().Value;

			// 平均損失率
			if (lose_positions.Length > 0) result.AverageLossRate = lose_positions.Select(position => position.ReturnRate).Average().Value;

			// 平均損益率
			if (result.Positions.Length > 0) result.AverageReturnRate = result.Positions.Select(position => position.ReturnRate).Average().Value;

			// 損益率の標準偏差
			if (result.Positions.Length > 0) result.StandardDeviationReturnRate
					= (decimal)result.Positions.Select(position => (double)position.ReturnRate).PopulationStandardDeviation();

			// 総勝ち数
			result.WinTradeNum = win_positions.Length;

			// 総負け数
			result.LoseTradeNum = lose_positions.Length;

			// 最大連勝数
			result.MaxConsecutiveWinTradeNum = GetMaxConsecutiveTradeNum(result.Positions, true);

			// 最大連敗数
			result.MaxConsecutiveLoseTradeNum = GetMaxConsecutiveTradeNum(result.Positions, false);

			// ドローダウンの配列取得
			Drawdown[] dds = GetDrawdowns(result.HistoricalAssetsArray);

			// 最大ドローダウン
			if (dds.Length > 0) result.MaxDrawdown = dds.Select(dd => dd.Amount).Max();

			// 最大ドローダウン率
			if (dds.Length > 0) result.MaxDrawdownRate = result.MaxDrawdownRate = dds.Select(dd => dd.Rate).Max();

			// 最小時価資産
			if (result.HistoricalAssetsArray.Length > 0) result.MinMarketAssets = result.HistoricalAssetsArray.Select(es => es.MarketAssets).Min();

			// 勝率
			if (result.Positions.Length > 0) result.WinRate = (decimal)win_positions.Length / (decimal)result.Positions.Length;

			// 期待損益
			if (result.Positions.Length > 0) result.ExpectedReturn = result.Return / result.Positions.Length;

			// 平均保持日数
			if (result.Positions.Length > 0) result.AverageHoldDays = result.Positions.Select(position => (decimal)position.HoldDays).Average();

			// プロフィットファクター
			if (Math.Abs(result.Loss) > 0) result.ProfitFactor = result.Profit / Math.Abs(result.Loss);

			return result;
		}

		/// <summary>
		/// 連勝数、連敗数を取得します。
		/// </summary>
		/// <param name="positions">ポジションの配列</param>
		/// <param name="iswin">勝ちかどうか</param>
		/// <returns>連勝数、連敗数</returns>
		private int GetMaxConsecutiveTradeNum(Position[] positions, bool iswin)
		{
			int max = 0, num = 0;
			for (int i = 0; i < positions.Length; i++)
			{
				if (IsExpectedResult(positions[i], iswin))
				{
					// 期待トレードは加算
					num++;
					if (num > max) max = num;
				}
				else
				{
					// 期待トレードでないなら連勝、連敗終了
					num = 0;
				}
			}
			return max;
		}

		/// <summary>
		/// 期待トレードかどうか判定します。
		/// </summary>
		/// <param name="position">ポジション</param>
		/// <param name="iswin">勝ちかどうか</param>
		/// <returns>期待トレードかどうか</returns>
		private bool IsExpectedResult(Position position, bool iswin)
		{
			return (iswin && position.IsWin) || (!iswin && position.IsLose);
		}

		/// <summary>
		/// ドローダウンの配列を取得します。
		/// </summary>
		/// <param name="has">資産履歴の配列</param>
		/// <returns>ドローダウンの配列</returns>
		private Drawdown[] GetDrawdowns(HistoricalAssets[] has)
		{
			List<Drawdown> dds = new List<Drawdown>();
			for (int i = 0; i < has.Length - 1; i++)
			{
				// 次の履歴履歴が自身の時価資産を超えているならスキップ
				if (has[i + 1].MarketAssets > has[i].MarketAssets) continue;

				// 自身を局所高値とする
				Drawdown dd = new Drawdown();
				dd.LocalHighHistoricalAssets = has[i];

				// 局所高値を回復する資産履歴を探す
				int recover_idx;
				for (recover_idx = i + 2; recover_idx < has.Length; recover_idx++)
				{
					if (has[recover_idx].MarketAssets > has[i].MarketAssets)
					{
						dd.RecoverHistoricalAssets = has[recover_idx];
						break;
					}
				}

				if (dd.RecoverHistoricalAssets != null)
				{
					// 局所高値を回復する資産履歴がある

					// 局所高値と局所高値を回復する資産履歴の間の資産履歴取得
					List<HistoricalAssets> inner_has = has.ToList().GetRange(i + 1, recover_idx - i - 1);

					// 局所安値取得
					foreach (HistoricalAssets ha in inner_has)
					{
						if (ha.MarketAssets < has[i].MarketAssets
							&& (dd.LocalLowHistoricalAssets == null || ha.MarketAssets < dd.LocalLowHistoricalAssets.MarketAssets))
							dd.LocalLowHistoricalAssets = ha;
					}
				}
				else
				{
					// 局所高値を回復する資産履歴が無い

					// 局所高値以後の資産履歴取得
					List<HistoricalAssets> inner_has = has.ToList().GetRange(i + 1, has.Length - i - 1);

					// 局所安値取得
					foreach (HistoricalAssets ha in inner_has)
					{
						if (ha.MarketAssets < has[i].MarketAssets
							&& (dd.LocalLowHistoricalAssets == null || ha.MarketAssets < dd.LocalLowHistoricalAssets.MarketAssets))
							dd.LocalLowHistoricalAssets = ha;
					}
				}

				// 局所安値があれば追加
				if (dd.LocalLowHistoricalAssets != null) dds.Add(dd);
			}
			return dds.ToArray();
		}

		#endregion
	}
}
