using System;
using System.Windows.Forms;

namespace PasiveRadar
{
    public partial class TuningNumber : UserControl
    {
        public delegate void MyDelegate();
        public event MyDelegate DigitalNumberChange;

        public int frequency;

        public TuningNumber()
        {
            InitializeComponent();
            Update_();
        }

        int Extract(long pos)
        {
            int a = (int)frequency;
            //Remove top
            int b = (int)(a / pos % 10);
            return b;
        }

        public void Update_(bool flag = true)//flag- send status change false -do not send
        {
            #region Labels

            label10.Text = label9.Text = "" + Extract(1);
            label14.Text = label16.Text = "" + Extract(10);
            label20.Text = label18.Text = "" + Extract(100);
            label30.Text = label32.Text = "" + Extract(1000);
            label26.Text = label28.Text = "" + Extract(10000);
            label22.Text = label24.Text = "" + Extract(100000);
            label42.Text = label44.Text = "" + Extract(1000000);
            label38.Text = label40.Text = "" + Extract(10000000);
            label34.Text = label36.Text = "" + Extract(100000000);
            label54.Text = label56.Text = "" + Extract(1000000000);


            if (frequency > 9)
            {
                label16.ForeColor = label14.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                label16.ForeColor = label14.ForeColor = System.Drawing.Color.Gray;
            }

            if (frequency > 99)
            {
                label20.ForeColor = label18.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                label20.ForeColor = label18.ForeColor = System.Drawing.Color.Gray;
            }

            if (frequency > 999)
            {
                label47.ForeColor = label30.ForeColor = label32.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                label47.ForeColor = label30.ForeColor = label32.ForeColor = System.Drawing.Color.Gray;
            }

            if (frequency > 9999)
            {
                label26.ForeColor = label28.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                label26.ForeColor = label28.ForeColor = System.Drawing.Color.Gray;
            }

            if (frequency > 99999)
            {
                label22.ForeColor = label24.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                label22.ForeColor = label24.ForeColor = System.Drawing.Color.Gray;
            }

            if (frequency > 999999)
            {
                label46.ForeColor = label42.ForeColor = label44.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                label46.ForeColor = label42.ForeColor = label44.ForeColor = System.Drawing.Color.Gray;
            }

            if (frequency > 9999999)
            {
                label38.ForeColor = label40.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                label38.ForeColor = label40.ForeColor = System.Drawing.Color.Gray;
            }

            if (frequency > 99999999)
            {

                label34.ForeColor = label36.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                label34.ForeColor = label36.ForeColor = System.Drawing.Color.Gray;
            }

            if (frequency > 999999999)
            {
                label45.ForeColor = label54.ForeColor = label56.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                label45.ForeColor = label54.ForeColor = label56.ForeColor = System.Drawing.Color.Gray;
            }
            #endregion

            if (DigitalNumberChange != null && flag == true)
                DigitalNumberChange();
        }

        #region 1
        private void label9_MouseMove(object sender, MouseEventArgs e)
        {
            label9.BackColor = System.Drawing.Color.LightSkyBlue;
        }

        private void label9_MouseLeave(object sender, EventArgs e)
        {
            label9.BackColor = System.Drawing.Color.Transparent;
        }

        private void label10_MouseLeave(object sender, EventArgs e)
        {
            label10.BackColor = System.Drawing.Color.Transparent;
        }

        private void label10_MouseMove(object sender, MouseEventArgs e)
        {
            label10.BackColor = System.Drawing.Color.LightCoral;
        }

        private void label10_Click(object sender, EventArgs e)
        {
            frequency++;

            Update_();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            frequency--;
            if (frequency < 0) frequency = 9;
            Update_();
        }
        #endregion
        #region 10
        private void label14_Click(object sender, EventArgs e)
        {
            frequency += 10;

            Update_();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            if (frequency >= 10)
                frequency -= 10;

            Update_();
        }

