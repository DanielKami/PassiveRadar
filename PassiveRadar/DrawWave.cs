using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows.Forms;

namespace PasiveRadar
{
    public class DrawWave : Draw
    {

        public double frequency;
        public double rate;
        public int Level;
        public float Gain;
        public uint BufferSize;
        public float FrequencyAtPointedLocation;
        public float FilterLoLocation;
        public float FilterUpLocation;

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
        private float ScaleStepX;
        private float HalfHight;
        private float ActivePlotAreaX;
        private float ActivePlotAreaY;
        private float ColHeight;
        private float Height_BottomMargin;
        private float Width_RightMargin;
        private float ft;
        private float midle;
        private double PointedFrequency;
        private float step_y;
        private float Up;
        private float st;
        private float data_frame;
        private float corr;
        private float scaleYfactor;

        public void Scene(Panel panelViewport, double[] data, int radio_nr)
        {
            Vector2 Point1, Point2;
            Color col = new Color(250, 250, 250);
            Color black = new Color(0, 0, 0, 200);
            service.GraphicsDevice.Clear(Color.Black);


            Point1.X = LeftMargin + 1;
            Point1.Y = TopMargin;

            graphisc.FiledRectangle(service, mSimpleEffect, Point1, ActivePlotAreaX - 2, ActivePlotAreaY - 1, new Color(210, 250, 255, 2), new Color(0, 0, 70, 0));

            Point2.X = LeftMargin;
            Point2.Y = TopMargin - (float)data[0] * ColHeight;

            int step;
            float tmp_y;
            float Max_y = 0;

            for (int i = 0; i < data_frame; i++)
            {
                tmp_y = (float)(data[i]);
                if (Math.Abs(tmp_y) > Math.Abs(Max_y)) Max_y = tmp_y;
            }
            Point2.X = LeftMargin + 0;
            Point2.Y = TopMargin - (Max_y + corr) * ColHeight;

            //////////////////////////////////////////////////////////////////////////////////
            for (uint j = 1; j < ActivePlotAreaX; j++)
            {
                //Find the max in window
                step = (int)(j * data_frame);
                Max_y = 0;
                for (int i = 0; i < data_frame; i++)
                {
                    tmp_y = (float)(data[i + step]);
                    if (Math.Abs(tmp_y) > Math.Abs(Max_y)) Max_y = tmp_y;
                }
                Point1.X = LeftMargin + j;
                Point1.Y = TopMargin - (Max_y + corr) * ColHeight;

                graphisc.FiledRectangle(service, mSimpleEffect, Point2.X, TopMargin, Point1.X, Point1.Y, Point2.Y, black, black);
                graphisc.Line(service, mSimpleEffect, Point1, Point2, col);

                Point2 = Point1;
            }
            ////////////////////////////////////////////////////////////////////////////////////
            Point1.X = LeftMargin;
            Point1.Y = Height_BottomMargin;
            graphisc.FiledRectangle(service, mSimpleEffect, Point1, ActivePlotAreaX, BottomMargin, new Color(0, 0, 0, 220), new Color(0, 0, 0, 220));

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, (rate/1000000).ToString("0.00") + " MSPS  Radio " + radio_nr, new Vector2(panelViewport.Width - 130, 1), graphisc.white, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);
            ScaleX(panelViewport.Width, panelViewport.Height);
            ScaleY(panelViewport.Width, panelViewport.Height);
            spriteBatch.End();
        }

        public void ScalePrepare(Panel panelViewport)
        {            
            ActivePlotAreaX = panelViewport.Width - LeftMargin - RightMargin;
            ActivePlotAreaY = panelViewport.Height - BottomMargin - TopMargin;
            HalfHight = ActivePlotAreaY / 2;
            if (ActivePlotAreaX == 0) ActivePlotAreaX = 1;
            if (ActivePlotAreaY == 0) ActivePlotAreaY = 1;
            Height_BottomMargin = panelViewport.Height - BottomMargin;
            Width_RightMargin = panelViewport.Width -RightMargin;
            //Find how menny MHz is one pixel
            MHz_perPixel = (float)(ActivePlotAreaX / (rate / 1000000));

            //Round frequency to nearest digit after coma
            ScaleX_round = 1.0f * (int)(frequency / 10000000) * 10;
            if (ScaleX_round < 100) ScaleX_round += 10;
            ScaleX_delta = (float)(frequency / 1000000 - ScaleX_round);

            //Number of pixels to shift
            PixelsXToStart = (ActivePlotAreaX) / 2 - ScaleX_delta * MHz_perPixel + LeftMargin;
            stepX = (float)Math.Round(100.0 / MHz_perPixel, 1);
            ScaleStepX = stepX * MHz_perPixel;

            midle = LeftMargin + ft * BufferSize / 2;
            ///////////////////////////////////////////////////////////////////////////////////////////

            ColHeight = 1.0f * (ActivePlotAreaY) / 100;
            ft = 1.0f * ActivePlotAreaX / BufferSize;
            PointedFrequency = (frequency - rate * 0.5) / 1000000;

            int PointOnScaleY = (int)(ActivePlotAreaY / 20);
            Up = 1000 / (Gain + 1);
            st = Up / (PointOnScaleY);
            //if (st < 1) st = 1;
            step_y = 1.0f * ActivePlotAreaY / Up;

            if (Gain == 0) Gain = 0.001f;
            scaleYfactor = 1000.0f / ActivePlotAreaY / Gain*2;

            data_frame = (float)(1.0 * BufferSize / ActivePlotAreaX);
            corr = Gain / 16f - 2.5f;

            //////////////////////////////////////////////////////////////////////////////////////////
        }


