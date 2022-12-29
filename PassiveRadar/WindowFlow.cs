using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Windows.Forms;

namespace PasiveRadar
{
    public class WindowFlow : WindowSupport
    {
        int dongle_nr;

        DrawFlow mDrawFlow;

        public WindowFlow(Panel _panelViewport, int _dongle_nr)
        {
            panelViewport = _panelViewport;
            dongle_nr = _dongle_nr;

            mDrawFlow = new DrawFlow();

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
            mDrawFlow.SizeChanged(panelViewport, service.GraphicsDevice, service, spriteBatch, spriteFont, mSimpleEffect, texture);
        }

        public void Update(Flags flags)
        {
            mDrawFlow.frequency = flags.frequency[dongle_nr];
            mDrawFlow.rate = flags.rate[dongle_nr];
            mDrawFlow.Gain = flags.Amplification[dongle_nr];
            mDrawFlow.BufferSize = flags.BufferSizeRadio[dongle_nr];
            mDrawFlow.Level = flags.Level[dongle_nr];
            mDrawFlow.ColorThemeNr = flags.ColorTheme;
            mDrawFlow.ScaleXPrepare(panelViewport, flags.ColorThemeTable);
        }

        public void Location(float x)
        {
            mDrawFlow.FrequencyAtPointedLocation = x;
        }

        //Start rander the scene
        public void RenderFlow(double[] data)
        {
            if (data.Length < mDrawFlow.BufferSize) return;
            if (resizing) return;

            if (this.service.GraphicsDevice != null)
                mDrawFlow.Scene(panelViewport, data);

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
