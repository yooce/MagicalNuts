using MagicalNuts.Indicator;
using System.ComponentModel;

namespace TradingChartSample
{
	public class SampleIndicator : IIndicator
	{
		[Category("サンプル")]
		[DisplayName("期間")]
		[Description("期間を設定します。")]
		public int Period { get; set; } = 25;

		public async Task SetUpAsync()
		{
		}

		public decimal[] GetValues(IndicatorCandleCollection candles)
		{
			// 必要期間に満たない
			if (candles.Count < Period) return null;

			// 移動平均
			decimal ma = candles.GetRange(0, Period).Select(candle => candle.Close).Average();

			return new decimal[] { ma };
		}
	}
}
