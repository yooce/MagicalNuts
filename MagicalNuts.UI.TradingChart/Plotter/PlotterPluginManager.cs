using MagicalNuts.Primitive;
using System;

namespace MagicalNuts.UI.TradingChart.Plotter
{
	/// <summary>
	/// プロッターのプラグイン管理を表します。
	/// </summary>
	public class PlotterPluginManager : PluginManager<IPlotter>
	{
		/// <summary>
		/// PlotterManagerクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="plugin_path">プラグインのパス</param>
		public PlotterPluginManager(string plugin_path) : base(plugin_path)
		{
		}

		/// <summary>
		/// 除外する型かどうか判定します。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>除外する型かどうか</returns>
		protected override bool IsExclude(Type type)
		{
			return type.FullName == "MagicalNuts.TradingChart.Plotter.CandlePlotter"
				|| type.FullName == "MagicalNuts.TradingChart.Plotter.VolumePlotter";
		}
	}
}
