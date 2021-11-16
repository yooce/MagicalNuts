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
	}
}
