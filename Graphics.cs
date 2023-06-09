using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows.Forms;

namespace PasiveRadar
{
    public class GraphicsHelper
    {
        public Color white = new Color(200, 200, 200);
        public Color gray = new Color(30, 30, 30, 5);
        public float zoomX, zoomY, zoom;
        Panel panelViewport;
        SpriteBatch spriteBatch;
        Texture2D texture;

        public void SetValues(Panel panelViewport_, SpriteBatch spriteBatch_, Texture2D texture_, float zoomX_,  float zoomY_ )
        {
            panelViewport = panelViewport_;
            spriteBatch = spriteBatch_;
            texture = texture_;
            zoomX=zoomX_;
            zoomY= zoomY_;
            if (zoomX > zoomY) zoom = zoomX;
            else
                zoom = zoomY;
            
        }
        public void Point(Vector2 p, Color col)
        {             
            spriteBatch.Draw(texture, p, null, col, 0,  new Vector2(0, 0), zoom, SpriteEffects.None, 0);
        }
        public void Pixel(Vector2 p, Color col)
        {
            spriteBatch.Draw(texture, p, null, col, 0, new Vector2(0, 0), new Vector2( 10*zoom,zoom), SpriteEffects.None, 0);
        }
        public void Point(GraphicsDeviceService serv, BasicEffect efect, int y, int x, Color col)
        {
            try
            {
                float DimensionX = zoom;
                float DimensionY = zoom;
                Vector2 Position = new Vector2(x*zoom, panelViewport.Height - 50 - y * zoomY);
                FiledRectangle(serv, efect, Position, DimensionX, DimensionY, col, col);
            }
            catch (Exception ex)
            {
                String str = "Plot point error. " + ex.ToString();
                MessageBox.Show(str);
                System.Windows.Forms.Application.Exit();
            }
        }

        public void Line(GraphicsDeviceService serv, BasicEffect efect, Vector2 vector1, Vector2 vector2, Color color)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[2];
            vertices[0].Position = new Vector3(vector1.X, vector1.Y, 0);
            vertices[0].Color = color;
            vertices[1].Position = new Vector3(vector2.X, vector2.Y, 0);
            vertices[1].Color = color;

            //Draw the line
            efect.CurrentTechnique.Passes[0].Apply();
            serv.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 1);

        }

        public void FiledRectangle(GraphicsDeviceService serv, BasicEffect efect, Vector2 Position, float DimensionX, float DimensionY, Color color1, Color color2)
        {
            VertexPositionColor[] vertices_tringle = new VertexPositionColor[3];
            vertices_tringle[0].Position = new Vector3(Position.X, Position.Y, 0);
            vertices_tringle[0].Color = color1;
            vertices_tringle[1].Position = new Vector3(Position.X + DimensionX, Position.Y, 0);
            vertices_tringle[1].Color = color1;
            vertices_tringle[2].Position = new Vector3(Position.X, Position.Y + DimensionY, 0);
            vertices_tringle[2].Color = color2;
            efect.CurrentTechnique.Passes[0].Apply();
            serv.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices_tringle, 0, 1);

            vertices_tringle[0].Position = new Vector3(Position.X + DimensionX, Position.Y, 0);
            vertices_tringle[0].Color = color1;
            vertices_tringle[1].Position = new Vector3(Position.X + DimensionX, Position.Y + DimensionY, 0);
            vertices_tringle[1].Color = color2;
            vertices_tringle[2].Position = new Vector3(Position.X, Position.Y + DimensionY, 0);
            vertices_tringle[2].Color = color2;
            efect.CurrentTechnique.Passes[0].Apply();
            serv.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices_tringle, 0, 1);

        }

        public void FiledRectangle(GraphicsDeviceService serv, BasicEffect efect, float X1, float Y1, float X2, float Y2, float Y3, Color color1, Color color2)
        {
            VertexPositionColor[] vertices_tringle = new VertexPositionColor[3];
            vertices_tringle[0].Position = new Vector3(X1, Y1, 0);
            vertices_tringle[0].Color = color1;
            vertices_tringle[1].Position = new Vector3(X2, Y1, 0);
            vertices_tringle[1].Color = color1;
            vertices_tringle[2].Position = new Vector3(X1, Y3, 0);
            vertices_tringle[2].Color = color2;
            efect.CurrentTechnique.Passes[0].Apply();
            serv.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices_tringle, 0, 1);

            vertices_tringle[0].Position = new Vector3(X2, Y1, 0);
            vertices_tringle[0].Color = color1;
            vertices_tringle[1].Position = new Vector3(X2, Y2, 0);
            vertices_tringle[1].Color = color2;
            vertices_tringle[2].Position = new Vector3(X1, Y3, 0);
            vertices_tringle[2].Color = color2;
            efect.CurrentTechnique.Passes[0].Apply();
            serv.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices_tringle, 0, 1);

        }
    }
}
