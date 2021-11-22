using MagicalNuts.BackTest;
using System.ComponentModel;

namespace BackTestSample
{
	public class DonchianChannelBreakOutProperties : StrategyProperties
	{
		[Category("ドンチアンチャネルブレイクアウト")]
		[DisplayName("高値の期間")]
		[Description("高値の期間を設定します。")]
		public int HighPeriod { get; set; } = 20;

		[Category("ドンチアンチャネルブレイクアウト")]
		[DisplayName("安値の期間")]
		[Description("安値の期間を設定します。")]
		public int LowPeriod { get; set; } = 20;
	}

	public class DonchianChannelBreakOut : IStrategy
	{
		private DonchianChannelBreakOutProperties _Properties = null;

		public DonchianChannelBreakOut()
		{
			_Properties = new DonchianChannelBreakOutProperties();
		}

		public string Name => "ドンチアンチャネルブレイクアウト";

		public object Properties => _Properties;

		public int ReferenceCandlesNum
		{
			get
			{
				List<int> periods = new List<int>() { _Properties.HighPeriod + 1, _Properties.LowPeriod + 1 };
				return periods.Max();
			}
		}

		public async Task SetUpAsync()
		{
		}

		public void GetOrders(BackTestStatus state, List<Order> orders)
		{
			// 必要期間に満たない
			if (state.Candles.Count < ReferenceCandlesNum) return;

			// １日前を基準にした過去20日間の最高値
			decimal high_1 = state.Candles.GetRange(1, _Properties.HighPeriod).Select(candle => candle.High).Max();

			// １日前を基準にした過去20日間の最安値
			decimal low_1 = state.Candles.GetRange(1, _Properties.LowPeriod).Select(candle => candle.Low).Min();

			// ロット数
			decimal lots = state.Candles.Stock.Unit;

			// 最高値ブレイク
			if (state.Candles[0].Close > high_1 && state.LastActiveLongPosition == null)
			{
				// 手仕舞い
				Position position = state.LastActiveShortPosition;
				if (position != null) orders.Add(Order.GetBuyMarketOrder(position));

				// 途転
				orders.Add(Order.GetBuyMarketOrder(state.Candles.Stock, lots));
			}

			// 最安値ブレイク
			if (state.Candles[0].Close < low_1 && state.LastActiveShortPosition == null)
			{
				// 手仕舞い
				Position position = state.LastActiveLongPosition;
				if (position != null) orders.Add(Order.GetSellMarketOrder(position));

				// 途転
				orders.Add(Order.GetSellMarketOrder(state.Candles.Stock, lots));
			}
		}
	}
}
