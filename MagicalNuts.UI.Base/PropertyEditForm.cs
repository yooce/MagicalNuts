using MagicalNuts.Primitive;
using System.Windows.Forms;

namespace MagicalNuts.UI.Base
{
	/// <summary>
	/// プロパティを編集するフォームを表します。
	/// </summary>
	public partial class PropertyEditForm : Form
	{
		/// <summary>
		/// PropertyEditFormクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="ph">プロパティ保持者</param>
		public PropertyEditForm(IPropertyHolder ph)
		{
			InitializeComponent();

			Text = ph.Name;
			propertyGrid.SelectedObject = ph.Properties;
		}

		/// <summary>
		/// OKボタンのClickイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行体</param>
		/// <param name="e">イベント引数</param>
		private void buttonOk_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}
