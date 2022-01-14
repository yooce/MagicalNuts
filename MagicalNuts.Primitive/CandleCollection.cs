using System;
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
	/// 期間情報
	/// </summary>
	public struct PeriodInfo
	{
		/// <summary>
		/// 期間単位
		/// </summary>
		public PeriodUnit Unit { get; set; }

		/// <summary>
		/// 期間
		/// </summary>
		public int Period { get; set; }

		/// <summary>
		/// PeriodInfo構造体の新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="unit">期間単位</param>
		/// <param name="period">期間</param>
		public PeriodInfo(PeriodUnit unit, int period)
		{
			Unit = unit;
			Period = period;
		}

		/// <summary>
		/// 同じ値かどうか判定します。
		/// </summary>
		/// <param name="obj">判定対象</param>
		/// <returns>同じ値かどうか</returns>
		public override bool Equals(object obj)
		{
			return obj is PeriodInfo other && Equals(other);
		}

		/// <summary>
		/// 同じ値かどうか判定します。
		/// </summary>
		/// <param name="pi">判定対象</param>
		/// <returns>同じ値かどうか</returns>
		public bool Equals(PeriodInfo pi)
		{
			return Unit == pi.Unit && Period == pi.Period;
		}

		/// <summary>
		/// ハッシュ値を取得します。
		/// </summary>
		/// <returns>ハッシュ値</returns>
		public override int GetHashCode()
		{
			return ((int)Unit, Period).GetHashCode();
		}

		/// <summary>
		/// 同じ期間情報かどうか判定します。
		/// </summary>
		/// <param name="lhs">期間情報</param>
		/// <param name="rhs">期間情報</param>
		/// <returns>同じ期間情報かどうか</returns>
		public static bool operator ==(PeriodInfo lhs, PeriodInfo rhs)
		{
			return lhs.Equals(rhs);
		}

		/// <summary>
		/// 同じ期間情報でないかどうか判定します。
		/// </summary>
		/// <param name="lhs">期間情報</param>
		/// <param name="rhs">期間情報</param>
		/// <returns>同じ期間情報でないかどうか</returns>
		public static bool operator !=(PeriodInfo lhs, PeriodInfo rhs) => !(lhs == rhs);
	}

	/// <summary>
	/// ロウソク足の集合を表します。
	/// </summary>
	/// <typeparam name="T">付加情報の型</typeparam>
	public class CandleCollection<T> : List<Candle>
	{
		/// <summary>
		/// 期間情報
		/// </summary>
		public PeriodInfo PeriodInfo { get; set; }

		/// <summary>
		/// 付加情報
		/// </summary>
		public T Additional { get; }

		/// <summary>
		/// CandleCollectionクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="candles">ロウソク足のリスト</param>
		/// <param name="add">付加情報</param>
		/// <param name="unit">期間単位</param>
		/// <param name="period">期間</param>
		public CandleCollection(List<Candle> candles, T add, PeriodUnit unit = PeriodUnit.Day, int period = 1)
		{
			// 入力チェック
			if ((unit == PeriodUnit.Second && 60 % period != 0)
				|| (unit == PeriodUnit.Minute && 60 % period != 0)
				|| (unit == PeriodUnit.Hour && 24 % period != 0)
				|| (unit == PeriodUnit.Week && period != 1)
				|| (unit == PeriodUnit.Month && 12 % period != 0)
				|| (unit == PeriodUnit.Year && period != 1)) throw new InvalidOperationException();

			Clear();
			AddRange(candles);
			Additional = add;
			PeriodInfo = new PeriodInfo(unit, period);
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

		/// <summary>
		/// 期間を変更します。
		/// </summary>
		/// <param name="pi">期間情報</param>
		/// <returns>ロウソク足の集合</returns>
		public CandleCollection<T> ConvertPeriod(PeriodInfo pi)
		{
			// 期間変更なしならそのまま返す
			if (pi == PeriodInfo) return this;

			// 器作成
			List<Candle> converteds = new List<Candle>();

			DateTime? prevDateTime = null;
			Candle converted = null;
			foreach (Candle candle in this)
			{
				// 次のロウソク足へ
				if (prevDateTime == null || candle.DateTime >= GetNextCandleDateTime(prevDateTime.Value, pi))
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

			return new CandleCollection<T>(converteds, Additional, pi.Unit, pi.Period);
		}

		/// <summary>
		/// 次のロウソク足の日時を取得します。
		/// </summary>
		/// <param name="prevDateTime">前のロウソク足の日時</param>
		/// <param name="pi">期間情報</param>
		/// <returns>次のロウソク足の日時</returns>
		private static DateTime GetNextCandleDateTime(DateTime prevDateTime, PeriodInfo pi)
		{
			DateTime nextDateTime = DateTime.MinValue;
			switch (pi.Unit)
			{
				case PeriodUnit.Second:
					nextDateTime = new DateTime(
						prevDateTime.Year, prevDateTime.Month, prevDateTime.Day, prevDateTime.Hour, prevDateTime.Minute, prevDateTime.Second / pi.Period * pi.Period);
					nextDateTime = nextDateTime.AddSeconds(pi.Period);
					break;
				case PeriodUnit.Minute:
					nextDateTime = new DateTime(
						prevDateTime.Year, prevDateTime.Month, prevDateTime.Day, prevDateTime.Hour, prevDateTime.Minute / pi.Period * pi.Period, 0);
					nextDateTime = nextDateTime.AddMinutes(pi.Period);
					break;
				case PeriodUnit.Hour:
					nextDateTime = new DateTime(prevDateTime.Year, prevDateTime.Month, prevDateTime.Day, prevDateTime.Hour / pi.Period * pi.Period, 0, 0);
					nextDateTime = nextDateTime.AddHours(pi.Period);
					break;
				case PeriodUnit.Day:
					nextDateTime = prevDateTime.Date;
					nextDateTime = nextDateTime.AddDays(pi.Period);
					break;
				case PeriodUnit.Week:
					nextDateTime = prevDateTime.Date.AddDays(1);
					while (nextDateTime.DayOfWeek != DayOfWeek.Monday)
					{
						nextDateTime = nextDateTime.AddDays(1);
					}
					break;
				case PeriodUnit.Month:
					nextDateTime = new DateTime(prevDateTime.Year, (prevDateTime.Month - 1) / pi.Period * pi.Period + 1, 1);
					nextDateTime = nextDateTime.AddMonths(pi.Period);
					break;
				case PeriodUnit.Year:
					nextDateTime = new DateTime(prevDateTime.Year, 1, 1);
					nextDateTime = nextDateTime.AddYears(pi.Period);
					break;
			}
			return nextDateTime;
		}
	}
}
