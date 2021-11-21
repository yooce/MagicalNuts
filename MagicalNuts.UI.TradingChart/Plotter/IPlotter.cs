using MagicalNuts.Primitive;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.UI.TradingChart.Plotter
{
	/// <summary>
	/// プロッターのインターフェースを表します。
	/// </summary>
	public interface IPlotter : IPropertyHolder
	{
		/// <summary>
		/// Seriesの配列を取得します。
		/// </summary>
		Series[] SeriesArray { get; }

		/// <summary>
		/// ChartAreaを設定します。
		/// </summary>
		/// <param name="mainChartArea">主ChartArea</param>
		/// <returns>使用する従ChartAreaの配列</returns>
		SubChartArea[] SetChartArea(MainChartArea mainChartArea);

		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		Task SetUpAsync();

		/// <summary>
		/// データをプロットします。
		/// </summary>
		/// <param name="code">銘柄コード</param>
		/// <param name="candles">ロウソク足のリスト</param>
		void Plot(string code, List<Candle> candles);
	}
}
