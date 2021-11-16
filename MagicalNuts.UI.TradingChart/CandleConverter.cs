using System;
using System.Collections.Generic;

namespace MagicalNuts.UI.TradingChart
{
	/// <summary>
	/// ロウソク足の期間を表します。
	/// </summary>
	public enum CandlePeriod
	{
		Dayly = 0, Weekly, Monthly, Yearly
	}

	/// <summary>
	/// ロウソク足の変換器を表します。
	/// </summary>
	public static class CandleConverter
	{
		/// <summary>
		/// ロウソク足を日足から目的の足に変換します。
		/// </summary>
		/// <param name="daily">日足</param>
		/// <param name="period">目的のロウソク足の期間</param>
		/// <returns>目的のロウソク足のリスト</returns>
		public static List<Primitive.Candle> ConvertFromDaily(List<Primitive.Candle> daily, CandlePeriod period)
		{
			// ソート
			daily.Sort(Primitive.Candle.CompareAscending);

			// 日足ならそのまま返す
			if (period == CandlePeriod.Dayly) return daily;

			// 器作成
			List<Primitive.Candle> converteds = new List<Primitive.Candle>();

			DateTime? prevDateTime = null;
			Primitive.Candle converted = null;
			foreach (Primitive.Candle candle in daily)
			{
				// 次のロウソク足へ
				if (NeedToChangeNextCandle(candle, prevDateTime, period))
				{
					// ロウソク足追加
					if (converted != null) converteds.Add(converted);

					// 新規ロウソク足
					converted = new Primitive.Candle();
					converted.DateTime = candle.DateTime;
					converted.Open = candle.Open;
					converted.High = candle.Open;
					converted.Low = candle.Open;
					converted.Close = candle.Open;
					converted.Volume = 0;
				}

				// 値設定
				if (candle.High > converted.High) converted.High = candle.High;
				if (candle.Low < converted.Low) converted.Low = candle.Low;
				converted.Close = candle.Close;
				converted.Volume += candle.Volume;

				// 日時を覚えておく
				prevDateTime = candle.DateTime;
			}

			return converteds;
		}

		/// <summary>
		/// 次のロウソク足に切り替えが必要かどうか判定します。
		/// </summary>
		/// <param name="candle">ロウソク足</param>
		/// <param name="prevDateTime">前回のロウソク足の日時</param>
		/// <param name="period">ロウソク足の期間</param>
		/// <returns>次のロウソク足に切り替えが必要かどうか</returns>
		private static bool NeedToChangeNextCandle(Primitive.Candle candle, DateTime? prevDateTime, CandlePeriod period)
		{
			// 前回日時が無い
			if (prevDateTime == null) return true;

			// 次のDateTime算出
			DateTime nextDateTime = DateTime.MinValue;
			switch (period)
			{
				case CandlePeriod.Dayly:
					nextDateTime = prevDateTime.Value.Date.AddDays(1);
					break;
				case CandlePeriod.Weekly:
					nextDateTime = prevDateTime.Value.Date.AddDays(1);
					while (nextDateTime.DayOfWeek != DayOfWeek.Monday)
					{
						nextDateTime = nextDateTime.AddDays(1);
					}
					break;
				case CandlePeriod.Monthly:
					nextDateTime = new DateTime(prevDateTime.Value.Year, prevDateTime.Value.Month, 1).AddMonths(1);
					break;
				case CandlePeriod.Yearly:
					nextDateTime = new DateTime(prevDateTime.Value.Year, 1, 1).AddYears(1);
					break;
			}

			// 次のDateTimeを超えているか
			return candle.DateTime >= nextDateTime;
		}
	}
}
