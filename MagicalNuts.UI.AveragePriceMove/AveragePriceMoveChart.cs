using MagicalNuts.AveragePriceMove;
using MagicalNuts.UI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.UI.AveragePriceMove
{
	/// <summary>
	/// 平均値動き推移のチャートを表します。
	/// </summary>
	[SupportedOSPlatform("windows")]
	public class AveragePriceMoveChart : Chart
	{
		/// <summary>
		/// AveragePriceMoveChartクラスの新しいインスタンスを初期化します。
		/// </summary>
		public AveragePriceMoveChart()
		{
			// ChartArea
			ChartAreas.Add(new ChartArea());

			// X軸設定
			ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
			ChartAreas[0].AxisX.MajorGrid.LineColor = ChartPalette.GridColor;

			// Y軸設定
			ChartAreas[0].AxisY2.MajorGrid.LineColor = ChartPalette.GridColor;
			ChartAreas[0].AxisY2.Crossing = 100.0;
			ChartAreas[0].AxisY2.MajorTickMark.Enabled = false;
			ChartAreas[0].AxisY2.ScrollBar.Enabled = false;
			ChartAreas[0].AxisY2.Title = "増減率（％）";
		}

		/// <summary>
		/// 銘柄ごとの平均値動き推移を設定します。
		/// </summary>
		/// <param name="apms">銘柄ごとの平均値動き推移</param>
		/// <returns>銘柄ごとのSeries</returns>
		public Series[] SetAveragePriceMove(params AnnualData<decimal>[][] apms)
		{
			// クリア
			Series.Clear();
			ChartAreas[0].AxisX.CustomLabels.Clear();

			// Series作成
			Series[] series = new Series[apms.Length];
			for (int i = 0; i < series.Length; i++)
			{
				series[i] = GenerateSeries();
				Series.Add(series[i]);
			}

			// データが１つも無い場合
			if (apms == null) return series;

			// プロット
			DateTime dt = new DateTime(2020, 1, 1); // 閏年が必要
			// 日付ごとに処理
			while (dt < new DateTime(2021, 1, 1))
			{
				// 銘柄ごとに処理
				for (int i = 0; i < apms.Length; i++)
				{
					// 月日で検索
					AnnualData<decimal> apm = null;
					foreach (AnnualData<decimal> temp in apms[i])
					{
						if (temp.Month == dt.Month && temp.Day == dt.Day)
						{
							apm = temp;
							break;
						}
					}

					// プロット
					if (apm != null)
					{
						DataPoint dp = new DataPoint(dt.DayOfYear, (double)apm.Data * 100);
						series[i].Points.Add(dp);
					}
				}

				// CustomLabel
				if (dt.AddDays(-1).Month != dt.Month) ChartAreas[0].AxisX.CustomLabels.Add(GetCustomLabelX(dt));

				// 翌日へ
				dt = dt.AddDays(1);
			}

			// Y軸設定更新
			UpdateAxisYSettings();

			return series.ToArray();
		}

		/// <summary>
		/// Seriesを生成します。
		/// </summary>
		/// <returns>Series</returns>
		private Series GenerateSeries()
		{
			Series series = new Series();
			series.ChartType = SeriesChartType.Line;
			series.YAxisType = AxisType.Secondary;
			return series;
		}

		/// <summary>
		/// Y軸設定を更新します。
		/// </summary>
		private void UpdateAxisYSettings()
		{
			// クリア
			ChartAreas[0].AxisY2.CustomLabels.Clear();

			// Y値取得
			List<double> values = new List<double>();
			foreach (Series series in Series)
			{
				double[][] ysArray = series.Points.Select(point => point.YValues).ToArray();
				foreach (double[] ys in ysArray)
				{
					values.AddRange(ys);
				}
			}

			// Y値が無い場合
			if (values.Count == 0) return;

			// サイズと位置
			double max = values.Max();
			double min = values.Min();
			double range = max - min;
			double height = range * (12.0 / 10.0);
			ChartAreas[0].AxisY2.ScaleView.Size = height;
			ChartAreas[0].AxisY2.ScaleView.Position = min - (height / 10.0);

			// CustomeLabel
			int interval = (int)(height / 5);
			if (interval == 0) interval = 1;
			// マイナス側
			for (double y = 100.0 - interval; y > ChartAreas[0].AxisY2.ScaleView.Position; y -= interval)
			{
				ChartAreas[0].AxisY2.CustomLabels.Add(GetCustomLabelY(y));
			}
			// プラス側
			for (double y = 100.0; y < ChartAreas[0].AxisY2.ScaleView.Position + (double)height; y += interval)
			{
				ChartAreas[0].AxisY2.CustomLabels.Add(GetCustomLabelY(y));
			}
		}

		/// <summary>
		/// X軸のCustomLabelを取得します。
		/// </summary>
		/// <param name="dt">日時</param>
		/// <returns>X軸のCustomLabel</returns>
		private CustomLabel GetCustomLabelX(DateTime dt)
		{
			return new CustomLabel(dt.DayOfYear - 50.0, dt.DayOfYear + 50.0, dt.Month.ToString() + "/" + dt.Day.ToString(), 0
				, LabelMarkStyle.None, GridTickTypes.Gridline);
		}

		/// <summary>
		/// Y軸のCustomLabelを取得します。
		/// </summary>
		/// <param name="y">Y値</param>
		/// <returns>Y軸のCustomLabel</returns>
		private CustomLabel GetCustomLabelY(double y)
		{
			return new CustomLabel(y - 1, y + 1, y.ToString("#.00"), 0, LabelMarkStyle.None, GridTickTypes.Gridline);
		}
	}
}
