using MagicalNuts.Primitive;
using System;

namespace MagicalNuts.BackTest
{
	/// <summary>
	/// バックテストの引数セットを表します。
	/// </summary>
	public class Arguments
	{
		/// <summary>
		/// 戦略
		/// </summary>
		public IStrategy Strategy { get; set; }

		/// <summary>
		/// 銘柄ごとのバックテスト用ロウソク足の集合
		/// </summary>
		public BackTestCandleCollection[] StockCandles { get; set; }

		/// <summary>
		/// 開始日時
		/// </summary>
		public DateTime BeginDateTime { get; set; }

		/// <summary>
		/// 終了日時
		/// </summary>
		public DateTime EndDateTime { get; set; }

		/// <summary>
		/// 期間情報
		/// </summary>
		public PeriodInfo PeriodInfo { get; set; }

		/// <summary>
		/// 手数料計算機
		/// </summary>
		public IFeeCalculator FeeCalculator { get; set; }

		/// <summary>
		/// 為替ストア
		/// </summary>
		public CurrencyStore CurrencyStore { get; set; }

		/// <summary>
		/// 通貨の小数点以下の桁数
		/// </summary>
		public int CurrencyDigits { get; set; }

		/// <summary>
		/// Argumentsクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="strategy">戦略</param>
		/// <param name="candles">バックテスト用ロウソク足の集合</param>
		/// <param name="begin">開始日時</param>
		/// <param name="end">終了日時</param>
		/// <param name="unit">期間単位</param>
		/// <param name="period">期間</param>
		public Arguments(IStrategy strategy, BackTestCandleCollection candles, DateTime begin, DateTime end, PeriodUnit unit = PeriodUnit.Day, int period = 1)
			: this(strategy, new BackTestCandleCollection[] { candles }, begin, end, unit, period)
		{
		}

		/// <summary>
		/// Argumentsクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="strategy">戦略</param>
		/// <param name="candles">バックテスト用ロウソク足の集合</param>
		/// <param name="begin">開始日時</param>
		/// <param name="end">終了日時</param>
		/// <param name="fc">手数料計算機</param>
		/// <param name="unit">期間単位</param>
		/// <param name="period">期間</param>
		public Arguments(IStrategy strategy, BackTestCandleCollection candles, DateTime begin, DateTime end, IFeeCalculator fc, PeriodUnit unit = PeriodUnit.Day, int period = 1)
			: this(strategy, new BackTestCandleCollection[] { candles }, begin, end, fc, unit, period)
		{
		}

		/// <summary>
		/// Argumentsクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="strategy">戦略</param>
		/// <param name="candles">バックテスト用ロウソク足の集合</param>
		/// <param name="begin">開始日時</param>
		/// <param name="end">終了日時</param>
		/// <param name="cs">為替ストア</param>
		/// <param name="unit">期間単位</param>
		/// <param name="period">期間</param>
		public Arguments(IStrategy strategy, BackTestCandleCollection candles, DateTime begin, DateTime end, CurrencyStore cs, PeriodUnit unit = PeriodUnit.Day, int period = 1)
			: this(strategy, new BackTestCandleCollection[] { candles }, begin, end, cs, unit, period)
		{
		}

		/// <summary>
		/// Argumentsクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="strategy">戦略</param>
		/// <param name="candles">バックテスト用ロウソク足の集合</param>
		/// <param name="begin">開始日時</param>
		/// <param name="end">終了日時</param>
		/// <param name="fc">手数料計算機</param>
		/// <param name="cs">為替ストア</param>
		/// <param name="unit">期間単位</param>
		/// <param name="period">期間</param>
		public Arguments(IStrategy strategy, BackTestCandleCollection candles, DateTime begin, DateTime end, IFeeCalculator fc, CurrencyStore cs, PeriodUnit unit = PeriodUnit.Day, int period = 1)
			: this(strategy, new BackTestCandleCollection[] { candles }, begin, end, fc, cs, unit, period)
		{
		}

		/// <summary>
		/// Argumentsクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="strategy">戦略</param>
		/// <param name="candles">銘柄ごとのバックテスト用ロウソク足の集合</param>
		/// <param name="begin">開始日時</param>
		/// <param name="end">終了日時</param>
		/// <param name="unit">期間単位</param>
		/// <param name="period">期間</param>
		public Arguments(IStrategy strategy, BackTestCandleCollection[] candles, DateTime begin, DateTime end, PeriodUnit unit = PeriodUnit.Day, int period = 1)
		{
			Strategy = strategy;
			StockCandles = candles;
			BeginDateTime = begin;
			EndDateTime = end;
			FeeCalculator = new FeeCalculatorNone();
			PeriodInfo = new PeriodInfo(unit, period);
		}

		/// <summary>
		/// Argumentsクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="strategy">戦略</param>
		/// <param name="candles">銘柄ごとのバックテスト用ロウソク足の集合</param>
		/// <param name="begin">開始日時</param>
		/// <param name="end">終了日時</param>
		/// <param name="fc">手数料計算機</param>
		/// <param name="unit">期間単位</param>
		/// <param name="period">期間</param>
		public Arguments(IStrategy strategy, BackTestCandleCollection[] candles, DateTime begin, DateTime end, IFeeCalculator fc, PeriodUnit unit = PeriodUnit.Day, int period = 1)
			: this(strategy, candles, begin, end, unit, period)
		{
			FeeCalculator = fc;
		}

		/// <summary>
		/// Argumentsクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="strategy">戦略</param>
		/// <param name="candles">銘柄ごとのバックテスト用ロウソク足の集合</param>
		/// <param name="begin">開始日時</param>
		/// <param name="end">終了日時</param>
		/// <param name="cs">為替ストア</param>
		/// <param name="unit">期間単位</param>
		/// <param name="period">期間</param>
		public Arguments(IStrategy strategy, BackTestCandleCollection[] candles, DateTime begin, DateTime end, CurrencyStore cs, PeriodUnit unit = PeriodUnit.Day, int period = 1)
			: this(strategy, candles, begin, end, unit, period)
		{
			CurrencyStore = cs;
		}

		/// <summary>
		/// Argumentsクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="strategy">戦略</param>
		/// <param name="candles">銘柄ごとのバックテスト用ロウソク足の集合</param>
		/// <param name="begin">開始日時</param>
		/// <param name="end">終了日時</param>
		/// <param name="fc">手数料計算機</param>
		/// <param name="cs">為替ストア</param>
		/// <param name="unit">期間単位</param>
		/// <param name="period">期間</param>
		public Arguments(IStrategy strategy, BackTestCandleCollection[] candles, DateTime begin, DateTime end, IFeeCalculator fc, CurrencyStore cs, PeriodUnit unit = PeriodUnit.Day, int period = 1)
			: this(strategy, candles, begin, end, unit, period)
		{
			FeeCalculator = fc;
			CurrencyStore = cs;
		}
	}
}
