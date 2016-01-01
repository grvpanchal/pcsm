namespace pcsm
{
    partial class Options
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.cfu_cb = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.scht_gb = new System.Windows.Forms.GroupBox();
            this.none_rb = new System.Windows.Forms.RadioButton();
            this.monthly_rb = new System.Windows.Forms.RadioButton();
            this.weekly_rb = new System.Windows.Forms.RadioButton();
            this.daily_rb = new System.Windows.Forms.RadioButton();
            this.hourly_rb = new System.Windows.Forms.RadioButton();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.treeView1 = new pcsm.tristate.TriStateTreeView();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.drive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.freespace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.freespacep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.scht_gb.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(417, 372);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(331, 372);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 30);
            this.button2.TabIndex = 2;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(485, 354);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBox3);
            this.tabPage1.Controls.Add(this.cfu_cb);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(477, 328);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(26, 51);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(93, 17);
            this.checkBox3.TabIndex = 7;
            this.checkBox3.Text = "Save Log files";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // cfu_cb
            // 
            this.cfu_cb.AutoSize = true;
            this.cfu_cb.Location = new System.Drawing.Point(26, 24);
            this.cfu_cb.Name = "cfu_cb";
            this.cfu_cb.Size = new System.Drawing.Size(168, 17);
            this.cfu_cb.TabIndex = 6;
            this.cfu_cb.Text = "Check For Updates on launch";
            this.cfu_cb.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tabControl2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(477, 328);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Scheduled";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(6, 6);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(465, 323);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.checkBox7);
            this.tabPage6.Controls.Add(this.groupBox1);
            this.tabPage6.Controls.Add(this.scht_gb);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(457, 297);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "General";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(9, 259);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(109, 17);
            this.checkBox7.TabIndex = 3;
            this.checkBox7.Text = "Show Notification";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox6);
            this.groupBox1.Controls.Add(this.checkBox5);
            this.groupBox1.Controls.Add(this.checkBox4);
            this.groupBox1.Location = new System.Drawing.Point(3, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(451, 92);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Scheduled Tasks";
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(6, 64);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(128, 17);
            this.checkBox6.TabIndex = 2;
            this.checkBox6.Text = "Disk Defragmentation";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(6, 41);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(89, 17);
            this.checkBox5.TabIndex = 1;
            this.checkBox5.Text = "Disk Cleanup";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(6, 18);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(106, 17);
            this.checkBox4.TabIndex = 0;
            this.checkBox4.Text = "Registry Cleanup";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // scht_gb
            // 
            this.scht_gb.Controls.Add(this.none_rb);
            this.scht_gb.Controls.Add(this.monthly_rb);
            this.scht_gb.Controls.Add(this.weekly_rb);
            this.scht_gb.Controls.Add(this.daily_rb);
            this.scht_gb.Controls.Add(this.hourly_rb);
            this.scht_gb.Location = new System.Drawing.Point(3, 107);
            this.scht_gb.Name = "scht_gb";
            this.scht_gb.Size = new System.Drawing.Size(451, 144);
            this.scht_gb.TabIndex = 1;
            this.scht_gb.TabStop = false;
            this.scht_gb.Text = "Select Schedule Type";
            // 
            // none_rb
            // 
            this.none_rb.AutoSize = true;
            this.none_rb.Location = new System.Drawing.Point(7, 116);
            this.none_rb.Name = "none_rb";
            this.none_rb.Size = new System.Drawing.Size(51, 17);
            this.none_rb.TabIndex = 4;
            this.none_rb.TabStop = true;
            this.none_rb.Text = "None";
            this.none_rb.UseVisualStyleBackColor = true;
            // 
            // monthly_rb
            // 
            this.monthly_rb.AutoSize = true;
            this.monthly_rb.Location = new System.Drawing.Point(7, 92);
            this.monthly_rb.Name = "monthly_rb";
            this.monthly_rb.Size = new System.Drawing.Size(62, 17);
            this.monthly_rb.TabIndex = 3;
            this.monthly_rb.TabStop = true;
            this.monthly_rb.Text = "Monthly";
            this.monthly_rb.UseVisualStyleBackColor = true;
            // 
            // weekly_rb
            // 
            this.weekly_rb.AutoSize = true;
            this.weekly_rb.Location = new System.Drawing.Point(7, 68);
            this.weekly_rb.Name = "weekly_rb";
            this.weekly_rb.Size = new System.Drawing.Size(61, 17);
            this.weekly_rb.TabIndex = 2;
            this.weekly_rb.TabStop = true;
            this.weekly_rb.Text = "Weekly";
            this.weekly_rb.UseVisualStyleBackColor = true;
            // 
            // daily_rb
            // 
            this.daily_rb.AutoSize = true;
            this.daily_rb.Location = new System.Drawing.Point(7, 44);
            this.daily_rb.Name = "daily_rb";
            this.daily_rb.Size = new System.Drawing.Size(48, 17);
            this.daily_rb.TabIndex = 1;
            this.daily_rb.TabStop = true;
            this.daily_rb.Text = "Daily";
            this.daily_rb.UseVisualStyleBackColor = true;
            // 
            // hourly_rb
            // 
            this.hourly_rb.AutoSize = true;
            this.hourly_rb.Location = new System.Drawing.Point(7, 20);
            this.hourly_rb.Name = "hourly_rb";
            this.hourly_rb.Size = new System.Drawing.Size(55, 17);
            this.hourly_rb.TabIndex = 0;
            this.hourly_rb.TabStop = true;
            this.hourly_rb.Text = "Hourly";
            this.hourly_rb.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.label4);
            this.tabPage5.Controls.Add(this.treeView2);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(457, 297);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "Registry cleanup";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Select Registry Sections:";
            // 
            // treeView2
            // 
            this.treeView2.CheckBoxes = true;
            this.treeView2.Location = new System.Drawing.Point(3, 28);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new System.Drawing.Size(451, 266);
            this.treeView2.TabIndex = 5;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.button3);
            this.tabPage3.Controls.Add(this.button4);
            this.tabPage3.Controls.Add(this.treeView1);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(457, 297);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Disk Cleanup";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(355, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(96, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "UnCheck All";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(253, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(96, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "Check All";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(3, 28);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(451, 266);
            this.treeView1.TabIndex = 6;
            this.treeView1.TriStateStyleProperty = pcsm.tristate.TriStateTreeView.TriStateStyles.Standard;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Select programs and Locations:";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.checkBox2);
            this.tabPage4.Controls.Add(this.checkBox1);
            this.tabPage4.Controls.Add(this.dataGridView1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(457, 297);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Defragmentation";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Select Drives to Defragment:";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(20, 128);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(91, 17);
            this.checkBox2.TabIndex = 7;
            this.checkBox2.Text = "Optimize MFT";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(21, 105);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(90, 17);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Optimize Disk";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.check,
            this.drive,
            this.freespace,
            this.freespacep});
            this.dataGridView1.Location = new System.Drawing.Point(6, 28);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(444, 70);
            this.dataGridView1.TabIndex = 5;
            // 
            // check
            // 
            this.check.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.check.Frozen = true;
            this.check.HeaderText = "#";
            this.check.Name = "check";
            this.check.Width = 20;
            // 
            // drive
            // 
            this.drive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.drive.FillWeight = 200F;
            this.drive.Frozen = true;
            this.drive.HeaderText = "Drive";
            this.drive.Name = "drive";
            this.drive.ReadOnly = true;
            this.drive.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.drive.Width = 57;
            // 
            // freespace
            // 
            this.freespace.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.freespace.HeaderText = "Free Space";
            this.freespace.Name = "freespace";
            this.freespace.ReadOnly = true;
            // 
            // freespacep
            // 
            this.freespacep.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.freespacep.HeaderText = "Free Space %";
            this.freespacep.Name = "freespacep";
            this.freespacep.ReadOnly = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 414);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.scht_gb.ResumeLayout(false);
            this.scht_gb.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn check;
        private System.Windows.Forms.DataGridViewTextBoxColumn drive;
        private System.Windows.Forms.DataGridViewTextBoxColumn freespace;
        private System.Windows.Forms.DataGridViewTextBoxColumn freespacep;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private tristate.TriStateTreeView treeView1;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.GroupBox scht_gb;
        private System.Windows.Forms.RadioButton none_rb;
        private System.Windows.Forms.RadioButton monthly_rb;
        private System.Windows.Forms.RadioButton weekly_rb;
        private System.Windows.Forms.RadioButton daily_rb;
        private System.Windows.Forms.RadioButton hourly_rb;
        private System.Windows.Forms.CheckBox cfu_cb;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox7;
    }
}