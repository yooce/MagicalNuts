using MagicalNuts.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MagicalNuts.BackTest.Test
{
	public class BackTestCandleCollectionTest
	{
		private Candle[] candles
		{
			get
			{
				List<Candle> temps = CandleProvider.GetStockACandles().ToList();

				// 2021/10/27ÇçÌèú
				foreach (Candle temp in temps)
				{
					if (temp.DateTime == new DateTime(2021, 10, 27))
					{
						temps.Remove(temp);
						break;
					}
				}

				return temps.ToArray();
			}
		}

		[Fact]
		public void GetShiftedCandlesTest()
		{
			BackTestCandleCollection bt_candles = new BackTestCandleCollection(candles.ToList(), new Stock(null));

			StrategyCandleCollection st_candles = bt_candles.GetShiftedCandles(new DateTime(2021, 11, 5));
			Assert.Equal(st_candles[0].DateTime, new DateTime(2021, 11, 5));
			
			Assert.Null(bt_candles.GetShiftedCandles(new DateTime(2021, 10, 27)));
		}

		[Fact]
		public void GetLatestCandleTest()
		{
			BackTestCandleCollection bt_candles = new BackTestCandleCollection(candles.ToList(), new Stock(null));

			Candle candle = bt_candles.GetLatestCandle(new DateTime(2021, 11, 5));
			Assert.Equal(candle.DateTime, new DateTime(2021, 11, 5));

			candle = bt_candles.GetLatestCandle(new DateTime(2021, 10, 27));
			Assert.Equal(candle.DateTime, new DateTime(2021, 10, 26));
		}
	}
}