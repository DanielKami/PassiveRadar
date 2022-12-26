using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasiveRadar
{
    //Class interpolate last points and calculate the best background corection from them
    class ClassRegresion
    {
        public float[] StorageArray;
        public float[] BackgroundArray;
        public uint NrElements;
        private uint AddEveryIndex;
        private uint Index;
        private uint Lenght;
        private uint Column, Rows;
        private float average_x;
        private float average_xx;
        private float f2;
        private float n_AvXf2;

        public void Initiate(Flags flags)
        {
            NrElements = (uint)flags.NrCorrectionPoints;
            AddEveryIndex = 0;
            Column = flags.Columns;
            Rows = flags.Rows;
            Lenght = Column * Rows;

            StorageArray = new float[Lenght *( NrElements+1)];
            BackgroundArray = new float[Lenght];

            //Mathematical preparation

            average_x = (NrElements + 1) / 2; //This is know since a step is always 1
            average_xx = average_x * average_x; //This is know since a step is always 1
            
            f2 = 0;
            for (uint i = 1; i < NrElements+1; i++)
                f2 += i * i ;
            f2 /= NrElements;
            f2 = f2 - average_xx; // f2 Var(x)

            n_AvXf2 = (NrElements - average_x) / f2;
        }

        public void Add(float[] InpArray, int AddEvery)
        {

            //Add the new image to storage
            if (AddEveryIndex == AddEvery)
            {
                //if(InpArray.Length>= StorageArray.Length)
                Array.Copy(InpArray, 0, StorageArray, Lenght * Index, Lenght);
                AddEveryIndex = 0;

                //Change the indexof added elements
                Index++;
                if (Index > NrElements)
                {
                    Index = 0;
                }

                //Do the hard work
                LinearRegresion();
            }
            AddEveryIndex++;



        }

        private void LinearRegresion()
        {
            
            Parallel.For(0, Lenght, new ParallelOptions { MaxDegreeOfParallelism = 12 }, Position =>
            // for (uint Position = 0; Position < Lenght; Position++) //Scaning points in the picture
            {
                float[] points = new float[NrElements];
                for (uint Frame = 0; Frame < NrElements; Frame++) //Scaning pictures
                {
                    points[Frame] = StorageArray[Position + Frame * Lenght];
                }
                float temp_pos = FindTheBestFit(points);
                if (temp_pos > BackgroundArray[Position] * 2)//Skip the piks, only 2 * average value (pick filter) is allowed
                    BackgroundArray[Position] = (BackgroundArray[Position] + temp_pos) / 2;
                else
                    BackgroundArray[Position] = temp_pos;
            }
            );
        }

        private float FindTheBestFit(float[] Points)
        {
            float average_y =0;

            //y average
            for (uint i = 0; i < NrElements; i++ )            
                average_y += Points[i];
            average_y /= NrElements;


            float f1 = Points[0];
            for (uint i = 1; i < NrElements; i++)
                f1 += (i+1) * Points[i];
            f1 /= NrElements;
            f1 -= average_x * average_y; //Cov(x,y)

            //6 steps approach
            //float b = f1 / f2;
            //float a = average_y - b * average_x;
            //return ( a + NrElements *b);

            //3 step approach
            // average_y - f1 / f2 * average_x + NrElements * f1 / f2
            //average_y + f1 / f2*(NrElements-average_x)
            // average_y + f1  * n_AvXf2
            return (average_y + f1  * n_AvXf2);

        }

        public void CorrectBackground(float[] input, float Weight)
        {
            for (uint Position = 0; Position < Lenght; Position++) //Scaning points in the picture
            {
                input[Position] -= BackgroundArray[Position] * Weight;
                if (input[Position] < 0) input[Position] = 0;
            }
        }

        public void Background(float[] input, float Weight)
        {
            for (uint Position = 0; Position < Lenght; Position++) //Scaning points in the picture
            {
                input[Position] = BackgroundArray[Position] * Weight;
                if (input[Position] < 0) input[Position] = 0;
            }
        }

    }
}
