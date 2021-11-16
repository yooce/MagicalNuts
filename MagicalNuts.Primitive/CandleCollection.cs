using System.Collections.Generic;

namespace MagicalNuts.Primitive
{
	/// <summary>
	/// ロウソク足の集合を表します。
	/// </summary>
	/// <typeparam name="T">付加情報の型</typeparam>
	public class CandleCollection<T> : List<Candle>
	{
		/// <summary>
		/// 付加情報
		/// </summary>
		public T Additional { get; }

		/// <summary>
		/// CandleCollectionクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="candles">ロウソク足のリスト</param>
		/// <param name="add">付加情報</param>
		public CandleCollection(List<Candle> candles, T add)
		{
			Clear();
			AddRange(candles);
			Additional = add;
		}

		/// <summary>
		/// ロウソク足のインデックスをずらします。
		/// </summary>
		/// <param name="i">ずらす個数</param>
		/// <returns>インデックスをずらしたロウソク足の集合</returns>
		public CandleCollection<T> Shift(int i)
		{
			return new CandleCollection<T>(GetRange(i, Count - i), Additional);
		}
	}
}
