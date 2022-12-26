using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows.Forms;

namespace PasiveRadar
{
    class DrawCorrelate : Draw
    {

        private GraphicsDeviceService service;
        private SpriteBatch spriteBatch = null;
        private SpriteFont spriteFont;
        private BasicEffect mSimpleEffect;
        GraphicsDevice graphicsDevice;
        private GraphicsHelper graphisc = new GraphicsHelper();

        RenderTarget2D renderTarget3, renderTarget4;
        Texture2D shadowMap3;
        Texture2D shadowMap4;
        Texture2D texture = null;
        public int Level;
        public int Amplitude;

        private float PixelsXToStart;
        private float stepX;
        float HalfHight;
        float ActivePlotAreaX;
        float ActivePlotAreaY;

        private float step_y;
        private int Up;
        private uint st;
        private float data_frame;
        float Height_BottomMargin;
        int Width_RightMargin;
        uint Positive;
        uint Negative;
        uint NegPos;
        int ScaleDelta;
        int DrawResynchronisationCounter = 0;
        bool startShow;

        public void Scene(Panel panelViewport, float[] data, Flags flags)
        {


            Vector2 Point1, Point2;
            Color col = new Color(250, 250, 250);
            Color black = new Color(0, 0, 0, 200);
            service.GraphicsDevice.Clear(Color.Black);

            Point1.X = LeftMargin + 1;
            Point1.Y = TopMargin;

            graphisc.FiledRectangle(service, mSimpleEffect, Point1, ActivePlotAreaX - 2, ActivePlotAreaY - 1, new Color(210, 250, 255, 2), new Color(0, 0, 70, 0));
            //////////////////////////////////////////////////////////////////////////////////
            int step;
            float tmp_y;
            float Max_y=0;

            for (uint i = 0; i < data_frame; i++)
            {
                tmp_y = (float)(data[i]);
                if (Math.Abs(tmp_y) > Math.Abs(Max_y)) Max_y = tmp_y;
            }

            Point2.X = LeftMargin;
            Point2.Y = Height_BottomMargin - Max_y * ActivePlotAreaY;

            for (uint j = 1; j < ActivePlotAreaX; j++)
            {
                //Find the max in window
                step = (int)(j * data_frame);
                Max_y = 0;
                for (uint i = 0; i < data_frame; i++)
                {
                    tmp_y = (float)(data[i + step]);
                    if (Math.Abs(tmp_y) > Math.Abs(Max_y)) Max_y = tmp_y;
                }
                Point1.X = LeftMargin + j;
                Point1.Y = Height_BottomMargin - Max_y * ActivePlotAreaY;

                graphisc.FiledRectangle(service, mSimpleEffect, Point2.X, TopMargin, Point1.X, Point1.Y, Point2.Y, black, black);
                graphisc.Line(service, mSimpleEffect, Point1, Point2, col);

                Point2 = Point1;
            }
            ////////////////////////////////////////////////////////////////////////////////////

            Point1.X = LeftMargin;
            Point1.Y = Height_BottomMargin;
            graphisc.FiledRectangle(service, mSimpleEffect, Point1, ActivePlotAreaX, BottomMargin, new Color(0, 0, 0, 220), new Color(0, 0, 0, 220));

            spriteBatch.Begin();
            
            DrawResynchronisation(flags.Resynchronisation);
            if(flags.AutoCorrelate)
                spriteBatch.DrawString(spriteFont, "Auto", new Vector2(1, 1), graphisc.white, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);

            spriteBatch.DrawString(spriteFont, "Correlation", new Vector2(panelViewport.Width - 60, 1), graphisc.white, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);

            float PosLevel = (float)(Height_BottomMargin - ActivePlotAreaY * flags.AcceptedLevel);
            graphisc.Line(service, mSimpleEffect, new Vector2(LeftMargin, PosLevel), new Vector2(ActivePlotAreaX + LeftMargin, PosLevel), Color.Red);

            ScaleX(panelViewport.Width, panelViewport.Height);
            ScaleY(panelViewport.Width, panelViewport.Height);
            spriteBatch.End();

        }

