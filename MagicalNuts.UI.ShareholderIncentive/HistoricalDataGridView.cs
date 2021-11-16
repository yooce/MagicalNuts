using MagicalNuts.ShareholderIncentive;
using System;
using System.Data;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace MagicalNuts.UI.ShareholderIncentive
{
	/// <summary>
	/// 株主優待権利付き最終日前後の値動きのヒストリカルデータを表示するDataGridViewを表します。
	/// </summary>
	[SupportedOSPlatform("windows")]
	public class HistoricalDataGridView : DataGridView
	{
		/// <summary>
		/// 最大値のセルカラー
		/// </summary>
		private static readonly Color MaxValueCellColor = Color.Pink;

		/// <summary>
		/// 最小値のセルカラー
		/// </summary>
		private static readonly Color MinValueCellColor = Color.LightSkyBlue;

		/// <summary>
		/// 株主優待権利付き最終日のセルカラー
		/// </summary>
		private static readonly Color LastDayCellColor = Color.LightYellow;

		/// <summary>
		/// 株主優待権利付き最終日前後の値動きのヒストリカルデータ
		/// </summary>
		protected HistoricalData HistoricalData = null;

		/// <summary>
		/// 株主優待権利付き最終日からの日数のカラムインデックス
		/// </summary>
		protected int ColumnDaysFromLastDayIndex = 0;

		/// <summary>
		/// 平均のカラムインデックス
		/// </summary>
		protected int ColumnAverageIndex = 1;

		/// <summary>
		/// 年間データの開始カラムインデックス
		/// </summary>
		protected int ColumnAnnualDataFirstIndex = 2;

		/// <summary>
		/// HistoricalDataGridViewクラスの新しいインスタンスを初期化します。
		/// </summary>
		public HistoricalDataGridView()
		{
			CellPainting += new DataGridViewCellPaintingEventHandler(dataGridView_CellPainting);
		}

		/// <summary>
		/// 株主優待権利付き最終日前後の値動きのヒストリカルデータを設定します。
		/// </summary>
		/// <param name="hd">株主優待権利付き最終日前後の値動きのヒストリカルデータ</param>
		public void SetHistoricalData(HistoricalData hd)
		{
			HistoricalData = hd;

			// DataTable作成
			DataTable dt = new DataTable();

			// カラム作成
			GenerateColumns(dt, hd);

			// 値格納
			SetValues(hd, dt);

			// DataGridViewに設定
			DataSource = dt;

			// フォーマット指定
			SetFormats(hd);
		}

		/// <summary>
		/// カラムを作成します。
		/// </summary>
		/// <param name="dt">DataTable</param>
		/// <param name="hd">株主優待権利付き最終日前後の値動きのヒストリカルデータ</param>
		protected virtual void GenerateColumns(DataTable dt, HistoricalData hd)
		{
			AddColumnDaysFromLastDay(dt);
			AddColumnAverage(dt);
			AddColumnAnnualDataList(dt, hd);
		}

		/// <summary>
		/// 株主優待権利付き最終日からの日数のカラムを追加します。
		/// </summary>
		/// <param name="dt">DataTable</param>
		protected void AddColumnDaysFromLastDay(DataTable dt)
		{
			dt.Columns.Add("日", typeof(int));
		}

		/// <summary>
		/// 平均のカラムを追加します。
		/// </summary>
		/// <param name="dt">DataTable</param>
		protected void AddColumnAverage(DataTable dt)
		{
			dt.Columns.Add("平均", typeof(decimal));
		}

		/// <summary>
		/// 年間データのカラムを追加します。
		/// </summary>
		/// <param name="dt">DataTable</param>
		/// <param name="hd">株主優待権利付き最終日前後の値動きのヒストリカルデータ</param>
		protected void AddColumnAnnualDataList(DataTable dt, HistoricalData hd)
		{
			for (int j = 0; j < hd.AnnualDataList.Count; j++)
			{
				dt.Columns.Add(hd.AnnualDataList[j].Year.ToString(), typeof(decimal));
			}
		}

		/// <summary>
		/// 値を設定します。
		/// </summary>
		/// <param name="hd">株主優待権利付き最終日前後の値動きのヒストリカルデータ</param>
		/// <param name="dt">DataTable</param>
		protected virtual void SetValues(HistoricalData hd, DataTable dt)
		{
			for (int i = -hd.BeforeNum; i <= hd.AfterNum; i++)
			{
				// 行作成
				DataRow dr = dt.NewRow();

				// 権利付き最終日からの日数
				dr[ColumnDaysFromLastDayIndex] = Math.Abs(i);

				// 平均
				if (hd.AverageAnnualData[i] != null) dr[ColumnAverageIndex] = hd.AverageAnnualData[i].Value;

				// 年間データ
				for (int j = 0; j < hd.AnnualDataList.Count; j++)
				{
					if (hd.AnnualDataList[j][i] != null) dr[ColumnAnnualDataFirstIndex + j] = hd.AnnualDataList[j][i].Value;
				}

				// 格納
				dt.Rows.Add(dr);
			}
		}

		/// <summary>
		/// フォーマットを設定します。
		/// </summary>
		/// <param name="hd">株主優待権利付き最終日前後の値動きのヒストリカルデータ</param>
		protected virtual void SetFormats(HistoricalData hd)
		{
			DataGridViewCellStyle cellStyleP = new DataGridViewCellStyle();
			cellStyleP.Format = "P";
			Columns[ColumnAverageIndex].DefaultCellStyle = cellStyleP;
			for (int j = 0; j < hd.AnnualDataList.Count; j++)
			{
				Columns[ColumnAnnualDataFirstIndex + j].DefaultCellStyle = cellStyleP;
			}
		}

		/// <summary>
		/// CellPaintingイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行体</param>
		/// <param name="e">イベント引数</param>
		private void dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			// 権利付き最終日前後の値動きのヒストリカルデータが無い場合は何もしない
			if (HistoricalData == null) return;

			// 権利付き最終日
			if (e.RowIndex == HistoricalData.BeforeNum) e.CellStyle.BackColor = LastDayCellColor;

			if (e.ColumnIndex == ColumnAverageIndex)
			{
				// 平均の最大最小値
				if (e.RowIndex == HistoricalData.AverageAnnualData.MaxValueIndex + HistoricalData.BeforeNum)
					e.CellStyle.BackColor = MaxValueCellColor;
				if (e.RowIndex == HistoricalData.AverageAnnualData.MinValueIndex + HistoricalData.BeforeNum)
					e.CellStyle.BackColor = MinValueCellColor;
			}
			else if (e.ColumnIndex >= ColumnAnnualDataFirstIndex)
			{
				// 各年の最大最小値
				if (e.RowIndex == HistoricalData.AnnualDataList[e.ColumnIndex - ColumnAnnualDataFirstIndex].MaxValueIndex
					+ HistoricalData.BeforeNum) e.CellStyle.BackColor = MaxValueCellColor;
				if (e.RowIndex == HistoricalData.AnnualDataList[e.ColumnIndex - ColumnAnnualDataFirstIndex].MinValueIndex
					+ HistoricalData.BeforeNum) e.CellStyle.BackColor = MinValueCellColor;
			}
		}
	}
}
