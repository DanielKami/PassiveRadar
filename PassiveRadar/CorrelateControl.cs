using System;
using System.Windows.Forms;

namespace PasiveRadar
{
    public partial class CorrelateControl : UserControl
    {
        public delegate void MyDelegate(Flags LocalFlags);
        public static event MyDelegate EventSettings;
        bool SET = false;  //Avoid sending settings to the main program
        public CorrelateControl()
        {
            InitializeComponent();
            Form1.FlagsDelegate += new Form1.MyDelegate(Initialize);
        }

        void Initialize(Flags flags)
        {
            SET = false;
            Flags LocalFlags = new Flags();
            LocalFlags.Negative = flags.Negative;
            LocalFlags.Positive = flags.Positive;
            LocalFlags.AcceptedLevel = flags.AcceptedLevel;
            LocalFlags.CorrelateAmplitude = flags.CorrelateAmplitude;
            LocalFlags.CorrelateLevel = flags.CorrelateLevel;
            LocalFlags.AutoCorrelate = flags.AutoCorrelate;


            if (LocalFlags.AcceptedLevel < 0) LocalFlags.AcceptedLevel = 0.8;

            trackBar1.Value = (int)LocalFlags.Negative;
            trackBar2.Value = (int)LocalFlags.Positive;          
            trackBar3.Value = (int)(LocalFlags.AcceptedLevel * 100);
            trackBar4.Value = LocalFlags.CorrelateAmplitude / 10;
            trackBar5.Value = LocalFlags.CorrelateLevel + 2000;

            checkBox1.Checked = LocalFlags.AutoCorrelate;
            if (LocalFlags.AutoCorrelate)
                trackBar3.Enabled = false;
            else
                trackBar3.Enabled = true;

            label3.Text = "" + LocalFlags.Negative;
            label4.Text = "" + LocalFlags.Positive;
            label6.Text = "" + (LocalFlags.AcceptedLevel).ToString("0.00");
            label9.Text = "" + LocalFlags.CorrelateAmplitude / 10;
            label10.Text = "" + LocalFlags.CorrelateLevel;
            SET = true;
        }

        void SendSettings()
        {
            if (!SET) return;
            Flags LocalFlags = new Flags();

            LocalFlags.Negative = (uint)trackBar1.Value;
            LocalFlags.Positive = (uint)trackBar2.Value;
            LocalFlags.AcceptedLevel = 0.01 * trackBar3.Value;
            LocalFlags.CorrelateAmplitude = trackBar4.Value * 10;
            LocalFlags.CorrelateLevel = trackBar5.Value - 2000;
            LocalFlags.AutoCorrelate = checkBox1.Checked;

            label3.Text = "" + LocalFlags.Negative;
            label4.Text = "" + LocalFlags.Positive;
            label6.Text = "" + (LocalFlags.AcceptedLevel).ToString("0.00");
            label9.Text = "" + LocalFlags.CorrelateAmplitude / 10;
            label10.Text = "" + LocalFlags.CorrelateLevel;

            if (LocalFlags.AutoCorrelate)
                trackBar3.Enabled = false;
            else
                trackBar3.Enabled = true;

            if (EventSettings != null)
                EventSettings(LocalFlags);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void label5_Click(object sender, EventArgs e)
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

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SendSettings();
        }
    }
}
