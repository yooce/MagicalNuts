namespace IncrementalTextBoxSample
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
			this.incrementalTextBox1 = new MagicalNuts.UI.Base.IncrementalTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.incrementalTextBox2 = new MagicalNuts.UI.Base.IncrementalTextBox();
			this.incrementalTextBox3 = new MagicalNuts.UI.Base.IncrementalTextBox();
			this.SuspendLayout();
			// 
			// incrementalTextBox1
			// 
			this.incrementalTextBox1.CandidateListHeight = 200;
			this.incrementalTextBox1.CandidateListShowThreshold = 30;
			this.incrementalTextBox1.DictionarySet = false;
			this.incrementalTextBox1.Location = new System.Drawing.Point(12, 13);
			this.incrementalTextBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.incrementalTextBox1.Name = "incrementalTextBox1";
			this.incrementalTextBox1.Size = new System.Drawing.Size(200, 23);
			this.incrementalTextBox1.TabIndex = 0;
			this.incrementalTextBox1.Decided += new MagicalNuts.UI.Base.IncrementalTextBox.IncrementalTextBoxEventHandler(this.incrementalTextBox1_Decided);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(250, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(52, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "decided:";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(308, 13);
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(200, 23);
			this.textBox1.TabIndex = 2;
			// 
			// incrementalTextBox2
			// 
			this.incrementalTextBox2.CandidateListHeight = 200;
			this.incrementalTextBox2.CandidateListShowThreshold = 30;
			this.incrementalTextBox2.DictionarySet = false;
			this.incrementalTextBox2.Enabled = false;
			this.incrementalTextBox2.Location = new System.Drawing.Point(12, 150);
			this.incrementalTextBox2.Name = "incrementalTextBox2";
			this.incrementalTextBox2.Size = new System.Drawing.Size(200, 23);
			this.incrementalTextBox2.TabIndex = 3;
			// 
			// incrementalTextBox3
			// 
			this.incrementalTextBox3.CandidateListHeight = 200;
			this.incrementalTextBox3.CandidateListShowThreshold = 30;
			this.incrementalTextBox3.DictionarySet = false;
			this.incrementalTextBox3.Enabled = false;
			this.incrementalTextBox3.Location = new System.Drawing.Point(218, 150);
			this.incrementalTextBox3.Name = "incrementalTextBox3";
			this.incrementalTextBox3.Size = new System.Drawing.Size(200, 23);
			this.incrementalTextBox3.TabIndex = 4;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 411);
			this.Controls.Add(this.incrementalTextBox3);
			this.Controls.Add(this.incrementalTextBox2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.incrementalTextBox1);
			this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MagicalNuts.UI.Base.IncrementalTextBox incrementalTextBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private MagicalNuts.UI.Base.IncrementalTextBox incrementalTextBox2;
		private MagicalNuts.UI.Base.IncrementalTextBox incrementalTextBox3;
	}
}

