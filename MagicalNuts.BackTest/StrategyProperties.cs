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
	}
}
