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
		private List<IStrategy> Strategies = null;
		private List<IFeeCalculator> Fees = null;
		private HistoricalAssetsChart chart = null;

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
			Fees = new PluginManager<IFeeCalculator>(null).Plugins;

			// コンボボックスに追加
			comboBoxStrategy.Items.AddRange(Strategies.Select(strategy => strategy.Name).ToArray());
			comboBoxFee.Items.AddRange(Fees.Select(fee => fee.Name).ToArray());
		}

		private async void buttonStart_Click(object sender, EventArgs e)
		{
			if (Strategies[comboBoxStrategy.SelectedIndex] is DonchianChannelBreakOut) await BackTestSingle("1717");
			else if (Strategies[comboBoxStrategy.SelectedIndex] is DonchianChannelBreakOutMulti) await BackTestMulti();
			else if (Strategies[comboBoxStrategy.SelectedIndex] is DollarCostAveraging) await BackTestSingle("N225");
		}

		public async Task BackTestSingle(string code)
		{
			if (comboBoxFee.SelectedIndex < 0 || comboBoxStrategy.SelectedIndex < 0) return;

			DateTime begin = dateTimePickerBegin.Value.Date;
			DateTime end = dateTimePickerEnd.Value.Date;

			// バックテスト
			IStrategy strategy = Strategies[comboBoxStrategy.SelectedIndex];

			Stock stock = new Stock(code, code, 100);

			// ロウソク足取得
			StreamReader sr = new StreamReader(code + ".json");
			string strN225 = sr.ReadToEnd();
			sr.Close();
			Candle[] org_candles = JsonSerializer.Deserialize<Candle[]>(strN225);
			org_candles.Reverse();

			// ロウソク足のコレクション
			IndicatorCandleCollection candles = new IndicatorCandleCollection(org_candles.ToList(), stock.Code);

			// 戦略準備
			await strategy.SetUpAsync();

			Arguments args = new Arguments(strategy, new BackTestCandleCollection(candles, stock), begin, end, Fees[comboBoxFee.SelectedIndex]);

			// バックテスト
			Controller controller = new Controller();
			BackTestResult result = await controller.BackTestAsync<BackTestResult>(args);

			// 結果表示
			Set(result);
		}

		public async Task BackTestMulti()
		{
			if (comboBoxFee.SelectedIndex < 0 || comboBoxStrategy.SelectedIndex < 0) return;

			DateTime begin = dateTimePickerBegin.Value.Date;
			DateTime end = dateTimePickerEnd.Value.Date;

			// 戦略準備
			IStrategy strategy = Strategies[comboBoxStrategy.SelectedIndex];
			await strategy.SetUpAsync();

			List<BackTestCandleCollection> originalStockSets = new List<BackTestCandleCollection>();
			StreamReader sr = new StreamReader("1717.json");
			string strN225 = sr.ReadToEnd();
			sr.Close();
			Candle[] candles = JsonSerializer.Deserialize<Candle[]>(strN225);
			candles.Reverse();

			originalStockSets.Add(new BackTestCandleCollection(new IndicatorCandleCollection(candles.ToList(), "1717"), new Stock("1717", "1717", 100)));

			sr = new StreamReader("1718.json");
			strN225 = sr.ReadToEnd();
			sr.Close();
			candles = JsonSerializer.Deserialize<Candle[]>(strN225);
			candles.Reverse();

			originalStockSets.Add(new BackTestCandleCollection(new IndicatorCandleCollection(candles.ToList(), "1718"), new Stock("1718", "1718", 100)));


			Arguments args = new Arguments(strategy, originalStockSets.ToArray(), begin, end, Fees[comboBoxFee.SelectedIndex]);

			/*
			// 当初資金
			Core.Strategies.StrategyStatus state
				= new Core.Strategies.StrategyStatus(((Core.Strategies.StrategyProperties)strategy.Properties).InitialBalance);
			state.SetOriginalStockSets(originalStockSets.ToList());
			//*/

			// バックテスト
			Controller controller = new Controller();
			BackTestResult result = await controller.BackTestAsync<BackTestResult>(args);

			// 結果表示
			Set(result);
		}

		private async Task<List<BackTestCandleCollection>> GetOriginalStockSets(IStrategy strategy
			, List<Stock> stocks, DateTime begin, DateTime end)
		{
			List<BackTestCandleCollection> osss = new List<BackTestCandleCollection>();
			for (int i = 0; i < stocks.Count; i++)
			{
				Console.WriteLine(stocks[i].ToString());

				StreamReader sr = new StreamReader("1718.json");
				string strN225 = sr.ReadToEnd();
				sr.Close();
				Candle[] candles = JsonSerializer.Deserialize<Candle[]>(strN225);
				candles.Reverse();

				osss.Add(new BackTestCandleCollection(new IndicatorCandleCollection(candles.ToList(), stocks[i].Code), stocks[i]));
			}
			return osss;
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

			PropertyEditForm form = new PropertyEditForm(Fees[comboBoxFee.SelectedIndex]);
			form.ShowDialog(this);
		}

		public void Set(BackTestResult result)
		{
			// 結果表示

			// チャート
			chart.SetHistoricalAssetsList(result.HistoricalAssetsArray);

			// ポジションリスト
			dataGridViewPosition.SetupColumns();
			List<Position> positions = new List<Position>(result.Positions);
			dataGridViewPosition.DataSource = positions;

			// バックテスト結果
			backTestResult.SetupColumns();
			backTestResult.DataSource = new BackTestResult[] { result };
		}
	}
}
