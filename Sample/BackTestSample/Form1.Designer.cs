
using MagicalNuts.UI.BackTest;

namespace BackTestSample
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.labelBegin = new System.Windows.Forms.Label();
			this.dateTimePickerBegin = new System.Windows.Forms.DateTimePicker();
			this.labelEnd = new System.Windows.Forms.Label();
			this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
			this.buttonStart = new System.Windows.Forms.Button();
			this.labelFee = new System.Windows.Forms.Label();
			this.comboBoxFee = new System.Windows.Forms.ComboBox();
			this.buttonFee = new System.Windows.Forms.Button();
			this.splitContainerSingle = new System.Windows.Forms.SplitContainer();
			this.splitContainerSingleTop = new System.Windows.Forms.SplitContainer();
			this.buttonCopy = new System.Windows.Forms.Button();
			this.positionGridView = new MagicalNuts.UI.BackTest.PositionGridView();
			this.backTestResultGridView = new MagicalNuts.UI.BackTest.BackTestResultGridView();
			this.comboBoxStrategy = new System.Windows.Forms.ComboBox();
			this.labelStrategy = new System.Windows.Forms.Label();
			this.buttonStrategy = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerSingle)).BeginInit();
			this.splitContainerSingle.Panel1.SuspendLayout();
			this.splitContainerSingle.Panel2.SuspendLayout();
			this.splitContainerSingle.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerSingleTop)).BeginInit();
			this.splitContainerSingleTop.Panel1.SuspendLayout();
			this.splitContainerSingleTop.Panel2.SuspendLayout();
			this.splitContainerSingleTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.positionGridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.backTestResultGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// labelBegin
			// 
			this.labelBegin.AutoSize = true;
			this.labelBegin.Location = new System.Drawing.Point(12, 16);
			this.labelBegin.Name = "labelBegin";
			this.labelBegin.Size = new System.Drawing.Size(34, 15);
			this.labelBegin.TabIndex = 4;
			this.labelBegin.Text = "開始:";
			// 
			// dateTimePickerBegin
			// 
			this.dateTimePickerBegin.Location = new System.Drawing.Point(52, 12);
			this.dateTimePickerBegin.Name = "dateTimePickerBegin";
			this.dateTimePickerBegin.Size = new System.Drawing.Size(125, 23);
			this.dateTimePickerBegin.TabIndex = 5;
			// 
			// labelEnd
			// 
			this.labelEnd.AutoSize = true;
			this.labelEnd.Location = new System.Drawing.Point(183, 16);
			this.labelEnd.Name = "labelEnd";
			this.labelEnd.Size = new System.Drawing.Size(34, 15);
			this.labelEnd.TabIndex = 6;
			this.labelEnd.Text = "終了:";
			// 
			// dateTimePickerEnd
			// 
			this.dateTimePickerEnd.Location = new System.Drawing.Point(223, 12);
			this.dateTimePickerEnd.Name = "dateTimePickerEnd";
			this.dateTimePickerEnd.Size = new System.Drawing.Size(125, 23);
			this.dateTimePickerEnd.TabIndex = 7;
			// 
			// buttonStart
			// 
			this.buttonStart.Location = new System.Drawing.Point(610, 40);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(75, 23);
			this.buttonStart.TabIndex = 8;
			this.buttonStart.Text = "開始";
			this.buttonStart.UseVisualStyleBackColor = true;
			this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
			// 
			// labelFee
			// 
			this.labelFee.AutoSize = true;
			this.labelFee.Location = new System.Drawing.Point(344, 44);
			this.labelFee.Name = "labelFee";
			this.labelFee.Size = new System.Drawing.Size(46, 15);
			this.labelFee.TabIndex = 16;
			this.labelFee.Text = "手数料:";
			// 
			// comboBoxFee
			// 
			this.comboBoxFee.FormattingEnabled = true;
			this.comboBoxFee.Location = new System.Drawing.Point(396, 41);
			this.comboBoxFee.Name = "comboBoxFee";
			this.comboBoxFee.Size = new System.Drawing.Size(125, 23);
			this.comboBoxFee.TabIndex = 17;
			// 
			// buttonFee
			// 
			this.buttonFee.Location = new System.Drawing.Point(527, 39);
			this.buttonFee.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.buttonFee.Name = "buttonFee";
			this.buttonFee.Size = new System.Drawing.Size(80, 25);
			this.buttonFee.TabIndex = 11;
			this.buttonFee.Text = "手数料設定";
			this.buttonFee.UseVisualStyleBackColor = true;
			this.buttonFee.Click += new System.EventHandler(this.buttonFee_Click);
			// 
			// splitContainerSingle
			// 
			this.splitContainerSingle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainerSingle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainerSingle.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.splitContainerSingle.Location = new System.Drawing.Point(0, 70);
			this.splitContainerSingle.Name = "splitContainerSingle";
			this.splitContainerSingle.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerSingle.Panel1
			// 
			this.splitContainerSingle.Panel1.Controls.Add(this.splitContainerSingleTop);
			// 
			// splitContainerSingle.Panel2
			// 
			this.splitContainerSingle.Panel2.Controls.Add(this.backTestResultGridView);
			this.splitContainerSingle.Size = new System.Drawing.Size(1008, 467);
			this.splitContainerSingle.SplitterDistance = 350;
			this.splitContainerSingle.TabIndex = 18;
			// 
			// splitContainerSingleTop
			// 
			this.splitContainerSingleTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainerSingleTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerSingleTop.Location = new System.Drawing.Point(0, 0);
			this.splitContainerSingleTop.Name = "splitContainerSingleTop";
			// 
			// splitContainerSingleTop.Panel1
			// 
			this.splitContainerSingleTop.Panel1.Controls.Add(this.buttonCopy);
			// 
			// splitContainerSingleTop.Panel2
			// 
			this.splitContainerSingleTop.Panel2.Controls.Add(this.positionGridView);
			this.splitContainerSingleTop.Size = new System.Drawing.Size(1008, 350);
			this.splitContainerSingleTop.SplitterDistance = 676;
			this.splitContainerSingleTop.TabIndex = 0;
			// 
			// buttonCopy
			// 
			this.buttonCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCopy.Location = new System.Drawing.Point(1233, 587);
			this.buttonCopy.Name = "buttonCopy";
			this.buttonCopy.Size = new System.Drawing.Size(75, 23);
			this.buttonCopy.TabIndex = 12;
			this.buttonCopy.Text = "コピー";
			this.buttonCopy.UseVisualStyleBackColor = true;
			// 
			// positionGridView
			// 
			this.positionGridView.AllowUserToAddRows = false;
			this.positionGridView.AllowUserToDeleteRows = false;
			this.positionGridView.AllowUserToOrderColumns = true;
			this.positionGridView.AllowUserToResizeRows = false;
			this.positionGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.positionGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.positionGridView.Location = new System.Drawing.Point(0, 0);
			this.positionGridView.Name = "positionGridView";
			this.positionGridView.ReadOnly = true;
			this.positionGridView.RowHeadersVisible = false;
			this.positionGridView.RowTemplate.Height = 25;
			this.positionGridView.Size = new System.Drawing.Size(326, 348);
			this.positionGridView.TabIndex = 0;
			// 
			// backTestResultGridView
			// 
			this.backTestResultGridView.AllowUserToAddRows = false;
			this.backTestResultGridView.AllowUserToDeleteRows = false;
			this.backTestResultGridView.AllowUserToOrderColumns = true;
			this.backTestResultGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.backTestResultGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.backTestResultGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.backTestResultGridView.DefaultCellStyle = dataGridViewCellStyle2;
			this.backTestResultGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.backTestResultGridView.Location = new System.Drawing.Point(0, 0);
			this.backTestResultGridView.Name = "backTestResultGridView";
			this.backTestResultGridView.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.backTestResultGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.backTestResultGridView.RowHeadersVisible = false;
			this.backTestResultGridView.RowTemplate.Height = 25;
			this.backTestResultGridView.Size = new System.Drawing.Size(1006, 111);
			this.backTestResultGridView.TabIndex = 0;
			// 
			// comboBoxStrategy
			// 
			this.comboBoxStrategy.FormattingEnabled = true;
			this.comboBoxStrategy.Location = new System.Drawing.Point(52, 41);
			this.comboBoxStrategy.Name = "comboBoxStrategy";
			this.comboBoxStrategy.Size = new System.Drawing.Size(200, 23);
			this.comboBoxStrategy.TabIndex = 19;
			// 
			// labelStrategy
			// 
			this.labelStrategy.AutoSize = true;
			this.labelStrategy.Location = new System.Drawing.Point(12, 44);
			this.labelStrategy.Name = "labelStrategy";
			this.labelStrategy.Size = new System.Drawing.Size(34, 15);
			this.labelStrategy.TabIndex = 20;
			this.labelStrategy.Text = "戦略:";
			// 
			// buttonStrategy
			// 
			this.buttonStrategy.Location = new System.Drawing.Point(258, 40);
			this.buttonStrategy.Name = "buttonStrategy";
			this.buttonStrategy.Size = new System.Drawing.Size(80, 23);
			this.buttonStrategy.TabIndex = 22;
			this.buttonStrategy.Text = "戦略設定";
			this.buttonStrategy.UseVisualStyleBackColor = true;
			this.buttonStrategy.Click += new System.EventHandler(this.buttonStrategy_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(1008, 537);
			this.Controls.Add(this.buttonStrategy);
			this.Controls.Add(this.labelStrategy);
			this.Controls.Add(this.comboBoxStrategy);
			this.Controls.Add(this.splitContainerSingle);
			this.Controls.Add(this.buttonFee);
			this.Controls.Add(this.comboBoxFee);
			this.Controls.Add(this.labelFee);
			this.Controls.Add(this.buttonStart);
			this.Controls.Add(this.dateTimePickerEnd);
			this.Controls.Add(this.labelEnd);
			this.Controls.Add(this.dateTimePickerBegin);
			this.Controls.Add(this.labelBegin);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.BackTestForm_Load);
			this.splitContainerSingle.Panel1.ResumeLayout(false);
			this.splitContainerSingle.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerSingle)).EndInit();
			this.splitContainerSingle.ResumeLayout(false);
			this.splitContainerSingleTop.Panel1.ResumeLayout(false);
			this.splitContainerSingleTop.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerSingleTop)).EndInit();
			this.splitContainerSingleTop.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.positionGridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.backTestResultGridView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label labelBegin;
		private System.Windows.Forms.DateTimePicker dateTimePickerBegin;
		private System.Windows.Forms.Label labelEnd;
		private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.Label labelFee;
		private System.Windows.Forms.ComboBox comboBoxFee;
		private System.Windows.Forms.Button buttonFee;
		private SplitContainer splitContainerSingle;
		private SplitContainer splitContainerSingleTop;
		private Button buttonCopy;
		private PositionGridView positionGridView;
		private BackTestResultGridView backTestResultGridView;
		private ComboBox comboBoxStrategy;
		private Label labelStrategy;
		private Button buttonStrategy;
	}
}