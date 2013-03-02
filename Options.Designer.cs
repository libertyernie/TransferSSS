namespace TransferSSS {
	partial class Options {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.common5_button = new System.Windows.Forms.Button();
			this.common5_label = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.mu_menumain_label = new System.Windows.Forms.Label();
			this.mu_menumain_change = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// common5_button
			// 
			this.common5_button.AutoSize = true;
			this.common5_button.Dock = System.Windows.Forms.DockStyle.Fill;
			this.common5_button.Location = new System.Drawing.Point(175, 0);
			this.common5_button.Margin = new System.Windows.Forms.Padding(0);
			this.common5_button.Name = "common5_button";
			this.common5_button.Size = new System.Drawing.Size(117, 24);
			this.common5_button.TabIndex = 1;
			this.common5_button.Text = "Change";
			this.common5_button.UseVisualStyleBackColor = true;
			this.common5_button.Click += new System.EventHandler(this.common5_button_Click);
			// 
			// common5_label
			// 
			this.common5_label.Dock = System.Windows.Forms.DockStyle.Fill;
			this.common5_label.Location = new System.Drawing.Point(3, 0);
			this.common5_label.Name = "common5_label";
			this.common5_label.Size = new System.Drawing.Size(169, 24);
			this.common5_label.TabIndex = 0;
			this.common5_label.Text = "common5_label";
			this.common5_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
			this.tableLayoutPanel1.Controls.Add(this.common5_button, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.common5_label, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.mu_menumain_label, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.mu_menumain_change, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(292, 48);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// mu_menumain_label
			// 
			this.mu_menumain_label.AutoSize = true;
			this.mu_menumain_label.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mu_menumain_label.Location = new System.Drawing.Point(3, 24);
			this.mu_menumain_label.Name = "mu_menumain_label";
			this.mu_menumain_label.Size = new System.Drawing.Size(169, 24);
			this.mu_menumain_label.TabIndex = 2;
			this.mu_menumain_label.Text = "mu_menumain_label";
			this.mu_menumain_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mu_menumain_change
			// 
			this.mu_menumain_change.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mu_menumain_change.Location = new System.Drawing.Point(175, 24);
			this.mu_menumain_change.Margin = new System.Windows.Forms.Padding(0);
			this.mu_menumain_change.Name = "mu_menumain_change";
			this.mu_menumain_change.Size = new System.Drawing.Size(117, 24);
			this.mu_menumain_change.TabIndex = 3;
			this.mu_menumain_change.Text = "button1";
			this.mu_menumain_change.UseVisualStyleBackColor = true;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// Options
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "Options";
			this.Text = "Options";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button common5_button;
		private System.Windows.Forms.Label common5_label;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label mu_menumain_label;
		private System.Windows.Forms.Button mu_menumain_change;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;

	}
}