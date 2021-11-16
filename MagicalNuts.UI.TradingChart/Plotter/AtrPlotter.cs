using MagicalNuts.Indicator;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.UI.TradingChart.Plotter
{
	/// <summary>
	/// ATRインジケーターのプロッター用拡張を表します。
	/// </summary>
	public class AtrIndicatorEx : AtrIndicator
	{
		/// <summary>
		/// 小数点以下の桁数を設定または取得します。
		/// </summary>
		[Category("プロット")]
		[DisplayName("小数点以下の桁数")]
		[Description("小数点以下の桁数を設定します。")]
		public int Digits { get; set; } = 2;

		/// <summary>
		/// 色を設定または取得します。
		/// </summary>
		[Category("プロット")]
		[DisplayName("色")]
		[Description("色を設定します。")]
		public Color Color { get; set; } = Color.FromArgb(163, 9, 27);
	}

	/// <summary>
	/// ATRのプロッターを表します。
	/// </summary>
	[SupportedOSPlatform("windows")]
	public class AtrPlotter : IndicatorPlotter<AtrIndicatorEx>
	{
		/// <summary>
		/// Series
		/// </summary>
		private Series Series = null;

		/// <summary>
		/// ChartArea
		/// </summary>
		private ChartArea ChartArea = null;

		/// <summary>
		/// AtrPlotterクラスの新しいインスタンスを初期化します。
		/// </summary>
		public AtrPlotter()
		{
			Series = new Series();
			Series.ChartType = SeriesChartType.Line;
			Series.YAxisType = AxisType.Secondary;
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		public override string Name => "ATR";

		/// <summary>
		/// Seriesの配列を取得します。
		/// </summary>
		public override Series[] SeriesArray => new Series[] { Series };

		/// <summary>
		/// プロパティを取得します。
		/// </summary>
		public override object Properties => Indicator;

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

				Series.Points.Add(new DataPoint(x, ConvertDecimalToDoubleArray(data)));
			}
		}

		/// <summary>
		/// ChartAreaを設定します。
		/// </summary>
		/// <param name="mainChartArea">主ChartArea</param>
		/// <returns>使用する従ChartAreaの配列</returns>
		public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
		{
			SubChartArea subChartArea = new SubChartArea();
			subChartArea.AxisY2.LabelStyle.Format = PriceFormatter.GetPriceFormatFromDigits(((AtrIndicatorEx)Properties).Digits);
			Series.ChartArea = subChartArea.Name;
			ChartArea = subChartArea;
			return new SubChartArea[] { subChartArea };
		}

		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public override async Task SetUpAsync()
		{
			Series.Color = ((AtrIndicatorEx)Properties).Color;
			if (ChartArea != null)
				ChartArea.AxisY2.LabelStyle.Format = PriceFormatter.GetPriceFormatFromDigits(((AtrIndicatorEx)Properties).Digits);

			await base.SetUpAsync();
		}
	}
}
