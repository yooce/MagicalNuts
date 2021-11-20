using MagicalNuts.Primitive;
using MagicalNuts.ShareholderIncentive;
using MagicalNuts.UI.ShareholderIncentive;
using System.Runtime.Versioning;
using Utf8Json;

namespace ShareholderIncentiveSample
{
	[SupportedOSPlatform("windows")]
	public partial class Form1 : Form
	{
		private LastDayPriceMoveChart chart = null;

		public Form1()
		{
			InitializeComponent();

			// チャート準備
			chart = new LastDayPriceMoveChart();
			chart.Dock = DockStyle.Fill;
			splitContainer.Panel1.Controls.Add(chart);
		}

		private async void Form1_Load(object sender, EventArgs e)
		{
			// 株価
			StreamReader sr = new StreamReader("1718.json");
			string str = sr.ReadToEnd();
			sr.Close();
			Candle[] candles = JsonSerializer.Deserialize<Candle[]>(str);

			// 株主優待権利確定日
			DateOfRightAllotment dra = new DateOfRightAllotment("1718", 6);

			// カレンダー
			SampleCalendar calendar = new SampleCalendar();
			await calendar.SetUpAsync();

			// 株主優待権利付き最終日前後の値動きのヒストリカルデータ
			Controller controller = new Controller();
			Arguments args = new Arguments(dra, 2020, 10, candles.ToArray(), calendar);
			HistoricalData hd = controller.GetHistoricalData(args);

			// エントリー日とイグジット日による期待値
			List<EntryExitExpectedValue> eeevs = controller.GetEntryExitExpectedValues(hd).ToList();
			eeevs.Sort(EntryExitExpectedValue.Compare);

			// 結果表示
			entryExitExpectedValueGridView.SetupColumns();
			historicalDataGridView.SetHistoricalData(hd);
			entryExitExpectedValueGridView.DataSource = eeevs;
			chart.SetHistoricalData(hd);
		}
	}
}
