namespace MagicalNuts.BackTest
{
	/// <summary>
	/// ドローダウンを表します。
	/// </summary>
	public class Drawdown
	{
		/// <summary>
		/// ドローダウン金額を取得します。
		/// </summary>
		public decimal Amount => LocalHighHistoricalAssets.MarketAssets - LocalLowHistoricalAssets.MarketAssets;

		/// <summary>
		/// ドローダウン率を取得します。
		/// </summary>
		public decimal Rate => 1 - LocalLowHistoricalAssets.MarketAssets / LocalHighHistoricalAssets.MarketAssets;

		/// <summary>
		/// 局所高値の資産履歴
		/// </summary>
		public HistoricalAssets LocalHighHistoricalAssets { get; set; }

		/// <summary>
		/// 局所安値の資産履歴
		/// </summary>
		public HistoricalAssets LocalLowHistoricalAssets { get; set; }

		/// <summary>
		/// 局所高値を回復した資産履歴
		/// </summary>
		public HistoricalAssets RecoverHistoricalAssets { get; set; }
	}
}
