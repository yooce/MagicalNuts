using MagicalNuts.Primitive;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MagicalNuts.Indicator
{
	/// <summary>
	/// ATRインジケーターを表します。
	/// </summary>
	public class AtrIndicator : IIndicator
	{
		/// <summary>
		/// 期間を設定または取得します。
		/// </summary>
		[Category("ATR")]
		[DisplayName("期間")]
		[Description("期間を設定します。")]
		public int Period { get; set; } = 14;

		/// <summary>
		/// 計算方法を設定または取得します。
		/// </summary>
		[Category("ATR")]
		[DisplayName("計算方法")]
		[Description("計算方法を設定します。")]
		public MovingAverageMethod MovingAverageMethod { get; set; } = MovingAverageMethod.Smma;

		/// <summary>
		/// 移動平均
		/// </summary>
		[Browsable(false)]
		private MovingAverageCalculator MovingAverageCalculator = null;

		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public virtual async Task SetUpAsync()
		{
			MovingAverageCalculator = new MovingAverageCalculator();
		}

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="candles">ロウソク足のコレクション</param>
		/// <returns>値</returns>
		public virtual decimal[] GetValues(IndicatorCandleCollection candles)
		{
			// 必要期間に満たない
			if (candles.Count < Period) return null;

			// 真の値幅（TR）
			List<decimal> trs = new List<decimal>();
			for (int i = 0; i < Period; i++)
			{
				if (candles.Count == i + 1) trs.Add(GetTrueRange(candles[i], null));
				else trs.Add(GetTrueRange(candles[i], candles[i + 1]));
			}

			// ATR
			decimal atr = MovingAverageCalculator.Get(trs.ToArray(), MovingAverageMethod);

			return new decimal[] { atr };
		}

		public static decimal GetTrueRange(Candle today, Candle yesterday)
		{
			List<decimal> ranges = new List<decimal>();
			ranges.Add(Math.Abs(today.High - today.Low));			// 当日高値 - 当日安値
			if (yesterday != null)
			{
				ranges.Add(Math.Abs(today.High - yesterday.Close)); // 当日高値 - 前日終値
				ranges.Add(Math.Abs(today.Low - yesterday.Close));  // 当日安値 - 前日終値
			}
			return ranges.Max();
		}
	}
}
