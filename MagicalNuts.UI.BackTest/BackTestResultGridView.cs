using System.ComponentModel;
using System.Runtime.Versioning;

namespace MagicalNuts.UI.BackTest
{
	/// <summary>
	/// バックテスト結果を表示するDataGridViewを表します。
	/// </summary>
	[SupportedOSPlatform("windows")]
	public class BackTestResultGridView : DataGridView
	{
		/// <summary>
		/// 初期資産カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnInitialAssets { get; private set; }

		/// <summary>
		/// 追加投資額カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnAdditionalInvestment { get; private set; }

		/// <summary>
		/// 利益カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnProfit { get; private set; }

		/// <summary>
		/// 損失カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnLoss { get; private set; }

		/// <summary>
		/// 損益カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnReturn { get; private set; }

		/// <summary>
		/// 平均利益率カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnAvgProfitRate { get; private set; }

		/// <summary>
		/// 平均損失率カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnAvgLossRate { get; private set; }

		/// <summary>
		/// 平均損益率カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnAvgReturnRate { get; private set; }

		/// <summary>
		/// 平均損益率の標準偏差カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnStdReturnRate { get; private set; }

		/// <summary>
		/// 総勝ち数カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnWinTradeNum { get; private set; }

		/// <summary>
		/// 総負け数カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnLoseTradeNum { get; private set; }

		/// <summary>
		/// 最大連勝数カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnMaxConWinTradeNum { get; private set; }

		/// <summary>
		/// 最大連敗数カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnMaxConLoseTradeNum { get; private set; }

		/// <summary>
		/// ドローダウンカラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnDrawdown { get; private set; }

		/// <summary>
		/// ドローダウン率カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnDrawdownRate { get; private set; }

		/// <summary>
		/// 最小時価資産カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnMinMarketAssets { get; private set; }

		/// <summary>
		/// 勝率カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnWinRate { get; private set; }

		/// <summary>
		/// 期待リターンカラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnExpectedReturn { get; private set; }

		/// <summary>
		/// 平均保持日数カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnAvgHoldDays { get; private set; }

		/// <summary>
		/// プロフィットファクターカラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnProfitFactor { get; private set; }

