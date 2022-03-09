
namespace TradingChartSample
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.toolStripMenuItemIndicator = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSample = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemMa = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemBb = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemAtr = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemMacd = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(382, 27);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(80, 23);
			this.numericUpDown1.TabIndex = 12;
			this.numericUpDown1.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(235, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 15);
			this.label1.TabIndex = 11;
			this.label1.Text = "画面あたり足数:";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(468, 27);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(50, 23);
			this.button2.TabIndex = 10;
			this.button2.Text = "縮小";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(326, 27);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(50, 23);
			this.button1.TabIndex = 9;
			this.button1.Text = "拡大";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            "分",
            "時間",
            "日",
            "週",
            "月",
            "年"});
			this.comboBox1.Location = new System.Drawing.Point(179, 27);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(50, 23);
			this.comboBox1.TabIndex = 8;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemIndicator});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
			this.menuStrip1.TabIndex = 13;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// toolStripMenuItemIndicator
			// 
			this.toolStripMenuItemIndicator.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSample,
            this.toolStripMenuItemMa,
            this.toolStripMenuItemBb,
            this.toolStripMenuItemAtr,
            this.toolStripMenuItemMacd});
			this.toolStripMenuItemIndicator.Name = "toolStripMenuItemIndicator";
			this.toolStripMenuItemIndicator.Size = new System.Drawing.Size(92, 20);
			this.toolStripMenuItemIndicator.Text = "インジケーター(&I)";
			// 
			// toolStripMenuItemSample
			// 
			this.toolStripMenuItemSample.Name = "toolStripMenuItemSample";
			this.toolStripMenuItemSample.Size = new System.Drawing.Size(169, 22);
			this.toolStripMenuItemSample.Text = "サンプル(&S)";
			this.toolStripMenuItemSample.Click += new System.EventHandler(this.toolStripMenuItemSample_Click);
			// 
			// toolStripMenuItemMa
			// 
			this.toolStripMenuItemMa.Name = "toolStripMenuItemMa";
			this.toolStripMenuItemMa.Size = new System.Drawing.Size(169, 22);
			this.toolStripMenuItemMa.Text = "移動平均(&M)";
			this.toolStripMenuItemMa.Click += new System.EventHandler(this.toolStripMenuItemMa_Click);
			// 
			// toolStripMenuItemBb
			// 
			this.toolStripMenuItemBb.Name = "toolStripMenuItemBb";
			this.toolStripMenuItemBb.Size = new System.Drawing.Size(169, 22);
			this.toolStripMenuItemBb.Text = "ボリンジャーバンド(&B)";
			this.toolStripMenuItemBb.Click += new System.EventHandler(this.toolStripMenuItemBb_Click);
			// 
			// toolStripMenuItemAtr
			// 
			this.toolStripMenuItemAtr.Name = "toolStripMenuItemAtr";
			this.toolStripMenuItemAtr.Size = new System.Drawing.Size(169, 22);
			this.toolStripMenuItemAtr.Text = "ATR(&A)";
			this.toolStripMenuItemAtr.Click += new System.EventHandler(this.toolStripMenuItemAtr_Click);
			// 
			// toolStripMenuItemMacd
			// 
			this.toolStripMenuItemMacd.Name = "toolStripMenuItemMacd";
			this.toolStripMenuItemMacd.Size = new System.Drawing.Size(169, 22);
			this.toolStripMenuItemMacd.Text = "MACD(&D)";
			this.toolStripMenuItemMacd.Click += new System.EventHandler(this.toolStripMenuItemMacd_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Location = new System.Drawing.Point(0, 56);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1008, 481);
			this.panel1.TabIndex = 14;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 31);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(34, 15);
			this.label2.TabIndex = 15;
			this.label2.Text = "銘柄:";
			// 
			// comboBox2
			// 
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Items.AddRange(new object[] {
            "日経平均株価",
            "BTC/USDT"});
			this.comboBox2.Location = new System.Drawing.Point(52, 28);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(121, 23);
			this.comboBox2.TabIndex = 16;
			this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(1008, 537);
			this.Controls.Add(this.comboBox2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.menuStrip1);
			this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox comboBox1;
		private MenuStrip menuStrip1;
		private Panel panel1;
		private ToolStripMenuItem toolStripMenuItemIndicator;
		private ToolStripMenuItem toolStripMenuItemMa;
		private ToolStripMenuItem toolStripMenuItemBb;
		private ToolStripMenuItem toolStripMenuItemAtr;
		private ToolStripMenuItem toolStripMenuItemMacd;
		private ToolStripMenuItem toolStripMenuItemSample;
		private Label label2;
		private ComboBox comboBox2;
	}
}

