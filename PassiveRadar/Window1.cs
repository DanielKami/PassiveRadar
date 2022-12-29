using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows.Forms;

namespace PasiveRadar
{
    public class Window : WindowSupport
    {
        int dongle_nr;

        DrawWave mDrawWave;
        DrawRadar mDrawRadar;
        DrawCorrelate mDrawCorrelate;


        public Window(Panel _panelViewport, int _dongle_nr)
        {
            panelViewport = _panelViewport;
            dongle_nr = _dongle_nr;
            mDrawWave = new DrawWave();
            mDrawRadar = new DrawRadar();
            mDrawCorrelate = new DrawCorrelate();

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
            mDrawRadar.SizeChanged(panelViewport, service.GraphicsDevice, service, spriteBatch, spriteFont, mSimpleEffect, texture);
            mDrawCorrelate.SizeChanged(panelViewport, service.GraphicsDevice, service, spriteBatch, spriteFont, mSimpleEffect, texture);

        }

        public void Update(Flags flags)
        {
            mDrawWave.frequency = flags.frequency[dongle_nr];
            mDrawWave.rate = flags.rate[dongle_nr];
            mDrawWave.Gain = flags.Amplification[dongle_nr];
            mDrawWave.ColorThemeNr = flags.ColorTheme;
            mDrawWave.BufferSize = flags.BufferSizeRadio[dongle_nr];
            mDrawWave.ScalePrepare(panelViewport);

            mDrawCorrelate.ColorThemeNr = flags.ColorTheme;
            mDrawCorrelate.Level = flags.CorrelateLevel;
            mDrawCorrelate.Amplitude = flags.CorrelateAmplitude;
            mDrawCorrelate.DrawPrepare(panelViewport, flags);

            mDrawRadar.rate = flags.rate[dongle_nr];
            mDrawRadar.BufferSize = flags.BufferSize;
            mDrawRadar.ColorThemeNr = flags.ColorTheme;
            mDrawRadar.DrawPrepare(panelViewport, flags);
            mDrawRadar.DeviceName = flags.DeviceName;
        }

        public void Location(float x)
        {
            mDrawWave.FrequencyAtPointedLocation = x;
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

        //Start rander the scene
        public void RenderDifference(double[] data)
        {
            if (data.Length < mDrawWave.BufferSize) return;
            if (resizing) return;

            if (this.service.GraphicsDevice != null)
                mDrawWave.OnFrameRenderDifference(panelViewport, data);

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

        //Start rander the scene
        public void RenderRadar(float[] data)
        {
            if (resizing) return;

            if (this.service.GraphicsDevice != null)
                mDrawRadar.Scene(panelViewport, data);

            try
            {
                if (service.GraphicsDevice != null)
                    service.GraphicsDevice.Present();
            }
            catch (Exception ex)
            {
                service.ResetDevice(panelViewport.Width, panelViewport.Height);
                String str = "Plot error. " + ex.ToString();
                //MessageBox.Show(str);
                //   System.Windows.Forms.Application.Exit();
            }
        }

        //Start rander the scene
        public void RenderCorrelate(float[] data, Flags flags)
        {
            if (resizing) return;

            if (this.service.GraphicsDevice != null)
                mDrawCorrelate.Scene(panelViewport, data, flags);

            try
            {
                if (service.GraphicsDevice != null)
                    service.GraphicsDevice.Present();
            }
            catch (Exception ex)
            {
                service.ResetDevice(panelViewport.Width, panelViewport.Height);
                String str = "Plot correlation error. " + ex.ToString();
                //MessageBox.Show(str);
                //   System.Windows.Forms.Application.Exit();
            }
        }
        //Start rander the scene
        public void RenderCorrelateFlow(float[] data, uint CorrelationShift)
        {
            if (resizing) return;

            if (this.service.GraphicsDevice != null)
                mDrawCorrelate.SceneFlow(panelViewport, data, CorrelationShift);

            try
            {
                if (service.GraphicsDevice != null)
                    service.GraphicsDevice.Present();
            }
            catch (Exception ex)
            {
                service.ResetDevice(panelViewport.Width, panelViewport.Height);
                String str = "Plot correlation error. " + ex.ToString();
                //MessageBox.Show(str);
                //   System.Windows.Forms.Application.Exit();
            }
        }


    }
}