        private void DrawResynchronisation(bool ReSync)
        {
            if (ReSync)
                startShow = true;

            if(startShow)
            {
                Color col = new Color((255 - DrawResynchronisationCounter*10), 0, 0);
                spriteBatch.DrawString(spriteFont, "Resynchronization", new Vector2(LeftMargin+20, 1), col, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);
 
                DrawResynchronisationCounter++;
                if (DrawResynchronisationCounter > 25)
                {
                    DrawResynchronisationCounter = 0;
                    startShow = false;
                }
            }

        }

        public void DrawPrepare(Panel panelViewport, Flags flags=null)
        {
            if (flags != null)
            {
                Positive = flags.Positive;
                Negative = flags.Negative;

                CreateColorTable1(ColorThemeNr, flags.ColorThemeTable);
            }
            
            NegPos = Positive + Negative;
            HalfHight = (panelViewport.Height - BottomMargin - TopMargin) / 2;
            ActivePlotAreaX = panelViewport.Width - LeftMargin - RightMargin;
            ActivePlotAreaY = panelViewport.Height - BottomMargin - TopMargin;
            Height_BottomMargin = panelViewport.Height - BottomMargin;
            Width_RightMargin = panelViewport.Width - RightMargin;
            if (ActivePlotAreaX == 0) ActivePlotAreaX = 1;
            if (ActivePlotAreaY == 0) ActivePlotAreaY = 1;

            stepX = ActivePlotAreaX / NegPos;
            data_frame = 1.0f / stepX;
            PixelsXToStart = stepX * Negative;
            ScaleDelta = (int)(30 / stepX);//Width of the text on scale
            ///////////////////////////////////////////////////////////////////////////////////////////

            int PointOnScaleY = (int)(ActivePlotAreaY / 20);
            Up = 10;
            st = (uint)(Up / (PointOnScaleY + 0.1));
            step_y = ActivePlotAreaY / Up;
        }


