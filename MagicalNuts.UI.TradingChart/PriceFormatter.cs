namespace MagicalNuts.UI.TradingChart
{
	/// <summary>
	/// 価格表示のフォーマットを表します。
	/// </summary>
	public static class PriceFormatter
	{
		/// <summary>
		/// 小数点以下の桁数から価格表示のフォーマットを取得します。
		/// </summary>
		/// <param name="digits">小数点以下の桁数</param>
		/// <returns>価格表示のフォーマット</returns>
		public static string GetPriceFormatFromDigits(int digits)
		{
			string format = "0";
			for (int i = 0; i < digits; i++)
			{
				if (i == 0) format += ".";
				format += "0";
			}
			return format;
		}

		/// <summary>
		/// 価格表示のフォーマットから小数点以下の桁数を取得します。
		/// </summary>
		/// <param name="format">価格表示のフォーマット</param>
		/// <returns>小数点以下の桁数</returns>
		public static int? GetDigitsFromFormat(string format)
		{
			// 認識できないフォーマットの場合
			if (!System.Text.RegularExpressions.Regex.IsMatch(format, @"[0-9]+\.[0-9]+")) return null;

			string[] strs = format.Split('.');
			return strs[1].Length;
		}
	}
}
