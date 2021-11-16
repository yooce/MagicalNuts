using MagicalNuts.UI.Base;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.UI.TradingChart
{
	/// <summary>
	/// TradingChartの従ChartAreaを表します。
	/// </summary>
	[SupportedOSPlatform("windows")]
	public class SubChartArea : TradingChartArea
	{
		/// <summary>
		/// 分割線
		/// </summary>
		public HorizontalLineAnnotation Splitter { get; private set; }

		/// <summary>
		/// 従ChartAreaを準備します。
		/// </summary>
		/// <param name="chart">チャートコントロール</param>
		public override void SetUp(Chart chart)
		{
			base.SetUp(chart);

			// 分割線
			Splitter = new HorizontalLineAnnotation();
			Splitter.AllowMoving = true;
			Splitter.LineWidth = 2;
			Splitter.LineColor = ChartPalette.SplitterColor;
			Splitter.X = 0;
			Splitter.Width = 100;

			// X軸
			AxisX.ScrollBar.Enabled = false;
			AxisX.MajorTickMark.Enabled = false;
			AxisX.MajorGrid.LineColor = ChartPalette.GridColor;

			// Y軸
			AxisY2.MajorGrid.LineColor = ChartPalette.GridColor;
			AxisY2.ScrollBar.Enabled = false;
			AxisY2.ScrollBar.ButtonStyle = ScrollBarButtonStyles.None;
			AxisY2.Crossing = 0.0;
			AxisY2.LabelAutoFitMaxFontSize = 8;
			AxisY2.LabelAutoFitMinFontSize = 8;
		}

		/// <summary>
		/// Y軸設定を更新します。
		/// </summary>
		/// <param name="begin_x">開始x座標</param>
		/// <param name="end_x">終了x座標</param>
		/// <param name="plotters">プロッターのリスト</param>
		public override void UpdateAxisYSettings(int begin_x, int end_x, List<Plotter.IPlotter> plotters)
		{
			// Y値取得
			double[] values = GetYValues(begin_x, end_x, plotters, AxisType.Secondary);

			if (values.Length > 0)
			{
				// 最高値、最低値を取得
				double max = (double)values.Max();
				double min = (double)values.Min();

				// 範囲決定
				AxisY2.ScaleView.Size = (max - min) * (12.0 / 10.0);

				// 位置決定
				AxisY2.ScaleView.Position = min - AxisY2.ScaleView.Size / 10.0;
			}
		}
	}
}
