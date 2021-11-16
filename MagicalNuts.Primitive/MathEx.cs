using System;

namespace MagicalNuts.Primitive
{
	/// <summary>
	/// 数値計算の拡張を表します。
	/// </summary>
	public static class MathEx
	{
		/// <summary>
		/// 小数点以下の指定された桁数で切り上げます。
		/// </summary>
		/// <param name="value">小数</param>
		/// <param name="digits">切り上げる小数点以下の桁数</param>
		/// <returns>切り上げた小数</returns>
		public static decimal Ceiling(decimal value, int digits)
		{
			// シフト用倍数
			decimal multi = (decimal)Math.Pow(10, digits);

			if (value >= 0) return Math.Ceiling(value * multi) / multi;
			return Math.Floor(value * multi) / multi;
		}

		/// <summary>
		/// 小数点以下の指定された桁数で切り下げます。
		/// </summary>
		/// <param name="value">小数</param>
		/// <param name="digits">切り下げる小数点以下の桁数</param>
		/// <returns>切り下げた小数</returns>
		public static decimal Floor(decimal value, int digits)
		{
			// シフト用倍数
			decimal multi = (decimal)Math.Pow(10, digits);

			if (value >= 0) return Math.Floor(value * multi) / multi;
			return Math.Ceiling(value * multi) / multi;
		}
	}
}
