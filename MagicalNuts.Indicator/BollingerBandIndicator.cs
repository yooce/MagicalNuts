using MathNet.Numerics.Statistics;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MagicalNuts.Indicator
{
	/// <summary>
	/// ボリンジャーバンドインジケーターを表します。
	/// </summary>
	public class BollingerBandIndicator : IIndicator
	{
		/// <summary>
		/// 標準偏差の倍率を設定または取得します。
		/// </summary>
		[Category("ボリンジャーバンド")]
		[DisplayName("標準偏差の倍率")]
		[Description("標準偏差の倍率を設定します。")]
		public decimal Deviation { get; set; } = 2;

		/// <summary>
		/// 移動平均を設定または取得します。
		/// </summary>
		[Category("ボリンジャーバンド")]
		[DisplayName("移動平均")]
		[Description("移動平均を設定します。")]
		[TypeConverter(typeof(MovingAverageIndicatorConverter))]
		public MovingAverageIndicator MovingAverageIndicator { get; set; }

		/// <summary>
		/// BollingerBandIndicatorクラスの新しいインスタンスを初期化します。
		/// </summary>
		public BollingerBandIndicator()
		{
			MovingAverageIndicator = new MovingAverageIndicator();
		}

		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public async Task SetUpAsync()
		{
			await MovingAverageIndicator.SetUpAsync();
		}

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="candles">ロウソク足のコレクション</param>
		/// <returns>値</returns>
		public decimal[] GetValues(IndicatorCandleCollection candles)
		{
			// 必要期間に満たない
			if (candles.Count < MovingAverageIndicator.Period) return null;

			// 移動平均
			decimal ma = MovingAverageIndicator.GetValues(candles)[0];

			// 標準偏差
			decimal std = (decimal)candles.GetRange(0, MovingAverageIndicator.Period).Select(candle =>
				(double)candle.GetPrice(MovingAverageIndicator.PriceType)).PopulationStandardDeviation();

			return new decimal[] { ma, ma + std * Deviation, ma - std * Deviation };
		}
	}
}
