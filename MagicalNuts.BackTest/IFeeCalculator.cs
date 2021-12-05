using MagicalNuts.Primitive;

namespace MagicalNuts.BackTest
{
	/// <summary>
	/// 手数料計算機のインターフェイスを表します。
	/// </summary>
	public interface IFeeCalculator : IPropertyHolder
	{
		/// <summary>
		/// エントリー時の手数料を取得します。
		/// </summary>
		/// <param name="state">バックテストの状態</param>
		/// <param name="price">エントリー価格</param>
		/// <param name="lots">エントリーロット数</param>
		/// <param name="currency">エントリー時の為替</param>
		/// <returns>エントリー時の手数料</returns>
		decimal GetEntryFee(BackTestStatus state, decimal price, decimal lots, decimal currency);

		/// <summary>
		/// イグジット時の手数料を取得します。
		/// </summary>
		/// <param name="state">バックテストの状態</param>
		/// <param name="price">イグジット価格</param>
		/// <param name="lots">イグジットロット数</param>
		/// <param name="currency">エントリー時の為替</param>
		/// <returns>イグジット時の手数料</returns>
		decimal GetExitFee(BackTestStatus state, decimal price, decimal lots, decimal currency);

		/// <summary>
		/// 月次手数料を取得します。
		/// </summary>
		/// <param name="state">バックテストの状態</param>
		/// <returns>月次手数料</returns>
		decimal GetMonthlyFee(BackTestStatus state);

		/// <summary>
		/// 年次手数料を取得します。
		/// </summary>
		/// <param name="state">バックテストの状態</param>
		/// <returns>年次手数料</returns>
		decimal GetYearlyFee(BackTestStatus state);
	}
}
