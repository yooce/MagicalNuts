using MagicalNuts.BackTest;
using System.ComponentModel;
using System.Runtime.Versioning;

namespace MagicalNuts.UI.BackTest
{
	/// <summary>
	/// ポジションを表示するDataGridViewを表します。
	/// </summary>
	[SupportedOSPlatform("windows")]
	public class PositionGridView : DataGridView
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
		/// ポジションの方向カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnDirection { get; private set; }

		/// <summary>
		/// ロット数カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnLots { get; private set; }

		/// <summary>
		/// エントリー日時カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnEntryDateTime { get; private set; }

		/// <summary>
		/// エントリー価格カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnEntryPrice { get; private set; }

		/// <summary>
		/// イグジット日時カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnExitDateTime { get; private set; }

		/// <summary>
		/// イグジット価格カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnExitPrice { get; private set; }

		/// <summary>
		/// 保持日数カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnHoldDays { get; private set; }

		/// <summary>
		/// 損益カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnReturn { get; private set; }

		/// <summary>
		/// 損益率カラム
		/// </summary>
		[Browsable(false)]
		public DataGridViewTextBoxColumn ColumnReturnRate { get; private set; }

		/// <summary>
		/// PositionGridViewクラスの新しいインスタンスを初期化します。
		/// </summary>
		public PositionGridView()
		{
			CellFormatting += new DataGridViewCellFormattingEventHandler(positionDataGridView_CellFormatting);
		}

		/// <summary>
		/// カラムを準備します。
		/// </summary>
		public virtual void SetupColumns()
		{
			// DataGridViewCellStyle
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

			// ColumnDirection
			ColumnDirection = new DataGridViewTextBoxColumn();
			ColumnDirection.DataPropertyName = "PositionDirection";
			ColumnDirection.HeaderText = "ポジション";
			ColumnDirection.Name = "ColumnDirection";

			// ColumnLots
			ColumnLots = new DataGridViewTextBoxColumn();
			ColumnLots.DataPropertyName = "Lots";
			ColumnLots.HeaderText = "株数";
			ColumnLots.Name = "ColumnLots";

			// ColumnEntryDateTime
			ColumnEntryDateTime = new DataGridViewTextBoxColumn();
			ColumnEntryDateTime.DataPropertyName = "EntryDateTime";
			ColumnEntryDateTime.HeaderText = "エントリー日";
			ColumnEntryDateTime.Name = "ColumnEntryDateTime";

			// ColumnEntryPrice
			ColumnEntryPrice = new DataGridViewTextBoxColumn();
			ColumnEntryPrice.DataPropertyName = "EntryPrice";
			ColumnEntryPrice.HeaderText = "エントリー価格";
			ColumnEntryPrice.Name = "ColumnEntryPrice";

			// ColumnExitDateTime
			ColumnExitDateTime = new DataGridViewTextBoxColumn();
			ColumnExitDateTime.DataPropertyName = "ExitDateTime";
			ColumnExitDateTime.HeaderText = "イグジット日";
			ColumnExitDateTime.Name = "ColumnExitDateTime";

			// ColumnExitPrice
			ColumnExitPrice = new DataGridViewTextBoxColumn();
			ColumnExitPrice.DataPropertyName = "ExitPrice";
			ColumnExitPrice.HeaderText = "イグジット価格";
			ColumnExitPrice.Name = "ColumnExitPrice";

			// ColumnHoldDays
			ColumnHoldDays = new DataGridViewTextBoxColumn();
			ColumnHoldDays.DataPropertyName = "HoldDays";
			ColumnHoldDays.HeaderText = "保持日数";
			ColumnHoldDays.Name = "ColumnHoldDays";

			// ColumnReturn
			ColumnReturn = new DataGridViewTextBoxColumn();
			ColumnReturn.DataPropertyName = "Return";
			ColumnReturn.HeaderText = "損益";
			ColumnReturn.Name = "ColumnReturn";

			// ColumnReturnRate
			ColumnReturnRate = new DataGridViewTextBoxColumn();
			ColumnReturnRate.DataPropertyName = "ReturnRate";
			ColumnReturnRate.DefaultCellStyle = cellStyleP;
			ColumnReturnRate.HeaderText = "損益率";
			ColumnReturnRate.Name = "ColumnReturnRate";

			// DataGridView
			Columns.Clear();
			Columns.AddRange(new DataGridViewColumn[]
			{
				ColumnCode,
				ColumnName,
				ColumnDirection,
				ColumnLots,
				ColumnEntryDateTime,
				ColumnEntryPrice,
				ColumnExitDateTime,
				ColumnExitPrice,
				ColumnHoldDays,
				ColumnReturn,
				ColumnReturnRate
			});
		}

		/// <summary>
		/// CellFormattingイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行体</param>
		/// <param name="e">イベント引数</param>
		private void positionDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (Columns[e.ColumnIndex].Name == ColumnDirection.Name)
			{
				// ポジションの方向を日本語に
				switch (e.Value)
				{
					case PositionDirection.Long:
						e.Value = "ロング";
						break;
					case PositionDirection.Short:
						e.Value = "ショート";
						break;
				}
			}
			else if (Columns[e.ColumnIndex].Name == ColumnEntryPrice.Name
				|| Columns[e.ColumnIndex].Name == ColumnExitPrice.Name)
			{
				// objectをdecimalで受けて、さらにdoubleにしないと何故か整数でも小数点以下が表示されてしまう
				if (e.Value != null) e.Value = (double)(decimal)e.Value;
			}
		}
	}
}
