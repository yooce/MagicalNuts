using MessagePack;

namespace MagicalNuts.Primitive
{
	/// <summary>
	/// 銘柄情報を表します。
	/// </summary>
	[MessagePackObject]
	public class Stock
	{
		/// <summary>
		/// 銘柄コード
		/// </summary>
		[Key(0)]
		public string Code { get; set; }

		/// <summary>
		/// 銘柄名
		/// </summary>
		[Key(1)]
		public string Name { get; set; }

		/// <summary>
		/// 単元数
		/// </summary>
		[Key(2)]
		public int Unit { get; set; }

		/// <summary>
		/// 市場の種類
		/// </summary>
		[Key(3)]
		public int MarketType { get; set; }

		/// <summary>
		/// Stockクラスの新しいインスタンスを初期化します。
		/// </summary>
		public Stock()
		{
		}

		/// <summary>
		/// Stockクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="code">銘柄コード</param>
		/// <param name="name">銘柄名</param>
		/// <param name="unit">単元数</param>
		/// <param name="mt">市場の種類</param>
		public Stock(string code, string name = null, int unit = 1, int mt = 0)
		{
			Code = code;
			Name = name;
			Unit = unit;
			MarketType = mt;
		}

		/// <summary>
		/// 現在のオブジェクトを表す文字列を返します。
		/// </summary>
		/// <returns>現在のオブジェクトを表す文字列</returns>
		public override string ToString()
		{
			if (Name == null) return Code;
			return Code + " " + Name;
		}
	}
}
