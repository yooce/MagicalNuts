using System.Collections.Generic;
using System.Linq;

namespace MagicalNuts.ShareholderIncentive
{
	/// <summary>
	/// 株主優待権利付き最終日前後の値動きのヒストリカルデータを表します。
	/// </summary>
	public class HistoricalData
	{
		/// <summary>
		/// 株主優待権利付き最終日前の日数
		/// </summary>
		public int BeforeNum { get; private set; }

		/// <summary>
		/// 株主優待権利付き最終日後の日数
		/// </summary>
		public int AfterNum { get; private set; }

		/// <summary>
		/// 平均年間データ
		/// </summary>
		public AnnualData AverageAnnualData { get; private set; }

		/// <summary>
		/// 年間データのリスト
		/// </summary>
		public List<AnnualData> AnnualDataList { get; private set; }

		/// <summary>
		/// HistoricalDataクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="before">株主優待権利付き最終日前の日数</param>
		/// <param name="after">株主優待権利付き最終日後の日数</param>
		public HistoricalData(int before, int after)
		{
			BeforeNum = before;
			AfterNum = after;
			AnnualDataList = new List<AnnualData>();
		}

		/// <summary>
		/// 年間データを生成します。
		/// </summary>
		/// <param name="year">年</param>
		/// <returns>年間データ</returns>
		public AnnualData GenerateAnnualData(int? year)
		{
			return new AnnualData(year, BeforeNum, AfterNum);
		}

		/// <summary>
		/// 平均年間データを計算します。
		/// </summary>
		public void CalculateAverageAnnualData()
		{
			// 平均年間データ作成
			AverageAnnualData = new AnnualData(null, BeforeNum, AfterNum);

			for (int i = -BeforeNum; i <= AfterNum; i++)
			{
				// データをまとめる
				List<decimal> values = new List<decimal>();
				for (int j = 0; j < AnnualDataList.Count; j++)
				{
					if (AnnualDataList[j][i] != null) values.Add(AnnualDataList[j][i].Value);
				}

				// 平均計算
				if (values.Count > 0) AverageAnnualData[i] = values.Average();
			}

			// 最大最小値のインデックス計算
			AverageAnnualData.CalculateMaxMinValueIndices();
		}
	}
}
