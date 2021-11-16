using MagicalNuts.Primitive;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MagicalNuts.Indicator.Test
{
	public class AtrIndicatorTest
	{
		private List<Candle> Candles
		{
			get
			{
				List<Candle> candles = new List<Candle>()
				{
					new Candle(new DateTime(2020, 8, 1), 0, 46.78m, 46.34m, 46.43m),
					new Candle(new DateTime(2020, 8, 2), 0, 47.32m, 46.64m, 46.87m),
					new Candle(new DateTime(2020, 8, 3), 0, 47.45m, 46.43m, 46.76m),
					new Candle(new DateTime(2020, 8, 4), 0, 47.20m, 46.83m, 46.92m),
					new Candle(new DateTime(2020, 8, 5), 0, 47.86m, 47.20m, 47.23m),
					new Candle(new DateTime(2020, 8, 6), 0, 47.90m, 47.27m, 47.49m),
					new Candle(new DateTime(2020, 8, 7), 0, 48.23m, 47.34m, 47.34m),
					new Candle(new DateTime(2020, 8, 8), 0, 47.90m, 47.23m, 47.43m),
					new Candle(new DateTime(2020, 8, 9), 0, 47.56m, 46.34m, 46.83m),
					new Candle(new DateTime(2020, 8, 10), 0, 47.45m, 46.34m, 46.47m),
					new Candle(new DateTime(2020, 8, 11), 0, 47.67m, 46.43m, 46.56m),
					new Candle(new DateTime(2020, 8, 12), 0, 47.69m, 46.83m, 47.32m),
					new Candle(new DateTime(2020, 8, 13), 0, 48.67m, 47.20m, 47.53m),
					new Candle(new DateTime(2020, 8, 14), 0, 47.99m, 47.47m, 47.78m),
					new Candle(new DateTime(2020, 8, 15), 0, 48.32m, 47.47m, 47.63m),
					new Candle(new DateTime(2020, 8, 16), 0, 47.97m, 47.41m, 47.56m),
					new Candle(new DateTime(2020, 8, 17), 0, 47.98m, 47.21m, 47.33m),
					new Candle(new DateTime(2020, 8, 18), 0, 47.89m, 47.46m, 47.64m),
					new Candle(new DateTime(2020, 8, 19), 0, 48.34m, 47.55m, 47.87m),
					new Candle(new DateTime(2020, 8, 20), 0, 48.32m, 47.87m, 47.98m),
					new Candle(new DateTime(2020, 8, 21), 0, 48.39m, 47.56m, 47.84m),
					new Candle(new DateTime(2020, 8, 22), 0, 48.11m, 47.25m, 47.37m),
					new Candle(new DateTime(2020, 8, 23), 0, 48.02m, 47.67m, 47.79m),
				};
				candles.Reverse();
				return candles;
			}
		}

		[Fact]
		public void GetTrueRangeTest()
		{
			decimal[] trs = new decimal[]
			{
				0.44m,
				0.89m,
				1.02m,
				0.44m,
				0.94m,
				0.67m,
				0.89m,
				0.67m,
				1.22m,
				1.11m,
				1.24m,
				1.13m,
				1.47m,
				0.52m,
				0.85m,
				0.56m,
				0.77m,
				0.56m,
				0.79m,
				0.45m,
				0.83m,
				0.86m,
				0.65m,
			};

			List<Candle> candles = Candles;
			for (int i = candles.Count - 1; i >= 0; i--)
			{
				if (i == candles.Count - 1) Assert.Equal(AtrIndicator.GetTrueRange(candles[i], null), trs[candles.Count - i - 1]);
				else Assert.Equal(AtrIndicator.GetTrueRange(candles[i], candles[i + 1]), trs[candles.Count - i - 1]);
			}
		}

		[Fact]
		public async Task GetValueTest()
		{
			decimal[][] atrs = new decimal[][]
			{
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				new decimal[] { 0.90m },
				new decimal[] { 0.90m },
				new decimal[] { 0.88m },
				new decimal[] { 0.87m },
				new decimal[] { 0.85m },
				new decimal[] { 0.84m },
				new decimal[] { 0.81m },
				new decimal[] { 0.82m },
				new decimal[] { 0.82m },
				new decimal[] { 0.81m },
			};

			AtrIndicator indicator = new AtrIndicator();
			indicator.Period = 14;
			indicator.MovingAverageMethod = MovingAverageMethod.Smma;
			await indicator.SetUpAsync();

			IndicatorCandleCollection candles = new IndicatorCandleCollection(Candles, null);
			for (int i = candles.Count - 1; i >= 0; i--)
			{
				decimal[] values = indicator.GetValues(candles.Shift(i));
				if (values == null) Assert.Equal(values, atrs[candles.Count - i - 1]);
				else Assert.Equal(Math.Round(values[0], 2), atrs[candles.Count - i - 1][0]);
			}
		}
	}
}
