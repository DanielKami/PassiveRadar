//#define INTFLAG
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace PasiveRadar
{
    class Ambiguity : Coniugate
    {

        [System.Runtime.InteropServices.DllImport(@"Ambiguity.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Initialize(uint BufferSize, uint col, uint row, float doopler_shift, short[] Name);

        [System.Runtime.InteropServices.DllImport(@"Ambiguity.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Run(int[] Data_In0, int[] Data_In1, float[] Data_Out, float amplification, float doppler_zoom, int shift, bool mode, short scale_type, bool remove_symetric);

        [System.Runtime.InteropServices.DllImport(@"Ambiguity.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Release();

        private readonly Object LockMem = new Object();
        private Thread ThreadGPU;

        public void Prepare(Flags flags)
        {
            short[] name = new short[Flags.MAX_DEVICE_NAME];
            //Name is a return string containing info about NVIDIA card
            int err = Initialize(flags.BufferSize, flags.Columns, flags.Rows, (float)flags.DopplerZoom, name);

            //copy the device name
            flags.DeviceName = "";
            for (int i = 0; i < Flags.MAX_DEVICE_NAME; i++)
                if (name[i] > 32 && name[i] < 126)
                    flags.DeviceName += (char)name[i];


            if (err < 0)
            {
                String str = "CUDA error. " + err;
                MessageBox.Show(str);
                StopGPU();
            }
        }

         public void StartGPU(int[] In0, int[] In1, float[] Out, Flags flags)
        {
            if (ThreadGPU == null) ThreadGPU = new Thread(() => ProcessGPU(In0, In1, Out, flags));

            if (ThreadGPU != null)
            {
                ThreadGPU.Name = "Thread GPU";
                ThreadGPU.Priority = System.Threading.ThreadPriority.Normal;
                ThreadGPU.Start();
            }
        }

        public void StopGPU()
        {
            if (ThreadGPU != null)
            {
                ThreadGPU.Join();//wait for thread termination
                ThreadGPU = null;
            }
        }

        void ProcessGPU(int[] In0, int[] In1, float[] Out, Flags flags)
        {
            //flags.Columns * flags.rows + flags.Columns;
            //The size can be different than dellared in buffer prepare, so it is better to set it constant
            var dataRadar = new float[1000 * 1000 + 1000];

            try
            {
                int err = Run(In0, In1, dataRadar, flags.PasiveGain, flags.DopplerZoom, flags.DistanceShift, flags.TwoDonglesMode, flags.scale_type, flags.remove_symetrics);
                if (err < 0)
                {
                    String str = "CUDA error. " + err;
                    MessageBox.Show(str);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            lock (LockMem)
            {
                System.Buffer.BlockCopy(dataRadar, 0, Out, 0, sizeof(float)*Out.Length);
            }
        }


        public void Release(Flags flags)
        {
            int err = Release();
            if (err < 0)
            {
                String str = "CUDA error. " + err;
                MessageBox.Show(str);
            }
        }


    }
}
