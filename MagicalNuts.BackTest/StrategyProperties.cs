using System.ComponentModel;

namespace MagicalNuts.BackTest
{
	/// <summary>
	/// 戦略のプロパティを表します。
	/// </summary>
	public class StrategyProperties
	{
		/// <summary>
		/// 初期資産
		/// </summary>
		[Category("バックテスト")]
		[DisplayName("初期資産")]
		[Description("初期資産を設定します。")]
		public decimal InitialAssets { get; set; } = 3000000;

		/// <summary>
		/// レバレッジ
		/// </summary>
		[Category("バックテスト")]
		[DisplayName("レバレッジ")]
		[Description("レバレッジを設定します。")]
		public decimal Leverage { get; set; } = 1;
	}
}
