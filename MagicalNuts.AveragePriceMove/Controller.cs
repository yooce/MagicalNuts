using MagicalNuts.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MagicalNuts.AveragePriceMove
{
	/// <summary>
	/// 平均値動き推移のコントローラーを表します。
	/// </summary>
	public static class Controller
	{
		/// <summary>
		/// 増減率を表します。
		/// </summary>
		public class UpDownRatio
		{
			/// <summary>
			/// 日時
			/// </summary>
			public DateTime DateTime { get; set; }

			/// <summary>
			/// 増減率
			/// </summary>
			public decimal Ratio { get; set; }

			/// <summary>
			/// UpDownRatioクラスの新しいインスタンスを初期化します。
			/// </summary>
			/// <param name="dt">日時</param>
			public UpDownRatio(DateTime dt)
			{
				DateTime = dt;
			}
		}

		/// <summary>
		/// 平均値動き推移を取得します。
		/// </summary>
		/// <param name="candles">ロウソク足の配列</param>
		/// <param name="pt">価格の種類</param>
		/// <param name="years">年数</param>
		/// <returns>平均値動き推移</returns>
		public static AnnualData<decimal>[] GetAveragePriceMove(Candle[] candles, PriceType pt, DateTime now, int years)
		{
			// 前日からの増減率
			UpDownRatio[] udrs = GetUpDownRatios(candles, pt);
			if (udrs == null) return null;

			// 対象期間決定
			DateTime begin = new DateTime(now.AddYears(-years).Year, 1, 1);
			DateTime end = new DateTime(now.Year, 1, 1).AddDays(-1);

			// 増減率を月日に分類
			AnnualData<List<UpDownRatio>>[] audrs = DistributeAnnualUpDownRatios(udrs, begin, end);
			if (audrs == null) return null;

			// 平均増減率
			AnnualData<decimal>[] aars = GetAnnualAverageRatio(audrs);

			// 平均値動き推移
			List<AnnualData<decimal>> aapms = new List<AnnualData<decimal>>();
			for (int i = 0; i < aars.Length; i++)
			{
				AnnualData<decimal> aapm = new AnnualData<decimal>(aars[i].Month, aars[i].Day);
				aapms.Add(aapm);
				if (i == 0) aapm.Data = aars[i].Data;
				else aapm.Data = aapms[i - 1].Data * aars[i].Data;
			}

			return aapms.ToArray();
		}

		/// <summary>
		/// 増減率の配列を取得します。
		/// </summary>
		/// <param name="candles">ロウソク足の配列</param>
		/// <param name="pt">価格の種類</param>
		/// <returns>増減率の配列</returns>
		private static UpDownRatio[] GetUpDownRatios(Candle[] candles, PriceType pt)
		{
			List<UpDownRatio> udrs = new List<UpDownRatio>();
			for (int i = 0; i < candles.Length; i++)
			{
				UpDownRatio udr = new UpDownRatio(candles[i].DateTime);
				if (i == 0)
				{
					// 初回を１とする
					udr.Ratio = 1;
				}
				else
				{
					// １つ前の終値が０ならスキップ
					if (candles[i - 1].GetPrice(pt) == 0) continue;

					// 増減率
					udr.Ratio = candles[i].GetPrice(pt) / candles[i - 1].GetPrice(pt);
				}
				udrs.Add(udr);
			}
			return udrs.ToArray();
		}

		/// <summary>
		/// 増減率を月日で分類したものを取得します。
		/// </summary>
		/// <param name="udrs">増減率の配列</param>
		/// <param name="begin">開始日時</param>
		/// <param name="end">終了日時</param>
		/// <returns>増減率を月日で分類したもの</returns>
		/// 
		private static AnnualData<List<UpDownRatio>>[] DistributeAnnualUpDownRatios(UpDownRatio[] udrs, DateTime begin, DateTime end)
		{
			List<AnnualData<List<UpDownRatio>>> audrs = new List<AnnualData<List<UpDownRatio>>>();
			foreach (UpDownRatio udr in udrs)
			{
				// 対象期間外ならスキップ
				if (udr.DateTime < begin || udr.DateTime > end) continue;

				// 既存を検索
				AnnualData<List<UpDownRatio>> audr = null;
				foreach (AnnualData<List<UpDownRatio>> temp in audrs)
				{
					if (temp.Month == udr.DateTime.Month && temp.Day == udr.DateTime.Day)
					{
						audr = temp;
						break;
					}
				}

				// 新規
				if (audr == null)
				{
					audr = new AnnualData<List<UpDownRatio>>(udr.DateTime.Month, udr.DateTime.Day);
					audr.Data = new List<UpDownRatio>();
					audrs.Add(audr);
				}

				// 追加
				audr.Data.Add(udr);
			}

			// データが１つも無い場合
			if (audrs.Count == 0) return null;

			// ソート
			audrs.Sort(AnnualData<List<UpDownRatio>>.Compare);

			return audrs.ToArray();
		}

		/// <summary>
		/// 平均増減率を取得します。
		/// </summary>
		/// <param name="audrs">増減率を月日で分類したもの</param>
		/// <returns>平均増減率</returns>
		private static AnnualData<decimal>[] GetAnnualAverageRatio(AnnualData<List<UpDownRatio>>[] audrs)
		{
			List<AnnualData<decimal>> aars = new List<AnnualData<decimal>>();
			foreach (AnnualData<List<UpDownRatio>> annualUdr in audrs)
			{
				AnnualData<decimal> aar = new AnnualData<decimal>(annualUdr.Month, annualUdr.Day);
				aar.Data = annualUdr.Data.Select(data => data.Ratio).Average();
				aars.Add(aar);
			}
			return aars.ToArray();
		}
	}
}
