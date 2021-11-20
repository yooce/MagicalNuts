using MagicalNuts.Primitive;
using MagicalNuts.UI.TradingChart.Plotter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.UI.TradingChart
{
	/// <summary>
	/// トレード用チャートコントロールを表します。
	/// </summary>
	[SupportedOSPlatform("windows")]
	public class TradingChart : Chart
	{
		/// <summary>
		/// ロウソク足の期間を取得または設定します。
		/// </summary>
		public CandlePeriod CandlePeriod
		{
			get => _CandlePeriod;
			set
			{
				_CandlePeriod = value;

				// すべてをプロット
				PlotAll();
			}
		}

		/// <summary>
		/// 画面あたりの足数を設定します。
		/// </summary>
		public int ScreenCandleNum
		{
			set
			{
				MainChartArea.AxisX.ScaleView.Size = value;

				// Y軸設定更新
				UpdateAxisYSettings();
			}
		}

		/// <summary>
		/// ロウソク足の期間
		/// </summary>
		private CandlePeriod _CandlePeriod = CandlePeriod.Dayly;

		/// <summary>
		/// 主ChartArea
		/// </summary>
		private MainChartArea MainChartArea = null;

		/// <summary>
		/// 従ChartAreaのリスト
		/// </summary>
		private List<SubChartArea> SubChartAreas = null;

		/// <summary>
		/// ドラッグ開始x座標
		/// </summary>
		private double DragStartX = double.NaN;

		/// <summary>
		/// プロッターのリスト
		/// </summary>
		private List<IPlotter> Plotters = null;

		/// <summary>
		/// 操作中の分割線
		/// </summary>
		private HorizontalLineAnnotation MovingSplitter = null;

		/// <summary>
		/// スクロール中かどうかを示します。
		/// </summary>
		private bool IsScrolling = false;

		/// <summary>
		/// 銘柄コード
		/// </summary>
		private string Code = null;

		/// <summary>
		/// 日足のリスト
		/// </summary>
		private List<Candle> DailyCandles = null;

		/// <summary>
		/// 表示中のロウソク足のリスト
		/// </summary>
		private List<Candle> DisplayCandles = null;

		/// <summary>
		/// 価格表示フォーマット
		/// </summary>
		private string PriceFormat = null;

		/// <summary>
		/// TradingChartクラスの新しいインスタンスを初期化します。
		/// </summary>
		public TradingChart() : base()
		{
			SubChartAreas = new List<SubChartArea>();
			Plotters = new List<IPlotter>();
		}

		/// <summary>
		/// チャートを準備します。
		/// </summary>
		public void SetUp()
		{
			// Chart
			ChartAreas.Clear();

			// Chartイベント
			MouseWheel += new MouseEventHandler(chart_MouseWheel);
			MouseMove += new MouseEventHandler(chart_MouseMove);
			MouseDown += new MouseEventHandler(chart_MouseDown);
			MouseUp += new MouseEventHandler(chart_MouseUp);
			AnnotationPositionChanging += new EventHandler<AnnotationPositionChangingEventArgs>(chart_AnnotationPositionChanging);
			AxisScrollBarClicked += new EventHandler<ScrollBarEventArgs>(chart_AxisScrollBarClicked);

			// 主ChartArea
			MainChartArea = new MainChartArea();
			ChartAreas.Add(MainChartArea);
			MainChartArea.SetUp(this);

			// デフォルトプロッター
			AddPlotter(new CandlePlotter());
			AddPlotter(new VolumePlotter());
		}

		/// <summary>
		/// 日足を設定します。
		/// </summary>
		/// <param name="code">銘柄コード</param>
		/// <param name="candles">ロウソク足の配列</param>
		/// <param name="digits">小数点以下の桁数</param>
		public void SetDailyCandles(string code, Candle[] candles, int digits = 2)
		{
			// 銘柄コード
			Code = code;

			// 日足設定
			DailyCandles = new List<Candle>();
			DailyCandles.AddRange(candles);

			// 価格表示フォーマット取得
			PriceFormat = PriceFormatter.GetPriceFormatFromDigits(digits);

			// カーソルインターバル
			MainChartArea.CursorY.Interval = MainChartArea.GetCursorIntervalFromDigits(digits);

			// 期間変換
			CandlePeriod = CandlePeriod.Dayly;
		}

		/// <summary>
		/// すべてをプロットします。
		/// </summary>
		private void PlotAll()
		{
			// 日足が無ければ何もしない
			if (DailyCandles == null) return;

			// チャートクリア
			Series.Clear();
			MainChartArea.Clear();
			foreach (SubChartArea subChartArea in SubChartAreas)
			{
				subChartArea.Clear();
			}

			// 期間変換
			DisplayCandles = CandleConverter.ConvertFromDaily(DailyCandles, _CandlePeriod);

			// 主ChartArea
			MainChartArea.SetCandles(DisplayCandles, PriceFormatter.GetDigitsFromFormat(PriceFormat).Value);

			// プロット
			foreach (IPlotter plotter in Plotters)
			{
				PlotPlotter(plotter, false);
			}

			// CustomLabel
			DateTime? prevLabelDateTime = null;
			for (int x = 0; x < DisplayCandles.Count; x++)
			{
				if (NeedCustomLabel(DisplayCandles[x], prevLabelDateTime))
				{
					// 主ChartArea
					MainChartArea.AxisX.CustomLabels.Add(GetMainCustomLabel(x, prevLabelDateTime));

					// 従ChartArea
					foreach (SubChartArea subChartArea in SubChartAreas)
					{
						subChartArea.AxisX.CustomLabels.Add(GetSubCustomLabel(x));
					}

					prevLabelDateTime = DisplayCandles[x].DateTime;
				}
			}

			// チャートにSeries追加
			foreach (IPlotter plotter in Plotters)
			{
				foreach (Series series in plotter.SeriesArray)
				{
					Series.Add(series);
				}
			}

			// 初期位置
			MainChartArea.AxisX.ScaleView.Position = DisplayCandles.Count - MainChartArea.AxisX.ScaleView.Size;

			// Y軸設定更新
			UpdateAxisYSettings();
		}

		/// <summary>
		/// Y軸設定を更新します。
		/// </summary>
		private void UpdateAxisYSettings()
		{
			// ウィンドウサイズ変更時にPositionがNaNの場合は何もしない
			if (double.IsNaN(MainChartArea.AxisX.ScaleView.Position)) return;

			// 開始位置決定
			int begin = (int)MainChartArea.AxisX.ScaleView.Position;
			if (begin < 0) begin = 0;
			else if (begin > DisplayCandles.Count - MainChartArea.AxisX.ScaleView.Size)
				begin = DisplayCandles.Count - (int)MainChartArea.AxisX.ScaleView.Size;

			// 終了位置決定
			int end = begin + (int)MainChartArea.AxisX.ScaleView.Size;

			// 主ChartArea
			MainChartArea.UpdateAxisYSettings(begin, end, Plotters);

			// 従ChartArea
			foreach (SubChartArea subChartArea in SubChartAreas)
			{
				subChartArea.UpdateAxisYSettings(begin, end, Plotters);
			}
		}

		/// <summary>
		/// 従ChartAreaを追加します。
		/// </summary>
		/// <param name="subChartArea">従ChartArea</param>
		private void AddSubChartArea(SubChartArea subChartArea)
		{
			// 準備
			subChartArea.SetUp(this);

			// 主ChartAreaとの連動設定
			subChartArea.AlignWithChartArea = MainChartArea.Name;
			subChartArea.AlignmentStyle = AreaAlignmentStyles.All;

			// 配置
			if (SubChartAreas.Count == 0)
			{
				// 前が主ChartArea
				subChartArea.Splitter.Y = 75;
				MainChartArea.Position.Height = (float)subChartArea.Splitter.Y;
			}
			else
			{
				// 前が従ChartArea
				SubChartArea prevSubChartArea = SubChartAreas.Last();
				subChartArea.Splitter.Y = prevSubChartArea.Splitter.Y + prevSubChartArea.Position.Height / 2;
				prevSubChartArea.Position.Height = prevSubChartArea.Position.Height / 2;
			}
			subChartArea.Position.X = 0;
			subChartArea.Position.Y = (float)subChartArea.Splitter.Y;
			subChartArea.Position.Width = 100;
			subChartArea.Position.Height = 100 - (float)subChartArea.Splitter.Y;

			// 分割線追加
			Annotations.Add(subChartArea.Splitter);

			// 従ChartArea追加
			SubChartAreas.Add(subChartArea);
			ChartAreas.Add(subChartArea);
		}

		#region Plotter

		/// <summary>
		/// プロットします。
		/// </summary>
		/// <param name="plotter">プロッター</param>
		/// <param name="updatey">Y軸設定を更新するかどうか</param>
		public void PlotPlotter(IPlotter plotter, bool updatey = true)
		{
			// ロウソク足設定済みでなければ何もしない
			if (DisplayCandles == null) return;

			// クリア
			foreach (Series series in plotter.SeriesArray)
			{
				series.Points.Clear();
			}

			// プロット
			plotter.Plot(Code, DisplayCandles);

			// Y軸設定更新
			if (updatey) UpdateAxisYSettings();
		}

		/// <summary>
		/// プロッターを追加します。
		/// </summary>
		/// <param name="plotter">プロッター</param>
		public void AddPlotter(IPlotter plotter)
		{
			// ChartArea設定
			SubChartArea[] subChartAreas = plotter.SetChartArea(MainChartArea);

			// 従ChartAreaを使う場合
			if (subChartAreas != null)
			{
				foreach (SubChartArea subChartArea in subChartAreas)
				{
					AddSubChartArea(subChartArea);
				}
			}

			// 追加
			Plotters.Add(plotter);

			// ロウソク足設定済みの場合
			if (DisplayCandles != null)
			{
				// 主ChartAreaの位置と大きさを覚えておく（従ChartAreaを操作しているうちに値がリセットされる？）
				double position = MainChartArea.AxisX.ScaleView.Position;
				double size = MainChartArea.AxisX.ScaleView.Size;

				// プロット
				PlotPlotter(plotter);

				foreach (Series series in plotter.SeriesArray)
				{
					// チャートにSeries追加
					Series.Add(series);

					// 従ChartAreaのCustomLabel
					foreach (SubChartArea subChartArea in SubChartAreas)
					{
						// 追加されたプロッターが使う従ChartAreaでなければスキップ
						if (subChartArea.Name != series.ChartArea) continue;

						DateTime? prevLabelDateTime = null;
						for (int x = 0; x < DisplayCandles.Count; x++)
						{
							if (NeedCustomLabel(DisplayCandles[x], prevLabelDateTime))
							{
								subChartArea.AxisX.CustomLabels.Add(GetSubCustomLabel(x));
								prevLabelDateTime = DisplayCandles[x].DateTime;
							}
						}

						// 位置
						subChartArea.AxisX.ScaleView.Position = position;
						subChartArea.AxisX.ScaleView.Size = size;
					}
				}
			}
		}

		/// <summary>
		/// プロッターを除去します。
		/// </summary>
		/// <param name="plotter">プロッター</param>
		public void RemovePlotter(IPlotter plotter)
		{
			// 除去
			Plotters.Remove(plotter);

			// ロウソク足設定済みの場合
			if (DisplayCandles != null)
			{
				foreach (Series series in plotter.SeriesArray)
				{
					// チャートからSeries除去
					Series.Remove(series);

					// 使用している従ChartAreaを探す
					for (int i = SubChartAreas.Count - 1; i >= 0; i--)
					{
						if (SubChartAreas[i].Name == series.ChartArea)
						{
							if (i == 0)
							{
								// 前が主ChartArea
								MainChartArea.Position.Height = MainChartArea.Position.Height + SubChartAreas[i].Position.Height;
							}
							else
							{
								// 前が従ChartArea
								SubChartAreas[i - 1].Position.Height
									= SubChartAreas[i - 1].Position.Height + SubChartAreas[i].Position.Height;
							}

							// 分割線除去
							Annotations.Remove(SubChartAreas[i].Splitter);

							// 従ChartArea除去
							ChartAreas.Remove(SubChartAreas[i]);
							SubChartAreas.RemoveAt(i);
						}
					}
				}

				// Y軸設定更新
				UpdateAxisYSettings();
			}
		}

		#endregion

		#region CustomLabel

		/// <summary>
		/// CustomLabelが必要かどうかを判定します。
		/// </summary>
		/// <param name="candle">ロウソク足</param>
		/// <param name="prevDateTime">前回CustomLabelを表示した日時</param>
		/// <returns>CustomLabelが必要かどうか</returns>
		private bool NeedCustomLabel(Candle candle, DateTime? prevDateTime)
		{
			// 前回日時が無い
			if (prevDateTime == null) return true;

			// 次のDateTime算出
			DateTime nextDateTime = DateTime.MinValue;
			switch (_CandlePeriod)
			{
				case CandlePeriod.Dayly:
					nextDateTime = new DateTime(prevDateTime.Value.Year, prevDateTime.Value.Month, 1).AddMonths(1);
					break;
				case CandlePeriod.Weekly:
					nextDateTime = new DateTime(prevDateTime.Value.Year, (prevDateTime.Value.Month - 1) / 3 * 3 + 1, 1).AddMonths(3);
					break;
				case CandlePeriod.Monthly:
					nextDateTime = new DateTime(prevDateTime.Value.Year, 1, 1).AddYears(1);
					break;
				case CandlePeriod.Yearly:
					nextDateTime = new DateTime(prevDateTime.Value.Year / 10 * 10, 1, 1).AddYears(10);
					break;
			}

			// 次のDateTimeを超えているか
			return candle.DateTime >= nextDateTime;
		}

		/// <summary>
		/// 主ChartArea用のCustomLabelを取得します。
		/// </summary>
		/// <param name="x">x座標</param>
		/// <param name="prev">前回CustomLabelを表示した日時</param>
		/// <returns>主ChartArea用のCustomLabel</returns>
		private CustomLabel GetMainCustomLabel(int x, DateTime? prev)
		{
			return new CustomLabel(x - 50.0, x + 50.0, GetMainCustomeLabelName(prev, DisplayCandles[x].DateTime), 0, LabelMarkStyle.None
				, GridTickTypes.Gridline);
		}

		/// <summary>
		/// 従ChartArea用のCustomLabelを取得します。
		/// </summary>
		/// <param name="x">x座標</param>
		/// <returns>従ChartArea用のCustomLabel</returns>
		private CustomLabel GetSubCustomLabel(int x)
		{
			return new CustomLabel(x - 50.0, x + 50.0, "", 0, LabelMarkStyle.None, GridTickTypes.Gridline);
		}

		/// <summary>
		/// 主ChartArea用のCustomeLabelの名前を取得します。
		/// </summary>
		/// <param name="prev">前回の日時</param>
		/// <param name="cur">今回の日時</param>
		/// <returns>主ChartArea用のCustomeLabelの名前</returns>
		private string GetMainCustomeLabelName(DateTime? prev, DateTime cur)
		{
			string name = "";
			switch (_CandlePeriod)
			{
				case CandlePeriod.Dayly:
				case CandlePeriod.Weekly:
					if (prev == null || prev.Value.Year != cur.Year) name = cur.ToString("yyyy");
					else name = cur.ToString("M月");
					break;
				case CandlePeriod.Monthly:
				case CandlePeriod.Yearly:
					name = cur.ToString("yyyy");
					break;
			}
			return name;
		}

		#endregion

		#region イベントハンドラ

		/// <summary>
		/// MouseMoveイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void chart_MouseMove(object sender, MouseEventArgs e)
		{
			if (MovingSplitter != null)
			{
				// 分割線の操作

				// ChartArea配置
				for (int i = 0; i < SubChartAreas.Count; i++)
				{
					if (SubChartAreas[i].Splitter == MovingSplitter)
					{
						if (i == 0)
						{
							// 前が主ChartArea
							float bottom = SubChartAreas[i].Position.Y + SubChartAreas[i].Position.Height;
							MainChartArea.Position.Height = (float)MovingSplitter.Y;
							SubChartAreas[i].Position.Height = (float)(bottom - MovingSplitter.Y);
							SubChartAreas[i].Position.Y = (float)MovingSplitter.Y;
						}
						else
						{
							// 前が従ChartArea
							float bottom = SubChartAreas[i].Position.Y + SubChartAreas[i].Position.Height;
							SubChartAreas[i - 1].Position.Height = (float)MovingSplitter.Y - SubChartAreas[i - 1].Position.Y;
							SubChartAreas[i].Position.Height = (float)(bottom - MovingSplitter.Y);
							SubChartAreas[i].Position.Y = (float)MovingSplitter.Y;
						}
						break;
					}
				}

				// 再描画
				Update();
			}
			else if (IsScrolling)
			{
				// スクロールバーの操作

				// Y軸設定更新
				UpdateAxisYSettings();
			}
			else
			{
				// ロウソク足が無かったら何もしない
				if (DisplayCandles == null) return;

				// マウス座標取得
				Point mouse = e.Location;

				// プロットエリアのヒットテスト
				HitTestResult[] results = HitTest(mouse.X, mouse.Y, false, ChartElementType.PlottingArea);
				foreach (var result in results)
				{
					// プロットエリアでなければスキップ
					if (result.ChartElementType != ChartElementType.PlottingArea || result.ChartArea == null) continue;

					// グラフ上の位置取得
					int x = (int)(MainChartArea.AxisX.PixelPositionToValue(mouse.X) + 0.5);

					// ロウソク足の範囲外ならスキップ
					if (x < 0 || x >= DisplayCandles.Count) continue;

					// カーソル更新
					MainChartArea.UpdateCursors(mouse, result, x, DisplayCandles.Count - 1, PriceFormat);
					foreach (SubChartArea subChartArea in SubChartAreas)
					{
						subChartArea.UpdateCursors(mouse, result, x, DisplayCandles.Count - 1, PriceFormat);
					}
				}

				// スクロール
				if (e.Button.HasFlag(MouseButtons.Left))
				{
					// マウス移動分だけチャートも移動
					MainChartArea.AxisX.ScaleView.Position -= (MainChartArea.AxisX.PixelPositionToValue(mouse.X) - DragStartX);

					// Y軸設定更新
					UpdateAxisYSettings();
				}
			}
		}

		/// <summary>
		/// MouseDownイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void chart_MouseDown(object sender, MouseEventArgs e)
		{
			// x座標を覚えておく
			DragStartX = MainChartArea.AxisX.PixelPositionToValue(e.Location.X);
		}

		/// <summary>
		/// AxisScrollBarClickedイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void chart_AxisScrollBarClicked(object sender, ScrollBarEventArgs e)
		{
			IsScrolling = true;
		}

		/// <summary>
		/// MouseUpイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void chart_MouseUp(object sender, MouseEventArgs e)
		{
			MovingSplitter = null;
			IsScrolling = false;

			// Y軸設定更新
			if (DisplayCandles != null) UpdateAxisYSettings();
		}

		/// <summary>
		/// MouseWheelイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void chart_MouseWheel(object sender, MouseEventArgs e)
		{
			// ホイールを動かした分だけチャートも移動
			MainChartArea.AxisX.ScaleView.Position += e.Delta / 120 * 60;

			// Y軸設定更新
			if (DisplayCandles != null) UpdateAxisYSettings();
		}

		/// <summary>
		/// AnnotationPositionChangingイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void chart_AnnotationPositionChanging(object sender, AnnotationPositionChangingEventArgs e)
		{
			// x座標は０固定
			e.NewLocationX = 0;

			// 操作中の分割線を覚えておく
			MovingSplitter = sender as HorizontalLineAnnotation;
		}

		#endregion
	}
}
