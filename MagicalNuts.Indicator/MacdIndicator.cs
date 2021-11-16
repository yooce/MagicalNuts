using MagicalNuts.Primitive;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MagicalNuts.Indicator
{
	/// <summary>
	/// MACDインジケーターを表します。
	/// </summary>
	public class MacdIndicator : IIndicator
	{
		/// <summary>
		/// シグナルの移動平均期間を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[DisplayName("シグナルの移動平均期間")]
		[Description("シグナルの移動平均期間を設定します。")]
		public int SignalPeriod { get; set; } = 9;

		/// <summary>
		/// シグナルの移動平均計算方法を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[DisplayName("シグナルの移動平均計算方法")]
		[Description("シグナルの移動平均計算方法を設定します。")]
		public MovingAverageMethod SignalMaMethod { get; set; } = MovingAverageMethod.Sma;

		/// <summary>
		/// 短期用移動平均を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[DisplayName("短期用移動平均")]
		[Description("短期移動平均を設定します。")]
		[TypeConverter(typeof(MovingAverageIndicatorConverter))]
		public MovingAverageIndicator FastMaIndicator { get; set; }

		/// <summary>
		/// 長期用移動平均を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[DisplayName("長期用移動平均")]
		[Description("長期移動平均を設定します。")]
		[TypeConverter(typeof(MovingAverageIndicatorConverter))]
		public MovingAverageIndicator SlowMaIndicator { get; set; }

		/// <summary>
		/// MACDのキュー
		/// </summary>
		[Browsable(false)]
		private Queue<decimal> MacdQueue = null;

		/// <summary>
		/// 移動平均計算機
		/// </summary>
		[Browsable(false)]
		private MovingAverageCalculator MovingAverageCalculator = null;

		/// <summary>
		/// MacdIndicatorの新しいインスタンスを初期化します。
		/// </summary>
		public MacdIndicator()
		{
			FastMaIndicator = new MovingAverageIndicator();
			FastMaIndicator.Period = 12;
			FastMaIndicator.MovingAverageMethod = MovingAverageMethod.Ema;
			SlowMaIndicator = new MovingAverageIndicator();
			SlowMaIndicator.Period = 26;
			SlowMaIndicator.MovingAverageMethod = MovingAverageMethod.Ema;
		}

		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public async Task SetUpAsync()
		{
			await FastMaIndicator.SetUpAsync();
			await SlowMaIndicator.SetUpAsync();
			MacdQueue = new Queue<decimal>();
			MovingAverageCalculator = new MovingAverageCalculator();
		}

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="candles">ロウソク足のコレクション</param>
		/// <returns>値</returns>
		public decimal[] GetValues(IndicatorCandleCollection candles)
		{
			// 必要期間に満たない
			if (candles.Count < FastMaIndicator.Period || candles.Count < SlowMaIndicator.Period) return null;

			// 移動平均
			decimal fast_ma = FastMaIndicator.GetValues(candles)[0];
			decimal slow_ma = SlowMaIndicator.GetValues(candles)[0];

			// MACD
			decimal macd = fast_ma - slow_ma;

			// キューに格納
			MacdQueue.Enqueue(macd);
			if (MacdQueue.Count > SignalPeriod) MacdQueue.Dequeue();

			// 必要期間に満たない
			if (MacdQueue.Count < SignalPeriod) return new decimal[] { macd };

			// シグナル
			decimal signal = MovingAverageCalculator.Get(MacdQueue.ToArray(), SignalMaMethod);

			return new decimal[] { macd, signal };
		}
	}
}
