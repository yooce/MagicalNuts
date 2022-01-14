using MagicalNuts.Primitive;
using MagicalNuts.UI.Base;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.UI.TradingChart
{
	/// <summary>
	/// 主ChartAreaを表します
	/// </summary>
	[SupportedOSPlatform("windows")]
	public class MainChartArea : TradingChartArea
	{
		/// <summary>
		/// ロウソク足の集合
		/// </summary>
		protected CandleCollection<string> Candles = null;

		/// <summary>
		/// カーソルラベルX
		/// </summary>
		private Label CursorLabelX = null;

		/// <summary>
		/// 価格表示板
		/// </summary>
		private PriceBoard PriceBoard = null;

		/// <summary>
		/// 主ChartAreaを準備します。
		/// </summary>
		/// <param name="chart">チャートコントロール</param>
		public override void SetUp(Chart chart)
		{
			base.SetUp(chart);

			// サイズ
			Position.X = 0;
			Position.Y = 0;
			Position.Width = 100;
			Position.Height = 100;

			// X軸
			AxisX.ScaleView.Size = 200;
			AxisX.MajorTickMark.Enabled = false;
			AxisX.MajorGrid.LineColor = ChartPalette.GridColor;
			AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
			AxisX.ScrollBar.ButtonColor = ChartPalette.ScrollBarColor;
			AxisX.LabelAutoFitMaxFontSize = 8;
			AxisX.LabelAutoFitMinFontSize = 8;

			// Y軸（株価）
			AxisY2.MajorGrid.LineColor = ChartPalette.GridColor;
			AxisY2.ScrollBar.Enabled = false;
			AxisY2.ScrollBar.ButtonStyle = ScrollBarButtonStyles.None;
			AxisY2.LabelAutoFitMaxFontSize = 8;
			AxisY2.LabelAutoFitMinFontSize = 8;
			AxisY2.MajorTickMark.Size = 0.3f;
			
			// Y軸（出来高）
			AxisY.MajorGrid.Enabled = false;
			AxisY.Enabled = AxisEnabled.False;
			AxisY.LabelStyle.Enabled = false;

			// カーソルラベルX
			CursorLabelX = new Label();
			SetUpCursorLabel(CursorLabelX);
			chart.Controls.Add(CursorLabelX);

			// 価格表示板
			PriceBoard = new PriceBoard();
			PriceBoard.Top = chart.Margin.Top;
			PriceBoard.Left = chart.Margin.Left;
			chart.Controls.Add(PriceBoard);
			PriceBoard.SetCandle(null, null, null);
		}

		/// <summary>
		/// ロウソク足を設定します。
		/// </summary>
		/// <param name="candles">ロウソク足の集合</param>
		/// <param name="digits">小数点以下の桁数</param>
		public void SetCandles(CandleCollection<string> candles, int digits)
		{
			Candles = candles;
			AxisY2.LabelStyle.Format = PriceFormatter.GetPriceFormatFromDigits(digits);
		}

		/// <summary>
		/// カーソルを更新します。
		/// </summary>
		/// <param name="mouse">マウス座標</param>
		/// <param name="result">ヒットテストの結果</param>
		/// <param name="x">x座標</param>
		/// <param name="max_x">最大x座標</param>
		/// <param name="format">価格表示のフォーマット</param>
		public override void UpdateCursors(Point mouse, HitTestResult result, int x, int max_x, string format)
		{
			base.UpdateCursors(mouse, result, x, max_x, format);

			// カーソルラベルX
			if ((int)Candles.PeriodInfo.Unit < (int)PeriodUnit.Day) CursorLabelX.Text = Candles[x].DateTime.ToString();
			else CursorLabelX.Text = Candles[x].DateTime.ToShortDateString();
			CursorLabelX.Left = mouse.X - CursorLabelX.Width / 2;
			CursorLabelX.Top = (int)(AxisY2.ValueToPixelPosition(AxisY2.ScaleView.Position) + 1 + AxisY2.ScrollBar.Size);

			// 価格表示板
			Candle prev = null;
			if (x > 0) prev = Candles[x - 1];
			PriceBoard.SetCandle(Candles[x], prev, format);
		}

		/// <summary>
		/// Y軸設定を更新します。
		/// </summary>
		/// <param name="begin_x">開始x座標</param>
		/// <param name="end_x">終了x座標</param>
		/// <param name="plotters">プロッターのリスト</param>
		public override void UpdateAxisYSettings(int begin_x, int end_x, List<Plotter.IPlotter> plotters)
		{
			// 価格

			// Y値取得
			double[] values = GetYValues(begin_x, end_x, plotters, AxisType.Secondary);
			
			if (values.Length > 0)
			{
				// 最高値、最安値取得
				double max = (double)values.Max();
				double min = (double)values.Min();

				// 範囲決定
				AxisY2.ScaleView.Size = (max - min) * (12.0 / 8.0);

				// 位置決定
				AxisY2.ScaleView.Position = min - AxisY2.ScaleView.Size / 4.0;
			}

			// 出来高

			// Y値取得
			values = GetYValues(begin_x, end_x, plotters, AxisType.Primary);

			// 範囲決定
			if (values.Length > 0) AxisY.ScaleView.Size = values.Max() * 4;
		}
	}
}
