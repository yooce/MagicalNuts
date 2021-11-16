using System;
using System.Collections.Generic;
using Xunit;

namespace MagicalNuts.Primitive.Test
{
	public class MovingAverageTest
	{
		[Fact]
		public void GetSmaTest()
		{
			List<decimal> data = new List<decimal>() { 500, 5000, 10000, 1000, 100, 10, 1 };
			decimal[] expected = { 37, 370, 3700, 5333, 5167 };
			int period = 3;
			MovingAverageCalculator mac = new MovingAverageCalculator();
			for (int i = data.Count - period; i >= 0; i--)
			{
				Assert.Equal(expected[data.Count - period - i], Math.Round(mac.GetSma(data.GetRange(i, period).ToArray())));
			}
		}

		[Fact]
		public void GetEmaTest()
		{
			List<decimal> data = new List<decimal>() { 500, 5000, 10000, 1000, 100, 10, 1 };
			decimal[] expected = { 37, 518, 5259, 5130, 2815 };
			int period = 3;
			MovingAverageCalculator mac = new MovingAverageCalculator();
			for (int i = data.Count - period; i >= 0; i--)
			{
				Assert.Equal(expected[data.Count - period - i], Math.Round(mac.GetEma(data.GetRange(i, period).ToArray())));
			}
		}

		[Fact]
		public void GetSmmaTest()
		{
			List<decimal> data = new List<decimal>() { 98, 99, 100, 102, 103, 104, 102, 103, 101, 100 };
			decimal[] expected = { 102, 102.2m, 102.16m, 101.728m, 101.182m, 100.546m };
			int period = 5;
			MovingAverageCalculator mac = new MovingAverageCalculator();
			for (int i = data.Count - period; i >= 0; i--)
			{
				Assert.Equal(expected[data.Count - period - i], Math.Round(mac.GetSmma(data.GetRange(i, period).ToArray()), 3));
			}
		}

		[Fact]
		public void GetLwmaTest()
		{
			List<decimal> data = new List<decimal>() { 500, 5000, 10000, 1000, 100, 10, 1 };
			decimal[] expected = { 54, 535, 5350, 6000, 3583 };
			int period = 3;
			MovingAverageCalculator mac = new MovingAverageCalculator();
			for (int i = data.Count - period; i >= 0; i--)
			{
				Assert.Equal(expected[data.Count - period - i], Math.Round(mac.GetLwma(data.GetRange(i, period).ToArray())));
			}
		}
	}
}
