using MagicalNuts.AveragePriceMove;
using MagicalNuts.Primitive;
using MagicalNuts.UI.AveragePriceMove;
using System.Runtime.Versioning;
using Utf8Json;

namespace AveragePriceMoveSample
{
	[SupportedOSPlatform("windows")]
	public partial class Form1 : Form
	{
		private AveragePriceMoveChart chart = null;
		private AnnualData<decimal>[] AnnualDataArrayN225 = null;
		private AnnualData<decimal>[] AnnualDataArray1718 = null;

		public Form1()
		{
			InitializeComponent();

			// AveragePriceMoveChart（デザイナーで編集できないため）
			chart = new AveragePriceMoveChart();
			chart.Dock = DockStyle.Fill;
			Controls.Add(chart);

			comboBox1.SelectedIndex = 0;
		}

		private async void Form1_Load(object sender, EventArgs e)
		{
			// 株価JSONロード
			StreamReader sr = new StreamReader("N225.json");
			string strN225 = sr.ReadToEnd();
			sr.Close();

			sr = new StreamReader("1718.json");
			string str1718 = sr.ReadToEnd();
			sr.Close();

			// デシリアライズ
			Candle[] candlesN225 = JsonSerializer.Deserialize<Candle[]>(strN225);
			Candle[] candles1718 = JsonSerializer.Deserialize<Candle[]>(str1718);

			// 平均値動きの推移取得
			AnnualDataArrayN225 = Controller.GetAveragePriceMove(candlesN225, PriceType.Close, new DateTime(2021, 1, 1), 10);
			AnnualDataArray1718 = Controller.GetAveragePriceMove(candles1718, PriceType.Close, new DateTime(2021, 1, 1), 10);

			// 平均値動きの推移グラフ表示
			SetChart();
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			// 平均値動きの推移グラフ表示
			if (AnnualDataArrayN225 != null && AnnualDataArray1718 != null) SetChart();
		}

		private void SetChart()
		{
			switch (comboBox1.SelectedIndex)
			{
				// １銘柄
				case 0:
					chart.SetAveragePriceMove(AnnualDataArrayN225);
					break;
				// ２銘柄
				case 1:
					chart.SetAveragePriceMove(AnnualDataArrayN225, AnnualDataArray1718);
					break;
			}
		}
	}
}
