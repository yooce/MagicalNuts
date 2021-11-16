using MagicalNuts.Indicator;
using MagicalNuts.UI.Base;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.UI.TradingChart.Plotter
{
	/// <summary>
	/// ロウソク足プロッターを表します。
	/// </summary>
	public class CandlePlotter : IndicatorPlotter<CandleIndicator>
	{
		/// <summary>
		/// Series
		/// </summary>
		private Series Series = null;

		/// <summary>
		/// CandleProtterの新しいインスタンスを初期化します。
		/// </summary>
		public CandlePlotter() : base()
		{
			Series = new Series();
			Series.ChartType = SeriesChartType.Candlestick;
			Series.YAxisType = AxisType.Secondary;
			Series["PriceUpColor"] = ChartPalette.PriceUpColor.R.ToString() + ", " + ChartPalette.PriceUpColor.G
				+ ", " + ChartPalette.PriceUpColor.B;
			Series["PriceDownColor"] = ChartPalette.PriceDownColor.R.ToString() + ", " + ChartPalette.PriceDownColor.G
				+ ", " + ChartPalette.PriceDownColor.B;
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		public override string Name => "ロウソク足";

		/// <summary>
		/// ChartAreaを設定します。
		/// </summary>
		/// <param name="mainChartArea">主ChartArea</param>
		/// <returns>使用する従ChartAreaの配列</returns>
		public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
		{
			Series.ChartArea = mainChartArea.Name;
			return null;
		}

		/// <summary>
		/// Seriesの配列を取得します。
		/// </summary>
		public override Series[] SeriesArray => new Series[] { Series };

		/// <summary>
		/// インジケーターをプロットします。
		/// </summary>
		/// <param name="candles">インジケーター用ロウソク足の集合</param>
		public override void PlotIndicator(IndicatorCandleCollection candles)
		{
			for (int x = 0; x < candles.Count; x++)
			{
				// 値
				DataPoint dp = new DataPoint(x, ConvertDecimalToDoubleArray(Indicator.GetValues(GetCandleCollection(x))));

				// 着色
				if (candles[x].Close >= candles[x].Open) dp.Color = ChartPalette.PriceUpColor;
				else dp.Color = ChartPalette.PriceDownColor;

				// 追加
				Series.Points.Add(dp);
			}
		}
	}
}
