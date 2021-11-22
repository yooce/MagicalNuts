using MagicalNuts.BackTest;
using MagicalNuts.Indicator;
using MagicalNuts.Primitive;
using MagicalNuts.UI.BackTest;
using MagicalNuts.UI.Base;
using System.Data;
using System.Runtime.Versioning;
using Utf8Json;

namespace BackTestSample
{
	[SupportedOSPlatform("windows")]
	public partial class Form1 : Form
	{
		private HistoricalAssetsChart chart = null;

		private List<IStrategy> Strategies = null;
		private List<IFeeCalculator> FeeCalculators = null;

		public Form1()
		{
			InitializeComponent();

			dateTimePickerBegin.Value = new DateTime(2011, 1, 1);
			dateTimePickerEnd.Value = new DateTime(2020, 12, 31);

			// チャート
			chart = new HistoricalAssetsChart();
			chart.Dock = DockStyle.Fill;
			splitContainerSingleTop.Panel1.Controls.Add(chart);
		}

		private async void BackTestForm_Load(object sender, EventArgs e)
		{
			// 戦略
			Strategies = new PluginManager<IStrategy>(null).Plugins;
			
			// 手数料
			FeeCalculators = new PluginManager<IFeeCalculator>(null).Plugins;

			// コンボボックスに追加
			comboBoxStrategy.Items.AddRange(Strategies.Select(strategy => strategy.Name).ToArray());
			comboBoxFee.Items.AddRange(FeeCalculators.Select(fee => fee.Name).ToArray());
		}

		private async void buttonStart_Click(object sender, EventArgs e)
		{
			if (Strategies[comboBoxStrategy.SelectedIndex] is DonchianChannelBreakOut) await BackTestSingle("1717");
			else if (Strategies[comboBoxStrategy.SelectedIndex] is DonchianChannelBreakOutMulti) await BackTestMulti();
			else if (Strategies[comboBoxStrategy.SelectedIndex] is DollarCostAveraging) await BackTestSingle("N225");
		}

		public async Task BackTestSingle(string code)
		{
			if (comboBoxStrategy.SelectedIndex < 0 || comboBoxFee.SelectedIndex < 0) return;

			DateTime begin = dateTimePickerBegin.Value.Date;
			DateTime end = dateTimePickerEnd.Value.Date;

			// ロウソク足
			StreamReader sr = new StreamReader(code + ".json");
			string str = sr.ReadToEnd();
			sr.Close();
			Candle[] candles = JsonSerializer.Deserialize<Candle[]>(str);
			candles.Reverse();

			// 戦略
			IStrategy strategy = Strategies[comboBoxStrategy.SelectedIndex];
			await strategy.SetUpAsync();

			// バックテスト
			Arguments args = new Arguments(strategy
				, new BackTestCandleCollection(candles.ToList(), new Stock(code, code, 100)), begin, end, FeeCalculators[comboBoxFee.SelectedIndex]);
			Controller controller = new Controller();
			BackTestResult result = await controller.BackTestAsync<BackTestResult>(args);

			// 結果表示
			SetResult(result);
		}

		public async Task BackTestMulti()
		{
			if (comboBoxStrategy.SelectedIndex < 0 || comboBoxFee.SelectedIndex < 0) return;

			DateTime begin = dateTimePickerBegin.Value.Date;
			DateTime end = dateTimePickerEnd.Value.Date;

			// ロウソク足
			List<BackTestCandleCollection> stock_candles = new List<BackTestCandleCollection>();
			StreamReader sr = new StreamReader("1717.json");
			string str = sr.ReadToEnd();
			sr.Close();
			Candle[] candles = JsonSerializer.Deserialize<Candle[]>(str);
			candles.Reverse();
			stock_candles.Add(new BackTestCandleCollection(candles.ToList(), new Stock("1717", "1717", 100)));

			sr = new StreamReader("1718.json");
			str = sr.ReadToEnd();
			sr.Close();
			candles = JsonSerializer.Deserialize<Candle[]>(str);
			candles.Reverse();
			stock_candles.Add(new BackTestCandleCollection(candles.ToList(), new Stock("1718", "1718", 100)));

			// 戦略
			IStrategy strategy = Strategies[comboBoxStrategy.SelectedIndex];
			await strategy.SetUpAsync();

			// バックテスト
			Arguments args = new Arguments(strategy, stock_candles.ToArray(), begin, end, FeeCalculators[comboBoxFee.SelectedIndex]);
			Controller controller = new Controller();
			BackTestResult result = await controller.BackTestAsync<BackTestResult>(args);

			// 結果表示
			SetResult(result);
		}

		private void buttonStrategy_Click(object sender, EventArgs e)
		{
			if (comboBoxStrategy.SelectedIndex < 0) return;

			PropertyEditForm form = new PropertyEditForm(Strategies[comboBoxStrategy.SelectedIndex]);
			form.ShowDialog(this);
		}

		private void buttonFee_Click(object sender, EventArgs e)
		{
			if (comboBoxFee.SelectedIndex < 0) return;

			PropertyEditForm form = new PropertyEditForm(FeeCalculators[comboBoxFee.SelectedIndex]);
			form.ShowDialog(this);
		}

		public void SetResult(BackTestResult result)
		{
			// 資産推移
			chart.SetHistoricalAssetsList(result.HistoricalAssetsArray);

			// ポジション履歴
			positionGridView.SetupColumns();
			positionGridView.DataSource = result.Positions;

			// バックテスト結果
			backTestResultGridView.SetupColumns();
			backTestResultGridView.DataSource = new BackTestResult[] { result };
		}
	}
}
