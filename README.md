# MagicalNuts

## MagicalNutsとは？

MagicalNutsは**C#による投資検証ライブラリ**です。使い方次第で、次のようなアプリケーションを実装できます。

* 株主優待イベント検証アプリケーション

![image](https://user-images.githubusercontent.com/63818926/147149475-bedf1a76-5c3c-4674-8021-feeba439276a.png)

* 独自指標を追加したチャートアプリケーション

![image](https://user-images.githubusercontent.com/63818926/147149555-720702b4-b897-43ec-99d9-903d66ff4f9a.png)

* バックテストアプリケーション

![image](https://user-images.githubusercontent.com/63818926/147149587-46b2283b-4773-4fed-b9a3-fa581794288c.png)

C#の取り回しのしやすさを活かし、**痒い所に手が届く**投資環境を作りたい方の１つの選択肢になれればと思います。MagicalNutsは10個のパッケージで構成されていますので、必要なものを選択してご利用ください。

## 目次
- [MagicalNuts](#magicalnuts)
	- [MagicalNutsとは？](#magicalnutsとは)
	- [目次](#目次)
	- [依存関係](#依存関係)
	- [免責事項](#免責事項)
	- [MagicalNuts.Primitive](#magicalnutsprimitive)
		- [Candle](#candle)
		- [Stock](#stock)
		- [Calendar](#calendar)
		- [MovingAverageCalculator](#movingaveragecalculator)
			- [単純移動平均](#単純移動平均)
			- [指数移動平均](#指数移動平均)
			- [平滑移動平均](#平滑移動平均)
			- [加重移動平均](#加重移動平均)
		- [MathEx](#mathex)
		- [PluginManager](#pluginmanager)
	- [MagicalNuts.UI.Base](#magicalnutsuibase)
		- [ControlExtensions](#controlextensions)
		- [StockIncrementalTextBox](#stockincrementaltextbox)
	- [MagicalNuts.AveragePriceMove、MagicalNuts.UI.AveragePriceMove](#magicalnutsaveragepricemovemagicalnutsuiaveragepricemove)
		- [計算例](#計算例)
	- [MagicalNuts.ShareholderIncentive、MagicalNuts.UI.ShareholderIncentive](#magicalnutsshareholderincentivemagicalnutsuishareholderincentive)
		- [計算例](#計算例-1)
	- [MagicalNuts.Indicator](#magicalnutsindicator)
	- [MagicalNuts.UI.TradingChart](#magicalnutsuitradingchart)
		- [価格と出来高の表示](#価格と出来高の表示)
		- [インジケーターの表示](#インジケーターの表示)
		- [インジケーター等の設定ヘルパー](#インジケーター等の設定ヘルパー)
	- [MagicalNuts.BackTest、MagicalNuts.UI.BackTest](#magicalnutsbacktestmagicalnutsuibacktest)
		- [売買戦略の実装](#売買戦略の実装)
		- [バックテストの実行](#バックテストの実行)
		- [手数料の実装](#手数料の実装)
		- [ドルコスト平均法のバックテスト](#ドルコスト平均法のバックテスト)
		- [複数銘柄を対象とした売買戦略](#複数銘柄を対象とした売買戦略)
		- [バックテスト結果とその拡張](#バックテスト結果とその拡張)
	- [Author](#author)
	- [ライセンス](#ライセンス)


## 依存関係

| パッケージ | 対応フレームワーク |
| --- | --- |
| MagicalNuts.UI.* | .NET 6 |
| 上記以外 | .NET Standard 2.0、2.1 |

## 免責事項

MagicalNutsはxUnitを用いた単体テストを可能な限り取り入れておりますが、未検証機能や不具合を含んでいる可能性がございます。MagicalNutsを投資活動にご利用にされる場合は、利用者様の方でも十分な検証をお願いいたします。

MagicalNutsは有価証券への投資を勧誘することを目的としておらず、また何らかの保証・約束をするものではございません。投資に関する決定は利用者様ご自身のご判断において行っていただきますようお願いいたします。MagicalNutsのご利用に起因するいかなる損害につきましても、作者は責任を負いかねます。

## MagicalNuts.Primitive

ロウソク足や銘柄情報など、他のパッケージでも使用する基本的なクラスを含むパッケージです。

### Candle

`Candle`はロウソク足を表すクラスです。

```cs
/// <summary>
/// ロウソク足を表します。
/// </summary>
public class Candle
{
	/// <summary>
	/// ロウソク足の開始日時
	/// </summary>
	public DateTime DateTime { get; set; }

	/// <summary>
	/// 始値
	/// </summary>
	public decimal Open { get; set; }

	/// <summary>
	/// 高値
	/// </summary>
	public decimal High { get; set; }

	/// <summary>
	/// 安値
	/// </summary>
	public decimal Low { get; set; }

	/// <summary>
	/// 終値
	/// </summary>
	public decimal Close { get; set; }

	/// <summary>
	/// 出来高
	/// </summary>
	public decimal Volume { get; set; }

	/// <summary>
	/// 売買代金
	/// </summary>
	public decimal TradingValue => Close * Volume;
}
```

いわゆるOHLCVで、これといって特筆すべきところはないと思いますが、１点だけ挙げるなら数値変数の型に`decimal`を使用しているところです。

当初は`double`を使っていたのですが、特に為替や米国株の株価を扱う際に、どうしても小数点以下の誤差が出てしまうことがあり、MagicalNutsでは基本的に小数を扱う際は`decimal`で統一しています。`decimal`は`double`に比べてデータサイズが大きく、処理時間もかかると思いますが、数値の正確性を優先しました。

MagicalNutsの各種機能を使用する場合、この`Candle`の配列やリスト、`CandleCollection`やその派生クラスのインスタンスをご用意いただくことになります。

### Stock

`Stock`は銘柄情報を表すクラスです。

```cs
/// <summary>
/// 銘柄情報を表します。
/// </summary>
public class Stock
{
	/// <summary>
	/// 銘柄コード
	/// </summary>
	public string Code { get; set; }

	/// <summary>
	/// 銘柄名
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// 単元数
	/// </summary>
	public int Unit { get; set; }

	/// <summary>
	/// 市場の種類
	/// </summary>
	public int MarketType { get; set; }
}
```

`Code`と`Name`は自明かもしれませんが、`Code = "7203"`、`Name = "トヨタ自動車"`というように銘柄コードと銘柄名を入れます。`Name`はMagicalNuts内では特に使用していないのですが、MagicalNutsを使ったアプリケーションを実装する際にあった方が良いと思うので入れています。

単元数の`Unit`は日本株で言えば`100`を入れます。ミニ株であれば、証券会社によって違うと思いますが`10`や`1`、米国株であれば`1`を入れておきます。

市場の種類`MarketType`は、米国など日本市場以外のマーケットを扱うことを想定して用意しましたが、日本株のみを扱う場合は`0`で良いですし、デフォルトでそうなります。

### Calendar

`Calendar`は市場の営業日、非営業日を判定する**基底クラス**です。

```cs
/// <summary>
/// カレンダーを表します。
/// </summary>
public class Calendar
{
	/// <summary>
	/// 非同期で準備します。
	/// </summary>
	/// <returns>成功したかどうか</returns>
	public virtual async Task<bool> SetUpAsync()
	{
		return true;
	}

	/// <summary>
	/// 休祝日かどうか判定します。
	/// </summary>
	/// <param name="dt">日時</param>
	/// <returns>休祝日かどうか</returns>
	public virtual bool IsHoliday(DateTime dt)
	{
		return (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday);
	}

	/// <summary>
	/// days日前の営業日を取得します。
	/// </summary>
	/// <param name="dt">基準日</param>
	/// <param name="days">日数</param>
	/// <returns>days日前の営業日</returns>
	public DateTime GetBusinessDayBefore(DateTime dt, int days)
	{
		for (int i = 0; i < days; i++)
		{
			dt = GetFirstBusinessDayToBefore(dt.AddDays(-1));
		}
		return dt;
	}

	/// <summary>
	/// days日後の営業日を取得します。
	/// </summary>
	/// <param name="dt">基準日</param>
	/// <param name="days">日数</param>
	/// <returns>days日後の営業日</returns>
	public DateTime GetBusinessDayAfter(DateTime dt, int days)
	{
		for (int i = 0; i < days; i++)
		{
			dt = GetFirstBusinessDayToAfter(dt.AddDays(1));
		}
		return dt;
	}

	/// <summary>
	/// dt以前（dt含む）で最初の営業日を取得します。
	/// </summary>
	/// <param name="dt">基準日</param>
	/// <returns>dt以前（dt含む）で最初の営業日</returns>
	public DateTime GetFirstBusinessDayToBefore(DateTime dt)
	{
		while (IsHoliday(dt))
		{
			dt = dt.AddDays(-1);
		}
		return dt;
	}

	/// <summary>
	/// dt以後（dt含む）で最初の営業日を取得します。
	/// </summary>
	/// <param name="dt">基準日</param>
	/// <returns>dt以後（dt含む）で最初の営業日</returns>
	public DateTime GetFirstBusinessDayToAfter(DateTime dt)
	{
		while (IsHoliday(dt))
		{
			dt = dt.AddDays(1);
		}
		return dt;
	}
}
```

非営業日を判定するメソッドは`IsHoliday()`ですが、このクラスでは週末の判定だけしかしないので、これを継承したクラスで`IsHoliday()`を`override`する必要があります。`ShareholderIncentiveSample`では`SampleCalendar`を実装し、週末に加えて祝日まで判定しています。

```cs
public class SampleCalendar : Calendar
{
	private readonly string[] datestrs =
	{
		"2020-01-01", "2020-01-02", "2020-01-03", "2020-01-13", "2020-02-11", "2020-02-24", "2020-03-20","2020-04-29", "2020-05-04", "2020-05-05",
		"2020-05-06", "2020-07-23", "2020-07-24", "2020-08-10", "2020-09-21", "2020-09-22", "2020-11-03", "2020-11-23", "2020-12-31"
	};

	private List<DateTime> Holidays = null;

	public override async Task<bool> SetUpAsync()
	{
		Holidays = new List<DateTime>();
		foreach (string datestr in datestrs)
		{
			Holidays.Add(DateTime.Parse(datestr));
		}
		return true;
	}

	public override bool IsHoliday(DateTime dt)
	{
		return dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday
			|| Holidays.Where(holiday => holiday.Date == dt).Count() > 0;
	}
}
```

`SampleCalendar`では、`SetUpAsync()`であらかじめ用意していた祝日を表す文字列の配列から`DateTime`のリストを作り、`override`した`IsHoliday()`でそれらを使った休祝日判定を行っています。尚、このクラスでは、2020年の祝日しか判定していませんのでご注意ください。

MagicalNutsでは`MagicalNuts.ShareholderIncentive`パッケージが`Calendar`の継承クラスを要求しますし、`MagicalNuts.BackTest`パッケージでも売買戦略によっては必要になってくると思います。

### MovingAverageCalculator

`MovingAverageCalculator`は移動平均を計算するクラスで、４種類の計算方法に対応しています。

```cs
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
```

enum名はメタトレーダーのプログラミング言語MQLに準拠しています。`MovingAverageCalculator`はどこからでも参照でき、次のように使います。

```cs
Candle[] candles = ...

calculator = new MovingAverageCalculator();
decimal ma = calculator.Get(candles.GetRange(0, 25).Select(candle => candle.Close).ToArray(), MovingAverageMethod.Sma);
```

以下`data`には、移動平均の計算期間分の価格が入っているものとします。

#### 単純移動平均

```cs
public decimal GetSma(decimal[] data)
{
	return data.Average();
}
```

名前のとおり、単純に価格の平均を返します。

#### 指数移動平均

```cs
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
```

前日の指数移動平均`PreviousMovingAverage`を使って計算します。初回は前日の指数移動平均がないので、単純移動平均で代替します。指数移動平均は最新の価格を過去の価格より重要視する、つまり最新の価格により敏感に反応する移動平均です。`a`は最新とそれ以外の価格の重みを計算するための係数です。

#### 平滑移動平均

```cs
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
```

平滑移動平均は指数移動平均と`a`の計算が異なるだけです。最新とそれ以外の価格の重みの計算が違うということですね。

`MovingAverageMethod.Ema`（指数移動平均）と`MovingAverageMethod.Smma`（平滑移動平均）は前日の移動平均を覚えておくメンバー変数を使用しますので、`calculator`を使い回さずに都度`new`するようにしてください。

#### 加重移動平均

```cs
public decimal GetLwma(decimal[] data)
{
	decimal sum1 = 0, sum2 = 0;
	for (int i = 0; i < data.Length; i++)
	{
		sum1 += (data.Length - i) * data[i];
		sum2 += i + 1;
	}
	return sum1 / sum2;
}
```

最新の価格を重要視するという意味では前述の２つの移動平均と同じですが、過去に行けば行くほど重要度を下げるという計算をしています。

### MathEx

`MathEx`は小数点以下の切り上げ、切り下げをするクラスです。

```cs
/// <summary>
/// 数値計算の拡張を表します。
/// </summary>
public static class MathEx
{
	/// <summary>
	/// 小数点以下の指定された桁数で切り上げます。
	/// </summary>
	/// <param name="value">小数</param>
	/// <param name="digits">切り上げる小数点以下の桁数</param>
	/// <returns>切り上げた小数</returns>
	public static decimal Ceiling(decimal value, int digits)
	{
		// シフト用倍数
		decimal multi = (decimal)Math.Pow(10, digits);

		if (value >= 0) return Math.Ceiling(value * multi) / multi;
		return Math.Floor(value * multi) / multi;
	}

	/// <summary>
	/// 小数点以下の指定された桁数で切り下げます。
	/// </summary>
	/// <param name="value">小数</param>
	/// <param name="digits">切り下げる小数点以下の桁数</param>
	/// <returns>切り下げた小数</returns>
	public static decimal Floor(decimal value, int digits)
	{
		// シフト用倍数
		decimal multi = (decimal)Math.Pow(10, digits);

		if (value >= 0) return Math.Floor(value * multi) / multi;
		return Math.Ceiling(value * multi) / multi;
	}
}
```

C#組み込みの`System.Math`クラスには小数点以下の四捨五入はあるのですが、切り上げと切り下げがないのでそれらを追加したクラスです。スタティッククラスなので、インスタンスの生成なしにどこからでも使用可能です。

### PluginManager

`PluginManager`は、Visual Studioのソリューション内の、指定されたクラスやインターフェイスを継承したクラスを検知し、その生成までをサポートするクラスです。`BackTestSample`では、バックテストに使う売買戦略や手数料計算機を、ドロップダウンリスト（C#ではコンボボックス）から選択することができます。

![image](https://user-images.githubusercontent.com/63818926/147149657-2f1b2b10-4c2a-43bc-b804-ce7e2fc4bdff.png)

`Form1`の、この部分の実装を見てみましょう。

```cs
private List<IStrategy> Strategies = null;
private List<IFeeCalculator> FeeCalculators = null;

// 戦略
Strategies = new PluginManager<IStrategy>().Plugins;

// 手数料
FeeCalculators = new PluginManager<IFeeCalculator>().Plugins;

// コンボボックスに追加
comboBoxStrategy.Items.AddRange(Strategies.Select(strategy => strategy.Name).ToArray());
comboBoxFee.Items.AddRange(FeeCalculators.Select(fee => fee.Name).ToArray());
```

`PluginManager`はジェネリッククラスで、インターフェイスや基底クラスの型を指定すると、ソリューション内でそれを継承したクラスを集めてきてくれます。（コンストラクタにDLLの入ったフォルダパスを渡すことで、DLL内で実装されているクラスまで集める機能も実装しているのですが未検証です。）`Plugins`プロパティで、それらのインスタンスのリストを得ることが可能です。

## MagicalNuts.UI.Base

他のUIパッケージでも使用する基本的な描画用クラスを含むパッケージです。

### ControlExtensions

`ControlExtensions`は、C#組み込みの`System.Windows.Forms.Control`にメソッドを追加するクラスです。

```cs
/// <summary>
/// Controlの拡張を表します。
/// </summary>
public static class ControlExtensions
{
	/// <summary>
	/// Controlを左寄せします。
	/// </summary>
	/// <param name="me">自Control</param>
	/// <param name="baseCtrl">基準Control</param>
	public static void AlignLeft(this Control me, Control baseCtrl)
	{
		me.Left = baseCtrl.Left + baseCtrl.Width + baseCtrl.Margin.Right + me.Margin.Left;
	}

	/// <summary>
	/// Controlを上寄せします。
	/// </summary>
	/// <param name="me">自Control</param>
	/// <param name="baseCtrl">基準Control</param>
	public static void AlignTop(this Control me, Control baseCtrl)
	{
		me.Top = baseCtrl.Top + baseCtrl.Height + baseCtrl.Margin.Bottom + me.Margin.Top;
	}

	/// <summary>
	/// クリップボードにキャプチャ画像をコピーします。
	/// </summary>
	/// <param name="me">自Control</param>
	/// <param name="zoom">倍率</param>
	public static void CopyToClipboard(this Control me, double zoom)
	{
		// キャプチャ
		Bitmap bmp = new Bitmap(me.Width, me.Height);
		me.DrawToBitmap(bmp, new Rectangle(0, 0, me.Width, me.Height));

		// 拡縮
		Bitmap canvas = new Bitmap((int)(me.Width * zoom), (int)(me.Height * zoom));
		Graphics g = Graphics.FromImage(canvas);
		g.DrawImage(bmp, 0, 0, (int)(bmp.Width * zoom), (int)(bmp.Height * zoom));

		// クリップボードにコピー
		Clipboard.SetImage(canvas);

		// 終了
		g.Dispose();
		canvas.Dispose();
		bmp.Dispose();
	}
}
```

コントロールの左寄せと上寄せ、クリップボードへのキャプチャコピーを実装しています。これらを使う場合は`using`した上で、

```cs
using MagicalNuts.UI.Base;

Label label1 = new Label();
label1.Left = label1.Margin.Left;

Label label2 = new Label();
label2.AlignLeft(label1);
```

というように使います。この例では、`label1`の右隣に「左揃え」で`label2`を配置しています。

### StockIncrementalTextBox

`StockIncrementalTextBox`はインクリメンタルサーチを備えたテキストボックスです。銘柄選択を用途として想定しており、次のイメージのように動作します。

![image](https://user-images.githubusercontent.com/63818926/147149699-470c0080-ca13-406d-8da2-048643809d25.png)

サンプルとして`IncrementalTextBoxSample`を用意しており、最も基本的な使い方としては、`Stock`の配列を用意して`SetCandidates()`に渡すだけです。

```cs
StockIncrementalTextBox incrementalTextBox = new StockIncrementalTextBox();
Stock[] Stocks = ...

incrementalTextBox1.SetCandidates(Stocks);
```

ただ、`SetCandidates()`は内部で辞書を生成するため、大量の検索対象を渡した場合、処理が重くなります。複数の`StockIncrementalTextBox`を配置し、それぞれの`SetCandidates()`をコールすると、その数だけ処理時間が増大してしまいます。そこで、先に辞書を作っておいて、それを各`StockIncrementalTextBox`に設定して高速化を図ることができます。

```cs
Dictionary<string, List<IncrementalTextBox.ListViewCandidate>> dict = await IncrementalTextBox.GetCandidateListViewItemDictionaryAsync(
	Stocks, StockIncrementalTextBox.StockKeysForDictionary, StockIncrementalTextBox.StockMatch);

incrementalTextBox2.CandidateListViewItemDictionary = dict;
incrementalTextBox3.CandidateListViewItemDictionary = dict;
```

`StockIncrementalTextBox`の検索対象は`Stock`クラスに限定していますが、その基底クラス`IncrementalTextBox`は投資検証に特化したクラスではないので、どんな種類のWindowsフォームアプリケーションでも使用できます。

```cs
IncrementalTextBox1 incrementalTextBox = new IncrementalTextBox();

string[] candidates = new string[] { "apple", "banana", "orange", "grape", "peach", "strawberry" };
incrementalTextBox1.SetCandidates(candidates);
```

## MagicalNuts.AveragePriceMove、MagicalNuts.UI.AveragePriceMove

`MagicalNuts.AveragePriceMove`は**平均値動きの推移**を計算するパッケージです。

「平均値動きの推移」は、正確には「前年最終売買日の終値を100%として指数化した過去10年間の値動きの平均値の推移」で、私はそれを略して平均値動きの推移と呼んでいます。これを、どんな銘柄に対してでも計算できるようにしたのが`MagicalNuts.AveragePriceMove`です。

私はこれを**株主優待イベント投資**で使っています。株主優待イベント投資は、株主優待権利獲得を目的とした買いによる株価上昇を狙う投資手法ですが、私は投資対象選択の条件の１つに次を挙げています。

**年間を通じて権利付き最終日に向けた動きだけがある**

例えば、下図は３月末優待のシダックス（4837）の平均値動きの推移ですが、株主優待権利付き最終日に向けた値動き以外は、年間で目立った動きが無いことがわかります。この場合、３月末に向けた株価上昇は株主優待の権利獲得を目的としている可能性が高いと判断できるため、このような条件を入れています。

![image](https://user-images.githubusercontent.com/63818926/147149738-d99484f2-90ea-455c-9094-5e1c880b6073.png)

`MagicalNuts.AveragePriceMove`はこの平均値動きの推移を計算するパッケージで、`MagicalNuts.UI.AveragePriceMove`はその計算結果をグラフ化するパッケージです。`AveragePriceMoveSample`というサンプルを用意していますので、そちらをご覧いただければと思いますが、使い方は簡単です。

まず、日足の株価データを`Candle`の配列として用意します。

```cs
Candle[] candles = ...
```

これを`AveragePriceMove.Controller`の`GetAveragePriceMove()`に渡すと、平均値動きの推移の計算結果を得ることができます。

```cs
var result = Controller.GetAveragePriceMove(candles, PriceType.Close, new DateTime(2021, 1, 1), 10);
```

３つめの引数で渡した日時の**前年末**（この場合は2020年末）までの平均値動きの推移を計算します。計算期間は４つめの引数で指定（この場合は10年）します。あとはこれを、`MagicalNuts.UI.AveragePriceMove`の`AveragePriceMoveChart`でグラフ化します。

```cs
AveragePriceMoveChart chart = new AveragePriceMoveChart();

...

chart.SetAveragePriceMove(result);
```

ここでは省いていますが、`System.Windows.Forms.Form`などへの`chart`の登録は必要です。また、複数銘柄の平均値動きの推移を同時にグラフ化することも可能です。`SetAveragePriceMove()`は可変長引数を受け取るようになっていますので、表示したい銘柄分の平均値動きの推移を渡してください。

```cs
chart.SetAveragePriceMove(result1, result2);
```

次の図は２銘柄分のグラフですが、３銘柄以上の同時表示も可能です。

![image](https://user-images.githubusercontent.com/63818926/147149756-3db9b9f2-b74c-47c7-aa45-775071055264.png)

### 計算例

平均値動きの推移は、次のような方法で計算されます。

1. 過去10年間の終値の前日からの増減率を計算
2. 同月同日の増減率の平均を計算
3. 前年最終売買日から順に前日の平均増減率に当日の平均増減率を乗算して平均値動きの推移を計算

ちょっとわかりにくいと思うので、2017年から2021年までの５年間の、1月4日から1月8日までの５日間を例にとって実際に計算してみます。まず、日々の終値が次のとおりだったとします。

| | 2017 | 2018 | 2019 | 2020 | 2021 |
| --- | ---: | ---: | ---: | ---: | ---: |
| 前年最終売買日 | 1,518.61 | 1,817.56 | 1,494.09 | 1,721.36 | 1,804.68 |
| 1月4日 | 1,554.48 | 1,863.82 | 1,471.16 | | 1,794.59 |
| 1月5日 | 1,555.68 | 1,880.34 | | | 1,791.22 |
| 1月6日 | 1,553.32 | | | 1,697.49 | 1,796.18 |
| 1月7日 | | | 1,512.53 | 1,725.05 | 1,826.30 |
| 1月8日 | | | 1,518.43 | 1,701.40 | 1,854.94 |

これを前日からの増減率（％）に変換します。

| | 2017 | 2018 | 2019 | 2020 | 2021 | 平均 |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| 1月4日 | 102.36 | 102.55 | 98.47 | | 99.44 | 100.70 |
| 1月5日 | 100.08 | 100.89 | | | 99.81 | 100.26 |
| 1月6日 | 99.85 | | | 98.61 | 100.28 | 99.58 |
| 1月7日 | | | 102.81 | 101.62 | 101.68 | 102.04 |
| 1月8日 | | | 100.39 | 98.63 | 101.57 | 100.20 |

一番右の列がそれぞれの日付の平均値です。休日分は省いて計算します。これを前日の平均値に乗算していくと次のように平均値動きが得られます。

| | 平均値動き |
| --- | ---: |
| 1月4日 | 100.70 |
| 1月5日 | 100.96 |
| 1月6日 | 99.84 |
| 1月7日 | 101.61 |
| 1月8日 |  102.24 |

こうして得られた平均値動きをグラフにしたものが**平均値動きの推移**というわけです。

![image](https://user-images.githubusercontent.com/63818926/147149791-ad29f79a-8683-479e-85b4-e01ec7940c0f.png)

私は各年の同月同日の終値から平均を出しましたが、他の計算方法として、１月の第１営業日の平均、第２営業日の平均というように、同月第〇営業日で平均を出すというやり方も考えられます。ですが、どちらにしても平均値動きの推移は**値動きの傾向**を見るためのものなので、その意味では大差ないものと考え、私は上記のような方法で計算しています。

## MagicalNuts.ShareholderIncentive、MagicalNuts.UI.ShareholderIncentive

`MagicalNuts.ShareholderIncentive`は株主優待イベント投資において、権利付き最終日の何日前に買って何日前に売ると期待値が高いのかを検証するパッケージです。サンプルとして`ShareholderIncentiveSample`を用意しており、`MagicalNuts.ShareholderIncentive`と`MagicalNuts.UI.ShareholderIncentive`を使って、下図のようなアプリケーションを実装しています。

![image](https://user-images.githubusercontent.com/63818926/147149820-0594aada-4798-4ca4-9042-3c16b7481341.png)

画面上部に権利付き最終日前後の値動き、左下に指数化された過去の株価とその平均、右下に権利付き最終日の〇日前に買って〇日前に売った場合の期待値を高い順に表示しています。それぞれ`MagicalNuts.UI.ShareholderIncentive`パッケージの`LastDayPriceMoveChart`、`HistoricalDataGridView`、`EntryExitExpectedValueGridView`クラスが該当します。

`MagicalNuts.ShareholderIncentive`の使い方を説明していきます。まず、`Candle`の配列を用意します。

```cs
Candle[] candles = ...
```

次に、株主優待権利確定日の情報を持つ`DateOfRightAllotment `のインスタンスを用意します。

```cs
DateOfRightAllotment dra = new DateOfRightAllotment("1718", 6);
```

この例ですと、銘柄コードは1718（美樹工業）で、６月末優待ということになります。6月20日のように日付まで決まっている銘柄であれば、

```cs
DateOfRightAllotment dra = new DateOfRightAllotment("3191", 6, 20);
```

のように指定します。また、権利付き最終日の決定のため、`MagicalNuts.Primitive.Calendar`を継承したクラスが必要です。これは明日解説しますが、簡単に言えば市場の営業日、非営業日を判断するクラスです。前述のサンプルでは`SampleCalendar`を実装しています。

```cs
SampleCalendar calendar = new SampleCalendar();
await calendar.SetUpAsync();
```

ここまで準備が整ったら`Controller.GetHistoricalData()`を使い、権利付き最終日を100として指数化した株価を取得します。

```cs
Controller controller = new Controller();
Arguments args = new Arguments(dra, 2020, 10, candles.ToArray(), calendar);
HistoricalData hd = controller.GetHistoricalData(args);
```

`Arguments`は指数化処理の設定をするクラスで、ここでは2020年までの過去10年間を指数化するように指定しています。この計算結果`hd`を使って、権利付き最終日の〇日前に買って〇日前に売った場合の期待値を取得します。

```cs
List<EntryExitExpectedValue> eeevs = controller.GetEntryExitExpectedValues(hd).ToList();
eeevs.Sort(EntryExitExpectedValue.Compare);
```

`EntryExitExpectedValue.Compare`は、`EntryExitExpectedValue`のリストを期待値の降順にソートするメソッドです。最後に、`MagicalNuts.UI.ShareholderIncentive`パッケージを使ってグラフや表にします。

```cs
entryExitExpectedValueGridView.SetupColumns();
historicalDataGridView.SetHistoricalData(hd);
entryExitExpectedValueGridView.DataSource = eeevs;
chart.SetHistoricalData(hd);
```

以上の手順で、前掲のようなアプリケーションを実装できます。

### 計算例

株主優待を実施している銘柄では、その権利が発生する権利付き最終日に向けて株価が上昇する傾向があります。権利付き最終日の翌日、権利落ち日に株価は急落します。株主優待イベント投資は、この株価上昇の始めに買い、権利付き最終日までのどこかで売って、このトレンドに乗ることを狙う投資手法です。

株主優待イベント投資を実践する場合、権利付き最終日に向けた値動きを集計し、権利付き最終日の何日前に買って何日前に売ると期待値が高いのかを検証します。具体的には、以下の手順で計算します。

1. 権利付き最終日前後、例えば前80日、後40日の過去10年間の終値を用意
2. それらを権利付き最終日の終値を100とした数値に指数化
3. 日ごとの平均値を算出

例えば、2018年から2020までの３年間の、権利付き最終日とその前５日間の終値が次のようだったとします。

| | 2018 | 2019 | 2020 |
| ---: | ---: | ---: | ---: |
| 5日前 | 5,260 | 3,675 | 3,890 |
| 4日前 | 5,340 | 3,680 | 3,890 |
| 3日前 | 5,310 | 3,700 | 3,915 |
| 2日前 | 5,310 | 3,780 | 3,925 |
| 1日前 | 5,240 | 3,840 | 3,980 |
| 権利付き最終日 | 5,280 | 3,890 | 3,985 |

これらを、権利付き最終日を100として指数化します。

| | 2018 | 2019 | 2020 | 平均 |
| ---: | ---: | ---: | ---: | ---: |
| 5日前 | 99.62 | 94.47 | 97.62 | 97.24 |
| 4日前 | 101.14 | 94.60 | 97.62 | 97.78 |
| 3日前 | 100.57 | 95.12 | 98.24 | 97.98 |
| 2日前 | 100.57 | 97.17 | 98.49 | 98.74 |
| 1日前 | 99.24 | 98.71 | 99.87 | 99.28 |
| 権利付き最終日 | 100 | 100 | 100 | 100 |

一番右の列が日ごとの平均値で、この数値が一番低い日に買って、一番高い日で売るのが最も期待値の高いトレードになるということですね。

## MagicalNuts.Indicator

移動平均やボリンジャーバンド、ATR、MACDなど、`MagicalNuts.UI.TradingChart`で表示するインジケーターや、`MagicalNuts.BackTest`の売買戦略で使用するインジケーターの実装に必要なパッケージです。インジケーターの実装には、`Indicator.IndicatorCandleCollection`を使用しますので、まずはその基底クラス`Primitive.CandleCollection`を見てみましょう。

```cs
/// <summary>
/// ロウソク足の集合を表します。
/// </summary>
/// <typeparam name="T">付加情報の型</typeparam>
public class CandleCollection<T> : List<Candle>
{
	/// <summary>
	/// 付加情報
	/// </summary>
	public T Additional { get; }

	/// <summary>
	/// CandleCollectionクラスの新しいインスタンスを初期化します。
	/// </summary>
	/// <param name="candles">ロウソク足のリスト</param>
	/// <param name="add">付加情報</param>
	public CandleCollection(List<Candle> candles, T add)
	{
		Clear();
		AddRange(candles);
		Additional = add;
	}

	/// <summary>
	/// ロウソク足のインデックスをずらします。
	/// </summary>
	/// <param name="i">ずらす個数</param>
	/// <returns>インデックスをずらしたロウソク足の集合</returns>
	public CandleCollection<T> Shift(int i)
	{
		return new CandleCollection<T>(GetRange(i, Count - i), Additional);
	}
}
```

要は、何らかの付加情報を持った`Candle`のリストということですね。

１つ注意が必要なことがありまして、コンストラクタに渡す`Candle`のリストは**時系列の降順**になっている必要があります。つまり、最新のロウソク足が最初にあって、添え字が大きくなるにつれて過去のロウソク足へと遡っていくようにソートしておく必要があるということです。ちょっと迷ったのですが、`Primitive.CandleCollection`ではソート順を保証せず、利用箇所での処理速度を優先することにしました。時系列の降順になっている理由は、インジケーターや売買戦略の実装で、メタトレーダーのMQLやトレードステーションのEasyLanguageの書き方を踏襲するためです。

これを継承した`Indicator.IndicatorCandleCollection`は、メンバー変数`Additional`に別名`Code`を与えており、要は銘柄コードを持った`Candle`のリストとご理解いただければ良いと思います。

```cs
/// <summary>
/// インジケーター用ロウソク足の集合を表します。
/// </summary>
public class IndicatorCandleCollection : CandleCollection<string>
{
	/// <summary>
	/// 銘柄コード
	/// </summary>
	public string Code => Additional;
}
```

この`IndicatorCandleCollection`をインジケーターが使用します。インジケーターは`IIndicator`インターフェイスを実装します。

```cs
/// <summary>
/// インジケーターのインターフェースを表します。
/// </summary>
public interface IIndicator
{
	/// <summary>
	/// 非同期で準備します。
	/// </summary>
	/// <returns>非同期タスク</returns>
	Task SetUpAsync();

	/// <summary>
	/// 値を取得します。
	/// </summary>
	/// <param name="candles">ロウソク足のコレクション</param>
	/// <returns>値</returns>
	decimal[] GetValues(IndicatorCandleCollection candles);
}
```

`SetUpAsync()`と`GetValues()`の２つのメソッドを実装することになります。`SetUpAsync()`でインジケーターの計算に必要なデータのロードなどを行い、`GetValues()`でインジケーターの計算を行います。

`GetValues()`は`IndicatorCandleCollection`を引数で受け取り、これを使ってインジケーターの値を計算して返します。戻り値は`decimal[]`となっており、インジケーターの値は複数になっても構いません。`TradingChartSample.SampleIndicator`は25日移動平均のサンプルです。その`GetValues()`の実装を見てみましょう。


```cs
public int Period = 25;

public decimal[] GetValues(IndicatorCandleCollection candles)
{
	// 必要期間に満たない
	if (candles.Count < Period) return null;

	// 移動平均
	decimal ma = candles.GetRange(0, Period).Select(candle => candle.Close).Average();

	return new decimal[] { ma };
}
```

ロウソク足の数が25日分揃っていない場合は移動平均を計算できないので`null`を返しています。揃っている場合は`candles`から先頭25個分の終値を取得して、それらの平均値を返しています。

例えば2021年12月10日の移動平均を計算する場合は、その日のロウソク足が`candles[0]`に入っている`candles`が`SampleIndicator`に渡され、その翌日12月11日の場合は、その日のロウソク足が`candles[0]`に入っている`candles`が渡されます。前述のとおり、時系列の降順にソートされた`candles`であることを前提に、`candles[0]`から必要な期間のロウソク足を参照してインジケーターを計算するとお考えいただければ良いと思います。この実装は、MQLやEasyLanguageの経験がある方には違和感ないものと思います。

## MagicalNuts.UI.TradingChart

`MagicalNuts.UI.TradingChart`は、株価チャートやインジケーターを表示するパッケージです。

### 価格と出来高の表示

日足のロウソク足と出来高を表示するのは非常に簡単です。

```cs
Candle[] candles = ...

TradingChart chart = new TradingChart();

...

chart.SetUp();
chart.SetDailyCandles(null, candles);
```

日足の`Candle`のリストを用意して、`TradingChart.SetUp()`と`TradingChart.SetDailyCandles()`を呼びだすだけで下図のような株価チャートを実装できます。

![image](https://user-images.githubusercontent.com/63818926/147149858-0fde1c26-7054-481c-8c75-eec9e2660bec.png)

週足、月足、年足への変更も非常に簡単で、`TradingChart.CandlePeriod`を設定するだけです。

```cs
// 週足
chart.CandlePeriod = CandlePeriod.Weekly;

// 月足
chart.CandlePeriod = CandlePeriod.Monthly;

// 年足
chart.CandlePeriod = CandlePeriod.Yearly;
```

また、チャートの拡縮にも対応しており、画面内のロウソク足の数を設定するイメージで`TradingChart.ScreenCandlesNum`を設定します。デフォルトは200で、減少させれば拡大、増加させれば縮小されます。

```cs
chart.ScreenCandlesNum = 250;
```

米国株や為替のチャートを表示をする場合に、グラフの軸目盛の小数点以下の桁数を設定する機能もあります。その場合は、`TradingChart.SetDailyCandles()`の第３引数に表示したい小数点以下の桁数を渡してください。

```cs
chart.SetDailyCandles(null, candles, 2);
```

これで、小数点以下２桁までを表示することができます。サンプルとして`TradingChartSample`を用意していますので、詳細はそちらをご覧ください。

### インジケーターの表示

`MagicalNuts.UI.TradingChart`でインジケーターをチャートに表示するには、`IPlotter`インターフェイスを実装します。

```cs
/// <summary>
/// プロッターのインターフェースを表します。
/// </summary>
public interface IPlotter : IPropertyHolder
{
	/// <summary>
	/// Seriesの配列を取得します。
	/// </summary>
	Series[] SeriesArray { get; }

	/// <summary>
	/// ChartAreaを設定します。
	/// </summary>
	/// <param name="mainChartArea">主ChartArea</param>
	/// <returns>使用する従ChartAreaの配列</returns>
	SubChartArea[] SetChartArea(MainChartArea mainChartArea);

	/// <summary>
	/// 非同期で準備します。
	/// </summary>
	/// <returns>非同期タスク</returns>
	Task SetUpAsync();

	/// <summary>
	/// データをプロットします。
	/// </summary>
	/// <param name="code">銘柄コード</param>
	/// <param name="candles">ロウソク足のリスト</param>
	void Plot(string code, List<Candle> candles);
}
```

下２つは、`SetUpAsync()`が準備処理、`Plot()`がグラフのプロット処理です。

C#の`System.Windows.Forms.DataVisualization.Charting.Chart`を使ったグラフプロットの経験がある方はご存じだと思いますが、一番上の`SeriesArray`はプロットした一連のデータを持った`Series`クラスの配列です。`SetChartArea()`は`MainChartArea`と`SubChartArea`の理解が必要ですので、`TradingChartSample`の画面を見てみましょう。

![image](https://user-images.githubusercontent.com/63818926/147149916-48a4c435-909a-4bda-b694-6e81699309d5.png)

上図は縦方向に３つの領域に分かれており、一番上は価格チャートと出来高、ボリンジャーバンドがプロットされた領域、２番目はATRがプロットされた領域、３番目はMACDがプロットされた領域です。一番上の価格チャートがプロットされた領域が`MainChartArea`で、下２つの価格チャート以外の領域が`SubChartArea`です。

MagicalNutsでは、チャートにインジケーターをプロットするクラスをプロッターと呼びます。`TradingChartSample`に`SamplePlotter`を実装しましたので、その`SetChartArea()`を見てみましょう。

```cs
private Series Series = new Series();

public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
{
	Series.ChartArea = mainChartArea.Name;
	return null;
}
```

まず、使用する`Series`をメンバー変数に持ちます。`SetChartArea()`にはチャートから`MainChartArea`が引数として渡されるので、メンバーの`Series`の描画対象領域として設定し、このプロッターは`SubChartArea`を使用しないのでnullを返しています。

`SubChartArea`を使う`MagicalNuts.UI.TradingChart.Plotter.AtrPlotter`の`SetChartArea()`を見てみましょう。

```cs
private Series Series = new Series();
private ChartArea ChartArea = null;

public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
{
	SubChartArea subChartArea = new SubChartArea();
	Series.ChartArea = subChartArea.Name;
	ChartArea = subChartArea;
	return new SubChartArea[] { subChartArea };
}
```

プロット先となる`SubChartArea`を作って`Series`に設定し、その存在をチャートに教えるために返しています。ここでは例示しませんが、`MainChartArea`と`SubChartArea`を同時に使用するプロッターも実装可能です。

ほとんどのプロッターは`IIndicator`を継承したインジケーターを、チャートにプロットするために実装されると思います。その場合、`IPlotter`を直接実装するのではなく、`IndicatorPlotter`という抽象クラスを継承するのが便利です。次に、説明に必要な箇所のみを抜粋した`IndicatorPlotter`を掲載します。

```cs
/// <summary>
/// インジケーターのプロッターを表します。
/// </summary>
/// <typeparam name="T">インジケーターの型を指定します。</typeparam>
public abstract class IndicatorPlotter<T> : IPlotter where T : IIndicator, new()
{
	/// <summary>
	/// インジケーター
	/// </summary>
	public T Indicator { get; private set; }

	/// <summary>
	/// IndicatorPlotterの新しいインスタンスを初期化します。
	/// </summary>
	public IndicatorPlotter()
	{
		Indicator = new T();
	}

	/// <summary>
	/// データをプロットします。
	/// </summary>
	/// <param name="code">銘柄コード</param>
	/// <param name="candles">ロウソク足のリスト</param>
	public void Plot(string code, List<Candle> candles)
	{
		// インジケーターをプロット
		PlotIndicator(candles);
	}

	/// <summary>
	/// インジケーターをプロットします。
	/// </summary>
	/// <param name="candles">インジケーター用ロウソク足のリスト</param>
	public abstract void PlotIndicator(IndicatorCandleCollection candles);

	/// <summary>
	/// 非同期で準備します。
	/// </summary>
	/// <returns>非同期タスク</returns>
	public virtual async Task SetUpAsync()
	{
		await Indicator.SetUpAsync();
	}

	/// <summary>
	/// x座標からインジケーター用ロウソク足のコレクションを取得します。
	/// </summary>
	/// <param name="x">x座標</param>
	/// <returns>インジケーター用ロウソク足のコレクション</returns>
	protected IndicatorCandleCollection GetCandleCollection(int x)
	{
		return new IndicatorCandleCollection(Candles.Shift(Candles.Count - x - 1), Candles.Code);
	}

	/// <summary>
	/// decimalの配列をdoubleの配列に変換します。
	/// </summary>
	/// <param name="decimals">decimalの配列</param>
	/// <returns>doubleの配列</returns>
	protected static double[] ConvertDecimalToDoubleArray(decimal[] decimals)
	{
		return decimals.Select(d => (double)d).ToArray();
	}
}
```

`IndicatorPlotter`はジェネリッククラスで、これを継承する場合、プロットするインジケータークラスの型を指定します。継承クラスの実装は、`PlotIndicator()`メソッドに注力することになります。`TradingChartSample.SamplePlotter`の実装を見てみましょう。

```cs
public class SamplePlotter : IndicatorPlotter<SampleIndicator>
{
	private Series Series = null;

	public override string Name => "サンプル";

	public override object Properties => Indicator;

	public override Series[] SeriesArray => new Series[] { Series };

	public SamplePlotter()
	{
		Series = new Series();
		Series.ChartType = SeriesChartType.Line;
		Series.YAxisType = AxisType.Secondary;
	}

	public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
	{
		Series.ChartArea = mainChartArea.Name;
		return null;
	}

	public override void PlotIndicator(IndicatorCandleCollection candles)
	{
		for (int x = 0; x < candles.Count; x++)
		{
			decimal[] data = Indicator.GetValues(GetCandleCollection(x));
			if (data == null) continue;

			Series.Points.Add(new DataPoint(x, ConvertDecimalToDoubleArray(data)));
		}
	}
}
```

このクラスは`SampleIndicator`をプロットするので、`IndicatorPlotter`に`SampleIndicator`を指定しています。（実際には、`SampleIndicator`を継承した`SampleIndicatorEx`を指定していますが、ここでは簡単のため`SampleIndicator`としています。）

`PlotIndicator()`には`candles.Count`回のfor文があります。まず、`x`時点の最新の`Candle`を先頭として過去に遡っていく`CandleCollection`を`GetCandleCollection()`で取得しています。それを`Indicator`（ここでは`SampleIndicator`）の`GetValues()`に渡し、移動平均値を取得しています。最後に、それを`DataPoint`にして`Series.Points`に追加しています。MagicalNutsでは小数のほとんどを`decimal`で扱いますが、`DataPoint`は`double`なので、その変換のために`ConvertDecimalToDoubleArray()`を使用しています。

![image](https://user-images.githubusercontent.com/63818926/147149961-9fb1c550-31e5-42b4-abae-e5611f60cc0f.png)

以上で、`SampleIndicator`の移動平均線をチャートにプロットすることができます。

### インジケーター等の設定ヘルパー

次のようなものはハードコーディングするのではなく、都度設定できた方が良いですよね。

* 移動平均インジケーターの計算日数
* インジケーターをチャートにプロットする色
* バックテストの初期資産

MagicalNutsにはインジケーターなどの設定を補助する仕組みがあります。その最も基本的なインターフェイスが`IPropertyHolder`です。

```cs
/// <summary>
/// プロパティ保持者のインターフェースを表します。
/// </summary>
public interface IPropertyHolder
{
	/// <summary>
	/// プロパティ名を取得します。
	/// </summary>
	string Name { get; }

	/// <summary>
	/// プロパティを取得します。
	/// </summary>
	object Properties { get; }
}
```

各種設定のことを「プロパティ」と言っており、都度設定したい項目を持つクラスが`IPropertyHolder`を実装します。前述の`IPlotter`は`IPropertyHolder`を継承しています。

```cs
public interface IPlotter : IPropertyHolder
```

`IPropertyHolder`の実装は`IndicatorPlotter`に委譲されていますが、これは抽象クラスなので、`IndicatorPlotter`を継承した`SamplePlotter`の関係個所を見てみましょう。

```cs
public class SamplePlotter : IndicatorPlotter<SampleIndicatorEx>
{
	public override string Name => "サンプル";

	public override object Properties => Indicator;
}
```

`Name`は`"サンプル"`を返し、`Properties`は`Indicator`を返しています。`Indicator`は何だったかと言うと`SampleIndicatorEx`です。（先程は簡単のため`SampleIndicator`と説明しました。）

```cs
public class SampleIndicatorEx : SampleIndicator
{
	[Category("プロット")]
	[DisplayName("色")]
	[Description("色を設定します。")]
	[DefaultValue(typeof(Color), "144, 30, 38")]
	public Color Color { get; set; } = Color.FromArgb(144, 30, 38);
}
```

チャートにプロットする色`Color`をプロパティに持っています。`SampleIndicatorEx`は`SampleIndicator`を継承しており、`SampleIndicator`は`Period`というプロパティを持っています。

```cs
[Category("サンプル")]
[DisplayName("期間")]
[Description("期間を設定します。")]
public int Period { get; set; } = 25;
```

ここまでをまとめると、`SamplePlotter.Properties`は、

* インジケーターをチャートにプロットする色
* 移動平均インジケーターの計算日数

を設定可能ということになります。

最後に、これらの設定UIのヘルパーとして`MagicalNuts.UI.Base`パッケージに`PropertyEditForm`を用意しています。使用例は次のとおりです。

```cs
TradingChart chart = new TradingChart();

...

SamplePlotter plotter = new SamplePlotter();

PropertyEditForm form = new PropertyEditForm(plotter);
if (form.ShowDialog() != DialogResult.OK) return null;

await plotter.SetUpAsync();
chart.AddPlotter(plotter);
```

詳細は`TradingChartSample`をご覧いただければと思いますが、次のような設定UIを簡単に実装することができます。

![image](https://user-images.githubusercontent.com/63818926/147149999-aaba216b-47bf-488a-af8d-8147dab5e051.png)

## MagicalNuts.BackTest、MagicalNuts.UI.BackTest

`MagicalNuts.BackTest`は売買戦略のバックテストを行うパッケージで、`MagicalNuts.UI.BackTest`はバックテスト結果をグラフ化したり、表を表示するパッケージです。

### 売買戦略の実装

売買戦略は`IStrategy`を実装する必要があります。

```cs
/// <summary>
/// 売買戦略のインターフェースを表します。
/// </summary>
public interface IStrategy : IPropertyHolder
{
	/// <summary>
	/// 参照するロウソク足の数
	/// </summary>
	int ReferenceCandlesNum { get; }

	/// <summary>
	/// 非同期で準備します。
	/// </summary>
	/// <returns>非同期タスク</returns>
	Task SetUpAsync();

	/// <summary>
	/// 注文を取得します。
	/// </summary>
	/// <param name="state">バックテストの状態</param>
	/// <param name="orders">注文のリスト</param>
	void GetOrders(BackTestStatus state, List<Order> orders);
}
```

`ReferenceCandlesNum`は参照する過去のロウソク足の数を入れておきます。例えば、25日移動平均を利用する売買戦略であれば、`25`を返すようにしておけば良いです。`SetUpAsync()`は準備処理です。

`GetOrders()`に具体的な売買処理を実装します。`GetOrders()`は`state`と`orders`の２つを引数に取り、簡単に言えば、`state`内のロウソク足やその他情報を見て、注文を`orders`に追加するイメージです。`BackTestSample`に`DonchianChannelBreakOut`という売買戦略を実装していますので、それを見ていきましょう。

`DonchianChannelBreakOut`は**ドンチアン・チャネル・ブレイクアウト**を実装したものです。これは、「価格が過去20日間の最高値を上抜けしたら買い、過去20日間の最安値を下抜けしたら空売りする」という売買戦略です。これを実装したのが`DonchianChannelBreakOut`です。

<dl>
  <dt><span style="color: #ff0000">注意</span></dt>
  <dd>DonchianChannelBreakOutは説明のための売買戦略です。この売買戦略では資産を毀損する可能性が高いため、決して実際の売買には採用しないでください。</dd>
</dl>

```cs
public class DonchianChannelBreakOut : IStrategy
{
	private DonchianChannelBreakOutProperties _Properties = null;

	public DonchianChannelBreakOut()
	{
		_Properties = new DonchianChannelBreakOutProperties();
		_Properties.InitialAssets = 1000000;
	}

	public string Name => "ドンチアンチャネルブレイクアウト";

	public object Properties => _Properties;

	public int ReferenceCandlesNum => Math.Max(_Properties.HighPeriod + 1, _Properties.LowPeriod + 1);

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
```

今日の終値が、前日までの過去20日間の最高値を上回ったら単元買い、前日までの過去20日間の最安値を下回ったら単元売り、という売買戦略を実装しています。同時に立てるポジションは１つだけで、いわゆる途転システムになります。

また、`IStrategy`が`IPropertyHolder`を継承しているため、前述の設定ヘルパー用に`Name`と`Properties`を返す必要があります。後者では、メンバー変数の`_Properties`が返っており、その型は`DonchianChannelBreakOutProperties`です。その実装を見てみましょう。

```cs
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
```

基底クラスの`StrategyProperties`は初期資産額の設定を持ちますから、`DonchianChannelBreakOutProperties`は、

* ブレイクアウトを判断する高値の期間
* ブレイクアウトを判断する安値の期間
* 初期資産額

の３つの値を設定できることになります。

![image](https://user-images.githubusercontent.com/63818926/147150035-e6273f92-3ba7-4102-93ec-a35606cf2b3a.png)

### バックテストの実行

バックテストを実行するにはまず、`Arguments`というバックテストの設定を持つクラスのインスタンスを作る必要があります。そのコンストラクタを見てみましょう。

```cs
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
```

引数を表にまとめます。

| 引数 | 型 | 内容 |
| --- | --- | --- |
| strategy | IStrategy | 売買戦略 |
| candles | BackTestCandleCollection | ロウソク足のリスト |
| begin | DateTime | バックテスト開始日 |
| end | DateTime | バックテスト終了日 |
| fc | IFeeCalculator | 手数料計算機 |
| cs | CurrencyStore | 為替ストア |

最後の２つは省略可能で、手数料計算機は後述します。為替ストアは米国株など、株価は日本円以外だが資産は日本円で扱うバックテストのための仕組みですが、現時点では検証が不十分なため、動作保証外とさせていただきます。先頭４つが必須の設定となり、例えば、次のように設定します。

```cs
// 売買戦略
IStrategy strategy = new DonchianChannelBreakOut();
await strategy.SetUpAsync();

// ロウソク足の配列（必ず降順にすること）
Candle[] candles = ...

// バックテスト開始日
DateTime begin = new DateTime(2011, 1, 1);

// バックテスト終了日
DateTime end = new DateTime(2020, 12, 31);

// バックテストの設定
Arguments args = new Arguments(strategy, new BackTestCandleCollection(candles.ToList(), new Stock("N225")), begin, end);
```

これで、ロウソク足の配列`candles`に対し、2011年1月1日から2020年12月31日までの10年間で、売買戦略`DonchianChannelBreakOut`のバックテストを行う設定になります。注意点としては、ロウソク足のリストを**必ず降順**で渡すことです。作成した`Arguments`のインスタンスを`Controller.BackTestAsync()`に渡せば、バックテストを実行できます。

```cs
Controller controller = new Controller();
BackTestResult result = await controller.BackTestAsync<BackTestResult>(args);
```

`BackTestAsync()`から、バックテスト結果を表す`BackTestResult`のインスタンスが得られます。`MagicalNuts.UI.BackTest`を使うと、`BackTestResult`を使って資産推移グラフ、ポジション履歴、バックテスト結果を表示することができます。それぞれ`HistoricalAssetsChart`、`PositionGridView`、`BackTestResultGridView`が該当します。

```cs
private HistoricalAssetsChart chart = new HistoricalAssetsChart();
private PositionGridView positionGridView = new PositionGridView();
private BackTestResultGridView backTestResultGridView = new BackTestResultGridView();

...

// 資産推移
chart.SetHistoricalAssetsList(result.HistoricalAssetsArray);

// ポジション履歴
positionGridView.SetupColumns();
positionGridView.DataSource = result.Positions;

// バックテスト結果
backTestResultGridView.SetupColumns();
backTestResultGridView.DataSource = new BackTestResult[] { result };
```

詳細は`BackTestSample`の`Form1.BackTestSingle()`をご覧ください。

![image](https://user-images.githubusercontent.com/63818926/147150072-81d26815-588d-432d-af17-538117ee6e83.png)

### 手数料の実装

手数料を考慮したバックテストを行うには、`MagicalNuts.BackTest`パッケージの`IFeeCalculator`を実装します。

```cs
/// <summary>
/// 手数料計算機のインターフェイスを表します。
/// </summary>
public interface IFeeCalculator : IPropertyHolder
{
	/// <summary>
	/// エントリー時の手数料を取得します。
	/// </summary>
	/// <param name="state">バックテストの状態</param>
	/// <param name="price">エントリー価格</param>
	/// <param name="lots">エントリーロット数</param>
	/// <param name="currency">エントリー時の為替</param>
	/// <returns>エントリー時の手数料</returns>
	decimal GetEntryFee(BackTestStatus state, decimal price, decimal lots, decimal currency);

	/// <summary>
	/// イグジット時の手数料を取得します。
	/// </summary>
	/// <param name="state">バックテストの状態</param>
	/// <param name="price">イグジット価格</param>
	/// <param name="lots">イグジットロット数</param>
	/// <param name="currency">エントリー時の為替</param>
	/// <returns>イグジット時の手数料</returns>
	decimal GetExitFee(BackTestStatus state, decimal price, decimal lots, decimal currency);

	/// <summary>
	/// 月次手数料を取得します。
	/// </summary>
	/// <param name="state">バックテストの状態</param>
	/// <returns>月次手数料</returns>
	decimal GetMonthlyFee(BackTestStatus state);

	/// <summary>
	/// 年次手数料を取得します。
	/// </summary>
	/// <param name="state">バックテストの状態</param>
	/// <returns>年次手数料</returns>
	decimal GetYearlyFee(BackTestStatus state);
}
```

MagicalNutsでは`IFeeCalculator`を**手数料計算機**のインターフェイスとしており、次の４種類の手数料を実装可能です。

* エントリー時の手数料
* イグジット時の手数料
* 月次手数料
* 年次手数料

一般的には前者２つを使うことが多いと思います。「月次手数料」はSBIネオモバイル証券など月間約定代金をもとに手数料が計算される場合、「年次手数料」は投資信託の信託報酬など年間コストがかかる場合を想定しています。

`BackTestSample`の`FeeCalculatorFixed`クラスの実装を見てみましょう。

```cs
public class FeeCalculatorFixed : IFeeCalculator
{
	private FeeFixedProperties _Properties = null;

	public FeeCalculatorFixed()
	{
		_Properties = new FeeFixedProperties();
	}

	public string Name => "定額";

	public object Properties => _Properties;

	public decimal GetEntryFee(BackTestStatus state, decimal price, decimal lots, decimal currency)
	{
		return _Properties.Fee;
	}

	public decimal GetExitFee(BackTestStatus state, decimal price, decimal lots, decimal currency)
	{
		return _Properties.Fee;
	}

	public decimal GetMonthlyFee(BackTestStatus state)
	{
		return 0;
	}

	public decimal GetYearlyFee(BackTestStatus state)
	{
		return 0;
	}
}
```

実際に返される手数料は`_Properties.Fee`となっています。`_Properties`の型は`FeeFixedProperties`です。

```cs
public class FeeFixedProperties
{
	[Category("定額")]
	[DisplayName("定額手数料")]
	[Description("定額手数料を設定します。")]
	public decimal Fee { get; set; } = 500;
}
```

つまり、`FeeCalculatorFixed`はデフォルトではエントリー時に500円、イグジット時に500円の往復1,000円を手数料として返します。その金額は設定UIで変更可能です。

![image](https://user-images.githubusercontent.com/63818926/147150111-b72119a8-72ec-4ad8-95ff-912d84eefd20.png)

ちなみに、バックテスト時に手数料計算機を指定しなかった場合は「手数料なし」となります。この場合は、`MagicalNuts.BackTest`に実装されている`FeeCalculatorNone`が手数料計算機として使われます。`FeeCalculatorNone`はすべての手数料をゼロで返すクラスです。

`IFeeCalculator`のメソッドはエントリーやイグジット時の価格と株数、`BackTestStatus`のインスタンスを受け取りますので、約定金額や月間の取引量に応じた手数料も計算可能だと思います。

### ドルコスト平均法のバックテスト

`MagicalNuts.BackTest`では**ドルコスト平均法**のバックテストも可能です。`BackTestSample`に、ドルコスト平均法による売買戦略`DollarCostAveraging`を実装しました。

```cs
public class DollarCostAveragingProperties : StrategyProperties
{
	[Category("ドルコスト平均法")]
	[DisplayName("毎月の買い付け日")]
	[Description("毎月の買い付け日を設定します。")]
	public int DayOfEveryMonth { get; set; } = 1;

	[Category("ドルコスト平均法")]
	[DisplayName("毎月の買い付け額")]
	[Description("毎月の買い付け額を設定します。")]
	public decimal BuyAmount { get; set; } = 50000;
}

public class DollarCostAveraging : IStrategy
{
	private DollarCostAveragingProperties _Properties = null;

	public DollarCostAveraging()
	{
		_Properties = new DollarCostAveragingProperties();
		_Properties.InitialAssets = 0;
	}

	public async Task SetUpAsync()
	{
	}

	public string Name => "ドルコスト平均法";

	public object Properties => _Properties;

	public int ReferenceCandlesNum => 0;

	public void GetOrders(BackTestStatus state, List<Order> orders)
	{
		// 買い付け日を取得
		DateTime dt = state.DateTime;
		dt = new DateTime(dt.Year, dt.Month, _Properties.DayOfEveryMonth);

		// 買い付け日でない場合
		if (state.DateTime != dt) return;

		// ロット数取得
		decimal lots = Math.Floor(_Properties.BuyAmount / state.Candles[0].Close);

		// 入金
		state.AdditionalInvest(Math.Ceiling(state.Candles[0].Close * lots));

		// 買い付け
		orders.Add(Order.GetBuyMarketOrder(state.Candles.Stock, lots));
	}
}
```

`GetOrders()`では、今日が毎月の買い付け日（デフォルトでは毎月１日）かどうかを確認し、そうでなければ何もせずに抜けます。買い付け日であった場合は、毎月の購入金額（デフォルトでは５万円）から買い付け株数を計算して買い注文を出しています。ドルコスト平均法では毎月の入金が必要ですが、それは`BackTestStatus.AdditionalInvest()`を使うことでシミュレーション可能です。

また、ドルコスト平均法のバックテストでは、`IFeeCalculator`を使った投資信託のコストシミュレーションも可能です。`BackTestSample`の`FeeCalculatorYearlyCost`の実装を見てみましょう。（説明に必要な箇所のみを抜粋しています。）

```cs
public class FeeYearlyPercentProperties
{
	[Category("年経費率")]
	[DisplayName("年経費率")]
	[Description("年経費率を設定します。")]
	public decimal CostRate { get; set; } = 0.001m;
}

public class FeeCalculatorYearlyCost : IFeeCalculator
{
	private FeeYearlyPercentProperties _Properties = new FeeYearlyPercentProperties();

	public decimal GetYearlyFee(BackTestStatus state)
	{
		return Math.Ceiling(state.MarketAssets * _Properties.CostRate);
	}
}
```

`BackTestSample`には、日経平均株価に対するドルコスト平均法のバックテストが実装されています。次のグラフはその資産推移です。

![image](https://user-images.githubusercontent.com/63818926/147150152-0db6abb3-5c58-42eb-a7aa-23e179a2334b.png)

### 複数銘柄を対象とした売買戦略

`MagicalNuts.BackTest`パッケージは、複数銘柄を対象としたバックテスト機能も持っています。前述した単独銘柄用の売買戦略`DonchianChannelBreakOut`では、最新の終値を取得するには次のようにしていました。

```cs
state.Candles[0].Close
```

このあたりの`BackTestStatus`の実装を確認してみましょう。

```cs
/// <summary>
/// 銘柄ごとの戦略用ロウソク足の集合
/// </summary>
public StrategyCandleCollection[] StockCandles { get; set; }

/// <summary>
/// 単独銘柄を対象とした戦略用ロウソク足の集合
/// </summary>
public StrategyCandleCollection Candles
{
	get
	{
		if (StockCandles == null || StockCandles.Length == 0) return null;
		return StockCandles[0];
	}
	set
	{
		StockCandles = new StrategyCandleCollection[] { value };
	}
}
```

メンバー変数に複数銘柄用の`StrategyCandleCollection[]`を持っていて、単独銘柄用に`StrategyCandleCollection[]`の先頭にアクセスする`Candles`を提供しています。つまり、`MagicalNuts.BackTest`は複数銘柄用が基本実装となっていて、単独銘柄用に便利なアクセサーを用意しているという設計です。

`BackTestSample`に複数銘柄用の売買戦略`DonchianChannelBreakOutMulti`がありますので、`GetOrders()`とその関係個所を見てみましょう。

```cs
public class DonchianChannelBreakOutMulti : IStrategy
{
	// 単独銘柄用戦略
	private DonchianChannelBreakOut SingleStrategy = null;

	public DonchianChannelBreakOutMulti()
	{
		SingleStrategy = new DonchianChannelBreakOut();
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
```

複数銘柄用の売買戦略の場合、イチから実装することも可能ですが、既に実装済みの単独銘柄用の売買戦略を使い回すこともできます。`DonchianChannelBreakOutMulti`は`state.StockCandles.Length`分の`For`文を回し、単独銘柄用の`DonchianChannelBreakOut`に１銘柄ずつ`StrategyCandleCollection`を渡しています。その際、`BackTestStatus.GetBackTestStatusForSingleStrategy()`を使って、複数銘柄用の`BackTestStatus`から単独銘柄用の`BackTestStatus`を作り出すことが可能です。

ちなみに、`DonchianChannelBreakOutMulti`では`Parallel`クラスによる並列処理を実装しています。C#は処理の並列化が簡単なところも大きな魅力の１つだと思います。（並列化は管理の難易度が高いので、きちんと理解した上で使用されることをおすすめします。）

`BackTestSample`は複数銘柄といっても２つだけですが、これを例えば、東証一部やマザーズ、もっと言えば日本の全上場株式を対象にすれば、かなり大がかりなバックテストを実現することも可能となります。

![image](https://user-images.githubusercontent.com/63818926/147150204-4f6b8014-9673-4f53-aa65-8467733491b8.png)

### バックテスト結果とその拡張

`MagicalNuts.BackTest`を使ったバックテストの流れは次のとおりです。

1. バックテストの設定クラス`Arguments`のインスタンスを用意
2. `Controller.BackTestAsync()`に`Arguments`のインスタンスを渡してバックテスト実行
3. バックテスト結果`BackTestResult`のインスタンスを得る

こうして得られた`BackTestResult`のメンバー変数は次のとおりです。

| メンバー変数 | 説明 |
| --- | --- |
| InitialAssets | 初期資産 |
| AdditionalInvestment | 追加投資額 |
| Profit | 総利益 |
| Loss | 総損失 |
| Return | 総損益 |
| AverageProfitRate | 平均利益率 |
| AverageLossRate | 平均損失率 |
| AverageReturnRate | 平均損益率 |
| StandardDeviationReturnRate | 損益率の標準偏差 |
| WinTradeNum | 総勝ち数 |
| LoseTradeNum | 総負け数 |
| MaxConsecutiveWinTradeNum | 最大連勝数 |
| MaxConsecutiveLoseTradeNum | 最大連敗数 |
| MaxDrawdown | 最大ドローダウン |
| MaxDrawdownRate | 最大ドローダウン率 |
| MinMarketAssets | 最小時価資産 |
| WinRate | 勝率 |
| ExpectedReturn | 期待損益 |
| AverageHoldDays | 平均保持日数 |
| ProfitFactor | プロフィットファクター |

ここでは各項目の詳細な説明はしませんが、他のシステムトレードソフトでも使われている一般的な項目を採用しました。

場合によっては、他の項目をバックテスト結果に追加したいケースも出てくると思います。その場合はまず、`BackTestResult`を継承したクラス（ここでは`BackTestResultEx`）を用意します。

```cs
public class BackTestResultEx : BackTestResult
{
	public decimal Extra { get; set; }
}
```

`Controller.BackTestAsync()`はジェネリックメソッドになっているので、呼び出し時に`BackTestResultEx`を指定すると、バックテスト結果を`BackTestResultEx`で受け取ることができます。

```cs
BackTestResultEx result = await controller.BackTestAsync<BackTestResultEx>(args);
result.Extra = ...
```

`Controller.BackTestAsync()`は`BackTestResultEx.Extra`を計算しませんので、ご自身で`BackTestResult`の内容から算出する必要があります。また、`Controller.BackTestAsync()`から呼ばれる`Controller.GetBackTestResult()`はオーバーライド可能なジェネリックメソッドになっているので、`Controller`を継承した`ControllerEx`などを実装して、`GetBackTestResult()`で`BackTestResultEx.Extra`を計算することも可能です。

```cs
public class ControllerEx : Controller
{
	protected override T GetBackTestResult<T>()
	{
		BackTestResultEx result = base.GetBackTestResult<BackTestResultEx>();
		result.Extra = ...
		return (T)(BackTestResult)result;
	}
}
```

## Author

yooce

* Blog : [https://yooce.hatenablog.com/](https://yooce.hatenablog.com/)

## ライセンス

[MIT license](https://en.wikipedia.org/wiki/MIT_License).
