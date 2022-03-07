using MagicalNuts.Indicator;
using MagicalNuts.Primitive;
using System.Collections.Generic;
using Utf8Json;

namespace MagicalNuts.BackTest
{
	/// <summary>
	/// 戦略用ロウソク足の集合を表します。
	/// </summary>
	[JsonFormatter(typeof(StrategyCandleCollectionFormatter))]
	public class StrategyCandleCollection : CandleCollection<Stock>
	{
		/// <summary>
		/// 銘柄情報
		/// </summary>
		public Stock Stock => Additional;

		/// <summary>
		/// StrategyCandleCollectionクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="candles">ロウソク足のリスト</param>
		/// <param name="stock">銘柄情報</param>
		/// <param name="unit">期間単位</param>
		/// <param name="period">期間</param>
		public StrategyCandleCollection(List<Candle> candles, Stock stock, PeriodUnit unit = PeriodUnit.Day, int period = 1) : base(candles, stock, unit, period)
		{
		}

		/// <summary>
		/// ロウソク足のインデックスをずらします。
		/// </summary>
		/// <param name="i">ずらす個数</param>
		/// <returns>インデックスをずらしたロウソク足の集合</returns>
		public new StrategyCandleCollection Shift(int i)
		{
			return new StrategyCandleCollection(GetRange(i, Count - i), Additional);
		}

		/// <summary>
		/// インジケーター用ロウソク足の集合を返します。
		/// </summary>
		/// <returns>インジケーター用ロウソク足の集合</returns>
		public IndicatorCandleCollection GetIndicatorCandleCollection()
		{
			return new IndicatorCandleCollection(this, Stock.Code);
		}

		/// <summary>
		/// StrategyCandleCollectionクラスのシリアライズ方法を表します。
		/// </summary>
		public class StrategyCandleCollectionFormatter : IJsonFormatter<StrategyCandleCollection>
		{
			/// <summary>
			/// シリアライズします。
			/// </summary>
			/// <param name="writer">JsonWriterのインスタンス</param>
			/// <param name="value">StrategyCandleCollectionのインスタンス</param>
			/// <param name="formatterResolver">IJsonFormatterResolverのインスタンス</param>
			public void Serialize(ref JsonWriter writer, StrategyCandleCollection value, IJsonFormatterResolver formatterResolver)
			{
				if (value == null) { writer.WriteNull(); return; }

				writer.WriteBeginObject();

				// 継承分を「Candles」とする
				writer.WritePropertyName("Candles");
				writer.WriteBeginArray();
				// 先頭
				if (value.Count != 0) formatterResolver.GetFormatterWithVerify<Candle>().Serialize(ref writer, value[0], formatterResolver);
				// ２つめ以降
				for (int i = 1; i < value.Count; i++)
				{
					writer.WriteValueSeparator();
					formatterResolver.GetFormatterWithVerify<Candle>().Serialize(ref writer, value[i], formatterResolver);
				}
				writer.WriteEndArray();
				writer.WriteValueSeparator();

				// Stock
				writer.WritePropertyName("Additional");
				formatterResolver.GetFormatterWithVerify<Stock>().Serialize(ref writer, value.Additional, formatterResolver);
				writer.WriteValueSeparator();

				// PeriodInfo
				writer.WritePropertyName("PeriodInfo");
				formatterResolver.GetFormatterWithVerify<PeriodInfo>().Serialize(ref writer, value.PeriodInfo, formatterResolver);

				writer.WriteEndObject();
			}

			/// <summary>
			/// デシリアライズします。
			/// </summary>
			/// <param name="reader">JsonReaderのインスタンス</param>
			/// <param name="formatterResolver">IJsonFormatterResolverのインスタンス</param>
			/// <returns></returns>
			public StrategyCandleCollection Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
			{
				if (reader.ReadIsNull()) return null;

				reader.ReadIsBeginObject();

				// 継承分
				reader.ReadPropertyName();
				List<Candle> candles = new List<Candle>();
				var count = 0;
				while (reader.ReadIsInArray(ref count))
				{
					candles.Add(formatterResolver.GetFormatterWithVerify<Candle>().Deserialize(ref reader, formatterResolver));
				}
				reader.ReadIsValueSeparator();

				// Stock
				reader.ReadPropertyName();
				Stock stock = formatterResolver.GetFormatterWithVerify<Stock>().Deserialize(ref reader, formatterResolver);
				reader.ReadIsValueSeparator();

				// PeriodInfo
				reader.ReadPropertyName();
				PeriodInfo pi = formatterResolver.GetFormatterWithVerify<PeriodInfo>().Deserialize(ref reader, formatterResolver);

				reader.ReadIsEndObject();

				// StrategyCandleCollection
				return new StrategyCandleCollection(candles, stock, pi.Unit, pi.Period);
			}
		}
	}
}
