using System;
using System.Windows.Forms;

namespace PasiveRadar
{
    public partial class Settings : Form
    {
        int TunerNr; //Inform for which radio the window is assigned
        public int itm;
        int[] tuner_gain_list;

 
        int[,] stage_gains_list;
        int[] nr_of_gains_in_stage;

        public static object Default { get; internal set; }

        public delegate void MyDelegate(int Radio, int item);
        public delegate void MyDelegateSettings(int index, int gain_index, uint rate, bool AGC, bool MGC, bool OffsetTuning, int FrequencyCorrection, int sampling, bool dithering,bool StagesFlag, int[] StageGain);
        public static event MyDelegate EventRadio;
        public static event MyDelegateSettings EventGain;

        public Settings(int _TunerNr)
        {
            InitializeComponent();
            TunerNr = _TunerNr;
            tuner_gain_list = new int[256];

            //Gains         
            stage_gains_list = new int[32, 256];
            nr_of_gains_in_stage = new int[32];

            // comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 10;
            comboBox3.SelectedIndex = 0;
            checkBox4.Enabled = false;

        }

        public void SetSettings(Radio radio)
        {
            //Rate
            if (radio == null) return;
            uint rate = radio.rate;

            //RF gain
            trackBar1.Maximum = radio.Number_of_gains - 1;
            for (int i = 0; i < radio.Number_of_gains; i++)
                tuner_gain_list[i] = radio.tuner_gain_list[i];
            trackBar1.Value = trackBar1.Maximum;
            if (trackBar1.Maximum>0)
            label1.Text = "" + 0.1 * tuner_gain_list[trackBar1.Maximum] + " dB";

            //Gain stages
            trackBar2.Maximum = radio.nr_of_gains_in_stage[0] - 1;
            for (int i = 0; i < radio.nr_of_gains_in_stage[0]; i++)
                stage_gains_list[0, i] = radio.stage_gains_list[0, i];
            trackBar2.Value = trackBar2.Maximum;

            trackBar3.Maximum = radio.nr_of_gains_in_stage[1] - 1;
            for (int i = 0; i < radio.nr_of_gains_in_stage[1]; i++)
                stage_gains_list[1, i] = radio.stage_gains_list[1, i];
            trackBar3.Value = trackBar3.Maximum;

            trackBar4.Maximum = radio.nr_of_gains_in_stage[2] - 1;
            for (int i = 0; i < radio.nr_of_gains_in_stage[2]; i++)
                stage_gains_list[2, i] = radio.stage_gains_list[2, i];
            trackBar4.Value = trackBar4.Maximum;

            if (radio.nr_of_gains_in_stage[0] > 0 && radio.nr_of_gains_in_stage[1] > 0 && radio.nr_of_gains_in_stage[2] > 0)
            {
                label2.Text = "" + 0.1 * stage_gains_list[0, radio.nr_of_gains_in_stage[0] - 1] + " dB";
                label3.Text = "" + 0.1 * stage_gains_list[1, radio.nr_of_gains_in_stage[1] - 1] + " dB";
                label4.Text = "" + 0.1 * stage_gains_list[2, radio.nr_of_gains_in_stage[2] - 1] + " dB";
            }
            int itm = 0;
            switch (rate)
            {
                case 250000:
                    itm = 0;
                    break;
                case 900001:
                    itm = 1;
                    break;
                case 1024000:
                    itm = 2;
                    break;
                case 1400000:
                    itm =3 ;
                    break;
                case 1800000:
                    itm = 4;
                    break;
                case 1920000:
                    itm = 5;
                    break;
                case 2048000:
                    itm = 6;
                    break;
                case 2400000:
                    itm = 7;
                    break;
                case 2560000:
                    itm = 8;
                    break;
                case 2800000:
                    itm = 9;
                    break;
                case 3200000:
                    itm = 10;
                    break;
            }
            comboBox2.SelectedIndex = itm;

            label10.Text = "" + radio.GetName();
            if (radio.dongle_type == 5)
                checkBox4.Enabled = true;
            else
                checkBox4.Enabled = false;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void Settings1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        public void ComboBoxRadio_Update(ref Find find)
        {
            comboBox1.Items.Clear();

            string str = "";

            //Add devices to the list
            for (int i = 0; i < find.NrOfDevices; i++)
            {
                if (find.StatusList[i] == 1)//is in use?
                    str = "*" + find.NameList[i];
                else
                    str = find.NameList[i];
                comboBox1.Items.Add(str);
            }
            comboBox1.SelectedIndex = itm;
        }


        void SendSettings()
        {
            uint rate = 1;
            int gain_index = trackBar1.Value;
            int[] gain_stage = new int[3];
            if (gain_index > 0)
            {
                label1.Text = "" + 0.1 * tuner_gain_list[gain_index] + " dB";


                gain_stage[0] = trackBar2.Value;
                if (gain_stage[0] > 0)
                    label2.Text = "" + 0.1 * stage_gains_list[0, gain_stage[0]] + " dB";

                gain_stage[1] = trackBar3.Value;
                if (gain_stage[1] > 0)
                    label3.Text = "" + 0.1 * stage_gains_list[1, gain_stage[1]] + " dB";

                gain_stage[2] = trackBar4.Value;
                if (gain_stage[2] > 0)
                    label4.Text = "" + 0.1 * stage_gains_list[2, gain_stage[2]] + " dB";
            }
            //Rate
            int Rate_Index = comboBox2.SelectedIndex;
            switch (Rate_Index)
            {
                case 0:
                    rate = 250000;
                    break;
                case 1:
                    rate = 900001;
                    break;
                case 2:
                    rate = 1024000;
                    break;
                case 3:
                    rate = 1400000;
                    break;
                case 4:
                    rate = 1800000;
                    break;
                case 5:
                    rate = 1920000;
                    break;
                case 6:
                    rate = 2048000;
                    break;
                case 7:
                    rate = 2400000;
                    break;
                case 8:
                    rate = 2560000;
                    break;
                case 9:
                    rate = 2800000;
                    break;
                case 10:
                    rate = 3200000;
                    break;
            }

            //Sampling mode
            int Sampling_Index = comboBox3.SelectedIndex;
            int sampling = 0;
            switch (Rate_Index)
            {
                case 0:
                    sampling = 0;
                    break;
                case 1:
                    sampling = 1;
                    break;
                case 2:
                    sampling = 2;
                    break;
            }


            bool AGC = checkBox1.Checked;
            bool MGC = checkBox2.Checked;
            bool ShiftOn = checkBox3.Checked;
            bool Dithering = checkBox4.Checked;
            bool StagesFlag = checkBox5.Checked;
            int shift = (int)numericUpDown1.Value;

            if (MGC)
                if (StagesFlag)
                {
                    trackBar1.Enabled = false;
                    trackBar2.Enabled = true;
                    trackBar3.Enabled = true;
                    trackBar4.Enabled = true;
                }
                else
                {
                    trackBar1.Enabled = true;
                    trackBar2.Enabled = false;
                    trackBar3.Enabled = false;
                    trackBar4.Enabled = false;
                }
            else
            {
                trackBar1.Enabled = false;
                trackBar2.Enabled = false;
                trackBar3.Enabled = false;
                trackBar4.Enabled = false;

            }

            if (ShiftOn)
                numericUpDown1.Enabled = true;
            else
                numericUpDown1.Enabled = false;

            if (EventGain != null)
                EventGain(TunerNr, gain_index, rate, AGC, MGC, ShiftOn, shift, sampling, Dithering, StagesFlag, gain_stage);

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

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != itm)
            {
                itm = comboBox1.SelectedIndex;
                if (EventRadio != null)
                    EventRadio(TunerNr, itm);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
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

        private void trackBar1_Scroll_1(object sender, EventArgs e)
        {
            SendSettings();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            SendSettings();
        }
    }
}
