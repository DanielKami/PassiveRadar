
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SDRdue
{
    partial class Form1
    {
        protected BasicEffect mSimpleEffect2;
        private SpriteBatch spriteBatch2 = null;
        protected SpriteFont spriteFont2;
        private Texture2D texture = null;

        //fonts
        private ContentManager content2;
        private GraphicsDeviceService service2;
        private ServiceContainer services2;

        void InitWindow2()
        {
            Size2();


            service2 = new GraphicsDeviceService(this.panelViewport2.Handle, this.panelViewport2.Width, this.panelViewport2.Height);
            service2.DeviceResetting += mWinForm_DeviceResetting2;
            service2.DeviceReset += mWinForm_DeviceReset2;

            services2 = new ServiceContainer();
            services2.AddService<IGraphicsDeviceService>(service2);
            content2 = new ContentManager(services2, "Content");
        }

        void OnLoadWindow2()
        {
            content2.RootDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            spriteBatch2 = new SpriteBatch(service2.GraphicsDevice);
            spriteFont2 = content2.Load<SpriteFont>("ft");
            service2.ResetDevice(this.Width, this.Height);
        }

        void Size2()
        {
            // panelViewport.Width = (int)(this.ClientRectangle.Width);
            // panelViewport2.Height = (int)(this.ClientRectangle.Height) - 90;
        }

        void mWinForm_DeviceReset2(Object sender, EventArgs e)
        {
            content2 = new ContentManager(services2, "Content");
            //Set the content rod directory
            content2.RootDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            // Re-Create effect
            mSimpleEffect2 = new BasicEffect(service2.GraphicsDevice);
            mSimpleEffect2.VertexColorEnabled = true;
            mSimpleEffect2.Projection = Matrix.CreateOrthographicOffCenter(0, service2.GraphicsDevice.Viewport.Width,     // left, right
            service2.GraphicsDevice.Viewport.Height, 0,    // bottom, top
            0, 1);
            // Configure device

            //Recreate bath
            spriteBatch2 = new SpriteBatch(service2.GraphicsDevice);

            //Recreate texture

            //Recreate spritefont
            spriteFont2 = content2.Load<SpriteFont>("ft");

            ScaleXPrepare();
        }

        private void mWinForm_DeviceResetting2(Object sender, EventArgs e)
        {
            // Dispose all

            if (content2 != null)
                content2.Dispose();

            if (mSimpleEffect2 != null)
                mSimpleEffect2.Dispose();

            if (spriteBatch2 != null)
                spriteBatch2.Dispose();
        }

        void SizeChange2()
        {
            Size2();

            if (service2.GraphicsDevice != null)
            {
                service2.GraphicsDevice.Dispose();
                service2.CreateDevice(this.panelViewport2.Handle, this.panelViewport2.Width, this.panelViewport2.Height);
                service2.ResetDevice(this.panelViewport2.Width, this.panelViewport2.Height);
            }
        }
    }
}
