namespace MagicalNuts.BackTest.Test
{
	internal class TestFeeCalculator : IFeeCalculator
	{
		public string Name => "テスト";

		public object Properties => null;

		public decimal GetEntryFee(BackTestStatus state, decimal price, decimal lots, decimal currency)
		{
			return 9.5m;
		}

		public decimal GetExitFee(BackTestStatus state, decimal price, decimal lots, decimal currency)
		{
			return 19.5m;
		}

		public decimal GetMonthlyFee(BackTestStatus state)
		{
			return 3.5m;
		}

		public decimal GetYearlyFee(BackTestStatus state)
		{
			return 7.5m;
		}
	}
}
