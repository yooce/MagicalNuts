using MagicalNuts.Primitive;
using MagicalNuts.UI.Base;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace MagicalNuts.UI.TradingChart
{
	/// <summary>
	/// 価格表示板を表します。
	/// </summary>
	[SupportedOSPlatform("windows")]
	public partial class PriceBoard : UserControl
	{
		/// <summary>
		/// PriceBoardクラスの新しいインスタンスを初期化します。
		/// </summary>
		public PriceBoard()
		{
			InitializeComponent();
		}

		/// <summary>
		/// マージンを設定します。
		/// </summary>
		/// <param name="margin">マージン座標</param>
		public void SetMargin(Point margin)
		{
			// X
			labelOpen.Left = margin.X + labelOpen.Margin.Left;
			labelOpenValue.AlignLeft(labelOpen);

			// Y
			labelOpen.Top = margin.Y + labelOpen.Margin.Top;
			labelOpenValue.Top = labelOpen.Top;
			labelHigh.Top = labelOpen.Top;
			labelHighValue.Top = labelOpen.Top;
			labelLow.Top = labelOpen.Top;
			labelLowValue.Top = labelOpen.Top;
			labelClose.Top = labelOpen.Top;
			labelCloseValue.Top = labelOpen.Top;
			labelUpDown.Top = labelOpen.Top;
			labelUpDownP.Top = labelOpen.Top;
			labelVolume.Top = labelOpen.Top;
			labelVolumeValue.Top = labelOpen.Top;
		}

		/// <summary>
		/// ロウソク足を設定します。
		/// </summary>
		/// <param name="cur">現在のロウソク足</param>
		/// <param name="prev">前のロウソク足</param>
		/// <param name="format">価格表示のフォーマット</param>
		public void SetCandle(Candle cur, Candle prev, string format)
		{
			// 現在のロウソク足が無い場合は非表示
			if (cur == null)
			{
				Visible = false;
				return;
			}

			// 値設定
			labelOpenValue.Text = cur.Open.ToString(format);
			labelHighValue.Text = cur.High.ToString(format);
			labelLowValue.Text = cur.Low.ToString(format);
			labelCloseValue.Text = cur.Close.ToString(format);
			labelVolumeValue.Text = cur.Volume.ToString();
			if (prev != null)
			{
				decimal diff = cur.Close - prev.Close;
				decimal diff_p = (cur.Close / prev.Close - 1) * 100;
				if (diff >= 0)
				{
					labelUpDown.Text = "+" + diff.ToString(format);
					labelUpDownP.Text = "(+" + diff_p.ToString("0.00") + "%)";
				}
				else
				{
					labelUpDown.Text = diff.ToString(format);
					labelUpDownP.Text = "(" + diff_p.ToString("0.00") + "%)";
				}
			}
			else
			{
				labelUpDown.Text = "-";
				labelUpDownP.Text = "(-%)";
			}

			// 色設定
			if (cur.Close - cur.Open >= 0)
			{
				labelOpenValue.ForeColor = ChartPalette.PriceUpColor;
				labelHighValue.ForeColor = ChartPalette.PriceUpColor;
				labelLowValue.ForeColor = ChartPalette.PriceUpColor;
				labelCloseValue.ForeColor = ChartPalette.PriceUpColor;
				labelVolumeValue.ForeColor = ChartPalette.PriceUpColor;
				labelUpDown.ForeColor = ChartPalette.PriceUpColor;
				labelUpDownP.ForeColor = ChartPalette.PriceUpColor;
			}
			else
			{
				labelOpenValue.ForeColor = ChartPalette.PriceDownColor;
				labelHighValue.ForeColor = ChartPalette.PriceDownColor;
				labelLowValue.ForeColor = ChartPalette.PriceDownColor;
				labelCloseValue.ForeColor = ChartPalette.PriceDownColor;
				labelVolumeValue.ForeColor = ChartPalette.PriceDownColor;
				labelUpDown.ForeColor = ChartPalette.PriceDownColor;
				labelUpDownP.ForeColor = ChartPalette.PriceDownColor;
			}

			// 配置
			labelHigh.AlignLeft(labelOpenValue);
			labelHighValue.AlignLeft(labelHigh);
			labelLow.AlignLeft(labelHighValue);
			labelLowValue.AlignLeft(labelLow);
			labelClose.AlignLeft(labelLowValue);
			labelCloseValue.AlignLeft(labelClose);
			labelUpDown.AlignLeft(labelCloseValue);
			labelUpDownP.AlignLeft(labelUpDown);
			labelVolume.AlignLeft(labelUpDownP);
			labelVolumeValue.AlignLeft(labelVolume);

			// 表示
			Visible = true;
		}
	}
}
