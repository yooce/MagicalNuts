using MagicalNuts.Primitive;
using System;
using System.Collections.Generic;

namespace MagicalNuts.BackTest.Test
{
	internal static class CandleProvider
	{
		public static Candle[] GetStockACandles()
		{
			List<Candle> candles = new List<Candle>();
			foreach (string line in System.IO.File.ReadLines("1717.csv"))
			{
				string[] columns = line.Split(',');
				candles.Add(new Candle(DateTime.Parse(columns[0]), decimal.Parse(columns[1]), decimal.Parse(columns[2])
					, decimal.Parse(columns[3]), decimal.Parse(columns[4]), decimal.Parse(columns[5])));
			}
			return candles.ToArray();
		}

		public static Candle[] GetStockBCandles()
		{
			List<Candle> candles = new List<Candle>();
			foreach (string line in System.IO.File.ReadLines("1719.csv"))
			{
				string[] columns = line.Split(',');
				candles.Add(new Candle(DateTime.Parse(columns[0]), decimal.Parse(columns[1]), decimal.Parse(columns[2])
					, decimal.Parse(columns[3]), decimal.Parse(columns[4]), decimal.Parse(columns[5])));
			}
			return candles.ToArray();
		}

		public static Candle[] GetCurrencyCandles()
		{
			List<Candle> candles = new List<Candle>();
			foreach (string line in System.IO.File.ReadLines("usdjpy.csv"))
			{
				string[] columns = line.Split(',');
				candles.Add(new Candle(DateTime.Parse(columns[0]), decimal.Parse(columns[1]), decimal.Parse(columns[2])
					, decimal.Parse(columns[3]), decimal.Parse(columns[4])));
			}
			return candles.ToArray();
		}
	}
}
