
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows.Forms;

namespace PasiveRadar
{
    class DrawFlow : Draw
    {

        SpriteBatch spriteBatch = null;
        Texture2D texture = null;
        GraphicsHelper graphisc = new GraphicsHelper();
        public GraphicsDeviceService service;
        BasicEffect mSimpleEffect;
        SpriteFont spriteFont;
        GraphicsDevice graphicsDevice;
        RenderTarget2D renderTarget3, renderTarget4;
        Texture2D shadowMap3;
        Texture2D shadowMap4;

        public double frequency;
        public double rate;
        public float Gain;
        public uint BufferSize;
        public int Level;
        public float FrequencyAtPointedLocation;

        private float ActivePlotAreaX;
        private float ActivePlotAreaY;
        private int HeightBootom;
        private int WithRight;
        private float data_frame;
        private float MHz_perPixel;
        private float ScaleX_round;
        private float PixelsXToStart;
        private float stepX;
        private  float ScaleStepX;
        private float midle;
        private double PointedFrequency;
        private int Up;
        private int st;
        private float ScaleX_delta;
        private float HalfHight;
        private float ft;
        private float ColHeight;
        private float step_y;
        private float scale_step;
        private Color[] ColorScale;
        private Vector2[] ColorScaleV;
        public void ScaleXPrepare(Panel panelViewport, Color[] UserColorTable)
        {

            HalfHight = (panelViewport.Height - BottomMargin - TopMargin) / 2;
            ActivePlotAreaX = panelViewport.Width - LeftMargin - RightMargin;
            ActivePlotAreaY = panelViewport.Height - TopMargin - BottomMargin;

            if (ActivePlotAreaX < 1) ActivePlotAreaX = 1;
            if (ActivePlotAreaY < 1) ActivePlotAreaY = 1;

            HeightBootom = panelViewport.Height - BottomMargin;
            WithRight = panelViewport.Width - RightMargin;
            CreateColorTable1(ColorThemeNr, UserColorTable, null);

            //Find how menny MHz is one pixel
            MHz_perPixel = (float)(ActivePlotAreaX / (rate / 1000000));

            //Round frequency to nearest digit after coma
            ScaleX_round = 1.0f * (int)(frequency / 10000000) * 10;
            if (ScaleX_round < 100) ScaleX_round += 10;
            ScaleX_delta = (float)(frequency / 1000000 - ScaleX_round);

            //Number of pixels to shift
            PixelsXToStart = ActivePlotAreaX / 2 - ScaleX_delta * MHz_perPixel + LeftMargin;
            stepX = (float)Math.Round(100.0 / MHz_perPixel, 1);
            ScaleStepX = stepX * MHz_perPixel;
            midle = LeftMargin + ft * BufferSize / 2;
            ///////////////////////////////////////////////////////////////////////////////////////////

            ColHeight = 1.0f * (ActivePlotAreaY) / 100;
            ft = 1.0f * ActivePlotAreaX / BufferSize;
            PointedFrequency = (frequency - rate * 0.5) / 1000000;

            Up = (int)(1000 / (Gain + 1));
            st = Up / 15;
            if (st < 1) st = 1;
            step_y = 1.0f * HeightBootom / Up;

            data_frame = (float)(1.0 * BufferSize / ActivePlotAreaX);

            scale_step = Draw.ColorTableSize / ActivePlotAreaY;

            ColorScale = new Color[(int)ActivePlotAreaY];
            ColorScaleV = new Vector2[(int)ActivePlotAreaY];

            for (int j = 0; j < (int)ActivePlotAreaY; j++)
            {
                ColorScaleV[j].X = ActivePlotAreaX + LeftMargin + 2;
                ColorScaleV[j].Y = HeightBootom - j;
                ColorScale[j] = ColorTable[(int)(j * scale_step)];
            }

        }


