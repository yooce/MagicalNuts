using MagicalNuts.Primitive;
using System;
using System.Linq;
using Xunit;

namespace MagicalNuts.BackTest.Test
{
	public class BackTestStatusTest
	{
		[Fact]
		public void LastActiveLongPositionTest()
		{
			BackTestStatus state = new BackTestStatus(0, null);

			Position position0 = new Position(null, PositionDirection.Short, 1, null, new DateTime(2021, 10, 1), 100, 100, 0);
			state.ActivePositions.Add(position0);
			Assert.Null(state.LastActiveLongPosition);

			Position position1 = new Position(null, PositionDirection.Long, 1, null, new DateTime(2021, 10, 1), 100, 100, 0);
			Position position2 = new Position(null, PositionDirection.Short, 1, null, new DateTime(2021, 10, 1), 100, 100, 0);
			Position position3 = new Position(null, PositionDirection.Short, 1, null, new DateTime(2021, 10, 1), 100, 100, 0);
			state.ActivePositions.Add(position1);
			state.ActivePositions.Add(position2);
			state.ActivePositions.Add(position3);
			Assert.Equal(position1, state.LastActiveLongPosition);
		}

		[Fact]
		public void LastActiveShortPositionTest()
		{
			BackTestStatus state = new BackTestStatus(0, null);

			Position position0 = new Position(null, PositionDirection.Long, 1, null, new DateTime(2021, 10, 1), 100, 100, 0);
			state.ActivePositions.Add(position0);
			Assert.Null(state.LastActiveShortPosition);

			Position position1 = new Position(null, PositionDirection.Long, 1, null, new DateTime(2021, 10, 1), 100, 100, 0);
			Position position2 = new Position(null, PositionDirection.Short, 1, null, new DateTime(2021, 10, 1), 100, 100, 0);
			Position position3 = new Position(null, PositionDirection.Short, 1, null, new DateTime(2021, 10, 1), 100, 100, 0);
			state.ActivePositions.Add(position1);
			state.ActivePositions.Add(position2);
			state.ActivePositions.Add(position3);
			Assert.Equal(position3, state.LastActiveShortPosition);
		}

		[Fact]
		public void ConstructorTest()
		{
			BackTestStatus state = new BackTestStatus(100, null);
			Assert.Equal(100, state.InitialAssets);
			Assert.Equal(100, state.BookAssets);
			Assert.Equal(100, state.MarketAssets);
			Assert.Equal(100, state.InvestmentAmount);
			Assert.Equal(100, state.NetBalance);
		}

		[Fact]
		public void AdditionalInvestTest()
		{
			BackTestStatus state = new BackTestStatus(100, null);
			state.AdditionalInvest(100);
			Assert.Equal(100, state.InitialAssets);
			Assert.Equal(200, state.BookAssets);
			Assert.Equal(200, state.InvestmentAmount);
			Assert.Equal(200, state.NetBalance);
		}

		[Fact]
		public void GetBackTestStatusForSingleStrategyTest()
		{
			BackTestStatus state = new BackTestStatus(100, null);
			state.DateTime = new DateTime(2021, 10, 11);
			Position position0 = new Position(new Stock("A"), PositionDirection.Long, 1, null, new DateTime(2021, 10, 1), 100, 100, 0);
			Position position1 = new Position(new Stock("B"), PositionDirection.Short, 1, null, new DateTime(2021, 10, 1), 100, 100, 0);
			Position position2 = new Position(new Stock("A"), PositionDirection.Long, 1, null, new DateTime(2021, 10, 1), 100, 100, 0);
			Position position3 = new Position(new Stock("A"), PositionDirection.Long, 1, null, new DateTime(2021, 10, 1), 100, 100, 0);
			Position position4 = new Position(new Stock("B"), PositionDirection.Long, 1, null, new DateTime(2021, 10, 1), 100, 100, 0);
			state.ActivePositions.Add(position0);
			state.ActivePositions.Add(position1);
			state.HistoricalPositions.Add(position2);
			state.HistoricalPositions.Add(position3);
			state.HistoricalPositions.Add(position4);

			BackTestCandleCollection bt_candles = new BackTestCandleCollection(CandleProvider.GetStockACandles().ToList(), new Stock("A", null));
			BackTestStatus single = state.GetBackTestStatusForSingleStrategy(bt_candles.GetShiftedCandles(new DateTime(2021, 10, 11)));

			Assert.Equal(100, single.InitialAssets);
			Assert.Equal(new DateTime(2021, 10, 11), single.DateTime);
			Assert.Equal(new DateTime(2021, 10, 11), single.Candles[0].DateTime);
			Assert.Equal(100, single.BookAssets);
			Assert.Equal(100, single.MarketAssets);
			Assert.Equal(100, single.InvestmentAmount);
			Assert.Equal(100, single.NetBalance);

			Assert.Single(single.ActivePositions);
			Assert.Equal(2, single.HistoricalPositions.Count);
		}
	}
}