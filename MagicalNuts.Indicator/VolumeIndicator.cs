using System.Threading.Tasks;

namespace MagicalNuts.Indicator
{
	/// <summary>
	/// 出来高インジケーターを表します。
	/// </summary>
	public class VolumeIndicator : IIndicator
	{
		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public async Task SetUpAsync()
		{
		}

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="candles">ロウソク足のコレクション</param>
		/// <returns>値</returns>
		public decimal[] GetValues(IndicatorCandleCollection candles)
		{
			return new decimal[] { candles[0].Volume };
		}
	}
}
