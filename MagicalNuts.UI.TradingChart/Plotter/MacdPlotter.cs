using MagicalNuts.Indicator;
using MagicalNuts.UI.Base;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.UI.TradingChart.Plotter
{
	/// <summary>
	/// MACDインジケーターのプロッター用拡張を表します。
	/// </summary>
	public class MacdIndicatorEx : MacdIndicator
	{
		/// <summary>
		/// 小数点以下の桁数を設定または取得します。
		/// </summary>
		[Category("プロット")]
		[DisplayName("小数点以下の桁数")]
		[Description("小数点以下の桁数を設定します。")]
		public int Digits { get; set; } = 2;

		/// <summary>
		/// MACDの色を設定または取得します。
		/// </summary>
		[Category("プロット")]
		[DisplayName("MACDの色")]
		[Description("MACDの色を設定します。")]
		public Color MacdColor { get; set; } = Color.FromArgb(0, 143, 250);

		/// <summary>
		/// シグナルの色を設定または取得します。
		/// </summary>
		[Category("プロット")]
		[DisplayName("シグナルの色")]
		[Description("シグナルの色を設定します。")]
		public Color SignalColor { get; set; } = Color.FromArgb(255, 103, 36);

		/// <summary>
		/// オシレーターがプラスで増加時の色を設定または取得します。
		/// </summary>
		[Category("プロット")]
		[DisplayName("オシレーターがプラスで増加時の色")]
		[Description("オシレーターがプラスで増加時の色を設定します。")]
		public Color OscillatorPlusUpColor { get; set; } = ChartPalette.PriceUpColor;

		/// <summary>
		/// オシレーターがプラスで減少時の色を設定または取得します。
		/// </summary>
		[Category("プロット")]
		[DisplayName("オシレーターがプラスで減少時の色")]
		[Description("オシレーターがプラスで減少時の色を設定します。")]
		public Color OscillatorPlusDownColor { get; set; } = Color.FromArgb(169, 224, 219);

		/// <summary>
		/// オシレーターがマイナスで増加時の色を設定または取得します。
		/// </summary>
		[Category("プロット")]
		[DisplayName("オシレーターがマイナスで増加時の色")]
		[Description("オシレーターがマイナスで増加時の色を設定します。")]
		public Color OscillatorMinusUpColor { get; set; } = Color.FromArgb(255, 204, 210);

		/// <summary>
		/// オシレーターがマイナスで減少時の色を設定または取得します。
		/// </summary>
		[Category("プロット")]
		[DisplayName("オシレーターがマイナスで減少時の色")]
		[Description("オシレーターがマイナスで減少時の色を設定します。")]
		public Color OscillatorMinusDownColor { get; set; } = ChartPalette.PriceDownColor;
	}

	/// <summary>
	/// MACDのプロッターを表します。
	/// </summary>
	[SupportedOSPlatform("windows")]
	public class MacdPlotter : IndicatorPlotter<MacdIndicatorEx>
	{
		/// <summary>
		/// Series
		/// </summary>
		private Series[] Series = null;

		/// <summary>
		/// ChartArea
		/// </summary>
		private ChartArea ChartArea = null;

		/// <summary>
		/// MacdPlotterの新しいインスタンスを初期化します。
		/// </summary>
		public MacdPlotter()
		{
			Series = new Series[3];
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
						Series[i].ChartType = SeriesChartType.Line;
						break;
					case 2:
						Series[i].ChartType = SeriesChartType.Column;
						break;
				}
			}
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		public override string Name => "MACD";

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

				// MACD
				Series[0].Points.Add(new DataPoint(x, (double)data[0]));

				if (data.Length > 1)
				{
					// MACDシグナル
					Series[1].Points.Add(new DataPoint(x, (double)data[1]));

					// MACDオシレーター
					Series[2].Points.Add(new DataPoint(x, (double)data[0] - (double)data[1]));
				}
			}

			// オシレーター色
			SetOscillatorColors();
		}

		private void SetOscillatorColors()
		{
			MacdIndicatorEx properties = (MacdIndicatorEx)Properties;

			DataPoint prevDp = null;
			foreach (DataPoint dp in Series[2].Points)
			{
				if (dp.YValues[0] >= 0)
				{
					if (prevDp != null && prevDp.YValues[0] > dp.YValues[0]) dp.Color = properties.OscillatorPlusDownColor;
					else dp.Color = properties.OscillatorPlusUpColor;
				}
				else
				{
					if (prevDp != null && prevDp.YValues[0] < dp.YValues[0]) dp.Color = properties.OscillatorMinusUpColor;
					else dp.Color = properties.OscillatorMinusDownColor;
				}
				prevDp = dp;
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
			subChartArea.AxisY2.LabelStyle.Format = PriceFormatter.GetPriceFormatFromDigits(((MacdIndicatorEx)Properties).Digits);
			foreach (Series series in Series)
			{
				series.ChartArea = subChartArea.Name;
			}
			ChartArea = subChartArea;
			return new SubChartArea[] { subChartArea };
		}

		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public override async Task SetUpAsync()
		{
			MacdIndicatorEx properties = (MacdIndicatorEx)Properties;

			// 移動平均
			Indicator.FastMaIndicator.Period = properties.FastMaIndicator.Period;
			Indicator.SlowMaIndicator.Period = properties.SlowMaIndicator.Period;

			// 色
			Series[0].Color = properties.MacdColor;
			Series[1].Color = properties.SignalColor;
			SetOscillatorColors();

			// 桁数
			if (ChartArea != null) ChartArea.AxisY2.LabelStyle.Format = PriceFormatter.GetPriceFormatFromDigits(properties.Digits);

			await base.SetUpAsync();
		}
	}
}
