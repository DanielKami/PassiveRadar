
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace PasiveRadar
{
    class DrawRadar : Draw
    {

        private readonly Object LockRadar = new Object();
        public float rate;
        public uint BufferSize;
        public string DeviceName;

        public GraphicsDeviceService service;
        protected SpriteBatch spriteBatch = null;
        protected SpriteFont spriteFont;
        protected BasicEffect mSimpleEffect;
        protected Texture2D texture = null;
        GraphicsHelper graphisc = new GraphicsHelper();


        protected Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
        int frames = 0;
        int frames_per_sec = 0;

        float zoomX, zoomY;
        uint Rows, Columns;
        Vector2[] p;
        private int x_ticks = 22;
        private float x_step;
        float center;
        //private int zoom_half;
        private float r_b_c;
        private double frequency;
        private float sample_rate;
        private uint ColRow;
        private float doppler_zoom;
        private float step_y;
        private float Yscale_shift;
        private float Up;
        private float y_ticks = 21;
        private float scaleYfactor;
        //private float BufferRate;
        private float c = 299792458f; //Speed of light m/s
        float ActivePlotAreaX;
        float ActivePlotAreaY;
        int y_bottom;
        int x_left;
        //int Width_RightMargin;
        float DistanceShift;
        int ColorTableSize_;


        public void SizeChanged(Panel panelViewport, GraphicsDevice graphicsDevice, GraphicsDeviceService _service, SpriteBatch _spriteBatch, SpriteFont _spriteFont, BasicEffect _mSimpleEffect, Texture2D _texture)

        {
            service = _service;
            spriteBatch = _spriteBatch;
            spriteFont = _spriteFont;
            mSimpleEffect = _mSimpleEffect;
            texture = _texture;
            DrawPrepare(panelViewport);
        }


        public void DrawPrepare(Panel panelViewport, Flags flags = null)
        {
            lock (LockRadar)
            {
                if (flags != null)
                {
                    Rows = flags.Rows;
                    Columns = flags.Columns;
                    doppler_zoom = flags.DopplerZoom;
                    DistanceShift = flags.DistanceShift;
                    BufferSize = flags.BufferSize;
                    CreateColorTable1(ColorThemeNr, flags.ColorThemeTable, flags);
                    frequency = flags.frequency[0];
                    sample_rate = flags.rate[0];
                }
                x_left = LeftMargin + 15;
                y_bottom = panelViewport.Height - BottomMargin - 30;
                ActivePlotAreaX = panelViewport.Width - x_left - RightMargin;
                ActivePlotAreaY = y_bottom - TopMargin;
                zoomX = 1.0f * ActivePlotAreaX / Columns;
                zoomY = 1.0f * ActivePlotAreaY / Rows;


                graphisc.SetValues(panelViewport, spriteBatch, texture, zoomX, zoomY);
                center = x_left + (ActivePlotAreaX / 2);
                //Accelerators
                //Scale X

                x_step = ActivePlotAreaX / x_ticks;

                // This is the Doppler shift change between ticks according to Max Manning dopplerfish.com
                r_b_c = (float)(sample_rate / Columns / ActivePlotAreaX / doppler_zoom * c / frequency / 2.0f); // TODO: / doppler_zoom;
                ColRow = Columns * Rows;

                p = new Vector2[ColRow];

                uint index_i;
                float x;
                for (uint i = 0; i < Columns; i++)
                {
                    x = x_left + i * zoomX;
                    index_i = i * Rows;

                    for (uint j = 0; j < Rows; j++)
                    {
                        p[index_i + j].Y = y_bottom - (j + 1) * zoomY;
                        p[index_i + j].X = x;
                    }
                }

                ColorTableSize_ = Draw.ColorTableSize - 1;

                ////////////////////////////////////////////////////////////////


                // Scale Y
                Up = ActivePlotAreaY;
                step_y = ActivePlotAreaY / y_ticks;
                float row_to_km = c / 1000 / sample_rate;
                Yscale_shift = DistanceShift * row_to_km;

                scaleYfactor = row_to_km / zoomY;

            }

        }

        public void Scene(Panel panelViewport, float[] data)
        {
            //Calculate frames per second
            frames++;
            if (watch.ElapsedMilliseconds >= 1000L)
            {
                frames_per_sec = frames;
                frames = 0;
                watch.Restart();
            }

            service.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            int col;
            if (ColRow == data.Length)
            {
                for (uint i = 0; i < ColRow; i++)
                {
                    col = (int)(data[i]);

                    if (col > ColorTableSize_)
                        col = ColorTableSize_;
                    if (col > 0)
                        graphisc.Point(p[i], ColorTable[col]);
                }
            }

            this.spriteBatch.Draw(texture, new Rectangle(0, y_bottom, panelViewport.Width, 1), Color.White);
            string drawString;

            spriteBatch.DrawString(spriteFont, "Doppler speed (m/s)", new Vector2(panelViewport.Width / 2 - 30, panelViewport.Height - 15), Color.White, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);
            spriteBatch.DrawString(spriteFont, "Radar", new Vector2(panelViewport.Width - 50, 1), graphisc.white, 0, new Vector2(0, 0), 0.3f, SpriteEffects.None, 0);

            //Additional info frames/sec
            drawString = "" + frames_per_sec + " fps   " + Rows + "x" + Columns + "    Device: " + DeviceName;
            spriteBatch.DrawString(spriteFont, drawString, new Vector2(1 + LeftMargin, 0), graphisc.white, 0, new Vector2(0, 0), 0.3f, SpriteEffects.None, 0);


            spriteBatch.End();
            spriteBatch.Begin();
            ScaleX(panelViewport.Width, panelViewport.Height);

            ScaleY(panelViewport.Width, panelViewport.Height);
            spriteBatch.End();

        }


        private void ScaleY(float Width, float Height)
        {
            float x = 5, y;
            string drawString;
            for (float i = step_y; i < Up; i += step_y)
            {

                drawString = "" + (scaleYfactor * i + Yscale_shift).ToString("0.00");
                y = y_bottom - i;
                {
                    this.spriteBatch.Draw(texture, new Rectangle(x_left, (int)y, (int)ActivePlotAreaX, 1), Color.FromNonPremultiplied(200, 1, 1, 60));
                    spriteBatch.DrawString(spriteFont, drawString, new Vector2(x, y - 6), graphisc.white, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);
                }
            }
        }

        private void ScaleX(float Width, float Height)
        {
            string drawString;
            float x, y;
            float ScaleValue, AbsScale;
            y = y_bottom;

            for (float i = 0; i <= ActivePlotAreaX; i += x_step)
            {
                x = i + x_left;
                this.spriteBatch.Draw(texture, new Rectangle((int)x, (int)TopMargin, 1, y_bottom - (int)TopMargin), Color.FromNonPremultiplied(200, 1, 1, 60));
                ScaleValue = (x - center) * r_b_c;
                AbsScale = Math.Abs(ScaleValue);
                if (AbsScale > 10)
                    drawString = "" + ScaleValue.ToString("0"); // Doppler speed
                else if (AbsScale > 0.01f)
                    drawString = "" + ScaleValue.ToString("0.0"); // Doppler speed
                else
                    drawString = "" + ScaleValue.ToString("0.00"); // Doppler speed




                float lt = spriteFont.MeasureString(drawString).Length() / 9;
                spriteBatch.DrawString(spriteFont, drawString, new Vector2(x - lt + 3, y + 10), Color.White, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);
            }
        }
    }
}
