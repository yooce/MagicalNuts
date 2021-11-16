using MagicalNuts.ShareholderIncentive;
using MagicalNuts.UI.Base;
using System.Runtime.Versioning;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.UI.ShareholderIncentive
{
	/// <summary>
	/// 株主優待権利付き最終日前後の値動きのチャートを表します。
	/// </summary>
	[SupportedOSPlatform("windows")]
	public class LastDayPriceMoveChart : Chart
	{
		/// <summary>
		/// LastDayPriceMoveChartクラスの新しいインスタンスを初期化します。
		/// </summary>
		public LastDayPriceMoveChart()
		{
			// ChartArea
			ChartAreas.Add(new ChartArea());

			// Y軸設定
			ChartAreas[0].AxisY.IsStartedFromZero = false;
			ChartAreas[0].AxisY.MajorGrid.LineColor = ChartPalette.GridColor;
			ChartAreas[0].AxisY.Crossing = 100.0;
		}

		/// <summary>
		/// 株主優待権利付き最終日前後の値動きのヒストリカルデータを設定します。
		/// </summary>
		/// <param name="hd">株主優待権利付き最終日前後の値動きのヒストリカルデータ</param>
		/// <returns>平均のSeriesと年間データのSeries</returns>
		public (Series, Series[]) SetHistoricalData(HistoricalData hd)
		{
			// 年数と日数を取得
			int years = hd.AnnualDataList.Count;
			int days = hd.BeforeNum + hd.AfterNum + 1;

			// 平均
			Series series_avg = GenerateSeries();
			for (int x = 0; x < days; x++)
			{
				decimal? d = hd.AverageAnnualData[x - hd.BeforeNum];
				if (d == null) continue;

				DataPoint dp = new DataPoint(x, (double)(d.Value * 100));
				series_avg.Points.Add(dp);
			}

			// 年間データ
			Series[] series_years = new Series[years];
			for (int i = 0; i < years; i++)
			{
				series_years[i] = GenerateSeries();
				for (int x = 0; x < days; x++)
				{
					decimal? d = hd.AnnualDataList[i][x - hd.BeforeNum];
					if (d == null) continue;

					DataPoint dp = new DataPoint(x, (double)(d.Value * 100));
					series_years[i].Points.Add(dp);
				}
			}

			// ガイド線
			for (int x = 0; x < days; x++)
			{
				if ((x - hd.BeforeNum) % 10 == 0)
				{
					CustomLabel cl = new CustomLabel(x - 50.0, x + 50.0, (x - hd.BeforeNum).ToString(), 0, LabelMarkStyle.None
						, GridTickTypes.TickMark);
					ChartAreas[0].AxisX.CustomLabels.Add(cl);
				}
			}

			// 権利付き最終日を表す線
			CustomLabel last_cl = new CustomLabel(hd.BeforeNum - 50.0, hd.BeforeNum + 50.0, "", 0, LabelMarkStyle.None
				, GridTickTypes.Gridline);
			ChartAreas[0].AxisX.CustomLabels.Add(last_cl);

			// 平均だけチャートに表示
			Series.Add(series_avg);

			return (series_avg, series_years);
		}

		/// <summary>
		/// Seriesを作成します。
		/// </summary>
		/// <returns>Series</returns>
		private Series GenerateSeries()
		{
			Series series = new Series();
			series.ChartType = SeriesChartType.Line;
			return series;
		}
	}
}
