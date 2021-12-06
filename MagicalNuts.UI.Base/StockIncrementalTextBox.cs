using MagicalNuts.Primitive;

namespace MagicalNuts.UI.Base
{
	/// <summary>
	/// 銘柄情報用のインクリメンタルサーチを備えたテクストボックスコントロールを表します。
	/// </summary>
	public class StockIncrementalTextBox : UI.Base.IncrementalTextBox
	{
		/// <summary>
		/// StockIncrementalTextBoxクラスの新しいインスタンスを初期化します。
		/// </summary>
		public StockIncrementalTextBox()
		{
			KeysForDictionary = StockKeysForDictionary;
			Match = StockMatch;
		}

		/// <summary>
		/// 候補の辞書用キーを取得します。
		/// </summary>
		/// <param name="candidate">候補</param>
		/// <returns>辞書用キーの配列</returns>
		private string[] StockKeysForDictionary(object candidate)
		{
			Stock stock = candidate as Stock;
			return new string[] { stock.Code.ToUpper()[0].ToString(), stock.Name.ToUpper()[0].ToString() };
		}

		/// <summary>
		/// キーに対して候補が相応しいかどうかを判定します。
		/// </summary>
		/// <param name="key">キー</param>
		/// <param name="candidate">候補</param>
		/// <returns>キーに対して候補が相応しいかどうか</returns>
		private bool StockMatch(string key, object candidate)
		{
			Stock stock = candidate as Stock;
			return stock.Code.ToUpper().StartsWith(key.ToUpper()) || stock.Name.ToUpper().StartsWith(key.ToUpper());
		}
	}
}
