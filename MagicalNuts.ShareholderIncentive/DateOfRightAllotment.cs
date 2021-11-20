using MessagePack;
using System;

namespace MagicalNuts.ShareholderIncentive
{
	/// <summary>
	/// 株主優待権利確定日を表します。
	/// </summary>
	[MessagePackObject]
	public class DateOfRightAllotment
	{
		/// <summary>
		/// 銘柄コード
		/// </summary>
		[Key(0)]
		public string Code { get; set; }

		/// <summary>
		/// 月
		/// </summary>
		[Key(1)]
		public byte Month { get; set; }

		/// <summary>
		/// 日
		/// </summary>
		[Key(2)]
		public byte? Day { get; set; }

		/// <summary>
		/// DateOfRightAllotmentクラスの新しいインスタンスを初期化します。
		/// </summary>
		public DateOfRightAllotment()
		{
		}

		/// <summary>
		/// DateOfRightAllotmentクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="code">銘柄コード</param>
		/// <param name="month">月</param>
		/// <param name="day">日</param>
		public DateOfRightAllotment(string code, byte month, byte? day = null)
		{
			Code = code;
			Month = month;
			Day = day;
		}
	}
}
