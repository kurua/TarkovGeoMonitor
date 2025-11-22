namespace TarkovGeoMonitor
{
    partial class TarkovGeoMonitor
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.runTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label_MMDB = new System.Windows.Forms.Label();
            this.label_EFTLogDir = new System.Windows.Forms.Label();
            this.label_targetLogDir = new System.Windows.Forms.Label();
            this.label_targetLogFile = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_Date = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label_Geo = new System.Windows.Forms.Label();
            this.label_IP = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.labelA1 = new System.Windows.Forms.Label();
            this.linkLabelA2 = new System.Windows.Forms.LinkLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView_log = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_log)).BeginInit();
            this.SuspendLayout();
            // 
            // runTimer
            // 
            this.runTimer.Interval = 5000;
            this.runTimer.Tick += new System.EventHandler(this.runTimer_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "MMDB";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "EFT LogDir";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Target LogDir";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "Target LogFile";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(91, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = " : ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(91, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = " : ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(91, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = " : ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(91, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 12);
            this.label8.TabIndex = 7;
            this.label8.Text = " : ";
            // 
            // label_MMDB
            // 
            this.label_MMDB.AutoSize = true;
            this.label_MMDB.Location = new System.Drawing.Point(112, 3);
            this.label_MMDB.Name = "label_MMDB";
            this.label_MMDB.Size = new System.Drawing.Size(73, 12);
            this.label_MMDB.TabIndex = 8;
            this.label_MMDB.Text = "#label_MMDB";
            // 
            // label_EFTLogDir
            // 
            this.label_EFTLogDir.AutoSize = true;
            this.label_EFTLogDir.Location = new System.Drawing.Point(112, 20);
            this.label_EFTLogDir.Name = "label_EFTLogDir";
            this.label_EFTLogDir.Size = new System.Drawing.Size(93, 12);
            this.label_EFTLogDir.TabIndex = 9;
            this.label_EFTLogDir.Text = "#label_EFTLogDir";
            // 
            // label_targetLogDir
            // 
            this.label_targetLogDir.AutoSize = true;
            this.label_targetLogDir.Location = new System.Drawing.Point(112, 37);
            this.label_targetLogDir.Name = "label_targetLogDir";
            this.label_targetLogDir.Size = new System.Drawing.Size(102, 12);
            this.label_targetLogDir.TabIndex = 10;
            this.label_targetLogDir.Text = "#label_targetLogDir";
            // 
            // label_targetLogFile
            // 
            this.label_targetLogFile.AutoSize = true;
            this.label_targetLogFile.Location = new System.Drawing.Point(112, 54);
            this.label_targetLogFile.Name = "label_targetLogFile";
            this.label_targetLogFile.Size = new System.Drawing.Size(106, 12);
            this.label_targetLogFile.TabIndex = 11;
            this.label_targetLogFile.Text = "#label_targetLogFile";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_Date);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label_Geo);
            this.groupBox1.Controls.Add(this.label_IP);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Location = new System.Drawing.Point(1, 79);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(395, 80);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Last connected game server";
            // 
            // label_Date
            // 
            this.label_Date.AutoSize = true;
            this.label_Date.Location = new System.Drawing.Point(57, 18);
            this.label_Date.Name = "label_Date";
            this.label_Date.Size = new System.Drawing.Size(63, 12);
            this.label_Date.TabIndex = 17;
            this.label_Date.Text = "#label_Date";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(36, 18);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(15, 12);
            this.label11.TabIndex = 19;
            this.label11.Text = " : ";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(5, 18);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 12);
            this.label12.TabIndex = 18;
            this.label12.Text = "Date";
            // 
            // label_Geo
            // 
            this.label_Geo.AutoSize = true;
            this.label_Geo.Location = new System.Drawing.Point(57, 58);
            this.label_Geo.Name = "label_Geo";
            this.label_Geo.Size = new System.Drawing.Size(59, 12);
            this.label_Geo.TabIndex = 16;
            this.label_Geo.Text = "#label_Geo";
            // 
            // label_IP
            // 
            this.label_IP.AutoSize = true;
            this.label_IP.Location = new System.Drawing.Point(57, 38);
            this.label_IP.Name = "label_IP";
            this.label_IP.Size = new System.Drawing.Size(49, 12);
            this.label_IP.TabIndex = 13;
            this.label_IP.Text = "#label_IP";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(36, 38);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(15, 12);
            this.label16.TabIndex = 15;
            this.label16.Text = " : ";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(36, 58);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(15, 12);
            this.label15.TabIndex = 13;
            this.label15.Text = " : ";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(5, 58);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(25, 12);
            this.label14.TabIndex = 14;
            this.label14.Text = "Geo";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(5, 38);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(15, 12);
            this.label13.TabIndex = 13;
            this.label13.Text = "IP";
            // 
            // labelA1
            // 
            this.labelA1.AutoSize = true;
            this.labelA1.Location = new System.Drawing.Point(542, 194);
            this.labelA1.Name = "labelA1";
            this.labelA1.Size = new System.Drawing.Size(95, 12);
            this.labelA1.TabIndex = 13;
            this.labelA1.Text = "IP Geolocation by";
            // 
            // linkLabelA2
            // 
            this.linkLabelA2.AutoSize = true;
            this.linkLabelA2.Location = new System.Drawing.Point(637, 194);
            this.linkLabelA2.Name = "linkLabelA2";
            this.linkLabelA2.Size = new System.Drawing.Size(37, 12);
            this.linkLabelA2.TabIndex = 14;
            this.linkLabelA2.TabStop = true;
            this.linkLabelA2.Text = "DB-IP";
            this.linkLabelA2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(676, 189);
            this.tabControl1.TabIndex = 16;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label_targetLogFile);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label_targetLogDir);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label_EFTLogDir);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label_MMDB);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(668, 163);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Monitor";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView_log);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(668, 163);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Log";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView_log
            // 
            this.dataGridView_log.AllowUserToAddRows = false;
            this.dataGridView_log.AllowUserToDeleteRows = false;
            this.dataGridView_log.AllowUserToResizeColumns = false;
            this.dataGridView_log.AllowUserToResizeRows = false;
            this.dataGridView_log.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_log.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_log.Location = new System.Drawing.Point(6, 6);
            this.dataGridView_log.Name = "dataGridView_log";
            this.dataGridView_log.RowTemplate.Height = 21;
            this.dataGridView_log.Size = new System.Drawing.Size(656, 149);
            this.dataGridView_log.TabIndex = 1;
            this.dataGridView_log.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView_log_RowsAdded);
            // 
            // TarkovGeoMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 214);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.linkLabelA2);
            this.Controls.Add(this.labelA1);
            this.Name = "TarkovGeoMonitor";
            this.Text = "TarkovGeoMonitor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_log)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer runTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label_MMDB;
        private System.Windows.Forms.Label label_EFTLogDir;
        private System.Windows.Forms.Label label_targetLogDir;
        private System.Windows.Forms.Label label_targetLogFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label_Geo;
        private System.Windows.Forms.Label label_IP;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelA1;
        private System.Windows.Forms.LinkLabel linkLabelA2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        internal System.Windows.Forms.DataGridView dataGridView_log;
        private System.Windows.Forms.Label label_Date;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
    }
}

