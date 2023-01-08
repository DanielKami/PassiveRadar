namespace PasiveRadar
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button5 = new System.Windows.Forms.Button();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.displayControl1 = new PasiveRadar.DisplayControl();
            this.button7 = new System.Windows.Forms.Button();
            this.radarControl1 = new PasiveRadar.RadarControl();
            this.button9 = new System.Windows.Forms.Button();
            this.correlateControl1 = new PasiveRadar.CorrelateControl();
            this.label8 = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.panelViewport3 = new System.Windows.Forms.Panel();
            this.splitContainer8 = new System.Windows.Forms.SplitContainer();
            this.panelViewport6 = new System.Windows.Forms.Panel();
            this.panelViewport7 = new System.Windows.Forms.Panel();
            this.panelViewport2 = new System.Windows.Forms.Panel();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.label51 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonFrequencyEqual = new System.Windows.Forms.Button();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).BeginInit();
            this.splitContainer8.Panel1.SuspendLayout();
            this.splitContainer8.Panel2.SuspendLayout();
            this.splitContainer8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.White;
            this.imageList1.Images.SetKeyName(0, "play.png");
            this.imageList1.Images.SetKeyName(1, "stop.png");
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.flowLayoutPanel1);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(190, 558);
            this.panel2.TabIndex = 34;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.button5);
            this.flowLayoutPanel1.Controls.Add(this.displayControl1);
            this.flowLayoutPanel1.Controls.Add(this.button7);
            this.flowLayoutPanel1.Controls.Add(this.radarControl1);
            this.flowLayoutPanel1.Controls.Add(this.button9);
            this.flowLayoutPanel1.Controls.Add(this.correlateControl1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(190, 558);
            this.flowLayoutPanel1.TabIndex = 56;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button5.ImageIndex = 0;
            this.button5.ImageList = this.imageList2;
            this.button5.Location = new System.Drawing.Point(0, 0);
            this.button5.Margin = new System.Windows.Forms.Padding(0);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(225, 30);
            this.button5.TabIndex = 56;
            this.button5.Text = "Display";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "ButtonOff.png");
            this.imageList2.Images.SetKeyName(1, "ButtonOn.png");
            // 
            // displayControl1
            // 
            this.displayControl1.Location = new System.Drawing.Point(0, 30);
            this.displayControl1.Margin = new System.Windows.Forms.Padding(0);
            this.displayControl1.Name = "displayControl1";
            this.displayControl1.Size = new System.Drawing.Size(223, 324);
            this.displayControl1.TabIndex = 57;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button7.FlatAppearance.BorderSize = 0;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button7.ImageIndex = 0;
            this.button7.ImageList = this.imageList2;
            this.button7.Location = new System.Drawing.Point(0, 354);
            this.button7.Margin = new System.Windows.Forms.Padding(0);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(225, 30);
            this.button7.TabIndex = 59;
            this.button7.Text = "Radar";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // radarControl1
            // 
            this.radarControl1.Location = new System.Drawing.Point(5, 389);
            this.radarControl1.Margin = new System.Windows.Forms.Padding(5);
            this.radarControl1.Name = "radarControl1";
            this.radarControl1.Size = new System.Drawing.Size(254, 979);
            this.radarControl1.TabIndex = 60;
            this.radarControl1.Load += new System.EventHandler(this.radarControl1_Load);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button9.FlatAppearance.BorderSize = 0;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button9.ImageIndex = 0;
            this.button9.ImageList = this.imageList2;
            this.button9.Location = new System.Drawing.Point(0, 1373);
            this.button9.Margin = new System.Windows.Forms.Padding(0);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(225, 30);
            this.button9.TabIndex = 61;
            this.button9.Text = "Correlation";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // correlateControl1
            // 
            this.correlateControl1.Location = new System.Drawing.Point(5, 1408);
            this.correlateControl1.Margin = new System.Windows.Forms.Padding(5);
            this.correlateControl1.Name = "correlateControl1";
            this.correlateControl1.Size = new System.Drawing.Size(223, 362);
            this.correlateControl1.TabIndex = 62;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(152, 683);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 16);
            this.label8.TabIndex = 48;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer7);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.panelViewport2);
            this.splitContainer3.Size = new System.Drawing.Size(1160, 558);
            this.splitContainer3.SplitterDistance = 365;
            this.splitContainer3.SplitterWidth = 5;
            this.splitContainer3.TabIndex = 0;
            this.splitContainer3.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.panelViewport3);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.splitContainer8);
            this.splitContainer7.Size = new System.Drawing.Size(365, 558);
            this.splitContainer7.SplitterDistance = 176;
            this.splitContainer7.SplitterWidth = 5;
            this.splitContainer7.TabIndex = 0;
            this.splitContainer7.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
            // 
            // panelViewport3
            // 
            this.panelViewport3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panelViewport3.Cursor = System.Windows.Forms.Cursors.Cross;
            this.panelViewport3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelViewport3.Location = new System.Drawing.Point(0, 0);
            this.panelViewport3.Margin = new System.Windows.Forms.Padding(4);
            this.panelViewport3.Name = "panelViewport3";
            this.panelViewport3.Size = new System.Drawing.Size(365, 176);
            this.panelViewport3.TabIndex = 29;
            this.panelViewport3.MouseLeave += new System.EventHandler(this.panelViewport_MouseLeave);
            this.panelViewport3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelViewport3_MouseMove);
            // 
            // splitContainer8
            // 
            this.splitContainer8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer8.Location = new System.Drawing.Point(0, 0);
            this.splitContainer8.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer8.Name = "splitContainer8";
            this.splitContainer8.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer8.Panel1
            // 
            this.splitContainer8.Panel1.Controls.Add(this.panelViewport6);
            // 
            // splitContainer8.Panel2
            // 
            this.splitContainer8.Panel2.Controls.Add(this.panelViewport7);
            this.splitContainer8.Size = new System.Drawing.Size(365, 377);
            this.splitContainer8.SplitterDistance = 173;
            this.splitContainer8.SplitterWidth = 5;
            this.splitContainer8.TabIndex = 0;
            this.splitContainer8.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
            // 
            // panelViewport6
            // 
            this.panelViewport6.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panelViewport6.Cursor = System.Windows.Forms.Cursors.Cross;
            this.panelViewport6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelViewport6.Location = new System.Drawing.Point(0, 0);
            this.panelViewport6.Margin = new System.Windows.Forms.Padding(4);
            this.panelViewport6.Name = "panelViewport6";
            this.panelViewport6.Size = new System.Drawing.Size(365, 173);
            this.panelViewport6.TabIndex = 30;
            // 
            // panelViewport7
            // 
            this.panelViewport7.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panelViewport7.Cursor = System.Windows.Forms.Cursors.Cross;
            this.panelViewport7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelViewport7.Location = new System.Drawing.Point(0, 0);
            this.panelViewport7.Margin = new System.Windows.Forms.Padding(4);
            this.panelViewport7.Name = "panelViewport7";
            this.panelViewport7.Size = new System.Drawing.Size(365, 199);
            this.panelViewport7.TabIndex = 31;
            // 
            // panelViewport2
            // 
            this.panelViewport2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panelViewport2.Cursor = System.Windows.Forms.Cursors.Cross;
            this.panelViewport2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelViewport2.Location = new System.Drawing.Point(0, 0);
            this.panelViewport2.Margin = new System.Windows.Forms.Padding(4);
            this.panelViewport2.Name = "panelViewport2";
            this.panelViewport2.Size = new System.Drawing.Size(790, 558);
            this.panelViewport2.TabIndex = 28;
            this.panelViewport2.Paint += new System.Windows.Forms.PaintEventHandler(this.panelViewport2_Paint);
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer4.IsSplitterFixed = true;
            this.splitContainer4.Location = new System.Drawing.Point(0, 71);
            this.splitContainer4.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer4.Size = new System.Drawing.Size(1355, 558);
            this.splitContainer4.SplitterDistance = 190;
            this.splitContainer4.SplitterWidth = 5;
            this.splitContainer4.TabIndex = 36;
            this.splitContainer4.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(1187, 49);
            this.label51.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(0, 16);
            this.label51.TabIndex = 88;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.buttonFrequencyEqual);
            this.groupBox1.Controls.Add(this.label51);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1355, 71);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(689, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 16);
            this.label3.TabIndex = 98;
            this.label3.Text = "Select reciver!";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(528, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 16);
            this.label1.TabIndex = 97;
            this.label1.Text = "Frequency ";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(0, 23);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(187, 28);
            this.button2.TabIndex = 96;
            this.button2.Text = "Show all";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1301, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 16);
            this.label2.TabIndex = 95;
            this.label2.Text = "v. 1.50";
            // 
            // buttonFrequencyEqual
            // 
            this.buttonFrequencyEqual.BackColor = System.Drawing.Color.Transparent;
            this.buttonFrequencyEqual.FlatAppearance.BorderSize = 0;
            this.buttonFrequencyEqual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFrequencyEqual.ForeColor = System.Drawing.Color.Transparent;
            this.buttonFrequencyEqual.ImageIndex = 0;
            this.buttonFrequencyEqual.ImageList = this.imageList3;
            this.buttonFrequencyEqual.Location = new System.Drawing.Point(609, 14);
            this.buttonFrequencyEqual.Margin = new System.Windows.Forms.Padding(4);
            this.buttonFrequencyEqual.Name = "buttonFrequencyEqual";
            this.buttonFrequencyEqual.Size = new System.Drawing.Size(39, 41);
            this.buttonFrequencyEqual.TabIndex = 92;
            this.buttonFrequencyEqual.UseVisualStyleBackColor = false;
            this.buttonFrequencyEqual.Click += new System.EventHandler(this.buttonFrequencyEqual_Click);
            // 
            // imageList3
            // 
            this.imageList3.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList3.ImageStream")));
            this.imageList3.TransparentColor = System.Drawing.Color.White;
            this.imageList3.Images.SetKeyName(0, "locked.jpg");
            this.imageList3.Images.SetKeyName(1, "unlocked.jpg");
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ImageIndex = 0;
            this.button1.ImageList = this.imageList1;
            this.button1.Location = new System.Drawing.Point(259, 13);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(63, 52);
            this.button1.TabIndex = 27;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1355, 629);
            this.Controls.Add(this.splitContainer4);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Pasive Radar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResizeBegin += new System.EventHandler(this.Form1_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.splitContainer8.Panel1.ResumeLayout(false);
            this.splitContainer8.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).EndInit();
            this.splitContainer8.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel panelViewport3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Button button5;
        private DisplayControl displayControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label51;
  
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button7;
        private RadarControl radarControl1;
 
        private System.Windows.Forms.Button buttonFrequencyEqual;
        private System.Windows.Forms.ImageList imageList3;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.Panel panelViewport6;
        private System.Windows.Forms.Button button9;
        private CorrelateControl correlateControl1;
        private System.Windows.Forms.SplitContainer splitContainer8;
        private System.Windows.Forms.Panel panelViewport7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panelViewport2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
    }
}

