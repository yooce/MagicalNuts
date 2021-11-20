namespace ShareholderIncentiveSample
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.splitContainerTop = new System.Windows.Forms.SplitContainer();
			this.historicalDataGridView = new MagicalNuts.UI.ShareholderIncentive.HistoricalDataGridView();
			this.entryExitExpectedValueGridView = new MagicalNuts.UI.ShareholderIncentive.EntryExitExpectedValueGridView();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).BeginInit();
			this.splitContainerTop.Panel1.SuspendLayout();
			this.splitContainerTop.Panel2.SuspendLayout();
			this.splitContainerTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.historicalDataGridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.entryExitExpectedValueGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer
			// 
			this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(0, 0);
			this.splitContainer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.splitContainerTop);
			this.splitContainer.Size = new System.Drawing.Size(1008, 537);
			this.splitContainer.SplitterDistance = 350;
			this.splitContainer.SplitterWidth = 5;
			this.splitContainer.TabIndex = 3;
			// 
			// splitContainerTop
			// 
			this.splitContainerTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainerTop.Cursor = System.Windows.Forms.Cursors.VSplit;
			this.splitContainerTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerTop.Location = new System.Drawing.Point(0, 0);
			this.splitContainerTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.splitContainerTop.Name = "splitContainerTop";
			// 
			// splitContainerTop.Panel1
			// 
			this.splitContainerTop.Panel1.Controls.Add(this.historicalDataGridView);
			// 
			// splitContainerTop.Panel2
			// 
			this.splitContainerTop.Panel2.Controls.Add(this.entryExitExpectedValueGridView);
			this.splitContainerTop.Size = new System.Drawing.Size(1008, 182);
			this.splitContainerTop.SplitterDistance = 520;
			this.splitContainerTop.SplitterWidth = 5;
			this.splitContainerTop.TabIndex = 1;
			// 
			// historicalDataGridView
			// 
			this.historicalDataGridView.AllowUserToAddRows = false;
			this.historicalDataGridView.AllowUserToDeleteRows = false;
			this.historicalDataGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.historicalDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.historicalDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.historicalDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
			this.historicalDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.historicalDataGridView.Location = new System.Drawing.Point(0, 0);
			this.historicalDataGridView.Name = "historicalDataGridView";
			this.historicalDataGridView.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.historicalDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.historicalDataGridView.RowHeadersVisible = false;
			this.historicalDataGridView.RowTemplate.Height = 25;
			this.historicalDataGridView.Size = new System.Drawing.Size(518, 180);
			this.historicalDataGridView.TabIndex = 0;
			// 
			// entryExitExpectedValueGridView
			// 
			this.entryExitExpectedValueGridView.AllowUserToAddRows = false;
			this.entryExitExpectedValueGridView.AllowUserToDeleteRows = false;
			this.entryExitExpectedValueGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.entryExitExpectedValueGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.entryExitExpectedValueGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.entryExitExpectedValueGridView.DefaultCellStyle = dataGridViewCellStyle5;
			this.entryExitExpectedValueGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.entryExitExpectedValueGridView.Location = new System.Drawing.Point(0, 0);
			this.entryExitExpectedValueGridView.Name = "entryExitExpectedValueGridView";
			this.entryExitExpectedValueGridView.ReadOnly = true;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle6.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.entryExitExpectedValueGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.entryExitExpectedValueGridView.RowHeadersVisible = false;
			this.entryExitExpectedValueGridView.RowTemplate.Height = 25;
			this.entryExitExpectedValueGridView.Size = new System.Drawing.Size(481, 180);
			this.entryExitExpectedValueGridView.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1008, 537);
			this.Controls.Add(this.splitContainer);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.splitContainerTop.Panel1.ResumeLayout(false);
			this.splitContainerTop.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).EndInit();
			this.splitContainerTop.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.historicalDataGridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.entryExitExpectedValueGridView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SplitContainer splitContainer;
		private SplitContainer splitContainerTop;
		private MagicalNuts.UI.ShareholderIncentive.HistoricalDataGridView historicalDataGridView;
		private MagicalNuts.UI.ShareholderIncentive.EntryExitExpectedValueGridView entryExitExpectedValueGridView;
	}
}