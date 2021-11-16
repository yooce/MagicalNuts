using System;
using System.Collections.Generic;

namespace MagicalNuts.BackTest
{
	/// <summary>
	/// 為替ストアを表します。
	/// </summary>
	public class CurrencyStore
	{
		/// <summary>
		/// 為替のバックテスト用ロウソク足の集合の辞書
		/// </summary>
		private Dictionary<int, BackTestCandleCollection> CandlesDicttionary = null;

		/// <summary>
		/// CurrencyStoreクラスの新しいインスタンスを初期化します。
		/// </summary>
		public CurrencyStore()
		{
			CandlesDicttionary = new Dictionary<int, BackTestCandleCollection>();
		}

		/// <summary>
		/// 為替のバックテスト用ロウソク足の集合を追加します。
		/// </summary>
		/// <param name="market">市場の種類</param>
		/// <param name="candles">為替のバックテスト用ロウソク足の集合</param>
		public void Add(int market, BackTestCandleCollection candles)
		{
			CandlesDicttionary.Add(market, candles);
		}

		/// <summary>
		/// 為替の価格を取得します。
		/// </summary>
		/// <param name="market">市場の種類</param>
		/// <param name="dt">日時</param>
		/// <returns>為替の価格</returns>
		public decimal GetPrice(int market, DateTime dt)
		{
			// 指定された市場の為替が無い場合は同通貨とみなす
			if (!CandlesDicttionary.ContainsKey(market)) return 1;

			// 前日以前で一番近いロウソク足を取得
			Primitive.Candle candle = CandlesDicttionary[market].GetLatestCandle(dt.AddDays(-1));

			// ロウソク足が無い場合は１（ほぼエラーだが取り回しにくくなるので）
			if (candle == null) return 1;

			return candle.Close;
		}
	}
}
