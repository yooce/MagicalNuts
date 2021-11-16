using MagicalNuts.Primitive;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MagicalNuts.Indicator
{
	/// <summary>
	/// 移動平均インジケーターを表します。
	/// </summary>
	public class MovingAverageIndicator : IIndicator
	{
		/// <summary>
		/// 期間を設定または取得します。
		/// </summary>
		[Category("移動平均")]
		[DisplayName("期間")]
		[Description("期間を設定します。")]
		public int Period { get; set; } = 25;

		/// <summary>
		/// 計算方法を設定または取得します。
		/// </summary>
		[Category("移動平均")]
		[DisplayName("計算方法")]
		[Description("計算方法を設定します。")]
		public MovingAverageMethod MovingAverageMethod { get; set; } = MovingAverageMethod.Sma;

		/// <summary>
		/// 価格の種類を設定または取得します。
		/// </summary>
		[Category("移動平均")]
		[DisplayName("価格の種類")]
		[Description("価格の種類を設定します。")]
		public PriceType PriceType { get; set; } = PriceType.Close;

		/// <summary>
		/// 移動平均計算機
		/// </summary>
		[Browsable(false)]
		private MovingAverageCalculator MovingAverage = null;

		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public async Task SetUpAsync()
		{
			MovingAverage = new MovingAverageCalculator();
		}

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="candles">インジケーター用ロウソク足の集合</param>
		/// <returns>値</returns>
		public decimal[] GetValues(IndicatorCandleCollection candles)
		{
			// 必要期間に満たない
			if (candles.Count < Period) return null;

			// 移動平均
			decimal ma = MovingAverage.Get(candles.GetRange(0, Period).Select(candle => candle.GetPrice(PriceType)).ToArray()
				, MovingAverageMethod);

			return new decimal[] { ma };
		}
	}

	/// <summary>
	/// 移動平均インジケーターのPropertyGrid用変換器を表します。
	/// </summary>
	public class MovingAverageIndicatorConverter : ExpandableObjectConverter
	{
		/// <summary>
		/// 変換器が型を変換できるかどうかを取得します。
		/// </summary>
		/// <param name="context">コンテキスト</param>
		/// <param name="destinationType">型</param>
		/// <returns>変換器が型を変換できるかどうか</returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(MovingAverageIndicator)) return true;
			return base.CanConvertTo(context, destinationType);
		}

		/// <summary>
		/// 変換されたオブジェクトを取得します。
		/// </summary>
		/// <param name="context">コンテキスト</param>
		/// <param name="culture">カルチャ</param>
		/// <param name="value">オブジェクト</param>
		/// <param name="destinationType">型</param>
		/// <returns>変換されたオブジェクト</returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is MovingAverageIndicator)
			{
				MovingAverageIndicator indicator = (MovingAverageIndicator)value;
				return indicator.Period.ToString() + "," + Enum.GetName(typeof(MovingAverageMethod), indicator.MovingAverageMethod)
					+ "," + Enum.GetName(typeof(PriceType), indicator.PriceType);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>
		/// 変換器が型を復元できるかどうかを取得します。
		/// </summary>
		/// <param name="context">コンテキスト</param>
		/// <param name="sourceType">型</param>
		/// <returns>変換器が型を復元できるかどうか</returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string)) return true;
			return base.CanConvertFrom(context, sourceType);
		}

		/// <summary>
		/// 復元されたオブジェクトを取得します。
		/// </summary>
		/// <param name="context">コンテキスト</param>
		/// <param name="culture">カルチャ</param>
		/// <param name="value">オブジェクト</param>
		/// <returns>復元されたオブジェクト</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				MovingAverageIndicator indicator = new MovingAverageIndicator();
				string[] strs = value.ToString().Split(',');
				if (strs.Length > 0) indicator.Period = int.Parse(strs[0]);
				if (strs.Length > 1)
				{
					foreach (MovingAverageMethod method in Enum.GetValues(typeof(MovingAverageMethod)))
					{
						if (Enum.GetName(typeof(MovingAverageMethod), method) == strs[1])
						{
							indicator.MovingAverageMethod = method;
							break;
						}
					}
				}
				if (strs.Length > 2)
				{
					foreach (PriceType pt in Enum.GetValues(typeof(PriceType)))
					{
						if (Enum.GetName(typeof(PriceType), pt) == strs[2])
						{
							indicator.PriceType = pt;
							break;
						}
					}
				}
				return indicator;
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}
