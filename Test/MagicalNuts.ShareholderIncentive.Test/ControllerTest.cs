using MagicalNuts.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MagicalNuts.ShareholderIncentive.Test
{
	public class ControllerTest
	{
		private Candle[] candles
		{
			get
			{
				List<Candle> candles = new List<Candle>();
				foreach (string line in System.IO.File.ReadLines("1718.csv"))
				{
					string[] columns = line.Split(',');
					candles.Add(new Candle(DateTime.Parse(columns[0]), decimal.Parse(columns[1]), decimal.Parse(columns[2])
						, decimal.Parse(columns[3]), decimal.Parse(columns[4]), decimal.Parse(columns[5])));
				}
				return candles.ToArray();
			}
		}

		private Arguments args
		{
			get
			{
				DateOfRightAllotment dra = new DateOfRightAllotment();
				dra.Month = 6;
				dra.Day = null;
				Arguments args = new Arguments(dra, 2020, 3, candles, new Calendar(), PriceType.Open);
				return args;
			}
		}

		[Fact]
		public void GetDayOfRightAllotmentTest()
		{
			Calendar calendar = new Calendar();
			DateOfRightAllotment dra = new DateOfRightAllotment();

			dra.Month = 9;
			dra.Day = null;
			Assert.Equal(new DateTime(2021, 9, 30), Controller.GetDayOfRightAllotment(dra, 2021, calendar));

			dra.Month = 10;
			dra.Day = null;
			Assert.Equal(new DateTime(2021, 10, 29), Controller.GetDayOfRightAllotment(dra, 2021, calendar));

			dra.Month = 10;
			dra.Day = 20;
			Assert.Equal(new DateTime(2021, 10, 20), Controller.GetDayOfRightAllotment(dra, 2021, calendar));

			dra.Month = 6;
			dra.Day = 20;
			Assert.Equal(new DateTime(2021, 6, 18), Controller.GetDayOfRightAllotment(dra, 2021, calendar));
		}

		[Fact]
		public void GetLastDayWithRightsTest()
		{
			Calendar calendar = new Calendar();
			DateOfRightAllotment dra = new DateOfRightAllotment();

			dra.Month = 12;
			dra.Day = 30;
			Assert.Equal(new DateTime(2008, 12, 24), Controller.GetLastDayWithRights(dra, 2008, calendar));

			dra.Month = 12;
			dra.Day = null;
			Assert.Equal(new DateTime(2017, 12, 26), Controller.GetLastDayWithRights(dra, 2017, calendar));

			dra.Month = 10;
			dra.Day = null;
			Assert.Equal(new DateTime(2021, 10, 27), Controller.GetLastDayWithRights(dra, 2021, calendar));
		}

		[Fact]
		public void GetHistoricalDataTest()
		{
			// 正解読み込み
			List<List<decimal>> expected = new List<List<decimal>>();
			foreach (string line in System.IO.File.ReadLines("expected.csv"))
			{
				string[] columns = line.Split(',');
				expected.Add(new List<decimal>() { decimal.Parse(columns[0]), decimal.Parse(columns[1]), decimal.Parse(columns[2])
					, decimal.Parse(columns[3]) });
			}

			// 実行
			Controller controller = new Controller();
			HistoricalData hd = controller.GetHistoricalData(args);

			// 検証
			for (int i = 0; i < hd.AnnualDataList.Count; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					Assert.Equal(Math.Round(expected[i][j], 4), Math.Round(hd.AnnualDataList[j][i - args.BeforeNum].Value, 4));
				}
				Assert.Equal(Math.Round(expected[i][3], 4), Math.Round(hd.AverageAnnualData[i - args.BeforeNum].Value, 4));
			}
		}

		[Fact]
		public void GetGetEntryExitExpectedValuesTest()
		{
			// 実行
			Controller controller = new Controller();
			HistoricalData hd = controller.GetHistoricalData(args);
			List<EntryExitExpectedValue> eeevs = controller.GetEntryExitExpectedValues(hd).ToList();

			// ソート
			eeevs.Sort(EntryExitExpectedValue.Compare);

			// 検証
			Assert.Equal(60, eeevs[0].EntryDaysBefore);
			Assert.Equal(0, eeevs[0].ExitDaysBefore);
			Assert.Equal(1, eeevs[0].WinRatio);
			Assert.Equal(Math.Round(0.146364239m, 4), Math.Round(eeevs[0].AverageProfit, 4));
			Assert.Equal(0, eeevs[0].AverageLoss);
			Assert.Equal(Math.Round(0.146364239m, 4), Math.Round(eeevs[0].ExpectedValue, 4));
			Assert.Equal(Math.Round(0.609850995m, 4), Math.Round(eeevs[0].AnnualReturn, 4));
		}
	}
}
