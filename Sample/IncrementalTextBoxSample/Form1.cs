using MagicalNuts.Primitive;
using MagicalNuts.UI.Base;

namespace IncrementalTextBoxSample
{
	public partial class Form1 : Form
	{
		private static readonly Stock[] Stocks = new Stock[]
		{
			new Stock("AA", "Alcoa Corporation Common Stock "),
			new Stock("AAA", "Listed Funds Trust AAF First Priority CLO Bond ETF"),
			new Stock("AAAU", "Perth Mint Physical Gold ETF"),
			new Stock("AACG", "ATA Creativity Global - American Depositary Shares, each representing two common shares"),
			new Stock("AACQ", "Artius Acquisition Inc. - Class A Common Stock"),
			new Stock("AACQU", "Artius Acquisition Inc. - Unit consisting of one ordinary share and one third redeemable warrant"),
			new Stock("AACQW", "Artius Acquisition Inc. - Warrant"),
			new Stock("AADR", "AdvisorShares Dorsey Wright ADR ETF"),
			new Stock("AAL", "American Airlines Group, Inc. - Common Stock"),
			new Stock("AAME", "Atlantic American Corporation - Common Stock"),
			new Stock("AAN", "Aarons Holdings Company, Inc. Common Stock"),
			new Stock("AAOI", "Applied Optoelectronics, Inc. - Common Stock"),
			new Stock("AAON", "AAON, Inc. - Common Stock"),
			new Stock("AAP", "Advance Auto Parts Inc Advance Auto Parts Inc W/I"),
			new Stock("AAPL", "Apple Inc. - Common Stock"),
			new Stock("AAT", "American Assets Trust, Inc. Common Stock"),
			new Stock("AAWW", "Atlas Air Worldwide Holdings - Common Stock"),
			new Stock("AAXJ", "iShares MSCI All Country Asia ex Japan Index Fund"),
			new Stock("AAXN", "Axon Enterprise, Inc. - Common Stock")
		};

		public Form1()
		{
			InitializeComponent();

			// incrementalTextBox1 : 最も基本的な使用例
			incrementalTextBox1.SetCandidates(Stocks);
		}

		private async void Form1_Load(object sender, EventArgs e)
		{
			// 複数のIncrementalTextBoxで、大量の同じ検索対象群を使用する場合、
			// それぞれのSetCandidatesを呼ぶと重くなるため、先に検索対象の辞書を作って、それぞれに設定する。
			// また、検索高速化のための辞書キー生成と、検索ワードと検索対象の関連付けを上書きする例にもなっている。
			Dictionary<string, List<IncrementalTextBox.ListViewCandidate>> dict = await IncrementalTextBox.GetCandidateListViewItemDictionaryAsync(
				Stocks, StockIncrementalTextBox.StockKeysForDictionary, StockIncrementalTextBox.StockMatch);
			incrementalTextBox2.CandidateListViewItemDictionary = dict;
			incrementalTextBox3.CandidateListViewItemDictionary = dict;
			incrementalTextBox2.Enabled = true;
			incrementalTextBox3.Enabled = true;
		}

		private void incrementalTextBox1_Decided(IncrementalTextBox sender, IncrementalTextBoxEventArgs e)
		{
			textBox1.Text = e.Decided.ToString();
		}
	}
}