        private void label16_MouseMove(object sender, MouseEventArgs e)
        {
            label16.BackColor = System.Drawing.Color.LightSkyBlue;
        }

        private void label16_MouseLeave(object sender, EventArgs e)
        {
            label16.BackColor = System.Drawing.Color.Transparent;
        }

        private void label14_MouseMove(object sender, MouseEventArgs e)
        {
            label14.BackColor = System.Drawing.Color.LightCoral;
        }

        private void label14_MouseLeave(object sender, EventArgs e)
        {
            label14.BackColor = System.Drawing.Color.Transparent;
        }
        #endregion
        #region 100
        private void label20_Click(object sender, EventArgs e)
        {
            if (frequency >= 100)
                frequency -= 100;
            Update_();
        }

        private void label18_Click(object sender, EventArgs e)
        {
            frequency += 100;
            Update_();
        }

        private void label18_MouseMove(object sender, MouseEventArgs e)
        {
            label18.BackColor = System.Drawing.Color.LightCoral;
        }

        private void label18_MouseLeave(object sender, EventArgs e)
        {
            label18.BackColor = System.Drawing.Color.Transparent;
        }

        private void label20_MouseMove(object sender, MouseEventArgs e)
        {
            label20.BackColor = System.Drawing.Color.LightSkyBlue;
        }

        private void label20_MouseLeave(object sender, EventArgs e)
        {
            label20.BackColor = System.Drawing.Color.Transparent;
        }

        #endregion
        #region 1 000
        private void label30_Click(object sender, EventArgs e)
        {

            frequency += 1000;
            Update_();
        }

        private void label32_Click(object sender, EventArgs e)
        {
            if (frequency >= 1000)
                frequency -= 1000;
            Update_();
        }

        private void label32_MouseMove(object sender, MouseEventArgs e)
        {
            label32.BackColor = System.Drawing.Color.LightSkyBlue;
        }

        private void label32_MouseLeave(object sender, EventArgs e)
        {
            label32.BackColor = System.Drawing.Color.Transparent;
        }

        private void label30_MouseMove(object sender, MouseEventArgs e)
        {
            label30.BackColor = System.Drawing.Color.LightCoral;
        }

        private void label30_MouseLeave(object sender, EventArgs e)
        {
            label30.BackColor = System.Drawing.Color.Transparent;
        }

        #endregion
        #region 10 000
        private void label26_Click(object sender, EventArgs e)
        {
            frequency += 10000;
            Update_();
        }

        private void label28_Click(object sender, EventArgs e)
        {
            if (frequency >= 10000)
                frequency -= 10000;
            Update_();
        }

        private void label28_MouseMove(object sender, MouseEventArgs e)
        {
            label28.BackColor = System.Drawing.Color.LightSkyBlue;
        }

        private void label28_MouseLeave(object sender, EventArgs e)
        {
            label28.BackColor = System.Drawing.Color.Transparent;
        }

        private void label26_MouseMove(object sender, MouseEventArgs e)
        {
            label26.BackColor = System.Drawing.Color.LightCoral;
        }

        private void label26_MouseLeave(object sender, EventArgs e)
        {
            label26.BackColor = System.Drawing.Color.Transparent;
        }
        #endregion
        #region 100 000
        private void label22_Click(object sender, EventArgs e)
        {
            frequency += 100000;

            Update_();
        }

        private void label24_Click(object sender, EventArgs e)
        {
            if (frequency >= 100000)
                frequency -= 100000;
            Update_();
        }
        private void label24_MouseMove(object sender, MouseEventArgs e)
        {
            label24.BackColor = System.Drawing.Color.LightSkyBlue;
        }

        private void label24_MouseLeave(object sender, EventArgs e)
        {
            label24.BackColor = System.Drawing.Color.Transparent;
        }

        private void label22_MouseMove(object sender, MouseEventArgs e)
        {
            label22.BackColor = System.Drawing.Color.LightCoral;
        }

