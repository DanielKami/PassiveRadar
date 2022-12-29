

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Windows.Forms;


namespace PasiveRadar
{
    public class WindowSupport
    {
        //fonts
        public ServiceContainer services;
        public BasicEffect mSimpleEffect;
        public Texture2D texture = null;
        public ContentManager content;
        public GraphicsDeviceService service;
        public SpriteBatch spriteBatch = null;
        public SpriteFont spriteFont;
        public Panel panelViewport;

        protected bool resizing = false;
        private readonly Object Lock = new Object();



        public void OnLoadWindow()
        {
            content = new ContentManager(services, "Content");
            content.RootDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            spriteBatch = new SpriteBatch(service.GraphicsDevice);
            spriteFont = content.Load<SpriteFont>("ft");
            service.ResetDevice(panelViewport.Width, panelViewport.Height);
        }

        protected void mWinForm_DeviceResetting(Object sender, EventArgs e)
        {
            if (content != null)
                content.Dispose();

            if (mSimpleEffect != null)
                mSimpleEffect.Dispose();

            if (spriteBatch != null)
                spriteBatch.Dispose();

            if (texture != null)
                texture.Dispose();

        }

        protected void DeviceReset()
        {
            if (content != null)
                content.Dispose();
            //Re-create content. Content depends on the graphics device. 
            content = new ContentManager(services, "Content");
            //Set the content rod directory
            content.RootDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            // Re-Create effect
            mSimpleEffect = new BasicEffect(service.GraphicsDevice);
            mSimpleEffect.VertexColorEnabled = true;
            mSimpleEffect.Projection = Matrix.CreateOrthographicOffCenter(0, service.GraphicsDevice.Viewport.Width,     // left, right
            service.GraphicsDevice.Viewport.Height, 0,    // bottom, top
            0, 1);

            //Recreate bath
            spriteBatch = new SpriteBatch(service.GraphicsDevice);

            //Recreate texture
            if (texture != null) texture.Dispose();
            texture = new Texture2D(service.GraphicsDevice, 1, 1, true, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[] { Color.White });// fill the texture with white
            if (texture == null) MessageBox.Show("texture null.", "Important Message");
            //Recreate spritefont
            spriteFont = content.Load<SpriteFont>("ft");

        }


        public void SizeChange()
        {
            lock (Lock)
            {
                if (service.GraphicsDevice != null)
                {
                    resizing = true;
                    service.GraphicsDevice.Dispose();
                    int Width, Height;
                    //if (panelViewport.Width < 1) Width = 1;
                    //else
                        Width = panelViewport.Width;

                    //if (panelViewport.Height < 1) Height = 1;
                    //else
                        Height = panelViewport.Height;

                    service.CreateDevice(panelViewport.Handle, Width, Height);
                    service.ResetDevice(Width, Height);

                    resizing = false;
                }
            }
        }
    }
}
