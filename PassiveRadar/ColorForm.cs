using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PasiveRadar
{
    public partial class ColorForm : Form
    {
        public  delegate void MyDelegate(Color[] color_table);
        public static event MyDelegate EventColor;
        public bool AcceptColors;
        private struct ColorPoint
        {
            public int PointPosition;
            public Color color;
        }

        public Color[] ColorTable = new Color[Draw.ColorTableSize];
        private List<ColorPoint> ListOfPoints = new List<ColorPoint>();


        public ColorForm(Color[] LocalColorTable)
        {
            InitializeComponent();
            //Set the color array
            Array.Copy(LocalColorTable, ColorTable, Draw.ColorTableSize);


            //Add the first point to be black
            ColorPoint ColPoint = new PasiveRadar.ColorForm.ColorPoint();
            ColPoint.PointPosition = 0;
            ColPoint.color = Color.FromArgb(255, 0, 0, 0);
            ListOfPoints.Add(ColPoint);
            DrawTable();
        }

        private void button_Ok(object sender, EventArgs e)
        {
            //Get the last color
            if (ListOfPoints.Count > 1)
            {
                Color color = ListOfPoints.Last().color;

                //Set the last point in color table to the last color in the List
                ColorPoint ColPoint = new PasiveRadar.ColorForm.ColorPoint();
                ColPoint.PointPosition = Draw.ColorTableSize;
                ColPoint.color = color;
                FillTableWithColorValues(ColPoint);
            }
            DrawTable();
            if (EventColor != null)
                EventColor(ColorTable);
            AcceptColors = true;


            this.Close(); 
        }

        //Cancel
        private void button_Cancel(object sender, EventArgs e)
        {
            AcceptColors = false;
            this.Close();
        }

        //Converts position from the picture to position in the table
        private int ConvertPositionToTable(int PositionX)
        {
            int Factor;
            if (pictureBox1.Width > 0)
                Factor = Draw.ColorTableSize / pictureBox1.Width;
            else return 0;

            return Factor * PositionX;
        }

        //Converts position in the table to position on the picture
        private int ConvertTableToPosition(int TablePositionX)
        {
            int Factor;
            int PicturePosition;
            if (pictureBox1.Width > 0)
                Factor = Draw.ColorTableSize / pictureBox1.Width;
            else return 0;
            PicturePosition = TablePositionX / Factor;
            if (PicturePosition >= pictureBox1.Width) PicturePosition = pictureBox1.Width - 1;

            return PicturePosition;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // Convert position to th table position
            int Table_position = ConvertPositionToTable(e.X);

            //Add the color point to the table
            ColorPoint ColPoint = new PasiveRadar.ColorForm.ColorPoint();
            ColPoint.PointPosition = Table_position;

            colorDialog1.ShowDialog();
            ColPoint.color = colorDialog1.Color;// Color.FromArgb(255, 255, 0);
            FillTableWithColorValues(ColPoint);

            DrawTable();
        }

        private void DrawTable()
        {
            //Draw color table
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            int x, y;
            for (y = 0; y < pictureBox1.Height; y++)
            {
                for (int i = 0; i < Draw.ColorTableSize; i++)
                {
                    x = ConvertTableToPosition(i);
                    ((Bitmap)pictureBox1.Image).SetPixel(x, y, ColorTable[i]);
                }
            }

            //second picture      
            pictureBox2.Image = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            for (y = 0; y < pictureBox2.Height; y++)
            {
                for (int i = 0; i < ListOfPoints.Count; i++)
                {
                    x = ConvertTableToPosition(ListOfPoints.ElementAt(i).PointPosition);
                    // Nice tringle
                    for (int k = 0; k < 10; k++)
                        if (x - k > 0  && x + k < pictureBox2.Image.Width)
                            if ( y + k < pictureBox2.Image.Height)
                            {
                                ((Bitmap)pictureBox2.Image).SetPixel(x - k, y + k, ListOfPoints.ElementAt(i).color);
                                ((Bitmap)pictureBox2.Image).SetPixel(x + k, y + k, ListOfPoints.ElementAt(i).color);
                            }
                }
            }
        }


        private void FillTableWithColorValues(ColorPoint ColPoint)
        {
            int LastPositionX = 0;
            ListOfPoints.Add(ColPoint);

            //Sort the list
            ListOfPoints = ListOfPoints.OrderBy(sel => sel.PointPosition).ToList();

            for (int i = 1; i < ListOfPoints.Count; i++)
            {
                //Fill the colors in the table
                for (int j = LastPositionX; j < ListOfPoints.ElementAt(i).PointPosition; j++)
                {
                    ColorTable[j] = LerpColors(j, LastPositionX, ListOfPoints.ElementAt(i).PointPosition, ListOfPoints.ElementAt(i - 1).color, ListOfPoints.ElementAt(i).color);
                }

                LastPositionX = ListOfPoints.ElementAt(i).PointPosition;
            }

        }


        Color LerpColors(int position, int previousPosition, int EndPosition, Color StartColor, Color EndColor)
        {
            int CorrectedCurrentPosition = position - previousPosition;
            int CorrectedLastPosition = EndPosition - previousPosition;

            float delR = EndColor.R - StartColor.R;
            float delG = EndColor.G - StartColor.G;
            float delB = EndColor.B - StartColor.B;

            float fraction = 1.0f * CorrectedCurrentPosition / CorrectedLastPosition;
            if (fraction == 0) fraction = 0.01f;

            int r = (int)(StartColor.R + delR * fraction);
            if (r < 0) r = 0; if (r > 255) r = 255;

            int g = (int)(StartColor.G + delG * fraction);
            if (g < 0) g = 0; if (g > 255) g = 255;

            int b = (int)(StartColor.B + delB * fraction);
            if (b < 0) b = 0; if (b > 255) b = 255;
            return Color.FromArgb(255, r, g, b);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ListOfPoints.Clear();
            Array.Clear(ColorTable, 0, Draw.ColorTableSize);

            //The first point is black
            ColorPoint ColPoint = new PasiveRadar.ColorForm.ColorPoint();
            ColPoint.PointPosition = 0;
            ColPoint.color = Color.FromArgb(255, 0, 0, 0);
            ListOfPoints.Add(ColPoint);

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            int x, y;
            for (y = 0; y < pictureBox1.Height; y++)
            {
                for (x = 0; x < pictureBox1.Width; x++)
                {
                    ((Bitmap)pictureBox1.Image).SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                }
            }
            pictureBox2.Image = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            for (y = 0; y < pictureBox2.Height; y++)
            {
                for (x = 0; x < pictureBox2.Width; x++)
                {
                    ((Bitmap)pictureBox2.Image).SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                }
            }
        }

        private void ColorForm_ResizeEnd(object sender, EventArgs e)
        {
            DrawTable();
        }
    }
}
