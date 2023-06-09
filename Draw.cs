using Microsoft.Xna.Framework;
using System;

namespace PasiveRadar
{
    public class Draw
    {
        public const int ColorTableSize = 1024;
        public static int LeftMargin = 30;
        public static int RightMargin = 20;
        public int BottomMargin = 40;
        public int TopMargin = 20;
        

        public int ColorThemeNr;

        public Color[] ColorTable;
        public Color[] CustomTable;

        public Draw()
        {
            ColorTable = new Color[ColorTableSize];
            CustomTable = new Color[ColorTableSize];

        }

        public void CreateColorTable1(int select, Color[] ColorThemeTable, Flags flags)
        {
            int i;
            int alpha;

            if (flags != null)
                alpha = flags.alpha;
            else
                alpha = 255;

            Array.Copy(ColorThemeTable, CustomTable, ColorTableSize);

            switch (select)
            {
                case 0:
                    //Blue
                    for (i = 0; i < 512; i++)
                        ColorTable[i] = Color.FromNonPremultiplied(0, 0, i / 2, alpha);

                    //Purple 
                    for (i = 0; i < 256; i++)
                        ColorTable[i + 512] = Color.FromNonPremultiplied(i, 0, 255, alpha);

                    //Red
                    for (i = 0; i < 256; i++)
                        ColorTable[i + 256 * 3] = Color.FromNonPremultiplied(255, 0, 255 - i, alpha);

                   
                    break;

                case 1:
                    //Green 512
                    for (i = 0; i < 512; i++)
                        ColorTable[i] = Color.FromNonPremultiplied(0,  0, i / 2, alpha);

                    //Yellow 512
                    for (i = 0; i < 512; i++)
                        ColorTable[i + 512] = Color.FromNonPremultiplied(i / 2, 255, 0, alpha);

                    break;
                case 2: //User defined color
                    //Array.Copy(CustomTable, ColorTable,  ColorTableSize);
                    for (i = 0; i < ColorTable.Length; i++)
                        ColorTable[i] = Color.FromNonPremultiplied(CustomTable[i].R, CustomTable[i].G, CustomTable[i].B, alpha);
                    break;

            }
        }         

    }
}
