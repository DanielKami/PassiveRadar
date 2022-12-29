using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using System.Diagnostics;

namespace SDRdue
{
    public class DrawWave : Draw
    {

        public double frequency;
        public double rate;
        public int Level;
        public float Gain;
        public int BufferSize;
        public float FrequencyAtPointedLocation;


        private GraphicsDeviceService service;
        private SpriteBatch spriteBatch = null;
        private SpriteFont spriteFont;
        private BasicEffect mSimpleEffect;

        private GraphicsHelper graphisc = new GraphicsHelper();
        
        private float MHz_perPixel;
        private float ScaleX_round;
        private float ScaleX_delta;
        private float PixelsXToStart;
        private float stepX;
        float HalfHight;
        float ActivePlotAreaX;
        private float ColHeight;
        private float ft;
        private float midle;
        private double PointedFrequency;
        private float step_y;
        private int Up;
        private int st;
        

        public void OnFrameRender(Panel panelViewport, double[] data)
        {
            Vector2 Point1, Point2;
            Color col = new Color(250, 250, 250);
            Color black = new Color(0, 0, 0, 200);
            service.GraphicsDevice.Clear(Color.Black);

            float Width;
            float Height;


            Width = panelViewport.Width;
            Height = panelViewport.Height;


            Point1.X = LeftMargin + 1;
            Point1.Y = TopMargin;

            graphisc.FiledRectangle(service, mSimpleEffect, Point1, panelViewport.Width - LeftMargin - RightMargin - 2, panelViewport.Height - BottomMargin - TopMargin - 1, new Color(210, 250, 255, 2), new Color(0, 0, 70, 0));


            Point2.X = LeftMargin;
            Point2.Y = TopMargin - (float)data[0] * ColHeight;
            float corr = Gain / 16f - 2.5f;
            for (int j = 1; j < BufferSize; j++)
            {
                Point1.X = LeftMargin + ft * j;
                Point1.Y = TopMargin - (float)(data[j] + corr) * ColHeight;

                graphisc.FiledRectangle(service, mSimpleEffect, Point2.X, TopMargin, Point1.X, Point1.Y, Point2.Y, black, black);
                graphisc.Line(service, mSimpleEffect, Point1, Point2, col);

                Point2 = Point1;
            }

            Point1.X = LeftMargin;
            Point1.Y = panelViewport.Height - BottomMargin;
            graphisc.FiledRectangle(service, mSimpleEffect, Point1, panelViewport.Width - LeftMargin - RightMargin, BottomMargin, new Color(0, 0, 0, 220), new Color(0, 0, 0, 220));

            spriteBatch.Begin();
            ScaleX(panelViewport.Width, panelViewport.Height);
            ScaleY(panelViewport.Width, panelViewport.Height);
            spriteBatch.End();

        }

        public void ScaleXPrepare(Panel panelViewport)
        {


            HalfHight = (panelViewport.Height - BottomMargin - TopMargin) / 2;
            ActivePlotAreaX = panelViewport.Width - LeftMargin - RightMargin;

            //Find how menny MHz is one pixel
            MHz_perPixel = (float)(ActivePlotAreaX / (rate / 1000000));

            //Round frequency to nearest digit after coma
            ScaleX_round = 1.0f * (int)(frequency / 10000000) * 10;
            if (ScaleX_round < 100) ScaleX_round += 10;
            ScaleX_delta = (float)(frequency / 1000000 - ScaleX_round);

            //Number of pixels to shift
            PixelsXToStart = (panelViewport.Width - LeftMargin - RightMargin) / 2 - ScaleX_delta * MHz_perPixel;
            stepX = (float)Math.Round(100.0 / MHz_perPixel, 1);

            midle = LeftMargin + ft * BufferSize / 2;
            ///////////////////////////////////////////////////////////////////////////////////////////

            ColHeight = 1.0f * (panelViewport.Height - BottomMargin - TopMargin) / 100;
            ft = 1.0f * ActivePlotAreaX / BufferSize;
            PointedFrequency = (frequency - rate * 0.5) / 1000000;

            Up = (int)(1000 / (Gain + 1));
            st = Up / 15;
            if (st < 1) st = 1;
            step_y = (panelViewport.Height - BottomMargin) / Up;         
        }


        public void SizeChanged(Panel panelViewport, GraphicsDevice graphicsDevice, GraphicsDeviceService _service, SpriteBatch _spriteBatch, SpriteFont _spriteFont, BasicEffect _mSimpleEffect)
        {
            service = _service;
            spriteBatch = _spriteBatch;
            spriteFont = _spriteFont;
            mSimpleEffect = _mSimpleEffect;           
         }

