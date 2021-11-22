using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagicalNuts.BackTest.Test
{
	internal class TestStrategy : IStrategy
	{
		private StrategyProperties _Properties = null;

		public int ReferenceCandlesNum => 0;

		public string Name => null;

		public object Properties => _Properties;

		public TestStrategy()
		{
			_Properties = new StrategyProperties();
			_Properties.InitialAssets = 100000;
		}

		public void GetOrders(BackTestStatus state, List<Order> orders)
		{
			// 手仕舞い
			foreach (Position position in state.ActivePositions)
			{
				orders.Add(Order.GetSellMarketOrder(position));
			}

			// 新規買い
			orders.Add(Order.GetBuyMarketOrder(state.Candles.Stock, state.Candles.Stock.Unit));
		}

		public async Task SetUpAsync()
		{
		}
	}
}