        public void SizeChanged(Panel panelViewport, GraphicsDevice _graphicsDevice, GraphicsDeviceService _service, SpriteBatch _spriteBatch, SpriteFont _spriteFont, BasicEffect _mSimpleEffect, Texture2D _texture)
        {

            service = _service;
            spriteBatch = _spriteBatch;
            spriteFont = _spriteFont;
            mSimpleEffect = _mSimpleEffect;
            texture = _texture;
            graphicsDevice = _graphicsDevice;
            graphisc.SetValues(panelViewport, spriteBatch, texture, 1, 1);

            ScaleXPrepare(panelViewport, CustomTable);

            shadowMap3 = null;
            shadowMap4 = null;

            //Protect ageinst to large bitmap size
            int Width = panelViewport.Width;
            if (Width >= 2048) Width = 2048;
            renderTarget3 = new RenderTarget2D(graphicsDevice, Width, (int)panelViewport.Height, false, graphicsDevice.DisplayMode.Format, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
            renderTarget4 = new RenderTarget2D(graphicsDevice, Width, (int)panelViewport.Height, false, graphicsDevice.DisplayMode.Format, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
        }

        public void Scene(Panel panelViewport, double[] data)
        {
            int Max_col = 0;
            int col = 0;
            Vector2 p;
            int step;

            //Insert the old screen photo
            graphicsDevice.SetRenderTarget(renderTarget3);
            graphicsDevice.Clear(Color.Black);

            //Insert the old screen photo
            if (shadowMap4 != null)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(shadowMap4, new Vector2(0, 1), null, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1);
                spriteBatch.End();
            }

            spriteBatch.Begin();

            p.Y = 1;

            if (data.Length > BufferSize)
            {
                for (int j = 0; j < ActivePlotAreaX; j++)
                {
                    //Find the max in window
                    step = (int)(j * data_frame);
                    Max_col = -100000;
                    for (int i = 0; i < data_frame; i++)
                    {
                        col = (int)(data[i + step]);
                        if (col > Max_col) Max_col = col;
                    }
                    p.X = j + LeftMargin;
                    Max_col *= 18;
                    Max_col += Level + 1000;

                    if (Max_col < 0) Max_col = 0;
                    if (Max_col > Draw.ColorTableSize-1) Max_col = Draw.ColorTableSize-1;

                    graphisc.Point(p, ColorTable[Max_col]);
                }
            }

            spriteBatch.End();

            Color BackC = Color.FromNonPremultiplied(new Vector4(0.0f, 0.0f, 0, .05f));
            graphisc.FiledRectangle(service, mSimpleEffect, new Vector2(0, HeightBootom - 10), panelViewport.Width, BottomMargin, BackC, BackC);


            //copy the shadowMap1 to shadowMap2 long way
            graphicsDevice.SetRenderTarget(null);
            shadowMap3 = (Texture2D)renderTarget3;
            graphicsDevice.SetRenderTarget(renderTarget4);
            graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            if (shadowMap3 != null)
                spriteBatch.Draw(shadowMap3, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1);
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
            shadowMap4 = (Texture2D)renderTarget4;
            spriteBatch.Begin();
            if (shadowMap4 != null)
                spriteBatch.Draw(shadowMap4, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1);
            spriteBatch.End();
            spriteBatch.Begin();
            ScaleXflow(panelViewport.Width, panelViewport.Height);
            ScaleYflow(panelViewport.Width, panelViewport.Height);
            spriteBatch.End();
        }


        public void ScaleXflow(float Width, float Height)
        {
            float x, y;
            string drawString;

            for (int i = -120; i < 120; i++)
            {

                x = (float)(PixelsXToStart + ScaleStepX * i) ;
                if (x > LeftMargin && x < WithRight)
                {
                    drawString = "" + (ScaleX_round + i * stepX).ToString("0.00");
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, 1), new Vector2(x, HeightBootom), graphisc.gray);
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, HeightBootom), new Vector2(x, HeightBootom + 5), graphisc.white);
                    x -= 15;
                    y = HeightBootom + 7;
                    spriteBatch.DrawString(spriteFont, drawString, new Vector2(x, y), graphisc.white, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);
                }
            }

            graphisc.Line(service, mSimpleEffect, new Vector2(LeftMargin, HeightBootom), new Vector2(WithRight, HeightBootom), graphisc.white);


            float CursorPos = (float)((FrequencyAtPointedLocation - PointedFrequency) * MHz_perPixel + LeftMargin);
            if (CursorPos >= LeftMargin && CursorPos <= WithRight)
            {
                graphisc.Line(service, mSimpleEffect, new Vector2(CursorPos, 1), new Vector2(CursorPos, HeightBootom), Color.Gray);
            }
        }

        public void ScaleYflow(float Width, float Height)
        {
            for (int j = 0; j < (int)ActivePlotAreaY; j++)
            {
                graphisc.Pixel(ColorScaleV[j], ColorScale[j]);
            }
        }

    }
}
