using MagicalNuts.BackTest;
using MagicalNuts.UI.Base;
using System.Runtime.Versioning;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.UI.BackTest
{
	/// <summary>
	/// 資産履歴のチャートを表します。
	/// </summary>
	[SupportedOSPlatform("windows")]
	public class HistoricalAssetsChart : Chart
	{
		/// <summary>
		/// HistoricalAssetsChartクラスの新しいインスタンスを初期化します。
		/// </summary>
		public HistoricalAssetsChart()
		{
			// ChartArea
			ChartAreas.Add(new ChartArea());

			// X軸
			ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
			ChartAreas[0].AxisX.MajorGrid.LineColor = ChartPalette.GridColor;

			// Y軸
			ChartAreas[0].AxisY.MajorGrid.LineColor = ChartPalette.GridColor;
			ChartAreas[0].AxisY.ScrollBar.Enabled = false;

			// 凡例
			Legend legend = new Legend();
			legend.Font = new Font("Yu Gothic UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
			Legends.Add(legend);
		}

		/// <summary>
		/// 資産履歴の配列を設定します。
		/// </summary>
		/// <param name="has">資産履歴の配列</param>
		public void SetHistoricalAssetsList(HistoricalAssets[] has)
		{
			// クリア
			Series.Clear();
			ChartAreas[0].AxisX.CustomLabels.Clear();

			// Series作成
			Series book_series = new Series();
			book_series.ChartType = SeriesChartType.Line;
			book_series.Color = ColorTranslator.FromHtml("#418CF0");
			book_series.Name = "簿価資産";
			Series market_series = new Series();
			market_series.ChartType = SeriesChartType.Line;
			market_series.Color = ColorTranslator.FromHtml("#FCB441");
			market_series.Name = "時価資産";
			Series invest_series = new Series();
			invest_series.ChartType = SeriesChartType.Line;
			invest_series.Color = ColorTranslator.FromHtml("#E0830A");
			invest_series.Name = "投資資金";

			// プロット
			for (int x = 0; x < has.Length; x++)
			{
				book_series.Points.AddXY(x, has[x].BookAssets);
				market_series.Points.AddXY(x, has[x].MarketAssets);
				invest_series.Points.AddXY(x, has[x].InvestmentAmount);
			}

			// Series追加
			Series.Add(market_series);
			Series.Add(invest_series);
			Series.Add(book_series);

			// CustomLabel
			DateTime? prevLabelDateTime = null;
			for (int x = 0; x < has.Length; x++)
			{
				if (prevLabelDateTime == null || prevLabelDateTime.Value.Year != has[x].DateTime.Year)
				{
					ChartAreas[0].AxisX.CustomLabels.Add(new CustomLabel(
						x - 50.0, x + 50.0, has[x].DateTime.ToString("yyyy"), 0, LabelMarkStyle.None, GridTickTypes.Gridline));
					prevLabelDateTime = has[x].DateTime;
				}
			}
		}
	}
}