        public void SizeChanged(Panel panelViewport, GraphicsDevice _graphicsDevice, GraphicsDeviceService _service, SpriteBatch _spriteBatch, SpriteFont _spriteFont, BasicEffect _mSimpleEffect, Texture2D _texture)
        {
            service = _service;
            spriteBatch = _spriteBatch;
            spriteFont = _spriteFont;
            mSimpleEffect = _mSimpleEffect;
            texture = _texture;
            graphicsDevice = _graphicsDevice;
            DrawPrepare(panelViewport);
            graphisc.SetValues(panelViewport, spriteBatch, texture, 1, 1);



            shadowMap3 = null;
            shadowMap4 = null;

            renderTarget3 = new RenderTarget2D(graphicsDevice, (int)panelViewport.Width, (int)panelViewport.Height, false, graphicsDevice.DisplayMode.Format, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
            renderTarget4 = new RenderTarget2D(graphicsDevice, (int)panelViewport.Width, (int)panelViewport.Height, false, graphicsDevice.DisplayMode.Format, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);

        }

        public void ScaleX(float Width, float Height)
        {
            float x, y;
            string drawString;


            //Positive direction           
            for (uint i = 0; i <= Positive; i += (uint)ScaleDelta)
            {
                drawString = "" + i.ToString("0");
                x = (float)(PixelsXToStart + stepX * i) + LeftMargin;
                if (x > LeftMargin && x < Width_RightMargin)
                {
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, TopMargin), new Vector2(x, Height_BottomMargin), graphisc.gray);
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, Height_BottomMargin), new Vector2(x, Height_BottomMargin + 5), graphisc.white);
                    float lt = spriteFont.MeasureString(drawString).Length() / 9;
                    x -= lt;
                    y = Height_BottomMargin + 7;
                    spriteBatch.DrawString(spriteFont, drawString, new Vector2(x, y), graphisc.white, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);
                }
            }

            //Negative
            for (int i = -ScaleDelta; i >= -Negative; i -= ScaleDelta)
            {
                drawString = "" + i.ToString("0");
                x = (float)(PixelsXToStart + stepX * i) + LeftMargin;
                if (x > LeftMargin && x < Width_RightMargin)
                {
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, TopMargin), new Vector2(x, Height_BottomMargin), graphisc.gray);
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, Height_BottomMargin), new Vector2(x, Height_BottomMargin + 5), graphisc.white);
                    float lt = spriteFont.MeasureString(drawString).Length() / 9;
                    x -= lt + 3;
                    y = Height_BottomMargin + 7;
                    spriteBatch.DrawString(spriteFont, drawString, new Vector2(x, y), graphisc.white, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);
                }
            }

            graphisc.Line(service, mSimpleEffect, new Vector2(LeftMargin, Height_BottomMargin), new Vector2(Width_RightMargin, Height_BottomMargin), graphisc.white);

            //zero
            graphisc.Line(service, mSimpleEffect, new Vector2(PixelsXToStart + LeftMargin, TopMargin), new Vector2(PixelsXToStart + LeftMargin, Height_BottomMargin), Color.Red);

        }

        private void ScaleY(float Width, float Height)
        {
            float x = LeftMargin, y;
            string drawString;

            float temp;
            if (st == 0) st = 1;
            for (uint i = 0; i <= Up; i += st)
            {
                temp = step_y * i;

                drawString = "" + ((Up - i) * 0.1).ToString("0.0");
                y = temp + TopMargin;
                if (y <= Height_BottomMargin)
                {
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, y), new Vector2(Width_RightMargin, y), graphisc.gray);
                    graphisc.Line(service, mSimpleEffect, new Vector2(x, y), new Vector2(x - 5, y), graphisc.white);
                    float lt = spriteFont.MeasureString(drawString).Length() / 2;
                    spriteBatch.DrawString(spriteFont, drawString, new Vector2(x + 17 - lt, y - 6), graphisc.white, 0, new Vector2(0, 0), 0.27f, SpriteEffects.None, 0);
                }
            }
            graphisc.Line(service, mSimpleEffect, new Vector2(LeftMargin, TopMargin), new Vector2(LeftMargin, Height_BottomMargin), graphisc.white);


        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                        Flow
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public void SceneFlow(Panel panelViewport, float[] data, uint CorrelationShift)
        {
            double Max_col = 0;
            double col = 0;
            Vector2 p;
            int step;
            int position;

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

            if (data.Length > NegPos)
            {

                for (uint j = 0; j < ActivePlotAreaX; j++)
                {
                    //Find the max in window
                    step = (int)(j * data_frame + CorrelationShift);
                    Max_col = 0;
                    for (int i = 0; i < data_frame; i++)
                    {
                        position = i + step;
                        if (position >= 0 && position < data.Length)
                        {
                            col = (data[position]);
                            if (col > Max_col) Max_col = col;
                        }
                    }
                    p.X = j + LeftMargin + PixelsXToStart;
                    Max_col *= Amplitude;
                    Max_col += Level;

                    if (Max_col < 0) Max_col = 0;
                    if (Max_col > Draw.ColorTableSize-1) Max_col = Draw.ColorTableSize-1;

                    graphisc.Point(p, ColorTable[(int)Max_col]);
                }

                //Negative
                for (uint j = 0; j < ActivePlotAreaX; j++)
                {
                    //Find the max in window
                    step = (int)(-j * data_frame + CorrelationShift);
                    Max_col = 0;
                    for (int i = 0; i < data_frame; i++)
                    {
                        position = i + step;
                        if (position >= 0 && position < data.Length)
                        {
                            col = data[position];
                            if (col > Max_col) Max_col = col;
                        }
                    }
                    p.X = -j + LeftMargin + PixelsXToStart;
                    Max_col *= Amplitude;
                    Max_col += Level;

                    if (Max_col < 0) Max_col = 0;
                    if (Max_col > Draw.ColorTableSize-1) Max_col = Draw.ColorTableSize-1;

                    graphisc.Point(p, ColorTable[(int)Max_col]);
                }
            }

            spriteBatch.End();

            Color BackC = Color.FromNonPremultiplied(new Vector4(0.0f, 0.0f, 0, .05f));
            graphisc.FiledRectangle(service, mSimpleEffect, new Vector2(0, panelViewport.Height - BottomMargin - 10), panelViewport.Width, BottomMargin, BackC, BackC);


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
            ScaleX(panelViewport.Width, panelViewport.Height);
            spriteBatch.End();
        }

    }
}
