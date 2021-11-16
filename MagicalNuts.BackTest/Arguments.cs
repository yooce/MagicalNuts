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
		public Arguments(IStrategy strategy, BackTestCandleCollection candles, DateTime begin, DateTime end)
			: this(strategy, new BackTestCandleCollection[] { candles }, begin, end)
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
		public Arguments(IStrategy strategy, BackTestCandleCollection candles, DateTime begin, DateTime end, IFeeCalculator fc)
			: this(strategy, new BackTestCandleCollection[] { candles }, begin, end, fc)
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
		public Arguments(IStrategy strategy, BackTestCandleCollection candles, DateTime begin, DateTime end, CurrencyStore cs)
			: this(strategy, new BackTestCandleCollection[] { candles }, begin, end, cs)
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
		public Arguments(IStrategy strategy, BackTestCandleCollection candles, DateTime begin, DateTime end, IFeeCalculator fc, CurrencyStore cs)
			: this(strategy, new BackTestCandleCollection[] { candles }, begin, end, fc, cs)
		{
		}

		/// <summary>
		/// Argumentsクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="strategy">戦略</param>
		/// <param name="candles">銘柄ごとのバックテスト用ロウソク足の集合</param>
		/// <param name="begin">開始日時</param>
		/// <param name="end">終了日時</param>
		public Arguments(IStrategy strategy, BackTestCandleCollection[] candles, DateTime begin, DateTime end)
		{
			Strategy = strategy;
			StockCandles = candles;
			BeginDateTime = begin;
			EndDateTime = end;
			FeeCalculator = new FeeCalculatorNone();
		}

		/// <summary>
		/// Argumentsクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="strategy">戦略</param>
		/// <param name="candles">銘柄ごとのバックテスト用ロウソク足の集合</param>
		/// <param name="begin">開始日時</param>
		/// <param name="end">終了日時</param>
		/// <param name="fc">手数料計算機</param>
		public Arguments(IStrategy strategy, BackTestCandleCollection[] candles, DateTime begin, DateTime end, IFeeCalculator fc)
			: this(strategy, candles, begin, end)
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
		public Arguments(IStrategy strategy, BackTestCandleCollection[] candles, DateTime begin, DateTime end, CurrencyStore cs)
			: this(strategy, candles, begin, end)
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
		public Arguments(IStrategy strategy, BackTestCandleCollection[] candles, DateTime begin, DateTime end, IFeeCalculator fc, CurrencyStore cs)
			: this(strategy, candles, begin, end)
		{
			FeeCalculator = fc;
			CurrencyStore = cs;
		}
	}
}