        private void label22_MouseLeave(object sender, EventArgs e)
        {
            label22.BackColor = System.Drawing.Color.Transparent;
        }
        #endregion
        #region 1 000 000
        private void label42_Click(object sender, EventArgs e)
        {
            frequency += 1000000;
            Update_();
        }

        private void label44_Click(object sender, EventArgs e)
        {
            if (frequency >= 1000000)
                frequency -= 1000000;
            Update_();
        }

        private void label44_MouseMove(object sender, MouseEventArgs e)
        {
            label44.BackColor = System.Drawing.Color.LightSkyBlue;
        }

        private void label44_MouseLeave(object sender, EventArgs e)
        {
            label44.BackColor = System.Drawing.Color.Transparent;
        }

        private void label42_MouseMove(object sender, MouseEventArgs e)
        {
            label42.BackColor = System.Drawing.Color.LightCoral;
        }

        private void label42_MouseLeave(object sender, EventArgs e)
        {
            label42.BackColor = System.Drawing.Color.Transparent;
        }

        #endregion
        #region 10 000 000
        private void label38_Click(object sender, EventArgs e)
        {
            frequency += 10000000;
            Update_();
        }

        private void label40_Click(object sender, EventArgs e)
        {
            if (frequency >= 10000000)
                frequency -= 10000000;
            Update_();
        }

        private void label40_MouseMove(object sender, MouseEventArgs e)
        {
            label40.BackColor = System.Drawing.Color.LightSkyBlue;
        }

        private void label40_MouseLeave(object sender, EventArgs e)
        {
            label40.BackColor = System.Drawing.Color.Transparent;
        }

        private void label38_MouseMove(object sender, MouseEventArgs e)
        {
            label38.BackColor = System.Drawing.Color.LightCoral;
        }

        private void label38_MouseLeave(object sender, EventArgs e)
        {
            label38.BackColor = System.Drawing.Color.Transparent;
        }
        #endregion
        #region 100 000 000
        private void label34_Click(object sender, EventArgs e)
        {
            frequency += 100000000;
            Update_();
        }

        private void label36_Click(object sender, EventArgs e)
        {
            if (frequency >= 100000000)
                frequency -= 100000000;
            Update_();
        }

        private void label36_MouseLeave(object sender, EventArgs e)
        {
            label36.BackColor = System.Drawing.Color.Transparent;
        }

        private void label36_MouseMove(object sender, MouseEventArgs e)
        {
            label36.BackColor = System.Drawing.Color.LightSkyBlue;
        }

        private void label34_MouseMove(object sender, MouseEventArgs e)
        {
            label34.BackColor = System.Drawing.Color.LightCoral;
        }

        private void label34_MouseLeave(object sender, EventArgs e)
        {
            label34.BackColor = System.Drawing.Color.Transparent;
        }
        #endregion
        #region 1 000 000 000
        private void label54_Click(object sender, EventArgs e)
        {
            frequency += 1000000000;
            Update_();
        }

        private void label56_Click(object sender, EventArgs e)
        {
            if (frequency >= 1000000000)
                frequency -= 1000000000;
            Update_();
        }

        private void label56_MouseLeave(object sender, EventArgs e)
        {
            label56.BackColor = System.Drawing.Color.Transparent;
        }

        private void label56_MouseMove(object sender, MouseEventArgs e)
        {

            label56.BackColor = System.Drawing.Color.LightSkyBlue;
        }

        private void label54_MouseMove(object sender, MouseEventArgs e)
        {
            label54.BackColor = System.Drawing.Color.LightCoral;
        }

        private void label54_MouseLeave(object sender, EventArgs e)
        {
            label54.BackColor = System.Drawing.Color.Transparent;
        }


        #endregion

        private void TuningNumber_Load(object sender, EventArgs e)
        {

        }
    }
}
