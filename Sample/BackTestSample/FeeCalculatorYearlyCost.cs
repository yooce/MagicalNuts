using MagicalNuts.BackTest;
using System.ComponentModel;

namespace BackTestSample
{
	public class FeeYearlyPercentProperties
	{
		[Category("年経費率")]
		[DisplayName("年経費率")]
		[Description("年経費率を設定します。")]
		public decimal CostRate { get; set; } = 0.001m;
	}

	public class FeeCalculatorYearlyCost : IFeeCalculator
	{
		private FeeYearlyPercentProperties _Properties = null;

		public FeeCalculatorYearlyCost()
		{
			_Properties = new FeeYearlyPercentProperties();
		}

		public string Name => "年経費率";

		public object Properties => _Properties;

		public decimal GetEntryFee(BackTestStatus state, decimal price, decimal lots, decimal currency)
		{
			return 0;
		}

		public decimal GetExitFee(BackTestStatus state, decimal price, decimal lots, decimal currency)
		{
			return 0;
		}

		public decimal GetMonthlyFee(BackTestStatus state)
		{
			return 0;
		}

		public decimal GetYearlyFee(BackTestStatus state)
		{
			return Math.Ceiling(state.MarketAssets * _Properties.CostRate);
		}
	}
}
