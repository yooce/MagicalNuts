using MagicalNuts.Primitive;

namespace MagicalNuts.BackTest
{
	/// <summary>
	/// 注文の種類
	/// </summary>
	public enum OrderType
	{
		BuyMarket = 0,	// 成行買い
		SellMarket,		// 成行売り
		BuyLimit,		// 指値買い
		SellLimit,		// 指値売り
		BuyStop,		// 逆指値買い
		SellStop		// 逆指値売り
	}

	/// <summary>
	/// 注文を表します。
	/// </summary>
	public class Order
	{
		/// <summary>
		/// 銘柄情報
		/// </summary>
		public Stock Stock { get; set; }

		/// <summary>
		/// 注文の種類
		/// </summary>
		public OrderType OrderType { get; set; }

		/// <summary>
		/// ロット数
		/// </summary>
		public decimal? Lots { get; set; }

		/// <summary>
		/// 指値または逆指値
		/// </summary>
		public decimal? Price { get; set; }

		/// <summary>
		/// 手仕舞いするポジション
		/// </summary>
		public Position PositionToClose { get; set; }

		/// <summary>
		/// 追加情報
		/// </summary>
		public object Additional { get; set; }

		/// <summary>
		/// Orderクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="stock">銘柄情報</param>
		/// <param name="ot">注文の種類</param>
		/// <param name="lots">ロット数</param>
		/// <param name="price">指値または逆指値</param>
		/// <param name="position">手仕舞いするポジション</param>
		/// <param name="additional">追加情報</param>
		private Order(Stock stock, OrderType ot, decimal? lots, decimal? price, Position position, object additional)
		{
			Stock = stock;
			OrderType = ot;
			Lots = lots;
			Price = price;
			PositionToClose = position;
			Additional = additional;
		}

		/// <summary>
		/// 新規成行買い注文を取得します。
		/// </summary>
		/// <param name="stock">銘柄情報</param>
		/// <param name="lots">ロット数</param>
		/// <param name="additional">追加情報</param>
		/// <returns>新規成行買い注文</returns>
		public static Order GetBuyMarketOrder(Stock stock, decimal lots, object additional = null)
		{
			return new Order(stock, OrderType.BuyMarket, lots, null, null, additional);
		}

		/// <summary>
		/// 返済成行買い注文を取得します。
		/// </summary>
		/// <param name="position">手仕舞いするポジション</param>
		/// <returns>返済成行買い注文</returns>
		public static Order GetBuyMarketOrder(Position position)
		{
			// ショートしか受け付けない
			if (position.PositionDirection != PositionDirection.Short) return null;

			return new Order(position.Stock, OrderType.BuyMarket, null, null, position, null);
		}

		/// <summary>
		/// 信用成行売り注文を取得します。
		/// </summary>
		/// <param name="stock">銘柄情報</param>
		/// <param name="lots">ロット数</param>
		/// <param name="additional">追加情報</param>
		/// <returns>信用成行売り注文</returns>
		public static Order GetSellMarketOrder(Stock stock, decimal lots, object additional = null)
		{
			return new Order(stock, OrderType.SellMarket, lots, null, null, additional);
		}

		/// <summary>
		/// 手仕舞い成行売り注文を取得します。
		/// </summary>
		/// <param name="position">手仕舞いするポジション</param>
		/// <returns>手仕舞い成行売り注文</returns>
		public static Order GetSellMarketOrder(Position position)
		{
			// ロングしか受け付けない
			if (position.PositionDirection != PositionDirection.Long) return null;

			return new Order(position.Stock, OrderType.SellMarket, null, null, position, null);
		}

		/// <summary>
		/// 新規指値買い注文を取得します。
		/// </summary>
		/// <param name="stock">銘柄情報</param>
		/// <param name="lots">ロット数</param>
		/// <param name="price">指値</param>
		/// <param name="additional">追加情報</param>
		/// <returns>新規指値買い注文</returns>
		public static Order GetBuyLimitOrder(Stock stock, decimal lots, decimal price, object additional = null)
		{
			return new Order(stock, OrderType.BuyLimit, lots, price, null, additional);
		}

