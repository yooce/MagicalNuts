using System;
using System.Threading.Tasks;

namespace MagicalNuts.Primitive
{
	/// <summary>
	/// カレンダーを表します。
	/// </summary>
	public class Calendar
	{
		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>成功したかどうか</returns>
		public virtual async Task<bool> SetUp()
		{
			return true;
		}

		/// <summary>
		/// 休祝日かどうか判定します。
		/// </summary>
		/// <param name="dt">日時</param>
		/// <returns>休祝日かどうか</returns>
		public virtual bool IsHoliday(DateTime dt)
		{
			return (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday);
		}

		/// <summary>
		/// days日前の営業日を取得します。
		/// </summary>
		/// <param name="dt">基準日</param>
		/// <param name="days">日数</param>
		/// <returns>days日前の営業日</returns>
		public DateTime GetBusinessDayBefore(DateTime dt, int days)
		{
			for (int i = 0; i < days; i++)
			{
				dt = GetFirstBusinessDayToBefore(dt.AddDays(-1));
			}
			return dt;
		}

		/// <summary>
		/// days日後の営業日を取得します。
		/// </summary>
		/// <param name="dt">基準日</param>
		/// <param name="days">日数</param>
		/// <returns>days日後の営業日</returns>
		public DateTime GetBusinessDayAfter(DateTime dt, int days)
		{
			for (int i = 0; i < days; i++)
			{
				dt = GetFirstBusinessDayToAfter(dt.AddDays(1));
			}
			return dt;
		}

		/// <summary>
		/// dt以前（dt含む）で最初の営業日を取得します。
		/// </summary>
		/// <param name="dt">基準日</param>
		/// <returns>dt以前（dt含む）で最初の営業日</returns>
		public DateTime GetFirstBusinessDayToBefore(DateTime dt)
		{
			while (IsHoliday(dt))
			{
				dt = dt.AddDays(-1);
			}
			return dt;
		}

		/// <summary>
		/// dt以後（dt含む）で最初の営業日を取得します。
		/// </summary>
		/// <param name="dt">基準日</param>
		/// <returns>dt以後（dt含む）で最初の営業日</returns>
		public DateTime GetFirstBusinessDayToAfter(DateTime dt)
		{
			while (IsHoliday(dt))
			{
				dt = dt.AddDays(1);
			}
			return dt;
		}
	}
}
