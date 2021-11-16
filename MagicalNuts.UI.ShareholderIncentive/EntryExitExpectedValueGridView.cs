using System.ComponentModel;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace MagicalNuts.UI.ShareholderIncentive
{
	/// <summary>
	/// エントリー日とイグジット日による期待値を表示するDataGridViewを表します。
	/// </summary>
	[SupportedOSPlatform("windows")]
	public class EntryExitExpectedValueGridView : DataGridView
	{
		/// <summary>
		/// 銘柄コードカラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnCode { get; private set; }

		/// <summary>
		/// 銘柄名カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnName { get; private set; }

		/// <summary>
		/// エントリー日カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnEntry { get; private set; }

		/// <summary>
		/// イグジット日カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnExit { get; private set; }

		/// <summary>
		/// 勝率カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnWinRate { get; private set; }

		/// <summary>
		/// 平均利益カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnAverageProfit { get; private set; }

		/// <summary>
		/// 平均損失カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnAverageLoss { get; private set; }

		/// <summary>
		/// 期待値カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnExpectedValue { get; private set; }

		/// <summary>
		/// 年率リターンカラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnAnnualReturn { get; private set; }

		/// <summary>
		/// カラムを準備します。
		/// </summary>
		public virtual void SetupColumns()
		{
			// DataGridViewCellStyle
			DataGridViewCellStyle cellStyleDays = new DataGridViewCellStyle();
			cellStyleDays.Format = "0日前";
			DataGridViewCellStyle cellStyleP = new DataGridViewCellStyle();
			cellStyleP.Format = "P";

			// ColumnCode
			ColumnCode = new DataGridViewTextBoxColumn();
			ColumnCode.DataPropertyName = "Code";
			ColumnCode.HeaderText = "コード";
			ColumnCode.Name = "ColumnCode";

			// ColumnName
			ColumnName = new DataGridViewTextBoxColumn();
			ColumnName.DataPropertyName = "Name";
			ColumnName.HeaderText = "銘柄名";
			ColumnName.Name = "ColumnName";

			// ColumnEntry
			ColumnEntry = new DataGridViewTextBoxColumn();
			ColumnEntry.DataPropertyName = "EntryDaysBefore";
			ColumnEntry.DefaultCellStyle = cellStyleDays;
			ColumnEntry.HeaderText = "エントリー";
			ColumnEntry.Name = "ColumnEntry";

			// ColumnExit
			ColumnExit = new DataGridViewTextBoxColumn();
			ColumnExit.DataPropertyName = "ExitDaysBefore";
			ColumnExit.DefaultCellStyle = cellStyleDays;
			ColumnExit.HeaderText = "イグジット";
			ColumnExit.Name = "ColumnExit";

			// ColumnWinRate
			ColumnWinRate = new DataGridViewTextBoxColumn();
			ColumnWinRate.DataPropertyName = "WinRatio";
			ColumnWinRate.DefaultCellStyle = cellStyleP;
			ColumnWinRate.HeaderText = "勝率";
			ColumnWinRate.Name = "ColumnWinRate";

			// ColumnAverageProfit
			ColumnAverageProfit = new DataGridViewTextBoxColumn();
			ColumnAverageProfit.DataPropertyName = "AverageProfit";
			ColumnAverageProfit.DefaultCellStyle = cellStyleP;
			ColumnAverageProfit.HeaderText = "平均利益";
			ColumnAverageProfit.Name = "ColumnAverageProfit";

			// ColumnAverageLoss
			ColumnAverageLoss = new DataGridViewTextBoxColumn();
			ColumnAverageLoss.DataPropertyName = "AverageLoss";
			ColumnAverageLoss.DefaultCellStyle = cellStyleP;
			ColumnAverageLoss.HeaderText = "平均損失";
			ColumnAverageLoss.Name = "ColumnAverageLoss";

			// ColumnExpectedValue
			ColumnExpectedValue = new DataGridViewTextBoxColumn();
			ColumnExpectedValue.DataPropertyName = "ExpectedValue";
			ColumnExpectedValue.DefaultCellStyle = cellStyleP;
			ColumnExpectedValue.HeaderText = "期待値";
			ColumnExpectedValue.Name = "ColumnExpectedValue";

			// ColumnAnnualReturn
			ColumnAnnualReturn = new DataGridViewTextBoxColumn();
			ColumnAnnualReturn.DataPropertyName = "AnnualReturn";
			ColumnAnnualReturn.DefaultCellStyle = cellStyleP;
			ColumnAnnualReturn.HeaderText = "年率リターン";
			ColumnAnnualReturn.Name = "ColumnAnnualReturn";

			// DataGridView
			Columns.Clear();
			Columns.AddRange(new DataGridViewColumn[]
			{
				ColumnCode,
				ColumnName,
				ColumnEntry,
				ColumnExit,
				ColumnWinRate,
				ColumnAverageProfit,
				ColumnAverageLoss,
				ColumnExpectedValue,
				ColumnAnnualReturn
			});
		}
	}
}
