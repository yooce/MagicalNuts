using MagicalNuts.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace MagicalNuts.BackTest.Test
{
	public class ControllerTest
	{
		[Fact]
		public void GetDaysStockCandlesTest()
		{
			List<Candle> candles = CandleProvider.GetStockACandles().ToList();

			// 2021/10/27を削除
			foreach (Candle candle in candles)
			{
				if (candle.DateTime == new DateTime(2021, 10, 27))
				{
					candles.Remove(candle);
					break;
				}
			}

			BackTestCandleCollection bt_candles_a = new BackTestCandleCollection(candles, new Stock(null));
			BackTestCandleCollection bt_candles_b = new BackTestCandleCollection(CandleProvider.GetStockBCandles().ToList(), new Stock(null));
			Controller controller = new Controller();

			StrategyCandleCollection[] st_candles = (StrategyCandleCollection[])controller.GetType().InvokeMember("GetDaysStockCandles"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller
				, new object[] { new BackTestCandleCollection[] { bt_candles_a, bt_candles_b }, new DateTime(2021, 10, 26) });
			Assert.Equal(2, st_candles.Length);

			st_candles = (StrategyCandleCollection[])controller.GetType().InvokeMember("GetDaysStockCandles"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller
				, new object[] { new BackTestCandleCollection[] { bt_candles_a, bt_candles_b }, new DateTime(2021, 10, 27) });
			Assert.Single(st_candles);
		}

		[Fact]
		public void EntryPositionTest()
		{
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(1000, null);
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });

			Position position = new Position(null, PositionDirection.Long, 2, null, new DateTime(2021, 11, 12), 100, 200, 10);
			controller.GetType().InvokeMember("EntryPosition", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod
				, null, controller, new object[] { position });
			Assert.Equal(990, state.BookAssets);
			Assert.Equal(790, state.NetBalance);
			Assert.Single(state.ActiveLongPositions);
			Assert.Empty(state.HistoricalPositions);
		}

		[Fact]
		public void ExitPositionTest()
		{
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(1000, null);
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });

			Position position = new Position(null, PositionDirection.Long, 2, null, new DateTime(2021, 11, 12), 100, 200, 10);
			controller.GetType().InvokeMember("EntryPosition", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod
				, null, controller, new object[] { position });

			position.Exit(new DateTime(2021, 11, 13), 200, 400, 20, 1);
			controller.GetType().InvokeMember("ExitPosition", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod
				, null, controller, new object[] { position });
			Assert.Equal(1170, state.BookAssets);
			Assert.Equal(1170, state.NetBalance);
			Assert.Empty(state.ActiveLongPositions);
			Assert.Single(state.HistoricalPositions);
		}

		[Fact]
		public void PayFeeTest()
		{
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(1000, null);
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });
			controller.GetType().InvokeMember("PayFee", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod
				, null, controller, new object[] { 50.0m });
			Assert.Equal(950, state.BookAssets);
			Assert.Equal(950, state.NetBalance);
		}

		[Fact]
		public void SettlementTest()
		{
			// ロウソク足準備
			Stock stock = new Stock("1717");
			List<BackTestCandleCollection> stock_candles = new List<BackTestCandleCollection>();
			stock_candles.Add(new BackTestCandleCollection(CandleProvider.GetStockACandles().ToList(), stock));

			// コントローラーとステート準備
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(1000, null);
			TestFeeCalculator feeCalculator = new TestFeeCalculator();
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });
			controller.GetType().InvokeMember("FeeCalculator", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { feeCalculator });
			controller.GetType().InvokeMember("StockCandlesDictionary", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { stock_candles.ToDictionary(candles => candles.Stock.Code) });

			// ポジションエントリー
			Position position = new Position(stock, PositionDirection.Long, 1, null, new DateTime(2021, 10, 7), 856, 856, 10);
			controller.GetType().InvokeMember("EntryPosition", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod
				, null, controller, new object[] { position });

			// 清算
			controller.GetType().InvokeMember("Settlement", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null
				, controller, new object[] { new DateTime(2021, 10, 7) });

			// 検証
			Assert.Equal(1015, state.BookAssets);
			Assert.Equal(1015, state.MarketAssets);
			Assert.Equal(1015, state.NetBalance);
			Assert.Empty(state.ActiveLongPositions);
			Assert.Single(state.HistoricalPositions);
		}

		[Fact]
		public void CalculateMarketAssetsTest()
		{
			// ロウソク足準備
			Stock stock = new Stock("1717");
			List<BackTestCandleCollection> stock_candles = new List<BackTestCandleCollection>();
			stock_candles.Add(new BackTestCandleCollection(CandleProvider.GetStockACandles().ToList(), stock));

			// コントローラーとステート準備
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(1000, null);
			state.DateTime = new DateTime(2021, 10, 7);
			TestFeeCalculator feeCalculator = new TestFeeCalculator();
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });
			controller.GetType().InvokeMember("FeeCalculator", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { feeCalculator });
			controller.GetType().InvokeMember("StockCandlesDictionary", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { stock_candles.ToDictionary(candles => candles.Stock.Code) });

			// ポジションエントリー
			Position position = new Position(stock, PositionDirection.Long, 1, null, new DateTime(2021, 10, 7), 856, 856, 10);
			controller.GetType().InvokeMember("EntryPosition", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod
				, null, controller, new object[] { position });

			// 時価資産計算
			state.MarketAssets = (decimal)controller.GetType().InvokeMember("CalculateMarketAssets"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, null);

			// 検証
			Assert.Equal(990, state.BookAssets);
			Assert.Equal(1035, state.MarketAssets);
			Assert.Equal(134, state.NetBalance);
			Assert.Single(state.ActiveLongPositions);
			Assert.Empty(state.HistoricalPositions);
		}

		[Fact]
		public void GetEntryFeeTest()
		{
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(0, null);
			TestFeeCalculator feeCalculator = new TestFeeCalculator();
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });
			controller.GetType().InvokeMember("FeeCalculator", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { feeCalculator });
			decimal fee = (decimal)controller.GetType().InvokeMember("GetEntryFee"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller
				, new object[] { 100.0m, 1.0m, 1.0m });
			Assert.Equal(10, fee);
		}

		[Fact]
		public void GetExitFeeTest()
		{
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(0, null);
			TestFeeCalculator feeCalculator = new TestFeeCalculator();
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });
			controller.GetType().InvokeMember("FeeCalculator", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { feeCalculator });
			decimal fee = (decimal)controller.GetType().InvokeMember("GetExitFee"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller
				, new object[] { 100.0m, 1.0m, 1.0m });
			Assert.Equal(20, fee);
		}

		[Fact]
		public void GetMonthlyFeeTest()
		{
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(0, null);
			TestFeeCalculator feeCalculator = new TestFeeCalculator();
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });
			controller.GetType().InvokeMember("FeeCalculator", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { feeCalculator });
			decimal fee = (decimal)controller.GetType().InvokeMember("GetMonthlyFee"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, null);
			Assert.Equal(4, fee);
		}

		[Fact]
		public void GetYearlyFeeTest()
		{
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(0, null);
			TestFeeCalculator feeCalculator = new TestFeeCalculator();
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });
			controller.GetType().InvokeMember("FeeCalculator", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { feeCalculator });
			decimal fee = (decimal)controller.GetType().InvokeMember("GetYearlyFee"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, null);
			Assert.Equal(8, fee);
		}

		[Fact]
		public void GetEntryExecutionAmountTest()
		{
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(0, null);
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });
			decimal amount = (decimal)controller.GetType().InvokeMember("GetEntryExecutionAmount"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller
				, new object[] { 630.6m, 2.0m, 1.0m });
			Assert.Equal(1262, amount);
		}

		[Fact]
		public void GetExitExecutionAmount()
		{
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(0, null);
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });
			decimal amount = (decimal)controller.GetType().InvokeMember("GetExitExecutionAmount"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller
				, new object[] { 630.6m, 2.0m, 1.0m });
			Assert.Equal(1261, amount);
		}

		[Fact]
		public void GetAdjustedLotsTest()
		{
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(1000, null);
			TestFeeCalculator feeCalculator = new TestFeeCalculator();
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });
			controller.GetType().InvokeMember("FeeCalculator", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { feeCalculator });

			decimal lots = (decimal)controller.GetType().InvokeMember("GetAdjustedLots"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller
				, new object[] { 400.0m, 2.0m, 1.0m, 1.0m });
			Assert.Equal(2.0m, lots);

			lots = (decimal)controller.GetType().InvokeMember("GetAdjustedLots"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller
				, new object[] { 495.0m, 3.0m, 1.0m, 1.0m });
			Assert.Equal(2.0m, lots);

			lots = (decimal)controller.GetType().InvokeMember("GetAdjustedLots"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller
				, new object[] { 200.0m, 6.0m, 1.0m, 2.0m });
			Assert.Equal(4.0m, lots);

			lots = (decimal)controller.GetType().InvokeMember("GetAdjustedLots"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller
				, new object[] { 999.0m, 3.0m, 1.0m, 2.0m });
			Assert.Equal(0.0m, lots);
		}

		[Fact]
		public void BuyMarketEntryTest()
		{
			// ロウソク足準備
			Stock stock = new Stock("1717");
			List<BackTestCandleCollection> stock_candles = new List<BackTestCandleCollection>();
			stock_candles.Add(new BackTestCandleCollection(CandleProvider.GetStockACandles().ToList(), stock));

			// コントローラーとステート準備
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(1000, null);
			state.DateTime = new DateTime(2021, 10, 7);
			TestFeeCalculator feeCalculator = new TestFeeCalculator();
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });
			controller.GetType().InvokeMember("FeeCalculator", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { feeCalculator });
			controller.GetType().InvokeMember("StockCandlesDictionary", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { stock_candles.ToDictionary(candles => candles.Stock.Code) });

			// 新規買い
			Order order = Order.GetBuyMarketOrder(stock, 2);
			(bool ret, _) = ((bool, Position))controller.GetType().InvokeMember("BuyMarket"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { order });
			Assert.True(ret);
			Assert.Equal("1717", state.ActivePositions[0].Stock.Code);
			Assert.Equal(PositionDirection.Long, state.ActivePositions[0].PositionDirection);
			Assert.Equal(1.0m, state.ActivePositions[0].Lots);
			Assert.Equal(new DateTime(2021, 10, 7), state.ActivePositions[0].EntryExecution.DateTime);
			Assert.Equal(856.0m, state.ActivePositions[0].EntryExecution.Price);
			Assert.Equal(856.0m, state.ActivePositions[0].EntryExecution.Amount);
			Assert.Equal(10, state.ActivePositions[0].EntryExecution.Fee);

			order = Order.GetBuyMarketOrder(stock, 1);
			(ret, _) = ((bool, Position))controller.GetType().InvokeMember("BuyMarket"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { order });
			Assert.False(ret);
			Assert.Single(state.ActivePositions);
		}

		[Fact]
		public void BuyMarketExitTest()
		{
			// ロウソク足準備
			Stock stock = new Stock("1717");
			List<BackTestCandleCollection> stock_candles = new List<BackTestCandleCollection>();
			stock_candles.Add(new BackTestCandleCollection(CandleProvider.GetStockACandles().ToList(), stock));

			// コントローラーとステート準備
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(1000, null);
			state.DateTime = new DateTime(2021, 8, 27);
			TestFeeCalculator feeCalculator = new TestFeeCalculator();
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });
			controller.GetType().InvokeMember("FeeCalculator", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { feeCalculator });
			controller.GetType().InvokeMember("StockCandlesDictionary", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { stock_candles.ToDictionary(candles => candles.Stock.Code) });

			// 新規売り
			Order order = Order.GetSellMarketOrder(stock, 1);
			controller.GetType().InvokeMember("SellMarket"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { order });

			// 返済買い
			state.DateTime = new DateTime(2021, 10, 7);
			order = Order.GetBuyMarketOrder(state.LastActiveShortPosition);
			(bool ret, _) = ((bool, Position))controller.GetType().InvokeMember("BuyMarket"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { order });
			Assert.True(ret);
			Assert.Equal(new DateTime(2021, 10, 7), state.HistoricalPositions[0].ExitExecution.DateTime);
			Assert.Equal(856.0m, state.HistoricalPositions[0].ExitExecution.Price);
			Assert.Equal(856.0m, state.HistoricalPositions[0].ExitExecution.Amount);
			Assert.Equal(20, state.HistoricalPositions[0].ExitExecution.Fee);
		}

		[Fact]
		public void SellMarketEntryTest()
		{
			// ロウソク足準備
			Stock stock = new Stock("1717");
			List<BackTestCandleCollection> stock_candles = new List<BackTestCandleCollection>();
			stock_candles.Add(new BackTestCandleCollection(CandleProvider.GetStockACandles().ToList(), stock));

			// コントローラーとステート準備
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(1000, null);
			state.DateTime = new DateTime(2021, 10, 7);
			TestFeeCalculator feeCalculator = new TestFeeCalculator();
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });
			controller.GetType().InvokeMember("FeeCalculator", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { feeCalculator });
			controller.GetType().InvokeMember("StockCandlesDictionary", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { stock_candles.ToDictionary(candles => candles.Stock.Code) });

			// 新規売り
			Order order = Order.GetSellMarketOrder(stock, 2);
			(bool ret, _) = ((bool, Position))controller.GetType().InvokeMember("SellMarket"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { order });
			Assert.True(ret);
			Assert.Equal("1717", state.ActivePositions[0].Stock.Code);
			Assert.Equal(PositionDirection.Short, state.ActivePositions[0].PositionDirection);
			Assert.Equal(1.0m, state.ActivePositions[0].Lots);
			Assert.Equal(new DateTime(2021, 10, 7), state.ActivePositions[0].EntryExecution.DateTime);
			Assert.Equal(856.0m, state.ActivePositions[0].EntryExecution.Price);
			Assert.Equal(856.0m, state.ActivePositions[0].EntryExecution.Amount);
			Assert.Equal(10, state.ActivePositions[0].EntryExecution.Fee);

			order = Order.GetBuyMarketOrder(stock, 1);
			(ret, _) = ((bool, Position))controller.GetType().InvokeMember("SellMarket"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { order });
			Assert.False(ret);
			Assert.Single(state.ActivePositions);
		}

		[Fact]
		public void SellMarketExitTest()
		{
			// ロウソク足準備
			Stock stock = new Stock("1717");
			List<BackTestCandleCollection> stock_candles = new List<BackTestCandleCollection>();
			stock_candles.Add(new BackTestCandleCollection(CandleProvider.GetStockACandles().ToList(), stock));

			// コントローラーとステート準備
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(1000, null);
			state.DateTime = new DateTime(2021, 5, 17);
			TestFeeCalculator feeCalculator = new TestFeeCalculator();
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });
			controller.GetType().InvokeMember("FeeCalculator", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { feeCalculator });
			controller.GetType().InvokeMember("StockCandlesDictionary", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { stock_candles.ToDictionary(candles => candles.Stock.Code) });

			// 新規買い
			Order order = Order.GetSellMarketOrder(stock, 1);
			controller.GetType().InvokeMember("SellMarket"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { order });

			// 手仕舞い売り
			state.DateTime = new DateTime(2021, 10, 7);
			order = Order.GetBuyMarketOrder(state.LastActiveShortPosition);
			(bool ret, _) = ((bool, Position))controller.GetType().InvokeMember("SellMarket"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { order });
			Assert.True(ret);
			Assert.Equal(new DateTime(2021, 10, 7), state.HistoricalPositions[0].ExitExecution.DateTime);
			Assert.Equal(856.0m, state.HistoricalPositions[0].ExitExecution.Price);
			Assert.Equal(856.0m, state.HistoricalPositions[0].ExitExecution.Amount);
			Assert.Equal(20, state.HistoricalPositions[0].ExitExecution.Fee);
		}

		[Fact]
		public void HandleOrdersTest()
		{
			// ロウソク足準備
			Stock stock = new Stock("1717");
			List<BackTestCandleCollection> stock_candles = new List<BackTestCandleCollection>();
			stock_candles.Add(new BackTestCandleCollection(CandleProvider.GetStockACandles().ToList(), stock));

			// コントローラーとステート準備
			Controller controller = new Controller();
			BackTestStatus state = new BackTestStatus(1000, null);
			TestFeeCalculator feeCalculator = new TestFeeCalculator();
			controller.GetType().InvokeMember("BackTestState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { state });
			controller.GetType().InvokeMember("FeeCalculator", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { feeCalculator });
			controller.GetType().InvokeMember("StockCandlesDictionary", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField
				, null, controller, new object[] { stock_candles.ToDictionary(candles => candles.Stock.Code) });

			// 前準備
			state.DateTime = new DateTime(2021, 10, 7);
			Order order = Order.GetBuyMarketOrder(stock, 1);
			controller.GetType().InvokeMember("BuyMarket"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { order });

			// 翌日に
			state.DateTime = new DateTime(2021, 10, 8);

			// ロットゼロ
			state.Orders.Add(Order.GetBuyMarketOrder(stock, 1));

			// 新規買い
			state.Orders.Add(Order.GetBuyMarketOrder(stock, 1));

			// 手仕舞い売り
			state.Orders.Add(Order.GetSellMarketOrder(state.LastActiveLongPosition));

			// 注文処理
			controller.GetType().InvokeMember("HandleOrders", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null
				, controller, null);

			// 検証
			Assert.Empty(state.Orders);
			Assert.Single(state.ActivePositions);
			Assert.Equal(new DateTime(2021, 10, 8), state.ActivePositions[0].EntryExecution.DateTime);
			Assert.Single(state.HistoricalPositions);
		}

		[Fact]
		public void GetMaxConsecutiveTradeNumTest()
		{
			// 勝ちポジション
			Position win = new Position(null, PositionDirection.Long, 1, null, DateTime.Now, 100, 100, 0);
			win.Exit(DateTime.Now, 200, 200, 0, 0);

			// 負けポジション
			Position lose = new Position(null, PositionDirection.Long, 1, null, DateTime.Now, 200, 200, 0);
			lose.Exit(DateTime.Now, 100, 100, 0, 0);

			// 連勝数、連敗数
			Position[] positions = new Position[] { lose, win, win, win, lose, lose };
			Controller controller = new Controller();
			int con_win = (int)controller.GetType().InvokeMember("GetMaxConsecutiveTradeNum"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { positions, true });
			int con_lose = (int)controller.GetType().InvokeMember("GetMaxConsecutiveTradeNum"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { positions, false });

			// 検証
			Assert.Equal(3, con_win);
			Assert.Equal(2, con_lose);
		}

		[Fact]
		public void IsExpectedResultTest()
		{
			// 勝ちポジション
			Position win = new Position(null, PositionDirection.Long, 1, null, DateTime.Now, 100, 100, 0);
			win.Exit(DateTime.Now, 200, 200, 0, 0);

			// 負けポジション
			Position lose = new Position(null, PositionDirection.Long, 1, null, DateTime.Now, 200, 200, 0);
			lose.Exit(DateTime.Now, 100, 100, 0, 0);

			// 検証
			Controller controller = new Controller();
			Assert.True((bool)controller.GetType().InvokeMember("IsExpectedResult"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { win, true }));
			Assert.False((bool)controller.GetType().InvokeMember("IsExpectedResult"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { win, false }));
			Assert.False((bool)controller.GetType().InvokeMember("IsExpectedResult"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { lose, true }));
			Assert.True((bool)controller.GetType().InvokeMember("IsExpectedResult"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { lose, false }));
		}

		[Fact]
		public void GetDrawdownsTest()
		{
			// 準備
			List<HistoricalAssets> has = new List<HistoricalAssets>();
			has.Add(new HistoricalAssets(DateTime.Now, 0, 100, 0));
			has.Add(new HistoricalAssets(DateTime.Now, 0, 90, 0));
			has.Add(new HistoricalAssets(DateTime.Now, 0, 120, 0));
			has.Add(new HistoricalAssets(DateTime.Now, 0, 90, 0));

			// ドローダウン
			Controller controller = new Controller();
			Drawdown[] dds = (Drawdown[])controller.GetType().InvokeMember("GetDrawdowns"
				, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, controller, new object[] { has.ToArray() });

			// 検証
			Assert.Equal(2, dds.Length);
			Assert.Equal(10, dds[0].Amount);
			Assert.Equal(0.1m, dds[0].Rate);
			Assert.Equal(30, dds[1].Amount);
			Assert.Equal(0.25m, dds[1].Rate);
		}

		[Fact]
		public async void BackTestTest()
		{
			// 準備
			Stock stock = new Stock("1717", null, 100);
			Arguments args = new Arguments(new TestStrategy(), new BackTestCandleCollection(CandleProvider.GetStockACandles().ToList(), stock)
				, new DateTime(2021, 9, 1), new DateTime(2021, 9, 30), new TestFeeCalculator());

			// バックテスト
			Controller controller = new Controller();
			BackTestResult result = await controller.BackTestAsync<BackTestResult>(args);

			// 検証
			Assert.Equal(100000, result.InitialAssets);
			Assert.Equal(0, result.AdditionalInvestment);
			Assert.Equal(13400, result.Profit);
			Assert.Equal(-9970, result.Loss);
			Assert.Equal(3430, result.Return);
			Assert.Equal(Math.Round(0.015035747m, 4), Math.Round(result.AverageProfitRate, 4));
			Assert.Equal(Math.Round(-0.012170379m, 4), Math.Round(result.AverageLossRate, 4));
			Assert.Equal(Math.Round(0.002148635m, 4), Math.Round(result.AverageReturnRate, 4));
			Assert.Equal(Math.Round(0.015897761m, 4), Math.Round(result.StandardDeviationReturnRate, 4));
			Assert.Equal(10, result.WinTradeNum);
			Assert.Equal(9, result.LoseTradeNum);
			Assert.Equal(5, result.MaxConsecutiveWinTradeNum);
			Assert.Equal(3, result.MaxConsecutiveLoseTradeNum);
			Assert.Equal(3550, result.MaxDrawdown);
			Assert.Equal(Math.Round(0.03344324m, 4), Math.Round(result.MaxDrawdownRate, 4));
			Assert.Equal(98990, result.MinMarketAssets);
			Assert.Equal(Math.Round(0.526315789m, 4), Math.Round(result.WinRate, 4));
			Assert.Equal(Math.Round(180.5263158m, 4), Math.Round(result.ExpectedReturn, 4));
			Assert.Equal(Math.Round(1.344032096m, 4), Math.Round(result.ProfitFactor, 4));
		}
	}
}