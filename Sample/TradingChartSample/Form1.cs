using MagicalNuts.Primitive;
using MagicalNuts.UI.Base;
using MagicalNuts.UI.TradingChart;
using MagicalNuts.UI.TradingChart.Plotter;
using System.Runtime.Versioning;

namespace TradingChartSample
{
	[SupportedOSPlatform("windows")]
	public partial class Form1 : Form
	{
		private TradingChart chart = null;
		private MovingAveragePlotter MovingAveragePlotter = null;
		private BollingerBandPlotter BollingerBandPlotter = null;
		private AtrPlotter AtrPlotter = null;
		private MacdPlotter MacdPlotter = null;

		public Form1()
		{
			InitializeComponent();

			// チャート
			chart = new TradingChart();
			chart.Dock = DockStyle.Fill;
			panel1.Controls.Add(chart);

			comboBox1.SelectedIndex = 0;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// 株価
			System.IO.StreamReader sr = new System.IO.StreamReader("N225.json");
			string str = sr.ReadToEnd();
			sr.Close();
			Candle[] candles = Utf8Json.JsonSerializer.Deserialize<Candle[]>(str);

			// チャート
			chart.SetUp();
			chart.SetDailyCandles(null, candles);
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			// 足種変更
			chart.CandlePeriod = (CandlePeriod)comboBox1.SelectedIndex;
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			// 拡縮
			chart.ScreenCandleNum = (int)numericUpDown1.Value;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			// 拡大
			if (numericUpDown1.Value - 50 < numericUpDown1.Minimum) numericUpDown1.Value = numericUpDown1.Minimum;
			else numericUpDown1.Value -= 50;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			// 縮小
			if (numericUpDown1.Value + 50 > numericUpDown1.Maximum) numericUpDown1.Value = numericUpDown1.Maximum;
			else numericUpDown1.Value += 50;
		}

		private async void toolStripMenuItemMa_Click(object sender, EventArgs e)
		{
			// 移動平均
			MovingAveragePlotter = await SetPlotter(MovingAveragePlotter);
			toolStripMenuItemMa.Checked = MovingAveragePlotter != null;
		}

		private async void toolStripMenuItemBb_Click(object sender, EventArgs e)
		{
			// ボリンジャーバンド
			BollingerBandPlotter = await SetPlotter(BollingerBandPlotter);
			toolStripMenuItemBb.Checked = BollingerBandPlotter != null;
		}

		private async void toolStripMenuItemAtr_Click(object sender, EventArgs e)
		{
			// ATR
			AtrPlotter = await SetPlotter(AtrPlotter);
			toolStripMenuItemAtr.Checked = AtrPlotter != null;
		}

		private async void toolStripMenuItemMacd_Click(object sender, EventArgs e)
		{
			// MACD
			MacdPlotter = await SetPlotter(MacdPlotter);
			toolStripMenuItemMacd.Checked = MacdPlotter != null;
		}

		private async Task<T> SetPlotter<T>(T plotter) where T : class, IPlotter, new()
		{
			if (plotter == null)
			{
				// 追加
				plotter = new T();
				await plotter.SetUpAsync();

				PropertyEditForm form = new PropertyEditForm(plotter);
				if (form.ShowDialog() != DialogResult.OK) return null;

				chart.AddPlotter(plotter);
				return plotter;
			}
			else
			{
				// 除去
				chart.RemovePlotter(plotter);
				return null;
			}
		}
	}
}
