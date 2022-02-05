using MagicalNuts.Primitive;
using System;
using System.ComponentModel;

namespace MagicalNuts.BackTest
{
	/// <summary>
	/// ポジションの方向
	/// </summary>
	public enum PositionDirection
	{
		Long = 0, Short
	}

	/// <summary>
	/// ポジションを表します。
	/// </summary>
	public class Position
	{
		/// <summary>
		/// 銘柄情報
		/// </summary>
		[Browsable(false)]
		public Stock Stock { get; set; }

		/// <summary>
		/// ポジションの方向
		/// </summary>
		public PositionDirection PositionDirection { get; set; }

		/// <summary>
		/// ロット数
		/// </summary>
		public decimal Lots { get; set; }

		/// <summary>
		/// 追加情報
		/// </summary>
		[Browsable(false)]
		public object Additional { get; set; }

		/// <summary>
		/// エントリー約定情報
		/// </summary>
		[Browsable(false)]
		public Execution EntryExecution { get; set; }

		/// <summary>
		/// イグジット約定情報
		/// </summary>
		[Browsable(false)]
		public Execution ExitExecution { get; set; }

		/// <summary>
		/// 損益
		/// </summary>
		public decimal? Return { get; set; }

		/// <summary>
		/// 損益率
		/// </summary>
		public decimal? ReturnRate { get; set; }

		/// <summary>
		/// 保持日数
		/// </summary>
		public int? HoldDays { get; set; }

		#region 表示用

		/// <summary>
		/// 銘柄コード
		/// </summary>
		public string Code => Stock == null ? null : Stock.Code;

		/// <summary>
		/// 銘柄名
		/// </summary>
		public string Name => Stock == null ? null : Stock.Name;

		/// <summary>
		/// エントリー日時
		/// </summary>
		public DateTime? EntryDateTime => EntryExecution == null ? (DateTime?)null : EntryExecution.DateTime;

		/// <summary>
		/// エントリー価格
		/// </summary>
		public decimal? EntryPrice => EntryExecution == null ? (decimal?)null : EntryExecution.Price;

		/// <summary>
		/// イグジット日時
		/// </summary>
		public DateTime? ExitDateTime => ExitExecution == null ? (DateTime?)null : ExitExecution.DateTime;

		/// <summary>
		/// イグジット価格
		/// </summary>
		public decimal? ExitPrice => ExitExecution == null ? (decimal?)null : ExitExecution.Price;

		#endregion

		/// <summary>
		/// イグジット済みかどうか
		/// </summary>
		[Browsable(false)]
		public bool IsExited => ExitExecution != null;

		/// <summary>
		/// 利益が出たかどうか
		/// </summary>
		[Browsable(false)]
		public bool IsWin => Return != null && Return.Value > 0;

		/// <summary>
		/// 損失が出たかどうか
		/// </summary>
		[Browsable(false)]
		public bool IsLose => Return != null && Return.Value < 0;

		/// <summary>
		/// Positionクラスの新しいインスタンスを初期化します。（シリアライザーのためのコンストラクタなので使用非推奨）
		/// </summary>
		public Position()
		{
		}

		/// <summary>
		/// Positionクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="stock">銘柄情報</param>
		/// <param name="pd">ポジションの方向</param>
		/// <param name="lots">ロット数</param>
		/// <param name="additional">追加情報</param>
		/// <param name="dt">日時</param>
		/// <param name="price">エントリー価格</param>
		/// <param name="amount">約定金額</param>
		/// <param name="fee">エントリー時の手数料</param>
		public Position(Stock stock, PositionDirection pd, decimal lots, object additional, DateTime dt, decimal price, decimal amount
			, decimal fee)
		{
			Stock = stock;
			PositionDirection = pd;
			Lots = lots;
			Additional = additional;
			EntryExecution = new Execution(dt, price, amount, fee);
		}

		/// <summary>
		/// イグジットします。
		/// </summary>
		/// <param name="dt">日時</param>
		/// <param name="price">イグジット価格</param>
		/// <param name="amount">約定金額</param>
		/// <param name="fee">イグジット時の手数料</param>
		/// <param name="days">保持日数</param>
		public void Exit(DateTime dt, decimal price, decimal amount, decimal fee, int days)
		{
			switch (PositionDirection)
			{
				case PositionDirection.Long:
					Return = amount - EntryExecution.Amount - EntryExecution.Fee - fee;
					break;
				case PositionDirection.Short:
					Return = EntryExecution.Amount - amount - EntryExecution.Fee - fee;
					break;
			}
			ReturnRate = Return / EntryExecution.Amount;
			HoldDays = days;
			ExitExecution = new Execution(dt, price, amount, fee);
		}
	}
}
