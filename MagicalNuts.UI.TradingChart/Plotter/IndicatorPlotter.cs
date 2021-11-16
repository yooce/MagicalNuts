using MagicalNuts.Indicator;
using MagicalNuts.Primitive;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.UI.TradingChart.Plotter
{
	/// <summary>
	/// インジケーターのプロッターを表します。
	/// </summary>
	/// <typeparam name="T">インジケーターの型を指定します。</typeparam>
	public abstract class IndicatorPlotter<T> : IPlotter where T : IIndicator, new()
	{
		/// <summary>
		/// インジケーター
		/// </summary>
		public T Indicator { get; private set; }

		/// <summary>
		/// インジケーター用ロウソク足のコレクション
		/// </summary>
		private IndicatorCandleCollection Candles = null;

		/// <summary>
		/// IndicatorPlotterの新しいインスタンスを初期化します。
		/// </summary>
		public IndicatorPlotter()
		{
			Indicator = new T();
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// プロパティを取得します。
		/// </summary>
		public virtual object Properties => null;

		/// <summary>
		/// ChartAreaを設定します。
		/// </summary>
		/// <param name="mainChartArea">主ChartArea</param>
		/// <returns>使用する従ChartAreaの配列</returns>
		public abstract SubChartArea[] SetChartArea(MainChartArea mainChartArea);

		/// <summary>
		/// Seriesの配列を取得します。
		/// </summary>
		public abstract Series[] SeriesArray { get; }

		/// <summary>
		/// データをプロットします。
		/// </summary>
		/// <param name="code">銘柄コード</param>
		/// <param name="candles">ロウソク足のリスト</param>
		public void Plot(string code, List<Candle> candles)
		{
			// 降順に変換
			List<Candle> reversed = new List<Candle>();
			reversed.AddRange(candles);
			reversed.Reverse();
			Candles = new IndicatorCandleCollection(reversed, code);

			// インジケーターをプロット
			PlotIndicator(Candles);
		}

		/// <summary>
		/// インジケーターをプロットします。
		/// </summary>
		/// <param name="candles">インジケーター用ロウソク足のリスト</param>
		public abstract void PlotIndicator(IndicatorCandleCollection candles);

		/// <summary>
		/// x座標からインジケーター用ロウソク足のコレクションを取得します。
		/// </summary>
		/// <param name="x">x座標</param>
		/// <returns>インジケーター用ロウソク足のコレクション</returns>
		protected IndicatorCandleCollection GetCandleCollection(int x)
		{
			return new IndicatorCandleCollection(Candles.Shift(Candles.Count - x - 1), Candles.Code);
		}

		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public virtual async Task SetUpAsync()
		{
			await Indicator.SetUpAsync();
		}

		/// <summary>
		/// decimalの配列をdoubleの配列に変換します。
		/// </summary>
		/// <param name="decimals">decimalの配列</param>
		/// <returns>doubleの配列</returns>
		protected static double[] ConvertDecimalToDoubleArray(decimal[] decimals)
		{
			return decimals.Select(d => (double)d).ToArray();
		}
	}
}
