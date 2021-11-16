using MagicalNuts.Indicator;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.UI.TradingChart.Plotter
{
	/// <summary>
	/// ボリンジャーバンドインジケーターのプロッター用拡張を表します。
	/// </summary>
	public class BollingerBandIndicatorEx : BollingerBandIndicator
	{
		/// <summary>
		/// 移動平均線の色を取得または設定します。
		/// </summary>
		[Category("プロット")]
		[DisplayName("移動平均線の色")]
		[Description("移動平均線の色を設定します。")]
		public Color MaColor { get; set; } = Color.FromArgb(144, 30, 38);

		/// <summary>
		/// ボリンジャーバンドの色を取得または設定します。
		/// </summary>
		[Category("プロット")]
		[DisplayName("ボリンジャーバンドの色")]
		[Description("ボリンジャーバンドの色を設定します。")]
		public Color BandColor { get; set; } = Color.FromArgb(0, 133, 131);

		/// <summary>
		/// ボリンジャーバンドのアルファ値を取得または設定します。
		/// </summary>
		[Category("プロット")]
		[DisplayName("ボリンジャーバンドのアルファ値")]
		[Description("ボリンジャーバンドのアルファ値を設定します。")]
		public int BandAlpha { get; set; } = 10;

		/// <summary>
		/// 高速モードかどうかを取得または設定します。
		/// </summary>
		[Category("プロット")]
		[DisplayName("高速モード")]
		[Description("高速モードかどうかを設定します。")]
		public bool FastMode { get; set; } = true;
	}

	/// <summary>
	/// ボリンジャーバンドのプロッターを表します。
	/// </summary>
	public class BollingerBandPlotter : IndicatorPlotter<BollingerBandIndicatorEx>
	{
		/// <summary>
		/// Series
		/// </summary>
		private Series[] Series = null;

		/// <summary>
		/// BollingerBandPlotterクラスの新しいインスタンスを初期化します。
		/// </summary>
		public BollingerBandPlotter()
		{
			Series = new Series[4];
			for (int i = 0; i < Series.Length; i++)
			{
				Series[i] = new Series();
				Series[i].YAxisType = AxisType.Secondary;
				switch (i)
				{
					case 0:
						Series[i].ChartType = SeriesChartType.Line;
						break;
					case 1:
						Series[i].ChartType = SeriesChartType.Range;
						break;
					default:
						Series[i].ChartType = SeriesChartType.Line;
						break;
				}
			}
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		public override string Name => "ボリンジャーバンド";

		/// <summary>
		/// プロパティを取得します。
		/// </summary>
		public override object Properties => Indicator;

		/// <summary>
		/// Seriesの配列を取得します。
		/// </summary>
		public override Series[] SeriesArray => Series;

		/// <summary>
		/// インジケーターをプロットします。
		/// </summary>
		/// <param name="candles">インジケーター用ロウソク足の集合</param>
		public override void PlotIndicator(IndicatorCandleCollection candles)
		{
			for (int x = 0; x < candles.Count; x++)
			{
				decimal[] data = Indicator.GetValues(GetCandleCollection(x));
				if (data == null) continue;

				Series[0].Points.Add(new DataPoint(x, (double)data[0]));
				Series[1].Points.Add(new DataPoint(x, new double[] { (double)data[1], (double)data[2] }));
				Series[2].Points.Add(new DataPoint(x, (double)data[1]));
				Series[3].Points.Add(new DataPoint(x, (double)data[2]));
			}
		}

		/// <summary>
		/// ChartAreaを設定します。
		/// </summary>
		/// <param name="mainChartArea">主ChartArea</param>
		/// <returns>使用する従ChartAreaの配列</returns>
		public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
		{
			foreach (Series series in Series)
			{
				series.ChartArea = mainChartArea.Name;
			}
			return null;
		}

		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public override async Task SetUpAsync()
		{
			BollingerBandIndicatorEx properties = (BollingerBandIndicatorEx)Properties;

			// 移動平均
			Indicator.MovingAverageIndicator.Period = properties.MovingAverageIndicator.Period;

			// 色
			for (int i = 0; i < Series.Length; i++)
			{
				switch (i)
				{
					case 0:
						Series[0].Color = properties.MaColor;
						break;
					case 1:
						Series[1].Color = Color.FromArgb(properties.BandAlpha, properties.BandColor);
						break;
					default:
						Series[i].Color = properties.BandColor;
						break;
				}
			}

			// 高速モード
			Series[1].Enabled = !properties.FastMode;

			await base.SetUpAsync();
		}
	}
}
