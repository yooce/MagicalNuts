using MagicalNuts.UI.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.UI.TradingChart
{
	/// <summary>
	/// TradingChartのChartAreaを表します。
	/// </summary>
	[SupportedOSPlatform("windows")]
	public abstract class TradingChartArea : ChartArea
	{
		/// <summary>
		/// カーソルラベルY
		/// </summary>
		private Label CursorLabelY = null;

		/// <summary>
		/// TradingChartAreaクラスの新しいインスタンスを初期化します。
		/// </summary>
		public TradingChartArea()
		{
			// 一意な名前
			Name = Guid.NewGuid().ToString();
		}

		/// <summary>
		/// TradingChartAreaを準備します。
		/// </summary>
		/// <param name="chart">チャートコントロール</param>
		public virtual void SetUp(Chart chart)
		{
			// カーソルX
			CursorX.LineColor = ChartPalette.CursorColor;
			CursorX.LineDashStyle = ChartDashStyle.Dash;
			CursorX.AxisType = AxisType.Primary;
			CursorX.Interval = 0;

			// カーソルY
			CursorY.LineColor = ChartPalette.CursorColor;
			CursorY.LineDashStyle = ChartDashStyle.Dash;
			CursorY.AxisType = AxisType.Secondary;
			CursorY.Interval = 0;

			// カーソルラベルY
			CursorLabelY = new Label();
			SetUpCursorLabel(CursorLabelY);
			CursorLabelY.Visible = false;
			chart.Controls.Add(CursorLabelY);
		}

		/// <summary>
		/// カーソルラベルを準備します。
		/// </summary>
		/// <param name="label">カーソルラベル</param>
		protected void SetUpCursorLabel(Label label)
		{
			label.BackColor = ChartPalette.CursorLabelColor;
			label.ForeColor = Color.White;
			label.AutoSize = true;
			label.TextAlign = ContentAlignment.MiddleCenter;
		}

		/// <summary>
		/// カーソルを更新します。
		/// </summary>
		/// <param name="mouse">マウス座標</param>
		/// <param name="result">ヒットテストの結果</param>
		/// <param name="x">x座標</param>
		/// <param name="max_x">最大x座標</param>
		/// <param name="format">価格表示のフォーマット</param>
		public virtual void UpdateCursors(Point mouse, HitTestResult result, int x, int max_x, string format)
		{
			if (result.ChartArea == this)
			{
				// マウスが自エリア内の場合

				// カーソル
				CursorX.SetCursorPixelPosition(mouse, false);
				CursorY.SetCursorPixelPosition(mouse, false);

				// カーソルラベルY
				CursorLabelY.Text = AxisY2.PixelPositionToValue(mouse.Y).ToString(format);
				CursorLabelY.Top = mouse.Y - CursorLabelY.Height / 2;
				if (AxisX.ScaleView.Position + AxisX.ScaleView.Size > max_x)
					CursorLabelY.Left = (int)(AxisX.ValueToPixelPosition(AxisX.ScaleView.Position + AxisX.ScaleView.Size) + 1);
				else CursorLabelY.Left = (int)(AxisX.ValueToPixelPosition(AxisX.ScaleView.Position + AxisX.ScaleView.Size + 1) + 1);
				CursorLabelY.Visible = true;
			}
			else
			{
				// マウスが他エリア内の場合

				// カーソル
				CursorY.Position = double.NaN;

				// カーソルラベルY
				CursorLabelY.Visible = false;
			}
		}

		/// <summary>
		/// Y軸設定を更新します。
		/// </summary>
		/// <param name="begin_x">開始x座標</param>
		/// <param name="end_x">終了x座標</param>
		/// <param name="plotters">プロッターのリスト</param>
		public abstract void UpdateAxisYSettings(int begin_x, int end_x, List<Plotter.IPlotter> plotters);

		/// <summary>
		/// 範囲内のY値の配列を取得します。
		/// </summary>
		/// <param name="begin_x">開始x座標</param>
		/// <param name="end_x">終了x座標</param>
		/// <param name="plotters">プロッターのリスト</param>
		/// <param name="at">対象Y軸</param>
		/// <returns>範囲内のY値の配列</returns>
		protected double[] GetYValues(int begin_x, int end_x, List<Plotter.IPlotter> plotters, AxisType at)
		{
			List<double> values = new List<double>();
			foreach (Plotter.IPlotter plotter in plotters)
			{
				foreach (Series series in plotter.SeriesArray)
				{
					// 自ChartAreaでなければスキップ
					if (series.ChartArea != Name) continue;

					// 対象Y軸でなければスキップ
					if (series.YAxisType != at) continue;

					foreach (DataPoint dp in series.Points)
					{
						// xが範囲外ならスキップ
						if (dp.XValue < begin_x || dp.XValue >= end_x) continue;

						// y値追加
						foreach (double y in dp.YValues)
						{
							values.Add(y);
						}
					}
				}
			}
			return values.ToArray();
		}

		/// <summary>
		/// ChartAreaをクリアします。
		/// </summary>
		public void Clear()
		{
			AxisX.CustomLabels.Clear();
		}

		/// <summary>
		/// 小数点以下の桁数からカーソルのインターバルを取得します。
		/// </summary>
		/// <param name="digits">小数点以下の桁数</param>
		/// <returns>カーソルのインターバル</returns>
		public double GetCursorIntervalFromDigits(int digits)
		{
			double interval = 1;
			for (int i = 0; i < digits; i++)
			{
				interval /= 10;
			}
			return interval;
		}
	}
}
