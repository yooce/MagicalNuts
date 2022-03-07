using System;
using System.Collections.Generic;
using System.Linq;

namespace MagicalNuts.BackTest
{
	/// <summary>
	/// バックテストの状態を表します。
	/// </summary>
	public class BackTestStatus
	{
		/// <summary>
		/// 日時
		/// </summary>
		public DateTime DateTime { get; set; }

		/// <summary>
		/// 銘柄ごとの戦略用ロウソク足の集合
		/// </summary>
		public StrategyCandleCollection[] StockCandles { get; set; }

		/// <summary>
		/// 初期資産
		/// </summary>
		public decimal InitialAssets { get; set; }

		/// <summary>
		/// 簿価資産
		/// </summary>
		public decimal BookAssets { get; set; }

		/// <summary>
		/// 時価資産
		/// </summary>
		public decimal MarketAssets { get; set; }

		/// <summary>
		/// 投資額
		/// </summary>
		public decimal InvestmentAmount { get; set; }

		/// <summary>
		/// 正味残高
		/// </summary>
		public decimal NetBalance { get; set; }

		/// <summary>
		/// 注文のリスト
		/// </summary>
		public List<Order> Orders { get; set; }

		/// <summary>
		/// 現在保有中のポジションのリスト
		/// </summary>
		public List<Position> ActivePositions { get; set; }

		/// <summary>
		/// ポジションの履歴
		/// </summary>
		public List<Position> HistoricalPositions { get; set; }

		/// <summary>
		/// 資産履歴のリスト
		/// </summary>
		public List<HistoricalAssets> HistoricalAssetsList { get; set; }

		/// <summary>
		/// 為替ストア
		/// </summary>
		public CurrencyStore CurrencyStore { get; set; }

		/// <summary>
		/// 通貨の小数点以下の桁数
		/// </summary>
		public int CurrencyDigits { get; set; }

		/// <summary>
		/// レバレッジ
		/// </summary>
		public decimal Leverage { get; set; }

		/// <summary>
		/// 単独銘柄を対象とした戦略用ロウソク足の集合
		/// </summary>
		public StrategyCandleCollection Candles
		{
			get
			{
				if (StockCandles == null || StockCandles.Length == 0) return null;
				return StockCandles[0];
			}
			set
			{
				StockCandles = new StrategyCandleCollection[] { value };
			}
		}

		/// <summary>
		/// 保有中のロングポジションのリスト
		/// </summary>
		public List<Position> ActiveLongPositions =>
			ActivePositions.Where(position => position.PositionDirection == PositionDirection.Long).ToList();

		/// <summary>
		/// 最後の保有中のロングポジション
		/// </summary>
		public Position LastActiveLongPosition => ActiveLongPositions.LastOrDefault();

		/// <summary>
		/// 保有中のショートポジションのリスト
		/// </summary>
		public List<Position> ActiveShortPositions =>
			ActivePositions.Where(position => position.PositionDirection == PositionDirection.Short).ToList();

		/// <summary>
		/// 最後の保有中のショートポジション
		/// </summary>
		public Position LastActiveShortPosition => ActiveShortPositions.LastOrDefault();

		/// <summary>
		/// BackTestStatusクラスの新しいインスタンスを初期化します。（シリアライザーのためのコンストラクタなので使用非推奨）
		/// </summary>
		public BackTestStatus()
		{
		}

		/// <summary>
		/// BackTestStatusクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="initial">初期資産</param>
		/// <param name="currencyStore">為替ストア</param>
		/// <param name="leverage">レバレッジ</param>
		public BackTestStatus(decimal initial, CurrencyStore currencyStore, int digits = 0, decimal leverage = 1)
		{
			InitialAssets = initial;
			BookAssets = initial;
			MarketAssets = initial;
			InvestmentAmount = initial;
			NetBalance = initial;
			Orders = new List<Order>();
			ActivePositions = new List<Position>();
			HistoricalPositions = new List<Position>();
			HistoricalAssetsList = new List<HistoricalAssets>();
			CurrencyStore = currencyStore;
			CurrencyDigits = digits;
			if (CurrencyStore == null) CurrencyStore = new CurrencyStore();
			Leverage = leverage;
		}

		/// <summary>
		/// 追加投資します。
		/// </summary>
		/// <param name="amount">追加投資額</param>
		public void AdditionalInvest(decimal amount)
		{
			// ここではMarketAssetsの更新はしない

			BookAssets += amount;
			InvestmentAmount += amount;
			NetBalance += amount;
		}

		/// <summary>
		/// 単独銘柄を対象とした戦略用バックテストの状態を取得します。
		/// </summary>
		/// <param name="candles">戦略用ロウソク足の集合</param>
		/// <returns>単独銘柄を対象とした戦略用バックテストの状態</returns>
		public BackTestStatus GetBackTestStatusForSingleStrategy(StrategyCandleCollection candles)
		{
			BackTestStatus state = new BackTestStatus(InitialAssets, CurrencyStore);
			state.DateTime = DateTime;
			state.Candles = candles;
			state.BookAssets = BookAssets;
			state.MarketAssets = MarketAssets;
			state.InvestmentAmount = InvestmentAmount;
			state.NetBalance = NetBalance;

			// ポジションは対象銘柄に絞る
			state.ActivePositions = ActivePositions.Where(position => position.Stock.Code == candles.Stock.Code).ToList();
			state.HistoricalPositions = HistoricalPositions.Where(position => position.Stock.Code == candles.Stock.Code).ToList();

			return state;
		}
	}
}
