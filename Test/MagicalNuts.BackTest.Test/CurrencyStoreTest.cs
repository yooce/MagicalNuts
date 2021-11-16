using MagicalNuts.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MagicalNuts.BackTest.Test
{
	public class CurrencyStoreTest
	{
		[Fact]
		public void GetPriceTest()
		{
			List<Candle> candles = CandleProvider.GetCurrencyCandles().ToList();

			// 2021/10/27ÇçÌèú
			foreach (Candle candle in candles)
			{
				if (candle.DateTime == new DateTime(2021, 10, 27))
				{
					candles.Remove(candle);
					break;
				}
			}

			BackTestCandleCollection bt_candles = new BackTestCandleCollection(candles, new Stock(null));
			CurrencyStore cs = new CurrencyStore();
			cs.Add(1, bt_candles);

			Assert.Equal(1, cs.GetPrice(0, new DateTime(2021, 10, 27)));

			Assert.Equal(114.14m, cs.GetPrice(1, new DateTime(2021, 10, 27)));
			Assert.Equal(114.14m, cs.GetPrice(1, new DateTime(2021, 10, 28)));
		}
	}
}