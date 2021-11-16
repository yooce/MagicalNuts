using MagicalNuts.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace MagicalNuts.AveragePriceMove.Test
{
	public class ControllerTest
	{
		private static readonly Candle[] data =
		{
			new Candle(new DateTime(2017, 1, 4), 0, 0, 0, 1554.48m),
			new Candle(new DateTime(2017, 1, 5), 0, 0, 0, 1555.68m),
			new Candle(new DateTime(2017, 1, 6), 0, 0, 0, 1553.32m),
			new Candle(new DateTime(2017, 1, 10), 0, 0, 0, 1542.31m),
			new Candle(new DateTime(2017, 1, 11), 0, 0, 0, 1550.40m),
			new Candle(new DateTime(2017, 1, 12), 0, 0, 0, 1535.41m),
			new Candle(new DateTime(2017, 1, 13), 0, 0, 0, 1544.89m),
			new Candle(new DateTime(2017, 1, 16), 0, 0, 0, 1530.64m),
			new Candle(new DateTime(2017, 1, 17), 0, 0, 0, 1509.10m),
			new Candle(new DateTime(2017, 1, 18), 0, 0, 0, 1513.86m),
			new Candle(new DateTime(2017, 1, 19), 0, 0, 0, 1528.15m),
			new Candle(new DateTime(2017, 1, 20), 0, 0, 0, 1533.46m),
			new Candle(new DateTime(2017, 1, 23), 0, 0, 0, 1514.63m),
			new Candle(new DateTime(2017, 1, 24), 0, 0, 0, 1506.33m),
			new Candle(new DateTime(2017, 1, 25), 0, 0, 0, 1521.58m),
			new Candle(new DateTime(2017, 1, 26), 0, 0, 0, 1545.01m),
			new Candle(new DateTime(2017, 1, 27), 0, 0, 0, 1549.25m),
			new Candle(new DateTime(2017, 1, 30), 0, 0, 0, 1543.77m),
			new Candle(new DateTime(2017, 1, 31), 0, 0, 0, 1521.67m),

			new Candle(new DateTime(2018, 1, 4), 0, 0, 0, 1863.82m),
			new Candle(new DateTime(2018, 1, 5), 0, 0, 0, 1880.34m),
			new Candle(new DateTime(2018, 1, 9), 0, 0, 0, 1889.29m),
			new Candle(new DateTime(2018, 1, 10), 0, 0, 0, 1892.11m),
			new Candle(new DateTime(2018, 1, 11), 0, 0, 0, 1888.09m),
			new Candle(new DateTime(2018, 1, 12), 0, 0, 0, 1876.24m),
			new Candle(new DateTime(2018, 1, 15), 0, 0, 0, 1883.90m),
			new Candle(new DateTime(2018, 1, 16), 0, 0, 0, 1894.25m),
			new Candle(new DateTime(2018, 1, 17), 0, 0, 0, 1890.82m),
			new Candle(new DateTime(2018, 1, 18), 0, 0, 0, 1876.86m),
			new Candle(new DateTime(2018, 1, 19), 0, 0, 0, 1889.74m),
			new Candle(new DateTime(2018, 1, 22), 0, 0, 0, 1891.92m),
			new Candle(new DateTime(2018, 1, 23), 0, 0, 0, 1911.07m),
			new Candle(new DateTime(2018, 1, 24), 0, 0, 0, 1901.23m),
			new Candle(new DateTime(2018, 1, 25), 0, 0, 0, 1884.56m),
			new Candle(new DateTime(2018, 1, 26), 0, 0, 0, 1879.39m),
			new Candle(new DateTime(2018, 1, 29), 0, 0, 0, 1880.45m),
			new Candle(new DateTime(2018, 1, 30), 0, 0, 0, 1858.13m),
			new Candle(new DateTime(2018, 1, 31), 0, 0, 0, 1836.71m),

			new Candle(new DateTime(2019, 1, 4), 0, 0, 0, 1471.16m),
			new Candle(new DateTime(2019, 1, 7), 0, 0, 0, 1512.53m),
			new Candle(new DateTime(2019, 1, 8), 0, 0, 0, 1518.43m),
			new Candle(new DateTime(2019, 1, 9), 0, 0, 0, 1535.11m),
			new Candle(new DateTime(2019, 1, 10), 0, 0, 0, 1522.01m),
			new Candle(new DateTime(2019, 1, 11), 0, 0, 0, 1529.73m),
			new Candle(new DateTime(2019, 1, 15), 0, 0, 0, 1542.72m),
			new Candle(new DateTime(2019, 1, 16), 0, 0, 0, 1537.77m),
			new Candle(new DateTime(2019, 1, 17), 0, 0, 0, 1543.20m),
			new Candle(new DateTime(2019, 1, 18), 0, 0, 0, 1557.59m),
			new Candle(new DateTime(2019, 1, 21), 0, 0, 0, 1566.37m),
			new Candle(new DateTime(2019, 1, 22), 0, 0, 0, 1556.43m),
			new Candle(new DateTime(2019, 1, 23), 0, 0, 0, 1547.03m),
			new Candle(new DateTime(2019, 1, 24), 0, 0, 0, 1552.60m),
			new Candle(new DateTime(2019, 1, 25), 0, 0, 0, 1566.10m),
			new Candle(new DateTime(2019, 1, 28), 0, 0, 0, 1555.51m),
			new Candle(new DateTime(2019, 1, 29), 0, 0, 0, 1557.09m),
			new Candle(new DateTime(2019, 1, 30), 0, 0, 0, 1550.76m),
			new Candle(new DateTime(2019, 1, 31), 0, 0, 0, 1567.49m),

			new Candle(new DateTime(2020, 1, 6), 0, 0, 0, 1697.49m),
			new Candle(new DateTime(2020, 1, 7), 0, 0, 0, 1725.05m),
			new Candle(new DateTime(2020, 1, 8), 0, 0, 0, 1701.40m),
			new Candle(new DateTime(2020, 1, 9), 0, 0, 0, 1729.05m),
			new Candle(new DateTime(2020, 1, 10), 0, 0, 0, 1735.16m),
			new Candle(new DateTime(2020, 1, 14), 0, 0, 0, 1740.53m),
			new Candle(new DateTime(2020, 1, 15), 0, 0, 0, 1731.06m),
			new Candle(new DateTime(2020, 1, 16), 0, 0, 0, 1728.72m),
			new Candle(new DateTime(2020, 1, 17), 0, 0, 0, 1735.44m),
			new Candle(new DateTime(2020, 1, 20), 0, 0, 0, 1744.16m),
			new Candle(new DateTime(2020, 1, 21), 0, 0, 0, 1734.97m),
			new Candle(new DateTime(2020, 1, 22), 0, 0, 0, 1744.13m),
			new Candle(new DateTime(2020, 1, 23), 0, 0, 0, 1730.50m),
			new Candle(new DateTime(2020, 1, 24), 0, 0, 0, 1730.44m),
			new Candle(new DateTime(2020, 1, 27), 0, 0, 0, 1702.57m),
			new Candle(new DateTime(2020, 1, 28), 0, 0, 0, 1692.28m),
			new Candle(new DateTime(2020, 1, 29), 0, 0, 0, 1699.95m),
			new Candle(new DateTime(2020, 1, 30), 0, 0, 0, 1674.77m),
			new Candle(new DateTime(2020, 1, 31), 0, 0, 0, 1684.44m),

			new Candle(new DateTime(2021, 1, 4), 0, 0, 0, 1794.59m),
			new Candle(new DateTime(2021, 1, 5), 0, 0, 0, 1791.22m),
			new Candle(new DateTime(2021, 1, 6), 0, 0, 0, 1796.18m),
			new Candle(new DateTime(2021, 1, 7), 0, 0, 0, 1826.30m),
			new Candle(new DateTime(2021, 1, 8), 0, 0, 0, 1854.94m),
			new Candle(new DateTime(2021, 1, 12), 0, 0, 0, 1857.94m),
			new Candle(new DateTime(2021, 1, 13), 0, 0, 0, 1864.40m),
			new Candle(new DateTime(2021, 1, 14), 0, 0, 0, 1873.28m),
			new Candle(new DateTime(2021, 1, 15), 0, 0, 0, 1856.61m),
			new Candle(new DateTime(2021, 1, 18), 0, 0, 0, 1845.49m),
			new Candle(new DateTime(2021, 1, 19), 0, 0, 0, 1855.84m),
			new Candle(new DateTime(2021, 1, 20), 0, 0, 0, 1849.58m),
			new Candle(new DateTime(2021, 1, 21), 0, 0, 0, 1860.64m),
			new Candle(new DateTime(2021, 1, 22), 0, 0, 0, 1856.64m),
			new Candle(new DateTime(2021, 1, 25), 0, 0, 0, 1862.00m),
			new Candle(new DateTime(2021, 1, 26), 0, 0, 0, 1848.00m),
			new Candle(new DateTime(2021, 1, 27), 0, 0, 0, 1860.07m),
			new Candle(new DateTime(2021, 1, 28), 0, 0, 0, 1838.85m),
			new Candle(new DateTime(2021, 1, 29), 0, 0, 0, 1808.78m),
		};

		[Fact]
		public void GetUpDownRatiosTest()
		{
			decimal[] expected =
			{
				1,
				0.998122134m,
				1.002769062m,
				1.016768921m,
				1.01568198m,
				1.001617303m,
				1.003476969m,
				1.004762926m,
				0.99110117m,
				0.994010589m,
				1.005608267m,
				0.996626864m,
				1.005979736m,
				0.997850202m,
				1.002886936m,
				0.992481203m,
				1.006531385m,
				0.988591827m,
				0.983647388m,
			};
			Candle[] candles = data.Where(candle => candle.DateTime.Year == 2021).ToArray();
			MethodInfo mi = typeof(Controller).GetMethod("GetUpDownRatios", BindingFlags.NonPublic | BindingFlags.Static);
			Controller.UpDownRatio[] uprs = (Controller.UpDownRatio[])mi.Invoke(null, new object[] { candles, PriceType.Close });
			for (int i = 0; i < uprs.Length; i++)
			{
				Assert.Equal(Math.Round(expected[i], 4), Math.Round(uprs[i].Ratio, 4));
			}
		}

		[Fact]
		public void DistributeAnnualUpDownRatiosTest()
		{
			int[,] expected = new int[,]
			{
				{ 1, 4, 4 },
				{ 1, 5, 3 },
				{ 1, 6, 3 },
				{ 1, 7, 3 },
				{ 1, 8, 3 },
				{ 1, 9, 3 },
				{ 1, 10, 4 },
				{ 1, 11, 3 },
				{ 1, 12, 3 },
				{ 1, 13, 2 },
				{ 1, 14, 2 },
				{ 1, 15, 4 },
				{ 1, 16, 4 },
				{ 1, 17, 4 },
				{ 1, 18, 4 },
				{ 1, 19, 3 },
				{ 1, 20, 3 },
				{ 1, 21, 3 },
				{ 1, 22, 4 },
				{ 1, 23, 4 },
				{ 1, 24, 4 },
				{ 1, 25, 4 },
				{ 1, 26, 3 },
				{ 1, 27, 3 },
				{ 1, 28, 3 },
				{ 1, 29, 4 },
				{ 1, 30, 4 },
				{ 1, 31, 4 },
			};

			MethodInfo mi = typeof(Controller).GetMethod("GetUpDownRatios", BindingFlags.NonPublic | BindingFlags.Static);
			Controller.UpDownRatio[] uprs = (Controller.UpDownRatio[])mi.Invoke(null, new object[] { data, PriceType.Close });

			mi = typeof(Controller).GetMethod("DistributeAnnualUpDownRatios", BindingFlags.NonPublic | BindingFlags.Static);
			AnnualData<List<Controller.UpDownRatio>>[] audrs = (AnnualData<List<Controller.UpDownRatio>>[])mi.Invoke(null
				, new object[] { uprs, new DateTime(2017, 1, 1), new DateTime(2021, 1, 31) });
			for (int i = 0; i < audrs.Length; i++)
			{
				Assert.Equal(expected[i, 0], audrs[i].Month);
				Assert.Equal(expected[i, 1], audrs[i].Day);
				Assert.Equal(expected[i, 2], audrs[i].Data.Count);
			}
		}

		[Fact]
		public void GetAnnualAverageRatioTest()
		{
			decimal[] expected =
			{
				1.022804988m,
				1.002585871m,
				1.028062393m,
				1.020375108m,
				1.00195766m,
				1.010665377m,
				0.997351181m,
				1.002731002m,
				0.995224215m,
				1.004825608m,
				1.003928871m,
				0.999558657m,
				0.997927393m,
				0.997883767m,
				0.999776632m,
				1.007303413m,
				1.001708772m,
				1.002115879m,
				0.999484387m,
				0.995997081m,
				0.998234235m,
				1.003234491m,
				1.001712108m,
				0.997723325m,
				0.991928668m,
				0.997439873m,
				0.991428957m,
				0.997679716m,
			};

			MethodInfo mi = typeof(Controller).GetMethod("GetUpDownRatios", BindingFlags.NonPublic | BindingFlags.Static);
			Controller.UpDownRatio[] uprs = (Controller.UpDownRatio[])mi.Invoke(null, new object[] { data, PriceType.Close });

			mi = typeof(Controller).GetMethod("DistributeAnnualUpDownRatios", BindingFlags.NonPublic | BindingFlags.Static);
			AnnualData<List<Controller.UpDownRatio>>[] audrs = (AnnualData<List<Controller.UpDownRatio>>[])mi.Invoke(null
				, new object[] { uprs, new DateTime(2017, 1, 1), new DateTime(2021, 1, 31) });

			mi = typeof(Controller).GetMethod("GetAnnualAverageRatio", BindingFlags.NonPublic | BindingFlags.Static);
			AnnualData<decimal>[] aars = (AnnualData<decimal>[])mi.Invoke(null, new object[] { audrs });

			for (int i = 0; i < aars.Length; i++)
			{
				Assert.Equal(Math.Round(expected[i], 4), Math.Round(aars[i].Data, 4));
			}
		}

		[Fact]
		public void GetAveragePriceMoveTest()
		{
			decimal[] expected =
			{
				1.022804988m,
				1.02544983m,
				1.054226406m,
				1.075706383m,
				1.077812251m,
				1.089307525m,
				1.086422146m,
				1.089389167m,
				1.084186479m,
				1.089418338m,
				1.093698522m,
				1.093215826m,
				1.090950019m,
				1.088641315m,
				1.088398147m,
				1.096347168m,
				1.098220575m,
				1.100544277m,
				1.099976822m,
				1.095573705m,
				1.093639179m,
				1.097176545m,
				1.09905503m,
				1.096552838m,
				1.087702196m,
				1.08491754m,
				1.075618666m,
				1.073122924m,
			};
			AnnualData<decimal>[] aapms
				= (AnnualData<decimal>[])Controller.GetAveragePriceMove(data, PriceType.Close, new DateTime(2022, 1, 1), 5);
			for (int i = 0; i < aapms.Length; i++)
			{
				Assert.Equal(Math.Round(expected[i], 4), Math.Round(aapms[i].Data, 4));
			}
		}
	}
}
