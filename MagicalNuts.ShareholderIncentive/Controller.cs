using MagicalNuts.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MagicalNuts.ShareholderIncentive
{
	/// <summary>
	/// 株主優待権利付き最終日前後の値動きのコントローラーを表します。
	/// </summary>
	public class Controller
	{
		/// <summary>
		/// デフォルトの年間日数
		/// </summary>
		public const int DefaultAnnualDays = 250;

		/// <summary>
		/// 株主優待権利付き最終日前後の値動きのヒストリカルデータを取得します。
		/// </summary>
		/// <param name="args">株主優待権利付き最終日前後の値動きの引数セット</param>
		/// <returns>株主優待権利付き最終日前後の値動きのヒストリカルデータ</returns>
		public virtual HistoricalData GetHistoricalData(Arguments args)
		{
			// ロウソク足がまったく無い
			if (args.Candles.Length == 0) return null;

			// 器作成
			HistoricalData hd = new HistoricalData(args.BeforeNum, args.AfterNum);

			// ロウソク足の最も古い年を出しておく
			int most_old_year = args.Candles.Select(candle => candle.DateTime.Year).Min();

			for (int year = args.LastYear; year > args.LastYear - args.Years; year--)
			{
				// 最も古い年より過去に行ってしまった
				if (year < most_old_year) break;

				// 権利付き最終日取得
				DateTime lastday = GetLastDayWithRights(args.DateOfRightAllotment, year, args.Calendar);

				// 権利付き最終日のロウソク足取得
				Candle lastday_stock_candle = args.Candles.Where(candle => candle.DateTime == lastday).SingleOrDefault();

				// 権利付き最終日のロウソク足が存在しない
				if (lastday_stock_candle == null) continue;

				// 権利付き最終日のインデックス取得
				int lastday_stock_idx = args.Candles.ToList().IndexOf(lastday_stock_candle);

				// 年間データ作成
				AnnualData stock_ad = new AnnualData(year, args.BeforeNum, args.AfterNum);

				// 権利付き最終日の価格を格納
				stock_ad[0] = args.Candles[lastday_stock_idx].GetPrice(args.PriceType);

				// 権利付き最終日前の価格を格納
				int stock_idx = lastday_stock_idx - 1;
				int before_idx = -1;
				while (stock_idx >= 0 && before_idx >= -args.BeforeNum)
				{
					// 祝日ならスキップ
					if (args.Calendar.IsHoliday(args.Candles[stock_idx].DateTime))
					{
						stock_idx--;
						continue;
					}

					// 格納
					stock_ad[before_idx] = args.Candles[stock_idx].GetPrice(args.PriceType);

					// 次へ
					stock_idx--;
					before_idx--;
				}

				// 権利付き最終日後を格納
				stock_idx = lastday_stock_idx + 1;
				int after_idx = 1;
				while (stock_idx < args.Candles.Length && after_idx <= args.AfterNum)
				{
					// 祝日ならスキップ
					if (args.Calendar.IsHoliday(args.Candles[stock_idx].DateTime))
					{
						stock_idx++;
						continue;
					}

					// 格納
					stock_ad[after_idx] = args.Candles[stock_idx].GetPrice(args.PriceType);

					// 次へ
					stock_idx++;
					after_idx++;
				}

				// 正規化
				AnnualData ad = new AnnualData(year, args.BeforeNum, args.AfterNum);
				for (int i = -args.BeforeNum; i <= args.AfterNum; i++)
				{
					ad[i] = stock_ad.GetNormalizedValue(i);
				}

				// 最大最小値のインデックス計算
				ad.CalculateMaxMinValueIndices();

				// 格納
				hd.AnnualDataList.Add(ad);
			}

			// 年間データが１つも無い場合
			if (hd.AnnualDataList.Count == 0) return null;

			// 平均計算
			hd.CalculateAverageAnnualData();

			return hd;
		}

		/// <summary>
		/// エントリー日とイグジット日による期待値の配列を取得します。
		/// </summary>
		/// <param name="hd">株主優待権利付き最終日前後の値動きのヒストリカルデータ</param>
		/// <param name="days">年間日数</param>
		/// <returns>エントリー日とイグジット日による期待値の配列</returns>
		public EntryExitExpectedValue[] GetEntryExitExpectedValues(HistoricalData hd, int days = DefaultAnnualDays)
		{
			// ヒストリカルデータが無い場合
			if (hd == null) return null;

			// 器作成
			List<EntryExitExpectedValue> eeevs = new List<EntryExitExpectedValue>();

			for (int entry_idx = -hd.BeforeNum; entry_idx < 1; entry_idx++)
			{
				for (int exit_idx = entry_idx + 1; exit_idx <= 0; exit_idx++)
				{
					decimal win_sum = 0, lose_sum = 0, win_count = 0, lose_count = 0;
					for (int i = 0; i < hd.AnnualDataList.Count; i++)
					{
						// エントリーとイグジットのデータが揃っていなければスキップ
						if (hd.AnnualDataList[i][entry_idx] == null || hd.AnnualDataList[i][exit_idx] == null) continue;

						// 損益率
						decimal diff_ratio = hd.AnnualDataList[i][exit_idx].Value - hd.AnnualDataList[i][entry_idx].Value;

						if (diff_ratio > 0)
						{
							// 勝ち
							win_sum += diff_ratio;
							win_count++;
						}
						else
						{
							// 負け（同値も手数料を考慮して負けとする）
							lose_sum += diff_ratio;
							lose_count++;
						}
					}

					// 取引がまったく無い場合はスキップ
					if (win_count == 0 && lose_count == 0) continue;

					// エントリー日とイグジット日による期待値作成
					EntryExitExpectedValue eeev = new EntryExitExpectedValue();
					eeev.EntryDaysBefore = -entry_idx;
					eeev.ExitDaysBefore = -exit_idx;
					eeev.WinRatio = win_count / (win_count + lose_count);
					if (win_count != 0) eeev.AverageProfit = win_sum / win_count;
					if (lose_count != 0) eeev.AverageLoss = lose_sum / lose_count;
					eeev.ExpectedValue
						= eeev.WinRatio * eeev.AverageProfit + (1 - eeev.WinRatio) * eeev.AverageLoss;
					eeev.AnnualReturn = eeev.ExpectedValue * days / (exit_idx - entry_idx);

					// 格納
					eeevs.Add(eeev);
				}
			}

			// ソート
			eeevs.Sort(EntryExitExpectedValue.Compare);

			return eeevs.ToArray();
		}

		/// <summary>
		/// 株主優待権利確定日を取得します。
		/// </summary>
		/// <param name="dra">株主優待権利確定日</param>
		/// <param name="year">年</param>
		/// <param name="calendar">カレンダー</param>
		/// <returns>株主優待権利確定日</returns>
		public static DateTime GetDayOfRightAllotment(DateOfRightAllotment dra, int year, Calendar calendar)
		{
			// カレンダーを考慮しない権利確定日取得
			DateTime dt;
			if (dra.Day == null) dt = new DateTime(year, dra.Month, 1).AddMonths(1).AddDays(-1); // 月末
			else dt = new DateTime(year, dra.Month, dra.Day.Value); // 日指定

			// 権利確定日決定
			return calendar.GetFirstBusinessDayToBefore(dt);
		}

		/// <summary>
		/// 株主優待権利付き最終日を取得します。
		/// </summary>
		/// <param name="dra">株主優待権利確定日</param>
		/// <param name="year">年</param>
		/// <param name="calendar">カレンダー</param>
		/// <returns>株主優待権利付き最終日</returns>
		public static DateTime GetLastDayWithRights(DateOfRightAllotment dra, int year, Calendar calendar)
		{
			// 権利確定日取得
			DateTime dt = GetDayOfRightAllotment(dra, year, calendar);

			// 権利付き最終日決定
			if (dt < calendar.GetBusinessDayAfter(new DateTime(2009, 11, 16), 3)) return calendar.GetBusinessDayBefore(dt, 4);
			else if (dt < calendar.GetBusinessDayAfter(new DateTime(2019, 7, 16), 2)) return calendar.GetBusinessDayBefore(dt, 3);
			return calendar.GetBusinessDayBefore(dt, 2);
		}

		/// <summary>
		/// 次の株主優待権利付き最終日を取得します。
		/// </summary>
		/// <param name="dra">株主優待権利確定日</param>
		/// <param name="now">現在日</param>
		/// <param name="calendar">カレンダー</param>
		/// <returns>次の株主優待権利付き最終日</returns>
		public static DateTime GetNextLastDayWithRights(DateOfRightAllotment dra, DateTime now, Calendar calendar)
		{
			// 今年の株主優待権利付き最終日取得
			DateTime dt = GetLastDayWithRights(dra, now.Year, calendar);

			// 現在日より過去なら翌年
			if (dt < now) dt = GetLastDayWithRights(dra, dt.Year + 1, calendar);

			return dt;
		}
	}
}
