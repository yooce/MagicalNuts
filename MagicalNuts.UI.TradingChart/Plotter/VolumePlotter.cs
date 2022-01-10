using MagicalNuts.Indicator;
using MagicalNuts.UI.Base;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.UI.TradingChart.Plotter
{
	/// <summary>
	/// 出来高プロッターを表します。
	/// </summary>
	public class VolumePlotter : IndicatorPlotter<VolumeIndicator>
	{
		/// <summary>
		/// Series
		/// </summary>
		private Series Series = null;

		/// <summary>
		/// VolumePlotterクラスの新しいインスタンスを初期化します。
		/// </summary>
		public VolumePlotter() : base()
		{
			Series = new Series();
			Series.ChartType = SeriesChartType.Column;
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		public override string Name => "出来高";

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
				// シフト済みインジケーター用ロウソク足の集合
				IndicatorCandleCollection shifted = GetCandleCollection(x);

				// 値
				DataPoint dp = new DataPoint(x, ConvertDecimalToDoubleArray(Indicator.GetValues(shifted)));

				// 着色
				if (shifted[0].Close >= shifted[0].Open) dp.Color = Color.FromArgb(127, ChartPalette.PriceUpColor);
				else dp.Color = Color.FromArgb(127, ChartPalette.PriceDownColor);

				// 追加
				Series.Points.Add(dp);
			}
		}
	}
}
