using System;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PasiveRadar
{
    public partial class RadarControl : UserControl
    {
        public delegate void MyDelegate(Flags LocalFlags);
        public static event MyDelegate RadarSettings;
        bool SET = false;  //Avoid sending settings to the main program
        uint BufferSize;

        public RadarControl()
        {
            InitializeComponent();
            Form1.FlagsDelegate += new Form1.MyDelegate(Initialize);
        }

        void Initialize(Flags flags)
        {
            SET = false;
            Flags LocalFlags = new Flags();
            LocalFlags.BufferSize = flags.BufferSize;
            LocalFlags.PasiveGain = flags.PasiveGain;
            LocalFlags.DopplerZoom = flags.DopplerZoom;
            LocalFlags.average = flags.average;
            LocalFlags.remove_symetrics = flags.remove_symetrics;
            LocalFlags.TwoDonglesMode = flags.TwoDonglesMode;
            LocalFlags.Rows = flags.Rows;
            LocalFlags.Columns = flags.Columns;
            LocalFlags.OpenCL = flags.OpenCL;
            LocalFlags.DistanceShift = flags.DistanceShift;
            LocalFlags.CorrectBackground = flags.CorrectBackground;
            LocalFlags.NrCorrectionPoints = flags.NrCorrectionPoints;
            LocalFlags.ColectEvery = flags.ColectEvery;
            LocalFlags.CorectionWeight = flags.CorectionWeight;
            LocalFlags.AMDdriver = flags.AMDdriver;
            LocalFlags.TwoDonglesMode = flags.TwoDonglesMode;
            LocalFlags.scale_type = flags.scale_type;
            LocalFlags.Nr_active_radio = flags.Nr_active_radio;
            LocalFlags.alpha = flags.alpha;
            LocalFlags.MaxAverage = flags.MaxAverage; //has to be the same as arrays size
            LocalFlags.FreezeBackground = flags.FreezeBackground; //has to be the same as arrays size



            trackBar4.Maximum = (int)LocalFlags.MaxAverage;

            //if (LocalFlags.BufferSize == 1024 * 32) comboBox1.SelectedIndex = 0;
            //if (LocalFlags.BufferSize == 1024 * 64) comboBox1.SelectedIndex = 1;
            //if (LocalFlags.BufferSize == 1024 * 128) comboBox1.SelectedIndex = 2;
            if (LocalFlags.BufferSize == 1024 * 256) comboBox1.SelectedIndex = 0;
            if (LocalFlags.BufferSize == 1024 * 512) comboBox1.SelectedIndex = 1;
            if (LocalFlags.BufferSize == 1024 * 1024) comboBox1.SelectedIndex = 2;
            if (LocalFlags.BufferSize == 1024 * 2048) comboBox1.SelectedIndex = 3;
            if (LocalFlags.BufferSize == 1024 * 4096) comboBox1.SelectedIndex = 4;

            trackBar2.Value = (int)(LocalFlags.PasiveGain * 10);
            trackBar3.Value = (int)LocalFlags.DopplerZoom;
            trackBar4.Value = (int)LocalFlags.average;
            trackBar5.Value = (int)LocalFlags.Rows;
            trackBar6.Value = (int)LocalFlags.DistanceShift;
            trackBar7.Value = LocalFlags.NrCorrectionPoints;
            trackBar8.Value = LocalFlags.ColectEvery;
            trackBar9.Value = (int)(LocalFlags.CorectionWeight * 100);
            trackBar10.Value = (int)LocalFlags.scale_type;
            trackBar_alpha.Value = LocalFlags.alpha;

            checkBox4.Checked = LocalFlags.remove_symetrics;
            checkBox3.Checked = LocalFlags.CorrectBackground;
            checkBox1.Checked = LocalFlags.TwoDonglesMode;

            label1.Text = "" + LocalFlags.PasiveGain;
            label2.Text = "" + LocalFlags.DopplerZoom;
            label7.Text = "" + LocalFlags.average;
            label13.Text = "" + LocalFlags.DistanceShift + "";
            label15.Text = "" + LocalFlags.NrCorrectionPoints;
            label17.Text = "" + LocalFlags.ColectEvery;
            label19.Text = "" + LocalFlags.CorectionWeight;
            label_alpha.Text = "" + LocalFlags.alpha;

            trackBar1.Value = (int)LocalFlags.Columns;


            label11.Text = "" + LocalFlags.Columns;
            label12.Text = "" + trackBar5.Value;// LocalFlags.Rows;

            if (LocalFlags.Nr_active_radio < 2)
            {
                checkBox1.Enabled = false;
                checkBox1.Checked = false;
            }
            else
                checkBox1.Enabled = true;
            //Memory protection
            MaxMemory();

            //Set active inactive controls
            SetActiveInactiveControls(LocalFlags);
            SET = true;

        }

        public void ActiveDeactivateColumnsControll(bool state)
        {
            // trackBar1.Enabled = state;
        }
        private void SetActiveInactiveControls(Flags LocalFlags)
        {

        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            //if (comboBox1.SelectedIndex == 0) BufferSize = 1024 * 32;
            //if (comboBox1.SelectedIndex == 1) BufferSize = 1024 * 64;
            //if (comboBox1.SelectedIndex == 2) BufferSize = 1024 * 128;
            if (comboBox1.SelectedIndex == 0) BufferSize = 1024 * 256;
            if (comboBox1.SelectedIndex == 1) BufferSize = 1024 * 512;
            if (comboBox1.SelectedIndex == 2) BufferSize = 1024 * 1024;
            if (comboBox1.SelectedIndex == 3) BufferSize = 1024 * 2048;
            if (comboBox1.SelectedIndex == 4) BufferSize = 1024 * 4096;



            SendSettings();
        }


        void MaxMemory()
        {
            ulong d = BufferSize * (uint)trackBar1.Value;
            label25.Text = "" + d;

            if (d > 1024L * 1024 * 400) //512
                label25.ForeColor = System.Drawing.Color.Yellow;
            else
                label25.ForeColor = System.Drawing.Color.Green;

            if (d > 1024L * 1024 * 512) //512
                label25.ForeColor = System.Drawing.Color.Red;


            if (comboBox1.SelectedIndex == 3)
            {
                if (trackBar1.Value > 255)
                {
                    trackBar1.Value = 255;
                    trackBar1.Maximum = 256;
                }
                if (trackBar5.Value > 2500)
                {
                    trackBar5.Value = 2500;
                    trackBar5.Maximum = 2501;
                }
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                if (trackBar1.Value > 140)
                {
                    trackBar1.Value = 140;
                    trackBar1.Maximum = 141;
                }
                if (trackBar5.Value > 1000)
                {
                    trackBar5.Value = 1000;
                    trackBar5.Maximum = 1001;                   
                }
 
            }
            else
            {
                trackBar1.Maximum = 512;
                trackBar5.Maximum = 4000;
            }

            trackBar1.Update();
            trackBar5.Update();
            trackBar1.Refresh();
            trackBar5.Refresh();

            int milliseconds = 100;
            Thread.Sleep(milliseconds);


        }
        void SendSettings()
        {

            MaxMemory();

            if (!SET) return;

            Flags LocalFlags = new Flags();

            LocalFlags.BufferSize = BufferSize;
            LocalFlags.PasiveGain = 0.1f * trackBar2.Value;
            LocalFlags.DopplerZoom = (uint)trackBar3.Value;
            LocalFlags.average = trackBar4.Value;
            LocalFlags.remove_symetrics = checkBox4.Checked;
            LocalFlags.DistanceShift = trackBar6.Value;
            LocalFlags.CorrectBackground = checkBox3.Checked;
            LocalFlags.NrCorrectionPoints = trackBar7.Value;
            LocalFlags.ColectEvery = trackBar8.Value;
            LocalFlags.CorectionWeight = 0.01f * trackBar9.Value;
            LocalFlags.scale_type = (short)trackBar10.Value;
            LocalFlags.TwoDonglesMode = checkBox1.Checked;
            LocalFlags.alpha = (byte)trackBar_alpha.Value;
            LocalFlags.FreezeBackground = checkBoxFreeze.Checked;


            label1.Text = "" + LocalFlags.PasiveGain;
            label2.Text = "" + LocalFlags.DopplerZoom;
            label7.Text = "" + LocalFlags.average;
            label13.Text = "" + LocalFlags.DistanceShift + "";
            label15.Text = "" + LocalFlags.NrCorrectionPoints;
            label17.Text = "" + LocalFlags.ColectEvery;
            label19.Text = "" + LocalFlags.CorectionWeight;
            label_alpha.Text = "" + LocalFlags.alpha;

            LocalFlags.Columns = (uint)trackBar1.Value;

            LocalFlags.Rows = (uint)trackBar5.Value;

            label11.Text = "" + LocalFlags.Columns;
            label12.Text = "" + LocalFlags.Rows;

            //Set active inactive
            SetActiveInactiveControls(LocalFlags);

            if (RadarSettings != null)
                RadarSettings(LocalFlags);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            SendSettings();//columns
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            SendSettings();//rows
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SendSettings();//backgroun
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SendSettings();//backgroun
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int temp_6value = trackBar6.Value;
            trackBar6.Value = 0;
            trackBar6.Maximum = trackBar1.Value - 1;

            if (trackBar6.Maximum < trackBar1.Value - 1)
            {
                trackBar6.Maximum = trackBar1.Value - 1;
            }

            if (trackBar6.Value > trackBar1.Value - 1)
                trackBar6.Value = trackBar6.Maximum;
            else
                trackBar6.Value = temp_6value;



            SendSettings();//columns


        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            SendSettings();//rows
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            SendSettings();//mix mode
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            SendSettings();//background
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            SendSettings();//nr corection points
        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            SendSettings();//colected every
        }

        private void trackBar9_Scroll(object sender, EventArgs e)
        {
            SendSettings();//colection weighht
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void trackBar10_Scroll(object sender, EventArgs e)
        {
            SendSettings();//nr corection points
        }

        private void trackBar_alpha_Scroll(object sender, EventArgs e)
        {
            SendSettings();//colection weighht
        }

        private void RadarControl_Load(object sender, EventArgs e)
        {

        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {


        }

        private void checkBoxFreeze_CheckedChanged(object sender, EventArgs e)
        {
            SendSettings();
        }
    }
}