        public void ScaleX(float Width, float Height)
        {
            float x, y;
            string drawString;
            float temp = stepX * MHz_perPixel;
            float Height_BottomMargin = Height - BottomMargin;

            for (int i = -20; i < 20; i++)
            {
                drawString = "" + (ScaleX_round + i * stepX).ToString("0.00");
                x = (float)(PixelsXToStart + temp * i) + LeftMargin;
                if (x > LeftMargin && x < Width - RightMargin)
                {
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, TopMargin), new Vector2(x, Height_BottomMargin), graphisc.gray);
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, Height_BottomMargin), new Vector2(x, Height_BottomMargin + 5), graphisc.white);
                    x -= 15;
                    y = Height - BottomMargin + 7;
                    spriteBatch.DrawString(spriteFont, drawString, new Vector2(x, y), graphisc.white, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);
                }
            }

            graphisc.Line(service, mSimpleEffect, new Vector2(LeftMargin, Height_BottomMargin), new Vector2(Width - RightMargin, Height_BottomMargin), graphisc.white);

            //midle
            graphisc.Line(service, mSimpleEffect, new Vector2(midle, TopMargin), new Vector2(midle, Height_BottomMargin), Color.Red);

            float CursorPos = (float)((FrequencyAtPointedLocation - PointedFrequency) * MHz_perPixel + LeftMargin);
            if (CursorPos >= LeftMargin && CursorPos <= Width - RightMargin)
            {
                drawString = "" + FrequencyAtPointedLocation.ToString("0.0000") + " MHz    ";
                spriteBatch.DrawString(spriteFont, drawString, new Vector2(10 + LeftMargin, 0), graphisc.white, 0, new Vector2(0, 0), 0.3f, SpriteEffects.None, 0);
                graphisc.Line(service, mSimpleEffect, new Vector2(CursorPos, TopMargin), new Vector2(CursorPos, Height_BottomMargin), Color.Yellow);
            }
        }

        private void ScaleY(float Width, float Height)
        {
            float x = LeftMargin, y;
            string drawString;

            float temp;
            for (int i = 0; i < Up; i += st)
            {
                temp = step_y * i;
                
                drawString = "" + temp.ToString("0");
                y = temp + TopMargin;
                if (y < Height - BottomMargin)
                {
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, y), new Vector2(Width - RightMargin, y), graphisc.gray);
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, y), new Vector2(x - 5, y), graphisc.white);
                    float lt = spriteFont.MeasureString(drawString).Length() / 2;
                    spriteBatch.DrawString(spriteFont, drawString, new Vector2(x + 17 - lt, y - 6), graphisc.white, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);
                }
            }
            graphisc.Line(service, mSimpleEffect, new Vector2(LeftMargin, TopMargin), new Vector2(LeftMargin, Height - BottomMargin), graphisc.white);
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void OnFrameRenderDifference(Panel panelViewport, double[] data)
        {
            Vector2 Point1, Point2;
            Color col = new Color(250, 250, 250);
            Color black = new Color(0, 0, 0, 200);
            service.GraphicsDevice.Clear(Color.Black);

            float Width = panelViewport.Width;
            float Height = panelViewport.Height;

            Point1.X = LeftMargin + 1;
            Point1.Y = TopMargin;

            graphisc.FiledRectangle(service, mSimpleEffect, Point1, ActivePlotAreaX - 2, HalfHight, new Color(60, 0, 0, 1), new Color(250, 190, 190, 9));
            Point1.Y = TopMargin + HalfHight;
            graphisc.FiledRectangle(service, mSimpleEffect, Point1, ActivePlotAreaX - 2, HalfHight, new Color(190, 190, 230, 1), new Color(0, 0, 60, 0));

            float corr = Gain / 16f - 2.5f;
            float HTB = TopMargin + HalfHight;
            Point2.X = LeftMargin + 1;
            Point2.Y = HTB - (float)(data[0] + corr) * ColHeight;

            for (int j = 1; j < BufferSize; j++)
            {
                Point1.X = LeftMargin + ft * j;
                Point1.Y = HTB - (float)(data[j] + corr) * ColHeight;

                graphisc.FiledRectangle(service, mSimpleEffect, Point2.X, TopMargin, Point1.X, Point1.Y, Point2.Y, black, black);
                graphisc.Line(service, mSimpleEffect, Point2, Point1, col);

                Point2 = Point1;
            }

            Point1.X = LeftMargin;
            Point1.Y = Height - BottomMargin;
            graphisc.FiledRectangle(service, mSimpleEffect, Point1, ActivePlotAreaX, BottomMargin, new Color(0, 0, 0, 220), new Color(0, 0, 0, 220));

            spriteBatch.Begin();
            ScaleX(Width, Height);
            ScaleYDif(Width, Height);
            spriteBatch.End();

        }

        private void ScaleYDif(float Width, float Height)
        {
            float x = LeftMargin, y;
            string drawString;

            float temp;
            for (int i = -Up; i < Up; i += st)
            {
                temp = step_y * i;
                drawString = "" + temp.ToString("0");
                y = temp + TopMargin + HalfHight;
                if (y < Height - BottomMargin && y > TopMargin)
                {
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, y), new Vector2(Width - RightMargin, y), graphisc.gray);
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, y), new Vector2(x - 5, y), graphisc.white);
                    float lt = spriteFont.MeasureString(drawString).Length() / 2;
                    spriteBatch.DrawString(spriteFont, drawString, new Vector2(x + 17 - lt, y - 6), graphisc.white, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);
                }
            }
            graphisc.Line(service, mSimpleEffect, new Vector2(LeftMargin, TopMargin), new Vector2(LeftMargin, Height - BottomMargin), graphisc.white);
        }


 
    }
}
