using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Windows.Forms;

namespace PasiveRadar
{
    public class WindowWave : WindowSupport
    {
        int dongle_nr;

        DrawWave mDrawWave;

        public WindowWave(Panel _panelViewport, int _dongle_nr)
        {
            panelViewport = _panelViewport;
            dongle_nr = _dongle_nr;
            mDrawWave = new DrawWave();

            service = new GraphicsDeviceService(panelViewport.Handle, panelViewport.Width, panelViewport.Height);
            service.DeviceResetting += mWinForm_DeviceResetting;
            service.DeviceReset += mWinForm_DeviceReset;

            services = new ServiceContainer();
            services.AddService<IGraphicsDeviceService>(service);
            content = new ContentManager(services, "Content");
        }

        void mWinForm_DeviceReset(Object sender, EventArgs e)
        {
            DeviceReset();
            mDrawWave.SizeChanged(panelViewport, service.GraphicsDevice, service, spriteBatch, spriteFont, mSimpleEffect);
        }

        public void Update(Flags flags)
        {
            mDrawWave.frequency = flags.frequency[dongle_nr];
            mDrawWave.rate = flags.rate[dongle_nr];
            mDrawWave.Gain = flags.Amplification[dongle_nr];
            mDrawWave.ColorThemeNr = flags.ColorTheme;
            mDrawWave.BufferSize = flags.BufferSizeRadio[dongle_nr];
            mDrawWave.ScalePrepare(panelViewport);

        }

        public void Location(float x, float lo, float up)
        {
            mDrawWave.FrequencyAtPointedLocation = x;
            if (lo != -1) mDrawWave.FilterLoLocation = lo;
            if (up != -1) mDrawWave.FilterUpLocation = up;
        }

        //Start rander the scene
        public void RenderWave(double[] data)
        {
            if (data.Length < mDrawWave.BufferSize) return;
            if (resizing) return;

            if (this.service.GraphicsDevice != null)
                mDrawWave.Scene(panelViewport, data, dongle_nr);

            try
            {
                if (service.GraphicsDevice != null)
                    service.GraphicsDevice.Present();
            }
            catch (Exception ex)
            {
                service.ResetDevice(panelViewport.Width, panelViewport.Height);
                String str = "Plot error. " + ex.ToString();
                // MessageBox.Show(str);
                //  System.Windows.Forms.Application.Exit();
            }
        }

    }
}
