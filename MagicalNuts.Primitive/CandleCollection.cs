using System.Collections.Generic;

namespace MagicalNuts.Primitive
{
	/// <summary>
	/// 期間単位
	/// </summary>
	public enum PeriodUnit
	{
		Second = 0, Minute, Hour, Day, Week, Month, Year
	}

	/// <summary>
	/// ロウソク足の集合を表します。
	/// </summary>
	/// <typeparam name="T">付加情報の型</typeparam>
	public class CandleCollection<T> : List<Candle>
	{
		/// <summary>
		/// 期間
		/// </summary>
		public int Period { get; }

		/// <summary>
		/// 期間単位
		/// </summary>
		public PeriodUnit PeriodUnit { get; }

		/// <summary>
		/// 付加情報
		/// </summary>
		public T Additional { get; }

		/// <summary>
		/// CandleCollectionクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="candles">ロウソク足のリスト</param>
		/// <param name="add">付加情報</param>
		/// <param name="period">期間</param>
		/// <param name="unit">期間単位</param>
		public CandleCollection(List<Candle> candles, T add, int period = 1, PeriodUnit unit = PeriodUnit.Day)
		{
			Clear();
			AddRange(candles);
			Additional = add;
			Period = period;
			PeriodUnit = unit;
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
