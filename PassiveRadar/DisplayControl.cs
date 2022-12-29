
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PasiveRadar
{
    public partial class DisplayControl : UserControl
    {

        public delegate void MyDelegate(Flags LocalFlags);
        public static event MyDelegate EventSettings;

        bool SET = false;  //Avoid sending settings to the main program
        bool CustomColorsAccepted;
        private Color[] LocalColorTable = new Color[Draw.ColorTableSize];
        int LocalColorTheme;
        private Flags LocalFlags; //Not elegant but works to comunicate with colorform
        public DisplayControl()
        {
            InitializeComponent();
            Form1.FlagsDelegate += new Form1.MyDelegate(Initialize);
        }

        public void FalseCheckBoxes()
        {
            checkBox3.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;

        }

        void Initialize(Flags flags)
        {
            SET = false;
            LocalFlags = new Flags();


            LocalFlags.TwoDonglesMode = flags.TwoDonglesMode;
            LocalFlags.showRadar0 = flags.showRadar0;
            LocalFlags.showBackground = flags.showBackground;

            LocalFlags.showCorrelateWave0 = flags.showCorrelateWave0;
            LocalFlags.showCorrelateFlow0 = flags.showCorrelateFlow0;

            LocalFlags.refresh_delay = flags.refresh_delay;
            LocalFlags.ColorTheme = flags.ColorTheme;

            LocalFlags.Radio_buffer_size = flags.Radio_buffer_size;

            for (int i = 0; i < Flags.MAX_DONGLES; i++)
            {
                LocalFlags.Amplification[i] = flags.Amplification[i];
                LocalFlags.Cumulation[i] = flags.Cumulation[i];
                LocalFlags.Level[i] = flags.Level[i];
            }

            //Execute
            if (LocalFlags.TwoDonglesMode == false)
            {
                checkBox7.Enabled = false;
                checkBox8.Enabled = false;
            }
            else
            {
                checkBox7.Enabled = true;
                checkBox8.Enabled = true;
            }

            checkBox3.Checked = LocalFlags.showRadar0;
            checkBox5.Checked = LocalFlags.showBackground;

            checkBox7.Checked = LocalFlags.showCorrelateWave0;
            checkBox8.Checked = LocalFlags.showCorrelateFlow0;

            trackBar3.Value = LocalFlags.refresh_delay;
            label6.Text = "" + trackBar3.Value + " ms";

            RadioBuffer_control.Value = LocalFlags.Radio_buffer_size;
            label2.Text = "" + LocalFlags.Radio_buffer_size;

            if (CustomColorsAccepted == false)
                comboBox1.SelectedIndex = LocalColorTheme = LocalFlags.ColorTheme;

            //Must be here for cancel the button, come back to settings from beginning without changes
            //Copy the color table to use it later for windows creation
            //If one dongle


            UpdateTable(flags);
            SET = true;
        }


        void SendSettings()
        {
            if (!SET) return;
            Flags LocalFlags = new Flags();

            LocalFlags.showRadar0 = checkBox3.Checked;
            LocalFlags.showBackground = checkBox5.Checked;
            LocalFlags.showCorrelateWave0 = checkBox7.Checked;
            LocalFlags.showCorrelateFlow0 = checkBox8.Checked;
            LocalFlags.refresh_delay = trackBar3.Value;
            label6.Text = "" + trackBar3.Value + "ms";

            LocalFlags.Radio_buffer_size = RadioBuffer_control.Value;
            label2.Text = "" + RadioBuffer_control.Value;

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    LocalFlags.ColorTheme = 0;
                    break;
                case 1:
                    LocalFlags.ColorTheme = 1;
                    break;
                case 2:
                    {
                        if (CustomColorsAccepted)
                            LocalFlags.ColorTheme = 2;
                        else
                            LocalFlags.ColorTheme = LocalColorTheme;
                        CustomColorsAccepted = false;
                    }
                    break;
            }


            if (EventSettings != null)
                EventSettings(LocalFlags);
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            SendSettings();
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

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            SendSettings();
        }


        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SET) return;
            if (comboBox1.SelectedIndex == 2)
            {
                ColorForm mColorForm = new ColorForm(LocalColorTable);
                mColorForm.ShowDialog();
                if (mColorForm.AcceptColors == true)
                {
                    CustomColorsAccepted = true;
                    LocalFlags.ColorTheme = 2;
                }
            }
            SendSettings();
        }

        public void UpdateTable(Flags flags)
        {

            for (int i = 0; i < Draw.ColorTableSize; i++)
            {
                //Convert colors from system to XNA
                int r = flags.ColorThemeTable[i].R;
                int g = flags.ColorThemeTable[i].G;
                int b = flags.ColorThemeTable[i].B;
                LocalColorTable[i] = Color.FromArgb(flags.alpha, r, g, b);
            }

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {

            SendSettings();
        }

        private void RadioBuffer_control_Scroll(object sender, EventArgs e)
        {
            SendSettings();
        }
    }
}
