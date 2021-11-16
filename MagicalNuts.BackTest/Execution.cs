using System;

namespace MagicalNuts.BackTest
{
	/// <summary>
	/// 約定情報を表します。
	/// </summary>
	public class Execution
	{
		/// <summary>
		/// 日時
		/// </summary>
		public DateTime DateTime { get; set; }

		/// <summary>
		/// 価格
		/// </summary>
		public decimal Price { get; set; }

		/// <summary>
		/// 約定金額
		/// </summary>
		public decimal Amount { get; set; }

		/// <summary>
		/// 手数料
		/// </summary>
		public decimal Fee { get; set; }

		/// <summary>
		/// Executionクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="dt">日時</param>
		/// <param name="price">価格</param>
		/// /// <param name="amount">約定金額</param>
		/// <param name="fee">手数料</param>
		public Execution(DateTime dt, decimal price, decimal amount, decimal fee)
		{
			DateTime = dt;
			Price = price;
			Amount = amount;
			Fee = fee;
		}
	}
}
