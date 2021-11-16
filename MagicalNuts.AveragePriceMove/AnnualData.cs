namespace MagicalNuts.AveragePriceMove
{
	/// <summary>
	/// 平均値動き推移の年間データを表します。
	/// </summary>
	/// <typeparam name="T">データの型</typeparam>
	public class AnnualData<T>
	{
		/// <summary>
		/// 月
		/// </summary>
		public int Month { get; set; }

		/// <summary>
		/// 日
		/// </summary>
		public int Day { get; set; }

		/// <summary>
		/// データ
		/// </summary>
		public T Data { get; set; }

		/// <summary>
		/// AnnualDataクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="month">月</param>
		/// <param name="day">日</param>
		public AnnualData(int month, int day)
		{
			Month = month;
			Day = day;
		}

		/// <summary>
		/// ソート用に２つの年間データを比較します。
		/// </summary>
		/// <param name="a">年間データ</param>
		/// <param name="b">年間データ</param>
		/// <returns>aの方が古ければ-1、同じなら0、aの方が新しければ1</returns>
		public static int Compare(AnnualData<T> a, AnnualData<T> b)
		{
			if (a.Month < b.Month) return -1;
			if (a.Month == b.Month)
			{
				if (a.Day < b.Day) return -1;
				if (a.Day == b.Day) return 0;
			}
			return 1;
		}
	}
}
