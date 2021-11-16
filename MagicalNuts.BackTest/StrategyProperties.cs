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
		[DisplayName("当初資産")]
		[Description("当初資産を設定します。")]
		public decimal InitialAssets { get; set; } = 3000000;
	}
}