		/// <summary>
		/// 返済指値買い注文を取得します。
		/// </summary>
		/// <param name="price">指値</param>
		/// <param name="position">手仕舞いするポジション</param>
		/// <returns>返済指値買い注文</returns>
		public static Order GetBuyLimitOrder(decimal price, Position position)
		{
			// ショートしか受け付けない
			if (position.PositionDirection != PositionDirection.Short) return null;

			return new Order(position.Stock, OrderType.BuyLimit, null, price, position, null);
		}

		/// <summary>
		/// 信用指値売り注文を取得します。
		/// </summary>
		/// <param name="stock">銘柄情報</param>
		/// <param name="lots">ロット数</param>
		/// <param name="price">指値</param>
		/// <param name="additional">追加情報</param>
		/// <returns>信用指値売り注文</returns>
		public static Order GetSellLimitOrder(Stock stock, decimal lots, decimal price, object additional = null)
		{
			return new Order(stock, OrderType.SellLimit, lots, price, null, additional);
		}

		/// <summary>
		/// 手仕舞い指値売り注文を取得します。
		/// </summary>
		/// <param name="price">指値</param>
		/// <param name="position">手仕舞いするポジション</param>
		/// <returns>手仕舞い指値売り注文</returns>
		public static Order GetSellLimitOrder(decimal price, Position position)
		{
			// ロングしか受け付けない
			if (position.PositionDirection != PositionDirection.Long) return null;

			return new Order(position.Stock, OrderType.SellLimit, null, price, position, null);
		}

		/// <summary>
		/// 新規逆指値買い注文を取得します。
		/// </summary>
		/// <param name="stock">銘柄情報</param>
		/// <param name="lots">ロット数</param>
		/// <param name="price">逆指値</param>
		/// <param name="additional">追加情報</param>
		/// <returns>新規逆指値買い注文</returns>
		public static Order GetBuyStopOrder(Stock stock, decimal lots, decimal price, object additional = null)
		{
			return new Order(stock, OrderType.BuyStop, lots, price, null, additional);
		}

		/// <summary>
		/// 返済逆指値買い注文を取得します。
		/// </summary>
		/// <param name="price">逆指値</param>
		/// <param name="position">手仕舞いするポジション</param>
		/// <returns>返済逆指値買い注文</returns>
		public static Order GetBuyStopOrder(decimal price, Position position)
		{
			// ショートしか受け付けない
			if (position.PositionDirection != PositionDirection.Short) return null;

			return new Order(position.Stock, OrderType.BuyStop, null, price, position, null);
		}

		/// <summary>
		/// 信用逆指値売り注文を取得します。
		/// </summary>
		/// <param name="stock">銘柄情報</param>
		/// <param name="lots">ロット数</param>
		/// <param name="price">逆指値</param>
		/// <param name="additional">追加情報</param>
		/// <returns>信用逆指値売り注文</returns>
		public static Order GetSellStopOrder(Stock stock, decimal lots, decimal price, object additional = null)
		{
			return new Order(stock, OrderType.SellStop, lots, price, null, additional);
		}

		/// <summary>
		/// 手仕舞い逆指値売り注文を取得します。
		/// </summary>
		/// <param name="stock">銘柄情報</param>
		/// <param name="price">逆指値</param>
		/// <param name="position">手仕舞いするポジション</param>
		/// <returns>手仕舞い逆指値売り注文</returns>
		public static Order GetSellStopOrder(decimal price, Position position)
		{
			// ロングしか受け付けない
			if (position.PositionDirection != PositionDirection.Long) return null;

			return new Order(position.Stock, OrderType.SellStop, null, price, position, null);
		}
	}
}
