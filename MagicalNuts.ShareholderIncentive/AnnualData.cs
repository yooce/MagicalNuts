namespace MagicalNuts.ShareholderIncentive
{
	/// <summary>
	/// 株主優待権利付き最終日前後の値動きの年間データを表します。
	/// </summary>
	public class AnnualData
	{
		/// <summary>
		/// 年
		/// </summary>
		public int? Year { get; private set; } = null;

		/// <summary>
		/// 最大値のインデックス
		/// </summary>
		public int? MaxValueIndex { get; private set; } = null;

		/// <summary>
		/// 最小値のインデックス
		/// </summary>
		public int? MinValueIndex { get; private set; } = null;

		/// <summary>
		/// データの配列
		/// </summary>

		private decimal?[] Data = null;

		/// <summary>
		/// 株主優待権利付き最終日のインデックス
		/// </summary>

		private int LastDayWithRightsIndex = 0;

		/// <summary>
		/// AnnualDataクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="year">年</param>
		/// <param name="before">株主優待権利付き最終日前の日数</param>
		/// <param name="after">株主優待権利付き最終日後の日数</param>
		public AnnualData(int? year, int before, int after)
		{
			Year = year;
			Data = new decimal?[before + after + 1];
			LastDayWithRightsIndex = before;
		}

		/// <summary>
		/// 株主優待権利付き最終日を０としたインデックスでデータを取得または設定します。
		/// </summary>
		/// <param name="i">株主優待権利付き最終日を０としたインデックス</param>
		/// <returns>データ</returns>
		public decimal? this[int i]
		{
			set => Data[LastDayWithRightsIndex + i] = value;
			get => Data[LastDayWithRightsIndex + i];
		}

		/// <summary>
		/// 株主優待権利付き最終日の価格で正規化したデータを取得します。
		/// </summary>
		/// <param name="i">株主優待権利付き最終日を０としたインデックス</param>
		/// <returns>株主優待権利付き最終日の価格で正規化したデータ</returns>
		public decimal? GetNormalizedValue(int i)
		{
			if (this[i] == null || this[0] == null || this[0] == 0) return null;
			return this[i].Value / this[0].Value;
		}

		/// <summary>
		/// 最大最小値のインデックスを計算します。
		/// </summary>
		public void CalculateMaxMinValueIndices()
		{
			// 最高値のインデックスを探す
			decimal max = decimal.MinValue;
			for (int i = 0; i < LastDayWithRightsIndex; i++)
			{
				if (Data[i] == null) continue;

				if (Data[i].Value > max)
				{
					max = Data[i].Value;
					MaxValueIndex = i - LastDayWithRightsIndex;
				}
			}

			// 最高値までの最安値のインデックスを探す
			if (MaxValueIndex != null)
			{
				decimal min = decimal.MaxValue;
				for (int i = 0; i < MaxValueIndex.Value + LastDayWithRightsIndex; i++)
				{
					if (Data[i] == null) continue;

					if (Data[i].Value < min)
					{
						min = Data[i].Value;
						MinValueIndex = i - LastDayWithRightsIndex;
					}
				}
			}
		}
	}
}
