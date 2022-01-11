using MagicalNuts.Indicator;
using MagicalNuts.Primitive;
using System.Collections.Generic;

namespace MagicalNuts.BackTest
{
	/// <summary>
	/// 戦略用ロウソク足の集合を表します。
	/// </summary>
	public class StrategyCandleCollection : CandleCollection<Stock>
	{
		/// <summary>
		/// 銘柄情報
		/// </summary>
		public Stock Stock => Additional;

		/// <summary>
		/// StrategyCandleCollectionクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="candles">ロウソク足のリスト</param>
		/// <param name="stock">銘柄情報</param>
		/// <param name="period">期間</param>
		/// <param name="unit">期間単位</param>
		public StrategyCandleCollection(List<Candle> candles, Stock stock, int period = 1, PeriodUnit unit = PeriodUnit.Day) : base(candles, stock, period, unit)
		{
		}

		/// <summary>
		/// ロウソク足のインデックスをずらします。
		/// </summary>
		/// <param name="i">ずらす個数</param>
		/// <returns>インデックスをずらしたロウソク足の集合</returns>
		public new StrategyCandleCollection Shift(int i)
		{
			return new StrategyCandleCollection(GetRange(i, Count - i), Additional);
		}

		/// <summary>
		/// インジケーター用ロウソク足の集合を返します。
		/// </summary>
		/// <returns>インジケーター用ロウソク足の集合</returns>
		public IndicatorCandleCollection GetIndicatorCandleCollection()
		{
			return new IndicatorCandleCollection(this, Stock.Code);
		}
	}
}
