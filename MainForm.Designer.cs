namespace QuantaViewer
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.fileListView = new System.Windows.Forms.ListView();
			this.chagneTextBox = new System.Windows.Forms.TextBox();
			this.updateButton = new System.Windows.Forms.Button();
			this.yearSummaryTextBox = new System.Windows.Forms.TextBox();
			this.buComboBox = new System.Windows.Forms.ComboBox();
			this.parseTextBox = new System.Windows.Forms.TextBox();
			this.parseButton = new System.Windows.Forms.Button();
			this.clearButton = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.emailButton = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.titleFilterComboBox = new System.Windows.Forms.ComboBox();
			this.filterButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.yearFilterTextBox = new System.Windows.Forms.TextBox();
			this.listView1 = new System.Windows.Forms.ListView();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// fileListView
			// 
			this.fileListView.Location = new System.Drawing.Point(8, 16);
			this.fileListView.Name = "fileListView";
			this.fileListView.Size = new System.Drawing.Size(368, 536);
			this.fileListView.TabIndex = 0;
			this.fileListView.UseCompatibleStateImageBehavior = false;
			this.fileListView.View = System.Windows.Forms.View.List;
			// 
			// chagneTextBox
			// 
			this.chagneTextBox.Location = new System.Drawing.Point(384, 48);
			this.chagneTextBox.Multiline = true;
			this.chagneTextBox.Name = "chagneTextBox";
			this.chagneTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.chagneTextBox.Size = new System.Drawing.Size(496, 528);
			this.chagneTextBox.TabIndex = 1;
			// 
			// updateButton
			// 
			this.updateButton.Location = new System.Drawing.Point(384, 16);
			this.updateButton.Name = "updateButton";
			this.updateButton.Size = new System.Drawing.Size(75, 23);
			this.updateButton.TabIndex = 2;
			this.updateButton.Text = "更新";
			this.updateButton.UseVisualStyleBackColor = true;
			// 
			// yearSummaryTextBox
			// 
			this.yearSummaryTextBox.Location = new System.Drawing.Point(8, 56);
			this.yearSummaryTextBox.Multiline = true;
			this.yearSummaryTextBox.Name = "yearSummaryTextBox";
			this.yearSummaryTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.yearSummaryTextBox.Size = new System.Drawing.Size(272, 312);
			this.yearSummaryTextBox.TabIndex = 3;
			// 
			// buComboBox
			// 
			this.buComboBox.FormattingEnabled = true;
			this.buComboBox.Location = new System.Drawing.Point(8, 24);
			this.buComboBox.Name = "buComboBox";
			this.buComboBox.Size = new System.Drawing.Size(272, 20);
			this.buComboBox.TabIndex = 4;
			// 
			// parseTextBox
			// 
			this.parseTextBox.Location = new System.Drawing.Point(72, 24);
			this.parseTextBox.Multiline = true;
			this.parseTextBox.Name = "parseTextBox";
			this.parseTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.parseTextBox.Size = new System.Drawing.Size(216, 72);
			this.parseTextBox.TabIndex = 6;
			// 
			// parseButton
			// 
			this.parseButton.Location = new System.Drawing.Point(8, 24);
			this.parseButton.Name = "parseButton";
			this.parseButton.Size = new System.Drawing.Size(56, 23);
			this.parseButton.TabIndex = 7;
			this.parseButton.Text = "Parse";
			this.parseButton.UseVisualStyleBackColor = true;
			// 
			// clearButton
			// 
			this.clearButton.Location = new System.Drawing.Point(8, 72);
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size(56, 23);
			this.clearButton.TabIndex = 8;
			this.clearButton.Text = "Clear";
			this.clearButton.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.emailButton);
			this.groupBox1.Controls.Add(this.parseButton);
			this.groupBox1.Controls.Add(this.parseTextBox);
			this.groupBox1.Controls.Add(this.clearButton);
			this.groupBox1.Location = new System.Drawing.Point(888, 480);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(296, 100);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "工號過濾";
			// 
			// emailButton
			// 
			this.emailButton.Location = new System.Drawing.Point(8, 48);
			this.emailButton.Name = "emailButton";
			this.emailButton.Size = new System.Drawing.Size(56, 23);
			this.emailButton.TabIndex = 9;
			this.emailButton.Text = "Email";
			this.emailButton.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.buComboBox);
			this.groupBox2.Controls.Add(this.yearSummaryTextBox);
			this.groupBox2.Location = new System.Drawing.Point(888, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(288, 384);
			this.groupBox2.TabIndex = 10;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "年資分析";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.titleFilterComboBox);
			this.groupBox3.Controls.Add(this.filterButton);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.yearFilterTextBox);
			this.groupBox3.Location = new System.Drawing.Point(888, 400);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(288, 72);
			this.groupBox3.TabIndex = 11;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "條件篩選";
			// 
			// titleFilterComboBox
			// 
			this.titleFilterComboBox.FormattingEnabled = true;
			this.titleFilterComboBox.Location = new System.Drawing.Point(8, 24);
			this.titleFilterComboBox.Name = "titleFilterComboBox";
			this.titleFilterComboBox.Size = new System.Drawing.Size(72, 20);
			this.titleFilterComboBox.TabIndex = 6;
			// 
			// filterButton
			// 
			this.filterButton.Location = new System.Drawing.Point(232, 24);
			this.filterButton.Name = "filterButton";
			this.filterButton.Size = new System.Drawing.Size(51, 23);
			this.filterButton.TabIndex = 5;
			this.filterButton.Text = "過濾";
			this.filterButton.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("新細明體", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label2.Location = new System.Drawing.Point(88, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "年份";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("新細明體", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.Location = new System.Drawing.Point(8, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "職位";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// yearFilterTextBox
			// 
			this.yearFilterTextBox.Location = new System.Drawing.Point(88, 24);
			this.yearFilterTextBox.Name = "yearFilterTextBox";
			this.yearFilterTextBox.Size = new System.Drawing.Size(40, 22);
			this.yearFilterTextBox.TabIndex = 1;
			// 
			// listView1
			// 
			this.listView1.Location = new System.Drawing.Point(8, 560);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(368, 24);
			this.listView1.TabIndex = 12;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.List;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1189, 594);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.updateButton);
			this.Controls.Add(this.chagneTextBox);
			this.Controls.Add(this.fileListView);
			this.Name = "MainForm";
			this.Text = "QuantaViewer";
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.Button emailButton;
		private System.Windows.Forms.ComboBox titleFilterComboBox;
		private System.Windows.Forms.Button filterButton;
		private System.Windows.Forms.TextBox yearFilterTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.Button parseButton;
		private System.Windows.Forms.TextBox parseTextBox;
		private System.Windows.Forms.ComboBox buComboBox;
		private System.Windows.Forms.TextBox yearSummaryTextBox;
		private System.Windows.Forms.Button updateButton;
		private System.Windows.Forms.TextBox chagneTextBox;
		private System.Windows.Forms.ListView fileListView;
	}
}
