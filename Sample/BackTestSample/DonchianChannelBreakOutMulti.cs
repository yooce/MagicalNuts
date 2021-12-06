using MagicalNuts.BackTest;

namespace BackTestSample
{
	public class DonchianChannelBreakOutMulti : IStrategy
	{
		// 単独銘柄用戦略
		private DonchianChannelBreakOut SingleStrategy = null;

		public DonchianChannelBreakOutMulti()
		{
			SingleStrategy = new DonchianChannelBreakOut();
		}

		public string Name => "ドンチアンチャネルブレイクアウト（マルチ）";

		public object Properties => SingleStrategy.Properties;

		public int ReferenceCandlesNum => SingleStrategy.ReferenceCandlesNum;

		public async Task SetUpAsync()
		{
			await SingleStrategy.SetUpAsync();
			((StrategyProperties)SingleStrategy.Properties).InitialAssets = 1000000;
		}

		public void GetOrders(BackTestStatus state, List<Order> orders)
		{
			foreach (StrategyCandleCollection candles in state.StockCandles)
			{
				// 必要期間に満たない
				if (candles.Count < SingleStrategy.ReferenceCandlesNum) return;

				// 単独銘柄用戦略
				SingleStrategy.GetOrders(state.GetBackTestStatusForSingleStrategy(candles), orders);
			}
		}
	}
}
