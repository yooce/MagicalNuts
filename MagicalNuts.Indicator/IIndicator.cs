using System.Threading.Tasks;

namespace MagicalNuts.Indicator
{
	/// <summary>
	/// インジケーターのインターフェースを表します。
	/// </summary>
	public interface IIndicator
	{
		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		Task SetUpAsync();

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="candles">ロウソク足のコレクション</param>
		/// <returns>値</returns>
		decimal[] GetValues(IndicatorCandleCollection candles);
	}
}
