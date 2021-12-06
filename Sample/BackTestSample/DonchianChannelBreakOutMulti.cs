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
			// 並列処理
			List<Order>[] stock_orders_array = new List<Order>[state.StockCandles.Length];
			Parallel.For(0, state.StockCandles.Length, i =>
			{
				// 注文リスト作成
				stock_orders_array[i] = new List<Order>();

				// 必要期間に満たない
				if (state.StockCandles[i].Count < SingleStrategy.ReferenceCandlesNum) return;

				// 単独銘柄用戦略
				SingleStrategy.GetOrders(state.GetBackTestStatusForSingleStrategy(state.StockCandles[i]), stock_orders_array[i]);
			});

			// 注文を合成
			foreach (List<Order> stock_orders in stock_orders_array)
			{
				orders.AddRange(stock_orders);
			}
		}
	}
}
