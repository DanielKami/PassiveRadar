using System;
using System.Threading;
using System.Threading.Tasks;


namespace PasiveRadar
{
    public class Calculate
    {

        private float[] IntermediateBuffer;

        uint cumulate_max = 10;
        uint cumulateIndex;
        uint BufferSize;
        uint data_position = 0;//Data position in radio buffer necessary for the short display buffer
        FFT fft;
        //Correlate correlate;
        private Thread ThreadFFT;



        public Calculate()
        {
                   fft = new FFT();
                  // correlate = new Correlate();
        }

        public void Init(uint _BufferSize, uint cumulate_max)
        {
            BufferSize = _BufferSize;

            fft.Init(BufferSize);
            IntermediateBuffer = new float[BufferSize * (cumulate_max + 1)];  
        }

        public void Reset()
        {
            data_position = 0;
        }

        public uint CopyToComplex(byte[] datain, ref Complex[] DataOut, bool reset)
        {
            uint a;

            for (uint i = 0; i < BufferSize/2-1; i++)
            {
                DataOut[i].Rea = datain[a = i * 2] - 127;
                DataOut[i].Imag = datain[a + 1] - 127;
            }

            return data_position;
        }

        public bool CopyToComplexRadar(byte[] datain1, byte[] datain2, ref Complex[] DataOut1, ref Complex[] DataOut2, ref double[] CorrelateArray, ref uint CorrelationShift, Flags flags)//uint negative, uint positive, double Level
        {
            uint i2;
            uint b;
            uint NegPos = flags.Negative + flags.Positive;


            for (uint i = 0; i < BufferSize/2; i++)
            {
                i2 = i * 2;
                DataOut1[i].Rea = datain1[i2] - 127;
                DataOut1[i].Imag = datain1[b = i2 + 1] - 127;

                DataOut2[i].Rea = datain2[i2] - 127;
                DataOut2[i].Imag = datain2[b] - 127;
            }

            return true;

        }

        public void FFT(Complex[] DataIn,  Complex[] DataOut)
        {
            ThreadFFT = new Thread(() => ProcessFFT(DataIn, DataOut));
            ThreadFFT.Start();
          
        }
        public void FFTWaitToComplete()
        {
            ThreadFFT.Join();
        }

        private void ProcessFFT(Complex[] DataIn, Complex[] DataOut)
        {
            fft.CalcFFT(DataIn, ref DataOut);
        }




        private Thread ThreadCA;
        //Start n threads for ambiguity function calculations
        public void Start(Complex[] DataIn, double[] dataout, uint _cumulate_max, float gain)
        {
            {
                if (ThreadCA == null)
                {
                    ThreadCA = new Thread(() => Average(DataIn, dataout,  _cumulate_max, gain));

                    if (ThreadCA != null)
                    {
                        ThreadCA.Name = "Thread calculate";
                        ThreadCA.Start();
                    }
                }
            }
    }

        //Stop threads
        public void WaitToFinischCalc()
        {
            if (ThreadCA != null)
            {
                ThreadCA.Join();//wait for thread termination
                ThreadCA = null;
            }
        }

        public void Average(Complex[] DataIn, double[] dataout, uint _cumulate_max, float gain)
        {
            if (cumulate_max != _cumulate_max)
            {
                IntermediateBuffer = new float[BufferSize * (_cumulate_max + 1)];
                cumulate_max = _cumulate_max;
            }


            //Calculate module from complex
            double max = 0;
            for (uint i = 0; i < BufferSize; i++)
            {
                dataout[i] = DataIn[i].Module();
                if (dataout[i] > max) max = dataout[i];
            }

            uint hb = BufferSize / 2;

            double temp_var;

            //Rotate 180 deg
            //for (uint i = 0; i < hb; i++)
            //{
            //    temp_var = dataout[i];
            //    dataout[i] = dataout[i + hb];
            //    dataout[i + hb] = temp_var;
            //}

            uint pos;

            //reverse
            for (uint i = 0; i < hb; i++)
            {
                pos = BufferSize - i;
                temp_var = dataout[i];
                dataout[i] = dataout[pos];
                dataout[pos] = temp_var;
            }

            cumulateIndex++;
            if (cumulateIndex >= cumulate_max)
                cumulateIndex = 0;


            //Scale to max
            uint Cm = cumulateIndex * BufferSize;
            float scale = 1.0f / (float)max;
            for (uint i = 0; i < BufferSize; i++)
                IntermediateBuffer[Cm + i] = (float)Math.Log10(dataout[i] * scale);

            float temp = 1.0f / cumulate_max * gain;

            System.Array.Clear(dataout, 0, (int)BufferSize);

            for (uint j = 0; j < cumulate_max; j++)
            {
                uint jF = j * BufferSize;
                for (uint i = 0; i < BufferSize; i++)
                    dataout[i] += IntermediateBuffer[jF + i] * temp;
            }


        }

        public void Difference(double[] data1, double[] data2, double[] data_out)
        {
            for (uint i = 0; i < BufferSize; i++)
                data_out[i] = data2[i] - data1[i];
        }
    }
}
