using MagicalNuts.Indicator;
using MagicalNuts.UI.TradingChart;
using MagicalNuts.UI.TradingChart.Plotter;
using System.ComponentModel;
using System.Windows.Forms.DataVisualization.Charting;

namespace TradingChartSample
{
	public class SampleIndicatorEx : SampleIndicator
	{
		[Category("プロット")]
		[DisplayName("色")]
		[Description("色を設定します。")]
		[DefaultValue(typeof(Color), "144, 30, 38")]
		public Color Color { get; set; } = Color.FromArgb(144, 30, 38);
	}

	public class SamplePlotter : IndicatorPlotter<SampleIndicatorEx>
	{
		private Series Series = null;

		public SamplePlotter()
		{
			Series = new Series();
			Series.ChartType = SeriesChartType.Line;
			Series.YAxisType = AxisType.Secondary;
		}

		public override string Name => "サンプル";

		public override object Properties => Indicator;

		public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
		{
			Series.ChartArea = mainChartArea.Name;
			return null;
		}

		public override Series[] SeriesArray => new Series[] { Series };

		public override void PlotIndicator(IndicatorCandleCollection candles)
		{
			for (int x = 0; x < candles.Count; x++)
			{
				decimal[] data = Indicator.GetValues(GetCandleCollection(x));
				if (data == null) continue;

				Series.Points.Add(new DataPoint(x, ConvertDecimalToDoubleArray(data)));
			}
		}

		public override async Task SetUpAsync()
		{
			Series.Color = Indicator.Color;
			await base.SetUpAsync();
		}
	}
}