        public void SizeChanged(Panel panelViewport, GraphicsDevice graphicsDevice, GraphicsDeviceService _service, SpriteBatch _spriteBatch, SpriteFont _spriteFont, BasicEffect _mSimpleEffect)
        {
            service = _service;
            spriteBatch = _spriteBatch;
            spriteFont = _spriteFont;
            mSimpleEffect = _mSimpleEffect;

            ScalePrepare(panelViewport);
         }

        public void ScaleX(float Width, float Height)
        {
            float x, y;
            string drawString;

            for (int i = -100; i < 120; i++)
            {             
                x = (float)(PixelsXToStart + ScaleStepX * i);
                if (x > LeftMargin && x < Width - RightMargin)
                {
                    drawString = "" + (ScaleX_round + i * stepX).ToString("0.00");
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, TopMargin), new Vector2(x, Height_BottomMargin), graphisc.gray);
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, Height_BottomMargin), new Vector2(x, Height_BottomMargin + 5), graphisc.white);
                    x -= 15;
                    y = Height_BottomMargin + 7;
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
                graphisc.Line(service, mSimpleEffect, new Vector2(CursorPos, TopMargin), new Vector2(CursorPos, Height_BottomMargin), Color.Gray);
            }

            //Filter bondaries
            float LoPos = (float)((FilterLoLocation ) * MHz_perPixel + LeftMargin);
            float UpPos = (float)((FilterUpLocation ) * MHz_perPixel + LeftMargin);
            float mid = (LoPos + UpPos) / 2;

            if (LoPos >= LeftMargin && LoPos <= Width_RightMargin)
            {
                graphisc.Line(service, mSimpleEffect, new Vector2(LoPos, TopMargin), new Vector2(LoPos, Height_BottomMargin), Color.Gray);
            }
            if (UpPos >= LeftMargin && UpPos <= Width_RightMargin)
            {
                graphisc.Line(service, mSimpleEffect, new Vector2(UpPos, TopMargin), new Vector2(UpPos, Height_BottomMargin), Color.Gray);
            }
            if (LoPos < LeftMargin) LoPos = LeftMargin;
            if (UpPos < LeftMargin) UpPos = LeftMargin;
            if (LoPos > Width_RightMargin) LoPos = Width_RightMargin;
            if (UpPos > Width_RightMargin) UpPos = Width_RightMargin;
            graphisc.FiledRectangle(service, mSimpleEffect, new Vector2(LoPos, TopMargin), UpPos- LoPos, ActivePlotAreaY - 1, new Color(50, 50, 50, 2), new Color(50, 50, 50, 2));
            
            graphisc.Line(service, mSimpleEffect, new Vector2(mid, TopMargin), new Vector2(mid, Height_BottomMargin), new Color(150, 0, 0, 2));

        }

        private void ScaleY(float Width, float Height)
        {
            float x = LeftMargin, y;
            string drawString;
            float temp;
            for (float i = 0; i < Up; i += st)
            {
                temp = step_y  * i;
               
                drawString = "" + (temp*scaleYfactor).ToString("0");
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

            int step;
            float tmp_y;
            float Max_y=0;           
            float HTB = TopMargin + HalfHight;

            for (int i = 0; i < data_frame; i++)
            {
                tmp_y = (float)(data[i]);
                if (Math.Abs(tmp_y) > Math.Abs(Max_y)) Max_y = tmp_y;
            }
            Point2.X = LeftMargin + 0;
            Point2.Y = HTB - (Max_y + corr) * ColHeight;

            //////////////////////////////////////////////////////////////////////////////////
            for (uint j = 1; j < ActivePlotAreaX; j++)
            {
                //Find the max in window
                step = (int)(j * data_frame);
                Max_y = 0;
                for (int i = 1; i < data_frame; i++)
                {
                    tmp_y = (float)(data[i + step]);
                    if (Math.Abs(tmp_y) > Math.Abs(Max_y)) Max_y = tmp_y;
                }
                Point1.X = LeftMargin + j;
                Point1.Y = HTB - (Max_y + corr) * ColHeight;

                graphisc.FiledRectangle(service, mSimpleEffect, Point2.X, TopMargin, Point1.X, Point1.Y, Point2.Y, black, black);
                graphisc.Line(service, mSimpleEffect, Point2, Point1, col);

                Point2 = Point1;
            }
            ////////////////////////////////////////////////////////////////////////////////////

            Point1.X = LeftMargin;
            Point1.Y = Height - BottomMargin;
            graphisc.FiledRectangle(service, mSimpleEffect, Point1, ActivePlotAreaX, BottomMargin, new Color(0, 0, 0, 220), new Color(0, 0, 0, 220));

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, "Difference", new Vector2(panelViewport.Width - 60, 1), graphisc.white, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);
            ScaleX(Width, Height);
            ScaleYDif(Width, Height);
            spriteBatch.End();

        }

        private void ScaleYDif(float Width, float Height)
        {
            float x = LeftMargin, y;
            string drawString;

            float temp;
            for (float i = -Up; i < Up; i += st)
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
