using MagicalNuts.BackTest;
using System.ComponentModel;

namespace BackTestSample
{
	public class DollarCostAveragingProperties : StrategyProperties
	{
		[Category("ドルコスト平均法")]
		[DisplayName("毎月の買い付け日")]
		[Description("毎月の買い付け日を設定します。")]
		public int DayOfEveryMonth { get; set; } = 1;

		[Category("ドルコスト平均法")]
		[DisplayName("毎月の買い付け額")]
		[Description("毎月の買い付け額を設定します。")]
		public decimal BuyAmount { get; set; } = 50000;
	}

	public class DollarCostAveraging : IStrategy
	{
		private DollarCostAveragingProperties _Properties = null;

		public DollarCostAveraging()
		{
			_Properties = new DollarCostAveragingProperties();
			_Properties.InitialAssets = 0;
		}

		public async Task SetUpAsync()
		{
		}

		public string Name => "ドルコスト平均法";

		public object Properties => _Properties;

		public int ReferenceCandlesNum => 0;

		public void GetOrders(BackTestStatus state, List<Order> orders)
		{
			// 買い付け日を取得
			DateTime dt = state.DateTime;
			dt = new DateTime(dt.Year, dt.Month, _Properties.DayOfEveryMonth);

			// 買い付け日でない場合
			if (state.DateTime != dt) return;

			// ロット数取得
			decimal lots = Math.Floor(_Properties.BuyAmount / state.Candles[0].Close);

			// 入金
			state.AdditionalInvest(Math.Ceiling(state.Candles[0].Close * lots));

			// 買い付け
			orders.Add(Order.GetBuyMarketOrder(state.Candles.Stock, lots));
		}
	}
}
