namespace MagicalNuts.BackTest
{
	/// <summary>
	/// 手数料なしの手数料計算機を表します。
	/// </summary>
	public class FeeCalculatorNone : IFeeCalculator
	{
		/// <summary>
		/// 手数料計算機名
		/// </summary>
		public string Name => "手数料なし";

		/// <summary>
		/// 手数料計算機のプロパティ
		/// </summary>
		public object Properties => null;

		/// <summary>
		/// エントリー時の手数料を取得します。
		/// </summary>
		/// <param name="state">バックテストの状態</param>
		/// <param name="price">エントリー価格</param>
		/// <param name="lots">エントリーロット数</param>
		/// <param name="currency">エントリー時の為替</param>
		/// <returns>エントリー時の手数料</returns>
		public decimal GetEntryFee(BackTestStatus state, decimal price, decimal lots, decimal currency)
		{
			return 0;
		}

		/// <summary>
		/// イグジット時の手数料を取得します。
		/// </summary>
		/// <param name="state">バックテストの状態</param>
		/// <param name="price">イグジット価格</param>
		/// <param name="lots">イグジットロット数</param>
		/// <param name="currency">エントリー時の為替</param>
		/// <returns>イグジット時の手数料</returns>
		public decimal GetExitFee(BackTestStatus state, decimal price, decimal lots, decimal currency)
		{
			return 0;
		}

		/// <summary>
		/// 月次手数料を取得します。
		/// </summary>
		/// <param name="state">バックテストの状態</param>
		/// <returns>月次手数料</returns>
		public decimal GetMonthlyFee(BackTestStatus state)
		{
			return 0;
		}

		/// <summary>
		/// 年次手数料を取得します。
		/// </summary>
		/// <param name="state">バックテストの状態</param>
		/// <returns>年次手数料</returns>
		public decimal GetYearlyFee(BackTestStatus state)
		{
			return 0;
		}
	}
}
