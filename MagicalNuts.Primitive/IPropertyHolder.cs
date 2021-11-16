namespace MagicalNuts.Primitive
{
	/// <summary>
	/// プロパティ保持者のインターフェースを表します。
	/// </summary>
	public interface IPropertyHolder
	{
		/// <summary>
		/// プロパティ名を取得します。
		/// </summary>
		string Name { get; }

		/// <summary>
		/// プロパティを取得します。
		/// </summary>
		object Properties { get; }
	}
}
