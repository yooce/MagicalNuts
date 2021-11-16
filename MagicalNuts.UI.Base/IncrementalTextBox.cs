using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicalNuts.UI.Base
{
	/// <summary>
	/// インクリメンタルサーチを備えたテクストボックスコントロールを表します。
	/// </summary>
	public class IncrementalTextBox : TextBox
	{
		/// <summary>
		/// 候補リスト項目を表します。
		/// </summary>
		public class ListViewCandidate : ListViewItem
		{
			/// <summary>
			/// 候補オブジェクト
			/// </summary>
			public object Candidate { get; set; }

			/// <summary>
			/// CandidateListViewItemクラスの新しいインスタンスを初期化します。
			/// </summary>
			/// <param name="candidate">候補オブジェクト</param>
			public ListViewCandidate(object candidate) : base()
			{
				Candidate = candidate;
				Text = Candidate.ToString();
			}
		}

		/// <summary>
		/// 候補リストの高さを取得または設定します。
		/// </summary>
		public int CandidateListHeight
		{
			get => CandidateListView.Height;
			set => CandidateListView.Height = value;
		}

		/// <summary>
		/// 決定された候補を取得します。
		/// </summary>
		public object DecidedCandidate { get; private set; } = null;

		/// <summary>
		/// 候補リストを表示する項目数の閾値を取得または設定します。候補数がこの数値以下なら候補リストが表示されます。
		/// </summary>
		public int CandidateListShowThreshold { get; set; } = 30;

		/// <summary>
		/// 候補リスト
		/// </summary>
		private ListView CandidateListView = null;

		/// <summary>
		/// 候補リスト項目の辞書
		/// </summary>
		public Dictionary<string, List<ListViewCandidate>> _CandidateListViewItemDictionary = null;

		/// <summary>
		/// 候補リスト項目の辞書
		/// </summary>
		public Dictionary<string, List<ListViewCandidate>> CandidateListViewItemDictionary
		{
			set
			{
				_CandidateListViewItemDictionary = value;
				DictionarySet = true;
				Enabled = true;
			}
		}

		/// <summary>
		/// 候補リスト項目の辞書が設定されたかどうか
		/// </summary>
		public bool DictionarySet { get; set; } = false;

		/// <summary>
		/// 変更前のテキスト
		/// </summary>
		private string PreviouseText = null;

		/// <summary>
		/// 親コントロールにイベントを登録したかどうか
		/// </summary>
		private bool ParentControlsEventRegistered = false;

		/// <summary>
		/// IncrementalTextBoxのイベントを処理するメソッドを表します。
		/// </summary>
		/// <param name="sender">イベント発生主体</param>
		/// <param name="e">IncrementalTextBox用イベント引数</param>
		public delegate void IncrementalTextBoxEventHandler(IncrementalTextBox sender, IncrementalTextBoxEventArgs e);

		/// <summary>
		/// 項目が決定されると発生します。
		/// </summary>
		[Description("項目が決定されると発生します。"), Category("IncrementalTextBox")]
		public event IncrementalTextBoxEventHandler Decided;

		/// <summary>
		/// 候補の辞書用キーの取得を処理します。
		/// </summary>
		/// <param name="candidate">候補</param>
		/// <returns>辞書用キーの配列</returns>
		public delegate string[] KeysForDictionaryHandler(object candidate);

		/// <summary>
		/// キーに対して候補が相応しいかの判定を処理します。
		/// </summary>
		/// <param name="key">キー</param>
		/// <param name="candidate">候補</param>
		/// <returns>キーに対して候補が相応しいかどうか</returns>
		public delegate bool MatchHandler (string key, object candidate);

		/// <summary>
		/// 候補の辞書用キーの取得を処理するメソッドを表します。
		/// </summary>
		private KeysForDictionaryHandler _KeysForDictionary = null;

		/// <summary>
		/// キーに対して候補が相応しいかの判定を処理するメソッドを表します
		/// </summary>
		private MatchHandler _Match = null;

		/// <summary>
		/// 候補の辞書用キーの取得を処理するメソッドを表します。
		/// </summary>
		public KeysForDictionaryHandler KeysForDictionary
		{
			set
			{
				if (value == null) _KeysForDictionary = DefaultKeysForDictionary;
				else _KeysForDictionary = value;
			}
		}

		/// <summary>
		/// キーに対して候補が相応しいかの判定を処理するメソッドを表します
		/// </summary>
		public MatchHandler Match
		{
			set
			{
				if (value == null) _Match = DefaultMatch;
				else _Match = value;
			}
		}

		/// <summary>
		/// IncrementalTextBoxクラスの新しいインスタンスを初期化します。
		/// </summary>
		public IncrementalTextBox() : base()
		{
			// 候補リスト作成
			CandidateListView = new ListView();
			CandidateListView.Visible = false;
			CandidateListView.View = View.List;
			CandidateListView.MultiSelect = false;
			CandidateListView.Height = 200;

			// 候補リストのイベント登録
			CandidateListView.Enter += new EventHandler(candidateListView_Enter);
			CandidateListView.KeyDown += new KeyEventHandler(candidateListView_KeyDown);
			CandidateListView.MouseClick += new MouseEventHandler(candidateListView_MouseClick);
			
			// テキストボックスのイベント登録
			MouseClick += new MouseEventHandler(textBox_MouseClick);
			TextChanged += new EventHandler(textBox_TextChanged);
			KeyDown += new KeyEventHandler(textBox_KeyDown);
			Leave += new EventHandler(textBox_Leave);

			// 既定のデリゲート登録
			_KeysForDictionary = DefaultKeysForDictionary;
			_Match = DefaultMatch;

			Enabled = false;
		}

		/// <summary>
		/// 候補リスト項目の辞書を取得します。
		/// </summary>
		/// <param name="candidates">候補</param>
		/// <param name="kfdh">候補リスト項目の辞書用キーを取得するデリゲート</param>
		/// <param name="mh">キーに対して候補が相応しいか判定するデリゲート</param>
		/// <returns></returns>
		public static async Task<Dictionary<string, List<ListViewCandidate>>> GetCandidateListViewItemDictionaryAsync(object[] candidates
			, KeysForDictionaryHandler kfdh = null, MatchHandler mh = null)
		{
			return await Task.Run(() => GetCandidateListViewItemDictionary(candidates, kfdh, mh));
		}

		/// <summary>
		/// 候補リスト項目の辞書を取得します。
		/// </summary>
		/// <param name="candidates">候補</param>
		/// <param name="kfdh">候補リスト項目の辞書用キーを取得するデリゲート</param>
		/// <param name="mh">キーに対して候補が相応しいか判定するデリゲート</param>
		/// <returns></returns>
		public static Dictionary<string, List<ListViewCandidate>>  GetCandidateListViewItemDictionary(object[] candidates
			, KeysForDictionaryHandler kfdh = null, MatchHandler mh = null)
		{
			// CandidateListViewItemに変換
			List<ListViewCandidate> allitems = new List<ListViewCandidate>();
			foreach (object candidate in candidates)
			{
				allitems.Add(new ListViewCandidate(candidate));
			}

			// 候補リスト項目の辞書作成
			Dictionary<string, List<ListViewCandidate>> dict = new Dictionary<string, List<ListViewCandidate>>();
			foreach (ListViewCandidate allitem in allitems)
			{
				// キーを取得
				string[] keys = null;
				if (kfdh == null) keys = DefaultKeysForDictionary(allitem.Candidate);
				else keys = kfdh(allitem.Candidate);

				foreach (string key in keys)
				{
					// 既存を取得
					List<ListViewCandidate> values = null;
					if (dict.ContainsKey(key)) values = dict[key];

					// 既存が存在しない場合
					if (values == null)
					{
						// 候補リスト抽出
						List<ListViewCandidate> items = null;
						if (mh == null) items = allitems.Where(item => DefaultMatch(key, item.Candidate)).ToList();
						else items = allitems.Where(item => mh(key, item.Candidate)).ToList();

						// ソート
						items.Sort((a, b) => string.Compare(a.Candidate.ToString(), b.Candidate.ToString(), StringComparison.OrdinalIgnoreCase));

						// 追加
						dict.Add(key, items);
					}
				}
			}

			return dict;
		}

		/// <summary>
		/// 候補を設定します。
		/// </summary>
		/// <param name="candidates">候補の配列</param>
		/// <returns>候補の配列</returns>
		public async Task SetCandidatesAsync(object[] candidates)
		{
			CandidateListViewItemDictionary = await GetCandidateListViewItemDictionaryAsync(candidates, _KeysForDictionary, _Match);
		}

		/// <summary>
		/// 候補を設定します。
		/// </summary>
		/// <param name="candidates">候補の配列</param>
		public void SetCandidates(object[] candidates)
		{
			CandidateListViewItemDictionary = GetCandidateListViewItemDictionary(candidates, _KeysForDictionary, _Match);
		}

		/// <summary>
		/// 候補リスト項目の辞書用キーを取得します。
		/// </summary>
		/// <param name="candidate">候補</param>
		/// <returns>辞書用キーの配列</returns>
		private static string[] DefaultKeysForDictionary(object candidate)
		{
			return new string[] { candidate.ToString().ToUpper()[0].ToString() };
		}

		/// <summary>
		/// キーに対して候補が相応しいか判定します。
		/// </summary>
		/// <param name="key">キー</param>
		/// <param name="candidate">候補</param>
		/// <returns>キーに対して候補が相応しいかどうか</returns>
		private static bool DefaultMatch(string key, object candidate)
		{
			return candidate.ToString().ToUpper().StartsWith(key.ToUpper());
		}

		/// <summary>
		/// 入力テキストのキーを取得します。
		/// </summary>
		/// <returns>入力テキストのキー</returns>
		private string GetInputTextKey()
		{
			if (string.IsNullOrEmpty(Text)) return null;
			return Text.ToUpper()[0].ToString();
		}

		/// <summary>
		/// 候補リスト上での決定を処理します。
		/// </summary>
		private void DecideOnCandidateListView()
		{
			// 選択項目が無い場合は何もしない
			if (CandidateListView.SelectedItems.Count == 0) return;

			// 選択項目取得
			ListViewCandidate item = CandidateListView.SelectedItems[0] as ListViewCandidate;

			// TextChangedイベントを外した上で、テキストボックスに候補名を入れる
			TextChanged -= new EventHandler(textBox_TextChanged);
			Text = item.Candidate.ToString();
			TextChanged += new EventHandler(textBox_TextChanged);

			// 決定
			DecidedCandidate = item.Candidate;
			Decided?.Invoke(this, new IncrementalTextBoxEventArgs(DecidedCandidate));

			// 候補リストを隠す
			CandidateListView.Hide();
			CandidateListView.Items.Clear();
		}

		/// <summary>
		/// テキストを設定しEnterキーを押したことにする
		/// </summary>
		/// <param name="text">テキスト</param>
		public void SetTextAndEnter(string text)
		{
			// TextChangedイベントを外した上で、テキストボックスに候補名を入れる
			TextChanged -= new EventHandler(textBox_TextChanged);
			Text = text;
			TextChanged += new EventHandler(textBox_TextChanged);

			// Enterキー押下
			HandleKeyEnter();
		}

		/// <summary>
		/// Enterキー押下を処理します。
		/// </summary>
		public void HandleKeyEnter()
		{
			// 入力文字列が無ければ何もしない
			if (string.IsNullOrEmpty(Text)) return;

			// 候補リスト項目の辞書を引く
			List<ListViewCandidate> values = _CandidateListViewItemDictionary[GetInputTextKey()];

			// 候補リスト項目を抽出
			ListViewCandidate[] items = values.Where(value => _Match(Text, value.Candidate)).ToArray();

			// 候補リスト項目が１件に絞れなければここまで
			if (items.Length != 1) return;

			// TextChangedイベントを外した上で、テキストボックスに候補名を入れる
			TextChanged -= new EventHandler(textBox_TextChanged);
			Text = items[0].Text;
			TextChanged += new EventHandler(textBox_TextChanged);

			// 決定
			DecidedCandidate = items[0].Candidate;
			Decided?.Invoke(this, new IncrementalTextBoxEventArgs(DecidedCandidate));
		}

		#region テキストボックス用イベントハンドラ

		/// <summary>
		/// テキストボックスのTextChangedイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void textBox_TextChanged(object sender, EventArgs e)
		{
			try
			{
				// 候補リスト項目の辞書が設定されていなかったら何もしない
				if (!DictionarySet) return;

				// 入力文字列が無かったら何もしない
				if (string.IsNullOrEmpty(Text)) return;

				// 入力文字列が前回と同じなら何もしない
				if (Text == PreviouseText) return;

				// キーが辞書に存在しかければ隠す
				if (!_CandidateListViewItemDictionary.ContainsKey(GetInputTextKey()))
				{
					CandidateListView.Hide();
					CandidateListView.Items.Clear();
					return;
				}

				// 辞書から候補リスト項目を引く
				List<ListViewCandidate> values = _CandidateListViewItemDictionary[GetInputTextKey()];

				// 候補リスト項目を抽出
				ListViewCandidate[] items = values.Where(value => _Match(Text, value.Candidate)).ToArray();

				// 候補リスト項目数が閾値を超えていたら隠す
				if (items.Length > CandidateListShowThreshold)
				{
					CandidateListView.Hide();
					CandidateListView.Items.Clear();
					return;
				}

				// 親コントロールにイベントを登録していない場合
				if (!ParentControlsEventRegistered)
				{
					// Formを探す
					Control form = Parent;
					while (!(form is Form))
					{
						form = form.Parent;
					}

					// 親子関係構築
					CandidateListView.Parent = form;
					CandidateListView.Parent.Controls.Add(CandidateListView);

					// イベント登録
					Control ctrl = Parent;
					while (ctrl != null)
					{
						ctrl.MouseClick += new MouseEventHandler(parentControles_MouseClick);
						ctrl = ctrl.Parent;
					}
				}

				// 候補リスト項目入れ替え
				CandidateListView.Items.Clear();
				CandidateListView.Items.AddRange(items);

				// 位置
				Point point = new Point(Left, Bottom + 1);
				CandidateListView.Location = CandidateListView.Parent.PointToClient(Parent.PointToScreen(point));
				CandidateListView.Width = Width;
				CandidateListView.BringToFront();

				// 表示
				CandidateListView.Show();
			}
			finally
			{
				// 今回の文字列を覚えておく
				PreviouseText = Text;
			}
		}

		/// <summary>
		/// テキストボックスのMouseClickイベントを処理します
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">Mouse用イベント引数</param>
		private void textBox_MouseClick(object sender, MouseEventArgs e)
		{
			// 全選択
			SelectionStart = 0;
			SelectionLength = Text.Length;

			// 候補リストに項目があるなら表示
			if (CandidateListView.Items.Count != 0) CandidateListView.Show();
		}

		/// <summary>
		/// テキストボックスのKeyDownイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">Keyイベント引数</param>
		private void textBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				// Enterキー
				case Keys.Enter:
					{
						HandleKeyEnter();
						break;
					}
				// ↑キー/↓キー
				case Keys.Up:
				case Keys.Down:
					{
						// 候補リストに入力フォーカスを移す
						CandidateListView.Focus();
						break;
					}
			}
		}

		/// <summary>
		/// テキストボックスのLeaveイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void textBox_Leave(object sender, EventArgs e)
		{
			if (!CandidateListView.Focused)
			{
				CandidateListView.Hide();
				CandidateListView.Items.Clear();
			}
		}

		#endregion

		#region 候補リスト用イベントハンドラ

		/// <summary>
		/// 候補リストのEnterイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void candidateListView_Enter(object sender, EventArgs e)
		{
			// 候補リストがあるのに選択項目が無い場合は先頭を選択
			if (CandidateListView.Items.Count != 0 && CandidateListView.SelectedItems.Count == 0) CandidateListView.Items[0].Selected = true;
		}

		/// <summary>
		/// 候補リストのKeyDownイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void candidateListView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				DecideOnCandidateListView();
			}
		}

		/// <summary>
		/// 候補リストのMouseClickイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void candidateListView_MouseClick(object sender, MouseEventArgs e)
		{
			DecideOnCandidateListView();
		}

		#endregion

		#region 親コントロール用イベントハンドラ

		/// <summary>
		/// 親コントロールのMouseClickイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void parentControles_MouseClick(object sender, MouseEventArgs e)
		{
			// 親コントロールでマウスクリックを検知した場合は候補リストを隠す
			CandidateListView.Hide();
			CandidateListView.Items.Clear();
		}

		#endregion
	}

	/// <summary>
	/// IncrementalTextBox用のイベントのデータを提供します。
	/// </summary>
	public class IncrementalTextBoxEventArgs : EventArgs
	{
		/// <summary>
		/// 決定されたオブジェクト
		/// </summary>
		public object Decided { get; set; }

		/// <summary>
		/// IncrementalTextBoxクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="decided">決定されたオブジェクト</param>
		public IncrementalTextBoxEventArgs(object decided)
		{
			Decided = decided;
		}
	}
}
