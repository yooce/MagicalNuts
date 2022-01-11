using MagicalNuts.Primitive;
using System;
using System.Collections.Generic;

namespace MagicalNuts.BackTest
{
	/// <summary>
	/// バックテスト用ロウソク足の集合を表します。
	/// </summary>
	public class BackTestCandleCollection : StrategyCandleCollection
	{
		/// <summary>
		/// インデックスの辞書
		/// </summary>
		private Dictionary<DateTime, int> IndexDictionary = null;

		/// <summary>
		/// BackTestCandleCollectionクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="stock">銘柄情報</param>
		/// <param name="candles">ロウソク足の集合</param>
		/// <param name="period">期間</param>
		/// <param name="unit">期間単位</param>
		public BackTestCandleCollection(List<Candle> candles, Stock stock, int period = 1, PeriodUnit unit = PeriodUnit.Day) : base(candles, stock, period, unit)
		{
			// インデックスの辞書作成
			IndexDictionary = new Dictionary<DateTime, int>();
			for (int i = 0; i < Count; i++)
			{
				IndexDictionary.Add(this[i].DateTime, i);
			}
		}

		/// <summary>
		/// 指定日時までシフトされたバックテスト用ロウソク足の集合を取得します。
		/// </summary>
		/// <param name="dt">日時</param>
		/// <returns>指定日時までシフトされたバックテスト用ロウソク足の集合</returns>
		public StrategyCandleCollection GetShiftedCandles(DateTime dt)
		{
			// dtと一致するインデックス取得
			int? idx = null;
			if (IndexDictionary.ContainsKey(dt)) idx = IndexDictionary[dt];

			// ロウソク足が無い
			if (idx == null) return null;

			// シフト
			return Shift(idx.Value);
		}

		/// <summary>
		/// 指定日時以前で一番近いロウソク足を取得します。
		/// </summary>
		/// <param name="dt">日時</param>
		/// <returns>指定日時以前で一番近いロウソク足</returns>
		public Candle GetLatestCandle(DateTime dt)
		{
			// dtのロウソク足が存在する場合
			if (IndexDictionary.ContainsKey(dt)) return this[IndexDictionary[dt]];

			// 指定日時以前で一番近いロウソク足を検索
			Candle candle = null;
			for (int i = 0; i < Count; i++)
			{
				if (this[i].DateTime <= dt)
				{
					candle = this[i];
					break;
				}
			}

			return candle;
		}
	}
}
