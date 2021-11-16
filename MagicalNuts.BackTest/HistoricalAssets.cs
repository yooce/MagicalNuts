using System;

namespace MagicalNuts.BackTest
{
	/// <summary>
	/// 資産履歴を表します。
	/// </summary>
	public class HistoricalAssets
	{
		/// <summary>
		/// 日時
		/// </summary>
		public DateTime DateTime { get; set; }

		/// <summary>
		/// 簿価資産
		/// </summary>
		public decimal BookAssets { get; set; }

		/// <summary>
		/// 時価資産
		/// </summary>
		public decimal MarketAssets { get; set; }

		/// <summary>
		/// 投資額
		/// </summary>
		public decimal InvestmentAmount { get; set; }

		/// <summary>
		/// HistoricalAssetsの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="dt">日時</param>
		/// <param name="book">簿価資産</param>
		/// <param name="market">時価資産</param>
		/// <param name="investment">投資額</param>
		public HistoricalAssets(DateTime dt, decimal book, decimal market, decimal investment)
		{
			DateTime = dt;
			BookAssets = book;
			MarketAssets = market;
			InvestmentAmount = investment;
		}
	}
}
