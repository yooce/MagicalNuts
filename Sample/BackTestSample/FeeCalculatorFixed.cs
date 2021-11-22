using MagicalNuts.BackTest;
using System.ComponentModel;

namespace BackTestSample
{
	public class FeeFixedProperties
	{
		[Category("定額")]
		[DisplayName("定額手数料")]
		[Description("定額手数料を設定します。")]
		public decimal Fee { get; set; } = 500;
	}

	public class FeeCalculatorFixed : IFeeCalculator
	{
		private FeeFixedProperties _Properties = null;

		public FeeCalculatorFixed()
		{
			_Properties = new FeeFixedProperties();
		}

		public string Name => "定額";

		public object Properties => _Properties;

		public decimal GetEntryFee(BackTestStatus state, decimal price, decimal lots, decimal currency)
		{
			return _Properties.Fee;
		}

		public decimal GetExitFee(BackTestStatus state, decimal price, decimal lots, decimal currency)
		{
			return _Properties.Fee;
		}

		public decimal GetMonthlyFee(BackTestStatus state)
		{
			return 0;
		}

		public decimal GetYearlyFee(BackTestStatus state)
		{
			return 0;
		}
	}
}
