namespace PasiveRadar
{
    partial class RadarControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar4 = new System.Windows.Forms.TrackBar();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.trackBar5 = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.trackBar6 = new System.Windows.Forms.TrackBar();
            this.label13 = new System.Windows.Forms.Label();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.trackBar7 = new System.Windows.Forms.TrackBar();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.trackBar8 = new System.Windows.Forms.TrackBar();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.trackBar9 = new System.Windows.Forms.TrackBar();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.trackBar10 = new System.Windows.Forms.TrackBar();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar10)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "32k",
            "64k",
            "128k",
            "256k",
            "512k",
            "1024k",
            "2048k (critical)",
            "4096k (critical)"
            });
            this.comboBox1.Location = new System.Drawing.Point(4, 350);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(132, 24);
            this.comboBox1.TabIndex = 60;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 330);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 16);
            this.label6.TabIndex = 58;
            this.label6.Text = "FFT buffer";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 190);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 16);
            this.label5.TabIndex = 57;
            this.label5.Text = "Doppler zoom";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 50);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 16);
            this.label4.TabIndex = 56;
            this.label4.Text = "Amplification";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 260);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 16);
            this.label3.TabIndex = 55;
            this.label3.Text = "Average";
            // 
            // trackBar4
            // 
            this.trackBar4.LargeChange = 1;
            this.trackBar4.Location = new System.Drawing.Point(-4, 280);
            this.trackBar4.Margin = new System.Windows.Forms.Padding(4);
            this.trackBar4.Maximum = 20;
            this.trackBar4.Minimum = 1;
            this.trackBar4.Name = "trackBar4";
            this.trackBar4.Size = new System.Drawing.Size(222, 56);
            this.trackBar4.TabIndex = 54;
            this.trackBar4.TabStop = false;
            this.trackBar4.TickFrequency = 5;
            this.trackBar4.Value = 5;
            this.trackBar4.Scroll += new System.EventHandler(this.trackBar4_Scroll);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(7, 510);
            this.checkBox4.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(149, 20);
            this.checkBox4.TabIndex = 53;
            this.checkBox4.Text = "Remove symetrices";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // trackBar3
            // 
            this.trackBar3.LargeChange = 10;
            this.trackBar3.Location = new System.Drawing.Point(0, 210);
            this.trackBar3.Margin = new System.Windows.Forms.Padding(4);
            this.trackBar3.Maximum = 5000;
            this.trackBar3.Minimum = 1;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Size = new System.Drawing.Size(222, 56);
            this.trackBar3.TabIndex = 52;
            this.trackBar3.TickFrequency = 100;
            this.trackBar3.Value = 1000;
            this.trackBar3.Scroll += new System.EventHandler(this.trackBar3_Scroll);
            // 
            // trackBar2
            // 
            this.trackBar2.LargeChange = 10;
            this.trackBar2.Location = new System.Drawing.Point(-4, 70);
            this.trackBar2.Margin = new System.Windows.Forms.Padding(4);
            this.trackBar2.Maximum = 100;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(222, 56);
            this.trackBar2.TabIndex = 51;
            this.trackBar2.Value = 1;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(162, 48);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 16);
            this.label1.TabIndex = 61;
            this.label1.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(163, 190);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 16);
            this.label2.TabIndex = 62;
            this.label2.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(163, 260);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 16);
            this.label7.TabIndex = 63;
            this.label7.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(0, 377);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 16);
            this.label9.TabIndex = 66;
            this.label9.Text = "Columns";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 436);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 16);
            this.label10.TabIndex = 67;
            this.label10.Text = "Rows";
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(-4, 397);
            this.trackBar1.Margin = new System.Windows.Forms.Padding(4);
            this.trackBar1.Maximum = 1000;
            this.trackBar1.Minimum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(222, 56);
            this.trackBar1.TabIndex = 70;
            this.trackBar1.TabStop = false;
            this.trackBar1.TickFrequency = 100;
            this.trackBar1.Value = 200;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // trackBar5
            // 
            this.trackBar5.LargeChange = 10;
            this.trackBar5.Location = new System.Drawing.Point(-4, 456);
            this.trackBar5.Margin = new System.Windows.Forms.Padding(4);
            this.trackBar5.Maximum = 1000;
            this.trackBar5.Minimum = 100;
            this.trackBar5.Name = "trackBar5";
            this.trackBar5.Size = new System.Drawing.Size(222, 56);
            this.trackBar5.TabIndex = 71;
            this.trackBar5.TabStop = false;
            this.trackBar5.TickFrequency = 100;
            this.trackBar5.Value = 100;
            this.trackBar5.Scroll += new System.EventHandler(this.trackBar5_Scroll);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(163, 377);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(14, 16);
            this.label11.TabIndex = 72;
            this.label11.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(163, 436);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(14, 16);
            this.label12.TabIndex = 73;
            this.label12.Text = "0";
            // 
            // trackBar6
            // 
            this.trackBar6.LargeChange = 1;
            this.trackBar6.Location = new System.Drawing.Point(-4, 138);
            this.trackBar6.Margin = new System.Windows.Forms.Padding(4);
            this.trackBar6.Maximum = 1000;
            this.trackBar6.Name = "trackBar6";
            this.trackBar6.Size = new System.Drawing.Size(222, 56);
            this.trackBar6.TabIndex = 74;
            this.trackBar6.TabStop = false;
            this.trackBar6.TickFrequency = 200;
            this.trackBar6.Value = 100;
            this.trackBar6.Scroll += new System.EventHandler(this.trackBar6_Scroll);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(163, 120);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(14, 16);
            this.label13.TabIndex = 75;
            this.label13.Text = "0";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(7, 538);
            this.checkBox3.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(151, 20);
            this.checkBox3.TabIndex = 76;
            this.checkBox3.Text = "Correct backgroumd";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // trackBar7
            // 
            this.trackBar7.LargeChange = 1;
            this.trackBar7.Location = new System.Drawing.Point(3, 707);
            this.trackBar7.Margin = new System.Windows.Forms.Padding(4);
            this.trackBar7.Maximum = 30;
            this.trackBar7.Minimum = 2;
            this.trackBar7.Name = "trackBar7";
            this.trackBar7.Size = new System.Drawing.Size(222, 56);
            this.trackBar7.TabIndex = 77;
            this.trackBar7.TabStop = false;
            this.trackBar7.TickFrequency = 10;
            this.trackBar7.Value = 30;
            this.trackBar7.Scroll += new System.EventHandler(this.trackBar7_Scroll);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(-1, 687);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(122, 16);
            this.label14.TabIndex = 78;
            this.label14.Text = "Nr correction points";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(162, 687);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(14, 16);
            this.label15.TabIndex = 79;
            this.label15.Text = "0";
            // 
            // trackBar8
            // 
            this.trackBar8.LargeChange = 1;
            this.trackBar8.Location = new System.Drawing.Point(0, 770);
            this.trackBar8.Margin = new System.Windows.Forms.Padding(4);
            this.trackBar8.Maximum = 100;
            this.trackBar8.Minimum = 1;
            this.trackBar8.Name = "trackBar8";
            this.trackBar8.Size = new System.Drawing.Size(222, 56);
            this.trackBar8.TabIndex = 80;
            this.trackBar8.TabStop = false;
            this.trackBar8.TickFrequency = 10;
            this.trackBar8.Value = 100;
            this.trackBar8.Scroll += new System.EventHandler(this.trackBar8_Scroll);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(-1, 750);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(98, 16);
            this.label16.TabIndex = 81;
            this.label16.Text = "Colected every";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(162, 750);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(14, 16);
            this.label17.TabIndex = 82;
            this.label17.Text = "0";
            // 
            // trackBar9
            // 
            this.trackBar9.LargeChange = 1;
            this.trackBar9.Location = new System.Drawing.Point(2, 841);
            this.trackBar9.Margin = new System.Windows.Forms.Padding(4);
            this.trackBar9.Maximum = 100;
            this.trackBar9.Name = "trackBar9";
            this.trackBar9.Size = new System.Drawing.Size(222, 56);
            this.trackBar9.TabIndex = 83;
            this.trackBar9.TabStop = false;
            this.trackBar9.TickFrequency = 10;
            this.trackBar9.Value = 100;
            this.trackBar9.Scroll += new System.EventHandler(this.trackBar9_Scroll);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(-1, 821);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(109, 16);
            this.label18.TabIndex = 84;
            this.label18.Text = "Correction weight";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(162, 821);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(14, 16);
            this.label19.TabIndex = 85;
            this.label19.Text = "0";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(0, 122);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(86, 16);
            this.label20.TabIndex = 86;
            this.label20.Text = "Distance shift";
            this.label20.Click += new System.EventHandler(this.label20_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(11, 11);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(138, 20);
            this.checkBox1.TabIndex = 87;
            this.checkBox1.Text = "Two dongle mode";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 573);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 16);
            this.label8.TabIndex = 88;
            this.label8.Text = "Sc ale type";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // trackBar10
            // 
            this.trackBar10.LargeChange = 1;
            this.trackBar10.Location = new System.Drawing.Point(11, 593);
            this.trackBar10.Margin = new System.Windows.Forms.Padding(4);
            this.trackBar10.Maximum = 2;
            this.trackBar10.Name = "trackBar10";
            this.trackBar10.Size = new System.Drawing.Size(165, 56);
            this.trackBar10.TabIndex = 89;
            this.trackBar10.TabStop = false;
            this.trackBar10.TickFrequency = 3;
            this.trackBar10.Value = 2;
            this.trackBar10.Scroll += new System.EventHandler(this.trackBar10_Scroll);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(25, 623);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(20, 16);
            this.label21.TabIndex = 90;
            this.label21.Text = "lin";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(77, 623);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(29, 16);
            this.label22.TabIndex = 91;
            this.label22.Text = "sqrt";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(147, 623);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(26, 16);
            this.label23.TabIndex = 92;
            this.label23.Text = "log";
            // 
            // RadarControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.trackBar10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.trackBar9);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.trackBar8);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.trackBar7);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.trackBar6);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.trackBar5);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.trackBar4);
            this.Controls.Add(this.trackBar3);
            this.Controls.Add(this.trackBar2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RadarControl";
            this.Size = new System.Drawing.Size(222, 913);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar10)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBar4;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.TrackBar trackBar3;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TrackBar trackBar5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TrackBar trackBar6;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.TrackBar trackBar7;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TrackBar trackBar8;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TrackBar trackBar9;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TrackBar trackBar10;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
    }
}
