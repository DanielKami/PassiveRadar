//Defint this flag in both Coniugates and ambiguity class
//#define INTFLAG

using System;
using System.Threading;

namespace PasiveRadar
{
    class Coniugate
    {
        private const uint Threadsnr = 8;
        uint size;
        uint HalfColumn;
        uint B_HC;

        private Thread[] ThreadC = new Thread[Threadsnr];

#if (INTFLAG)
        protected int[] coniugate;
#else
        protected float[] coniugate;
#endif


        //Start 4 threads for ambiguity function calculations
        private void Start(Complex[] In1, Complex[] In2, uint BufferSize, uint Columns)
        {

            size = (uint)Math.Ceiling(1.0 * (Columns - 1) / Threadsnr);
            HalfColumn = Columns / 2;
            B_HC = BufferSize - HalfColumn;

            uint i;
            for (i = 0; i < Threadsnr; i++)
            {
                if (ThreadC[i] == null)
                {
                    uint j = i;//trick to pass i to lambda ;)
                    ThreadC[i] = new Thread(() => CalculateConiugateThread(j, In1, In2, BufferSize, Columns));

                    ThreadC[i].Name = "Thread Coniugate" + i;
                    ThreadC[i].Start();
                }
            }
        }

        //Stop threads
        private void Stop()
        {
            for (int i = 0; i < Threadsnr; i++)
            {
                if (ThreadC[i] != null)
                {
                    ThreadC[i].Join();//wait for thread termination
                    ThreadC[i] = null;
                }
            }
        }


        public void CalculateConiugate(Complex[] In1, Complex[] In2, uint BufferSize, uint Columns)
        {
            Start(In1, In2, BufferSize, Columns);

            //Wait for thread termination
            Stop();
        }

        private void CalculateConiugateThread(uint index, Complex[] In1, Complex[] In2, uint BufferSize, uint Columns)
        {


            //Divide thread tasks
            uint start = (Columns - 1) * index / Threadsnr;
            uint end = start + size;
            uint fi;
            uint mi_index_buffer;
            uint miH;

            for (uint mi = start; mi < end; mi++) // doppler shift
            {
                miH = mi - HalfColumn;
                mi_index_buffer = mi * BufferSize;

                for (uint f = HalfColumn; f < B_HC; f += 2)
                {
                    //fi = f + mi shift the iterator of mi for the second function
                    //The coniugate
#if (INTFLAG)
                        coniugate[f + mi_index_buffer] = (int)((In1[f].Real * In2[fi = f + mi].Real + In1[f].Imag * In2[fi].Imag) * precision);
#else
                    coniugate[f + mi_index_buffer] = (In1[f].Re * In2[fi = f + miH].Re + In1[f].Imag * In2[fi].Imag);
#endif
                }
            }

        }
    }
}
