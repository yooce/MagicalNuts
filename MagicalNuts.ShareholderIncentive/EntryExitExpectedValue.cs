using System.ComponentModel;

namespace MagicalNuts.ShareholderIncentive
{
	/// <summary>
	/// エントリー日とイグジット日による期待値を表します。
	/// </summary>
	public class EntryExitExpectedValue
	{
		/// <summary>
		/// 銘柄コード
		/// </summary>
		[DisplayName("コード")]
		public string Code { get; set; }

		/// <summary>
		/// 銘柄名
		/// </summary>
		[DisplayName("銘柄名")]
		public string Name { get; set; }

		/// <summary>
		/// エントリーした株主優待権利付き最終日前の日数
		/// </summary>
		[DisplayName("エントリー")]
		public int EntryDaysBefore { get; set; }

		/// <summary>
		/// イグジットした株主優待権利付き最終日前の日数
		/// </summary>
		[DisplayName("イグジット")]
		public int ExitDaysBefore { get; set; }

		/// <summary>
		/// 勝率
		/// </summary>
		[DisplayName("勝率")]
		public decimal WinRatio { get; set; }

		/// <summary>
		/// 平均損益
		/// </summary>
		[DisplayName("平均収益")]
		public decimal AverageProfit { get; set; }

		/// <summary>
		/// 平均損失
		/// </summary>
		[DisplayName("平均損失")]
		public decimal AverageLoss { get; set; }

		/// <summary>
		/// 期待値
		/// </summary>
		[DisplayName("期待値")]
		public decimal ExpectedValue { get; set; }

		/// <summary>
		/// 年率リターン
		/// </summary>
		[DisplayName("年率リターン")]
		public decimal AnnualReturn { get; set; }

		/// <summary>
		/// ソート用に２つのエントリー日とイグジット日による期待値を比較します。
		/// </summary>
		/// <param name="a">エントリー日とイグジット日による期待値</param>
		/// <param name="b">エントリー日とイグジット日による期待値</param>
		/// <returns>期待値がaの方が高ければ-1、同じなら0、aの方が低ければ1</returns>
		public static int Compare(EntryExitExpectedValue a, EntryExitExpectedValue b)
		{
			if (a.ExpectedValue > b.ExpectedValue) return -1;
			if (a.ExpectedValue == b.ExpectedValue) return 0;
			return 1;
		}
	}
}
