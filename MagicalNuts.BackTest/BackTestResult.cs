namespace MagicalNuts.BackTest
{
	/// <summary>
	/// バックテスト結果を表します。
	/// </summary>
	public class BackTestResult
	{
		/// <summary>
		/// 初期資産
		/// </summary>
		public decimal InitialAssets { get; set; }

		/// <summary>
		/// 追加投資額
		/// </summary>
		public decimal AdditionalInvestment { get; set; }

		/// <summary>
		/// 総利益
		/// </summary>
		public decimal Profit { get; set; }

		/// <summary>
		/// 総損失
		/// </summary>
		public decimal Loss { get; set; }

		/// <summary>
		/// 総損益
		/// </summary>
		public decimal Return { get; set; }

		/// <summary>
		/// 平均利益率
		/// </summary>
		public decimal AverageProfitRate { get; set; }

		/// <summary>
		/// 平均損失率
		/// </summary>
		public decimal AverageLossRate { get; set; }

		/// <summary>
		/// 平均損益率
		/// </summary>
		public decimal AverageReturnRate { get; set; }

		/// <summary>
		/// 損益率の標準偏差
		/// </summary>
		public decimal StandardDeviationReturnRate { get; set; }

		/// <summary>
		/// 総勝ち数
		/// </summary>
		public int WinTradeNum { get; set; }

		/// <summary>
		/// 総負け数
		/// </summary>
		public int LoseTradeNum { get; set; }

		/// <summary>
		/// 最大連勝数
		/// </summary>
		public int MaxConsecutiveWinTradeNum { get; set; }

		/// <summary>
		/// 最大連敗数
		/// </summary>
		public int MaxConsecutiveLoseTradeNum { get; set; }

		/// <summary>
		/// 最大ドローダウン
		/// </summary>
		public decimal MaxDrawdown { get; set; }

		/// <summary>
		/// 最大ドローダウン率
		/// </summary>
		public decimal MaxDrawdownRate { get; set; }

		/// <summary>
		/// 最小時価資産
		/// </summary>
		public decimal MinMarketAssets { get; set; }

		/// <summary>
		/// 勝率
		/// </summary>
		public decimal WinRate { get; set; }

		/// <summary>
		/// 期待損益
		/// </summary>
		public decimal ExpectedReturn { get; set; }

		/// <summary>
		/// 平均保持日数
		/// </summary>
		public decimal AverageHoldDays { get; set; }

		/// <summary>
		/// プロフィットファクター
		/// </summary>
		public decimal ProfitFactor { get; set; }

		/// <summary>
		/// ポジションの配列
		/// </summary>
		public Position[] Positions { get; set; }

		/// <summary>
		/// 資産履歴の配列
		/// </summary>
		public HistoricalAssets[] HistoricalAssetsArray { get; set; }
	}
}
