using MagicalNuts.Primitive;
using System.Collections.Generic;

namespace MagicalNuts.Indicator
{
	/// <summary>
	/// インジケーター用ロウソク足の集合を表します。
	/// </summary>
	public class IndicatorCandleCollection : CandleCollection<string>
	{
		/// <summary>
		/// 銘柄コード
		/// </summary>
		public string Code => Additional;

		/// <summary>
		/// IndicatorCandleCollectionクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="candles">ロウソク足のリスト</param>
		/// <param name="code">銘柄コード</param>
		public IndicatorCandleCollection(List<Candle> candles, string code) : base(candles, code)
		{
		}

		/// <summary>
		/// ロウソク足のインデックスをずらします。
		/// </summary>
		/// <param name="i">ずらす個数</param>
		/// <returns>インデックスをずらしたロウソク足の集合</returns>
		public new IndicatorCandleCollection Shift(int i)
		{
			return new IndicatorCandleCollection(GetRange(i, Count - i), Code);
		}
	}
}
