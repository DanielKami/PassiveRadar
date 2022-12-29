//http://www.labbookpages.co.uk/audio/firWindowing.html
//https://en.wikipedia.org/wiki/Window_function

using System;
using System.Threading;

namespace PasiveRadar
{
    class Filters : FourierFilters
    {
        private readonly Object demod_Lock = new Object();

        public Filters()
        {

        }

 

       public void UberSampling(byte[] In, byte[] Out)
        {
            uint a;
            for (uint i = 0; i < In.Length - 2; i+=2)
            {
                Out[a = i * 2] = In[i];
                Out[a + 1] = In[i+1];
                Out[a + 2] = (byte)((In[i] + In[i + 2]) / 2);
                Out[a + 3] = (byte)((In[i+1] + In[i + 3]) / 2);
            }
        }

        public void UberSampling(float[] In, float[] Out)
        {
            uint a;
            for (uint i = 0; i < In.Length -4; i += 2)
            {
                Out[a = i * 2] = In[i];
                Out[a + 1] = In[i + 1];
                Out[a + 2] = (In[i] + In[i + 2]) / 2;
                Out[a + 3] = (In[i + 1] + In[i + 3]) / 2;
            }
        }


        public void UberSampling(Complex[] In, Complex[] Out)
        {
            uint a;
            for (uint i = 0; i < In.Length/2 - 1; i ++)
            {
                Out[a = i * 2] = In[i];
                Out[a + 1] = (In[i] + In[i + 1]) *0.5f;
            }
        }

        
        public void ReduceSampling(Complex[] In, Complex[] Out, uint times)
        {
            uint a;
            for (uint i = 0; i < In.Length/times - 1; i++)
            {
                Out[i] = In[a = i * times];
            }
        }
        public void ReduceSampling(int[] In, int[] Out)
        {
            int a;
            for (int i = 0; i < In.Length / 2 - 2; i+= 2)
            {
                Out[i] = (In[a = i * 2]+ In[a + 2]) / 2;
                Out[i+1] = (In[a +1] + In[a + 3]) / 2;
            }
        }

        void rotate_90(byte[] buf)
        /* 90 rotation is 1+0j, 0+1j, -1+0j, 0-1j
           or [0, 1, -3, 2, -4, -5, 7, -6] */
        {
            uint i;
            byte tmp;
            for (i = 0; i < buf.Length; i += 8)
            {
                /* uint8_t negation = 255 - x */
                tmp = (byte)(255 - buf[i + 3]);
                buf[i + 3] = buf[i + 2];
                buf[i + 2] = tmp;

                buf[i + 4] = (byte)(255 - buf[i + 4]);
                buf[i + 5] = (byte)(255 - buf[i + 5]);

                tmp = (byte)(255 - buf[i + 6]);
                buf[i + 6] = buf[i + 7];
                buf[i + 7] = tmp;
            }
        }

        void rotate_180(byte[] buf)
        /* 180 rotation is 1+0j, 
               0  1   2  3   4   5  6   7
           or [0, 1; -2, -3;  4, 5; -6, -7] */

        {
            uint a;
            for (uint i = 0; i < buf.Length; i += 8)
            {
                /* uint8_t negation = 255 - x */
                buf[a = i + 2] = (byte)(255 - buf[a]);
                buf[a = i + 3] = (byte)(255 - buf[a]);

                buf[a = i + 6] = (byte)(255 - buf[a]);
                buf[a = i + 7] = (byte)(255 - buf[a]);

            }
        }

    }
}
