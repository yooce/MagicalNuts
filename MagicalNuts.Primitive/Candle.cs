using MessagePack;
using System;

namespace MagicalNuts.Primitive
{
	/// <summary>
	/// 価格の種類を表します。
	/// </summary>
	public enum PriceType
	{
		Open = 0, High, Low, Close
	}

	/// <summary>
	/// ロウソク足を表します。
	/// </summary>
	[MessagePackObject]
	public class Candle
	{
		/// <summary>
		/// ロウソク足の開始日時
		/// </summary>
		[Key(0)]
		public DateTime DateTime { get; set; }

		/// <summary>
		/// 始値
		/// </summary>
		[Key(1)]
		public decimal Open { get; set; }

		/// <summary>
		/// 高値
		/// </summary>
		[Key(2)]
		public decimal High { get; set; }

		/// <summary>
		/// 安値
		/// </summary>
		[Key(3)]
		public decimal Low { get; set; }

		/// <summary>
		/// 終値
		/// </summary>
		[Key(4)]
		public decimal Close { get; set; }

		/// <summary>
		/// 出来高
		/// </summary>
		[Key(5)]
		public decimal Volume { get; set; }

		/// <summary>
		/// 売買代金
		/// </summary>
		[Key(6)]
		public decimal TradingValue => Close * Volume;

		/// <summary>
		/// Candleクラスの新しいインスタンスを初期化します。
		/// </summary>
		public Candle()
		{
		}

		/// <summary>
		/// Candleクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="dt">ロウソク足の開始日時</param>
		/// <param name="open">始値</param>
		/// <param name="high">高値</param>
		/// <param name="low">安値</param>
		/// <param name="close">終値</param>
		public Candle(DateTime dt, decimal open, decimal high, decimal low, decimal close)
		{
			DateTime = dt;
			Open = open;
			High = high;
			Low = low;
			Close = close;
		}

		/// <summary>
		/// Candleクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="dt">ロウソク足の開始日時</param>
		/// <param name="open">始値</param>
		/// <param name="high">高値</param>
		/// <param name="low">安値</param>
		/// <param name="close">終値</param>
		/// <param name="volume">出来高</param>
		public Candle(DateTime dt, decimal open, decimal high, decimal low, decimal close, decimal volume)
		{
			DateTime = dt;
			Open = open;
			High = high;
			Low = low;
			Close = close;
			Volume = volume;
		}

		/// <summary>
		/// ソート用に２つのロウソク足を昇順に比較します。
		/// </summary>
		/// <param name="a">ロウソク足</param>
		/// <param name="b">ロウソク足</param>
		/// <returns>aの方が古ければ-1、同じなら0、aの方が新しければ1</returns>
		public static int CompareAscending(Candle a, Candle b)
		{
			if (a.DateTime < b.DateTime) return -1;
			if (a.DateTime == b.DateTime) return 0;
			return 1;
		}

		/// <summary>
		/// ソート用に２つのロウソク足を降順に比較します。
		/// </summary>
		/// <param name="a">ロウソク足</param>
		/// <param name="b">ロウソク足</param>
		/// <returns>aの方が新しければ-1、同じなら0、aの方が古ければ1</returns>
		public static int CompareDescending(Candle a, Candle b)
		{
			if (a.DateTime > b.DateTime) return -1;
			if (a.DateTime == b.DateTime) return 0;
			return 1;
		}

		/// <summary>
		/// 価格を取得します。
		/// </summary>
		/// <param name="pt">価格の種類</param>
		/// <returns>価格</returns>
		public decimal GetPrice(PriceType pt)
		{
			decimal price = 0;
			switch (pt)
			{
				case PriceType.Open:
					price = Open;
					break;
				case PriceType.High:
					price = High;
					break;
				case PriceType.Low:
					price = Low;
					break;
				case PriceType.Close:
					price = Close;
					break;
			}
			return price;
		}
	}
}
