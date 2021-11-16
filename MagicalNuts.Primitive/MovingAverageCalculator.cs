using System.Linq;

namespace MagicalNuts.Primitive
{
	/// <summary>
	/// 移動平均の計算方法
	/// </summary>
	public enum MovingAverageMethod
	{
		Sma,    // 単純移動平均
		Ema,    // 指数移動平均
		Smma,   // 平滑移動平均
		Lwma    // 加重移動平均
	}

	/// <summary>
	/// 移動平均計算機を表します。
	/// </summary>
	public class MovingAverageCalculator
	{
		/// <summary>
		/// 前回の移動平均
		/// </summary>
		private decimal? PreviousMovingAverage = null;

		/// <summary>
		/// 初期化します。
		/// </summary>
		public void Reset()
		{
			PreviousMovingAverage = null;
		}

		/// <summary>
		/// 移動平均を取得します。
		/// </summary>
		/// <param name="data">対象データ（Sma以外は降順で渡すこと）</param>
		/// <param name="method">移動平均の計算方法</param>
		/// <returns>移動平均</returns>
		public decimal Get(decimal[] data, MovingAverageMethod method)
		{
			decimal ma = 0;
			switch (method)
			{
				case MovingAverageMethod.Sma:
					ma = GetSma(data);
					break;
				case MovingAverageMethod.Ema:
					ma = GetEma(data);
					break;
				case MovingAverageMethod.Smma:
					ma = GetSmma(data);
					break;
				case MovingAverageMethod.Lwma:
					ma = GetLwma(data);
					break;
			}
			return ma;
		}

		/// <summary>
		/// 単純移動平均を取得します。
		/// </summary>
		/// <param name="data">対象データ</param>
		/// <returns>単純移動平均</returns>
		public decimal GetSma(decimal[] data)
		{
			return GetSmaStatic(data);
		}

		/// <summary>
		/// 指数移動平均を取得します。
		/// </summary>
		/// <param name="data">対象データ（降順で渡すこと）</param>
		/// <returns>指数移動平均</returns>
		public decimal GetEma(decimal[] data)
		{
			// 係数
			decimal a = 2.0m / (decimal)(data.Length + 1);

			// 初回の移動平均
			if (PreviousMovingAverage == null) PreviousMovingAverage = GetSma(data);
			// 次回以降
			else PreviousMovingAverage = a * data[0] + (1 - a) * PreviousMovingAverage.Value;

			return PreviousMovingAverage.Value;
		}

		/// <summary>
		/// 平滑移動平均を取得します。
		/// </summary>
		/// <param name="data">対象データ（降順で渡すこと）</param>
		/// <returns>平滑移動平均</returns>
		public decimal GetSmma(decimal[] data)
		{
			// 係数
			decimal a = 1.0m / (decimal)data.Length;

			// 初回の移動平均
			if (PreviousMovingAverage == null) PreviousMovingAverage = GetSma(data);
			// 次回以降
			else PreviousMovingAverage = a * data[0] + (1 - a) * PreviousMovingAverage.Value;

			return PreviousMovingAverage.Value;
		}

		/// <summary>
		/// 加重移動平均を取得します。
		/// </summary>
		/// <param name="data">対象データ（降順で渡すこと）</param>
		/// <returns>加重移動平均</returns>
		public decimal GetLwma(decimal[] data)
		{
			return GetLwmaStatic(data);
		}

		/// <summary>
		/// 単純移動平均を取得します。
		/// </summary>
		/// <param name="data">対象データ</param>
		/// <returns>単純移動平均</returns>
		public static decimal GetSmaStatic(decimal[] data)
		{
			return data.Average();
		}

		/// <summary>
		/// 加重移動平均を取得します。
		/// </summary>
		/// <param name="data">対象データ（降順で渡すこと）</param>
		/// <returns>加重移動平均</returns>
		public static decimal GetLwmaStatic(decimal[] data)
		{
			decimal sum1 = 0, sum2 = 0;
			for (int i = 0; i < data.Length; i++)
			{
				sum1 += (data.Length - i) * data[i];
				sum2 += i + 1;
			}
			return sum1 / sum2;
		}
	}
}