		/// <summary>
		/// カラムを準備します。
		/// </summary>
		public virtual void SetupColumns()
		{
			// DataGridViewCellStyle
			DataGridViewCellStyle cellStyleP = new DataGridViewCellStyle();
			cellStyleP.Format = "P";
			DataGridViewCellStyle cellStylePoint = new DataGridViewCellStyle();
			cellStylePoint.Format = "0.00";

			// ColumnInitialAssets
			ColumnInitialAssets = new DataGridViewTextBoxColumn();
			ColumnInitialAssets.DataPropertyName = "InitialAssets";
			ColumnInitialAssets.HeaderText = "初期資産";
			ColumnInitialAssets.Name = "ColumnInitialAssets";

			// ColumnAdditionalInvestment
			ColumnAdditionalInvestment = new DataGridViewTextBoxColumn();
			ColumnAdditionalInvestment.DataPropertyName = "AdditionalInvestment";
			ColumnAdditionalInvestment.HeaderText = "追加投資額";
			ColumnAdditionalInvestment.Name = "ColumnAdditionalInvestment";

			// ColumnProfit
			ColumnProfit = new DataGridViewTextBoxColumn();
			ColumnProfit.DataPropertyName = "Profit";
			ColumnProfit.HeaderText = "総利益";
			ColumnProfit.Name = "ColumnProfit";

			// ColumnLoss
			ColumnLoss = new DataGridViewTextBoxColumn();
			ColumnLoss.DataPropertyName = "Loss";
			ColumnLoss.HeaderText = "総損失";
			ColumnLoss.Name = "ColumnLoss";

			// ColumnReturn
			ColumnReturn = new DataGridViewTextBoxColumn();
			ColumnReturn.DataPropertyName = "Return";
			ColumnReturn.HeaderText = "総損益";
			ColumnReturn.Name = "ColumnReturn";

			// ColumnAvgProfitRate
			ColumnAvgProfitRate = new DataGridViewTextBoxColumn();
			ColumnAvgProfitRate.DataPropertyName = "AverageProfitRate";
			ColumnAvgProfitRate.DefaultCellStyle = cellStyleP;
			ColumnAvgProfitRate.HeaderText = "平均利益率";
			ColumnAvgProfitRate.Name = "ColumnAvgProfitRate";

			// ColumnAvgLossRate
			ColumnAvgLossRate = new DataGridViewTextBoxColumn();
			ColumnAvgLossRate.DataPropertyName = "AverageLossRate";
			ColumnAvgLossRate.DefaultCellStyle = cellStyleP;
			ColumnAvgLossRate.HeaderText = "平均損失率";
			ColumnAvgLossRate.Name = "ColumnAvgLossRate";

			// ColumnAvgReturnRate
			ColumnAvgReturnRate = new DataGridViewTextBoxColumn();
			ColumnAvgReturnRate.DataPropertyName = "AverageReturnRate";
			ColumnAvgReturnRate.DefaultCellStyle = cellStyleP;
			ColumnAvgReturnRate.HeaderText = "平均損益率";
			ColumnAvgReturnRate.Name = "ColumnAvgReturnRate";

			// ColumnSdProfit
			ColumnStdReturnRate = new DataGridViewTextBoxColumn();
			ColumnStdReturnRate.DataPropertyName = "StandardDeviationReturnRate";
			ColumnStdReturnRate.DefaultCellStyle = cellStyleP;
			ColumnStdReturnRate.HeaderText = "損益率標準偏差";
			ColumnStdReturnRate.Name = "ColumnSdProfit";

			// ColumnProfitTradeNum
			ColumnWinTradeNum = new DataGridViewTextBoxColumn();
			ColumnWinTradeNum.DataPropertyName = "WinTradeNum";
			ColumnWinTradeNum.HeaderText = "総勝ち数";
			ColumnWinTradeNum.Name = "ColumnProfitTradeNum";

			// ColumnLossTradeNum
			ColumnLoseTradeNum = new DataGridViewTextBoxColumn();
			ColumnLoseTradeNum.DataPropertyName = "LoseTradeNum";
			ColumnLoseTradeNum.HeaderText = "総負け数";
			ColumnLoseTradeNum.Name = "ColumnLossTradeNum";

			// ColumnMaxConWinTradeNum
			ColumnMaxConWinTradeNum = new DataGridViewTextBoxColumn();
			ColumnMaxConWinTradeNum.DataPropertyName = "MaxConsecutiveWinTradeNum";
			ColumnMaxConWinTradeNum.HeaderText = "最大連勝数";
			ColumnMaxConWinTradeNum.Name = "ColumnMaxConWinTradeNum";

			// ColumnMaxConLoseTradeNum
			ColumnMaxConLoseTradeNum = new DataGridViewTextBoxColumn();
			ColumnMaxConLoseTradeNum.DataPropertyName = "MaxConsecutiveLoseTradeNum";
			ColumnMaxConLoseTradeNum.HeaderText = "最大連敗数";
			ColumnMaxConLoseTradeNum.Name = "ColumnMaxConLoseTradeNum";

			// ColumnDrawdown
			ColumnDrawdown = new DataGridViewTextBoxColumn();
			ColumnDrawdown.DataPropertyName = "MaxDrawdown";
			ColumnDrawdown.HeaderText = "最大ドローダウン";
			ColumnDrawdown.Name = "ColumnDrawdown";

			// ColumnDrawdownRate
			ColumnDrawdownRate = new DataGridViewTextBoxColumn();
			ColumnDrawdownRate.DataPropertyName = "MaxDrawdownRate";
			ColumnDrawdownRate.DefaultCellStyle = cellStyleP;
			ColumnDrawdownRate.HeaderText = "最大ドローダウン率";
			ColumnDrawdownRate.Name = "ColumnDrawdownRate";

			// ColumnMinMarketAssets
			ColumnMinMarketAssets = new DataGridViewTextBoxColumn();
			ColumnMinMarketAssets.DataPropertyName = "MinMarketAssets";
			ColumnMinMarketAssets.HeaderText = "最小時価資産";
			ColumnMinMarketAssets.Name = "ColumnMinMarketAssets";

			// ColumnWinRate
			ColumnWinRate = new DataGridViewTextBoxColumn();
			ColumnWinRate.DataPropertyName = "WinRate";
			ColumnWinRate.DefaultCellStyle = cellStyleP;
			ColumnWinRate.HeaderText = "勝率";
			ColumnWinRate.Name = "ColumnWinRate";

			// ColumnExpectedReturn
			ColumnExpectedReturn = new DataGridViewTextBoxColumn();
			ColumnExpectedReturn.DataPropertyName = "ExpectedReturn";
			ColumnExpectedReturn.DefaultCellStyle = cellStyleP;
			ColumnExpectedReturn.HeaderText = "期待損益";
			ColumnExpectedReturn.Name = "ColumnExpectedReturn";

			// ColumnAvgHoldDays
			ColumnAvgHoldDays = new DataGridViewTextBoxColumn();
			ColumnAvgHoldDays.DataPropertyName = "AverageHoldDays";
			ColumnAvgHoldDays.DefaultCellStyle = cellStyleP;
			ColumnAvgHoldDays.HeaderText = "平均保持日数";
			ColumnAvgHoldDays.Name = "ColumnAvgHoldDays";

			// ColumnProfitFactor
			ColumnProfitFactor = new System.Windows.Forms.DataGridViewTextBoxColumn();
			ColumnProfitFactor.DataPropertyName = "ProfitFactor";
			ColumnProfitFactor.DefaultCellStyle = cellStylePoint;
			ColumnProfitFactor.HeaderText = "PF";
			ColumnProfitFactor.Name = "ColumnProfitFactor";
			
			// DataGridView
			Columns.AddRange(new DataGridViewColumn[]
			{
				ColumnInitialAssets,
				ColumnAdditionalInvestment,
				ColumnProfit,
				ColumnLoss,
				ColumnReturn,
				ColumnAvgProfitRate,
				ColumnAvgLossRate,
				ColumnAvgReturnRate,
				ColumnStdReturnRate,
				ColumnWinTradeNum,
				ColumnLoseTradeNum,
				ColumnMaxConWinTradeNum,
				ColumnMaxConLoseTradeNum,
				ColumnDrawdown,
				ColumnDrawdownRate,
				ColumnMinMarketAssets,
				ColumnWinRate,
				ColumnExpectedReturn,
				ColumnAvgHoldDays,
				ColumnProfitFactor
			});
		}
	}
}
