using MagicalNuts.Primitive;

namespace MagicalNuts.ShareholderIncentive
{
	/// <summary>
	/// 株主優待権利付き最終日前後の値動きの引数セットを表します。
	/// </summary>
	public class Arguments
	{
		/// <summary>
		/// デフォルトの株主優待権利付き最終日前の日数
		/// </summary>
		public const int DefaultBeforeNum = 80;

		/// <summary>
		/// デフォルトの株主優待権利付き最終日後の日数
		/// </summary>
		public const int DefaultAfterNum = 40;

		/// <summary>
		/// 株主優待権利確定日
		/// </summary>
		public DateOfRightAllotment DateOfRightAllotment { get; set; }

		/// <summary>
		/// 最終年
		/// </summary>
		public int LastYear { get; set; }

		/// <summary>
		/// 年数
		/// </summary>
		public int Years { get; set; }

		/// <summary>
		/// ロウソク足の配列
		/// </summary>
		public Candle[] Candles { get; set; }

		/// <summary>
		/// カレンダー
		/// </summary>
		public Calendar Calendar { get; set; }

		/// <summary>
		/// 価格の種類
		/// </summary>
		public PriceType PriceType { get; set; }

		/// <summary>
		/// 株主優待権利付き最終日前の日数
		/// </summary>
		public int BeforeNum { get; set; }

		/// <summary>
		/// 株主優待権利付き最終日後の日数
		/// </summary>
		public int AfterNum { get; set; }

		/// <summary>
		/// Argumentsクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="dra">株主優待権利確定日</param>
		/// <param name="lastYear">最終年</param>
		/// <param name="years">年数</param>
		/// <param name="candles">ロウソク足の配列</param>
		/// <param name="calendar">カレンダー</param>
		/// <param name="pt">価格の種類</param>
		/// <param name="before">株主優待権利付き最終日前の日数</param>
		/// <param name="after">株主優待権利付き最終日後の日数</param>
		public Arguments(DateOfRightAllotment dra, int lastYear, int years, Candle[] candles, Calendar calendar
			, PriceType pt = PriceType.Close, int before = DefaultBeforeNum, int after = DefaultAfterNum)
		{
			DateOfRightAllotment = dra;
			LastYear = lastYear;
			Years = years;
			Candles = candles;
			Calendar = calendar;
			PriceType = pt;
			BeforeNum = before;
			AfterNum = after;
		}
	}
}
