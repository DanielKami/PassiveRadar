using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasiveRadar
{
    class FourierFilters
    {
        protected uint taps;

        protected int two_taps;
        protected double FreqLoNorm;
        protected double FreqUpNorm;
        protected uint M;
        protected uint HM;
        protected float[] f_taps;
        protected int[] i_taps;
        private float[] temp_taps;
        double[] m_sr;

        float[] m_flo_re;
        float[] m_flo_im;
        float[] old_data;
        int[] old_dataInt;

        private readonly Object demod_Lock = new Object();

        /// <summary>
        /// Initialize filters and prepare the weights.
        /// </summary>
        /// <param name="filter"></param> filter : LowPassFilter, HighPassFilter, BandPassFilter
        /// <param name="filterType"></param> 0-RECTANGULAR, 1-BARTLETT, 2-HANNING, 3-HAMMING, 4-BLACKMAN
        /// <param name="num_taps"></param> Number of filter taps (size of filter windows in bytes)
        /// <param name="SampleRate"></param> Input sample rate
        /// <param name="LowFreq"></param> Low frequency
        /// <param name="UpFreq"></param> Up frequency
        /// <returns></returns>


        public string Ini(string filter, int filterType, int num_taps, double SampleRate, double LowFreq, double UpFreq)
        {

            taps = (uint)num_taps;
            two_taps = (int)taps * 2;

            if (LowFreq >= UpFreq)
                LowFreq = UpFreq - 1;
            if (LowFreq <= 0)
                LowFreq = 0;

            //if (LowFreq >= SampleRate / 2)
            //    LowFreq = SampleRate / 2;
            if (UpFreq <= 0)
                UpFreq = 1;
            //if (UpFreq >= SampleRate / 2)
            //    UpFreq = SampleRate / 2;

            FreqLoNorm = 2.0 * Math.PI * LowFreq / SampleRate; // The normalised transition frequency
            FreqUpNorm = 2.0 * Math.PI * UpFreq / SampleRate; // The normalised transition frequency
            M = (taps - 1);//M - This is the filter order, it is always equal to the number of taps minus 1
            HM = M / 2;

            f_taps = new float[taps];
            i_taps = new int[taps];
            temp_taps = new float[taps];
            m_sr = new double[taps];

            m_flo_re = new float[taps];
            m_flo_im = new float[taps];
            old_data = new float[taps * 2];
            old_dataInt = new int[taps * 2];

            Reset();

            if (filter == "BandPassFilter") designBPF();
            else
            if (filter == "LowPassFilter") designLPF();
            else
            if (filter == "HighPassFilter") designHPF();

            FilterType(filterType);

            //Copt taps
            lock (demod_Lock)
            {
                Array.Copy(temp_taps, f_taps, num_taps);

                for(int i=0; i<taps;i++)
                {
                    i_taps[i] = (int)f_taps[i]*100;
                }
            }
            return "Ok";
        }

        void Reset()
        {

        }

        void designHPF()
        {
            int n;
            double mm;

            for (n = 0; n < taps; n++)
            {
                mm = n - HM;
                if (mm == 0.0) temp_taps[n] = (float)(1.0 - FreqLoNorm / Math.PI);
                else temp_taps[n] = (float)(-Math.Sin(mm * FreqLoNorm) / (mm * Math.PI));
            }
        }

        void designLPF()
        {
            int n;
            double mm;

            for (n = 0; n < taps; n++)
            {
                mm = n - HM;
                if (mm == 0.0) temp_taps[n] = (float)(FreqLoNorm / Math.PI);
                else temp_taps[n] = (float)(Math.Sin(mm * FreqLoNorm) / (mm * Math.PI));
            }
        }

        void designBPF()
        {
            int n;
            double mm;

            for (n = 0; n < taps; n++)
            {
                mm = 1.0 * n - HM;
                if (mm == 0.0) temp_taps[n] = (float)((FreqUpNorm - FreqLoNorm) / Math.PI);

                else
                    temp_taps[n] = (float)((Math.Sin(mm * FreqUpNorm) - Math.Sin(mm * FreqLoNorm)) / (mm * Math.PI));
            }
        }

        //m_num_taps = Lenght
        void FilterType(int type)
        {
            int n;

            switch (type)
            {
                case 0: //RECTANGULAR
                    for (n = 0; n < taps; n++)
                    {
                        temp_taps[n] *= 1.0f;
                    }
                    break;

                case 1: //BARTLETT
                    for (n = 0; n < taps; n++)
                    {
                        double tmp = (double)n - (double)HM;
                        double val = 1.0 - (2.0 * Math.Abs(tmp)) / M;
                        temp_taps[n] *= (float)val;
                    }
                    break;

                case 2: // HANNING:
                    for (n = 0; n < taps; n++)
                    {
                        double val = 0.5 - 0.5 * Math.Cos(2.0 * Math.PI * n / M);
                        temp_taps[n] *= (float)val;
                    }
                    break;

                case 3: // HAMMING:
                    for (n = 0; n < taps; n++)
                    {
                        double val = 0.54 - 0.46 * Math.Cos(2.0 * Math.PI * n / M);
                        temp_taps[n] *= (float)val;
                    }
                    break;

                case 4: // BLACKMAN:
                    for (n = 0; n < taps; n++)
                    {
                        double val = 0.42 - 0.5 * Math.Cos(2.0 * Math.PI * n / M) + 0.08 * Math.Cos(4.0 * Math.PI * n / M);
                        temp_taps[n] *= (float)val;
                    }
                    break;

                case 5: // Blackman–Nuttall :
                    {
                        double a0 = 0.3635819;
                        double a1 = 0.4891775;
                        double a2 = 0.1365995;
                        double a3 = 0.0106411;
                        for (n = 0; n < taps; n++)
                        {
                            double val = a0 - a1 * Math.Cos(2.0 * Math.PI * n / M) + a2 * Math.Cos(4.0 * Math.PI * n / M) - a3 * Math.Cos(6.0 * Math.PI * n / M);
                            temp_taps[n] *= (float)val;
                        }
                    }
                    break;

                case 6: // Blackman–Harris :
                    {
                        double a0 = 0.35876;
                        double a1 = 0.48829;
                        double a2 = 0.14128;
                        double a3 = 0.01168;
                        for (n = 0; n < taps; n++)
                        {
                            double val = a0 - a1 * Math.Cos(2.0 * Math.PI * n / M) + a2 * Math.Cos(4.0 * Math.PI * n / M) - a3 * Math.Cos(6.0 * Math.PI * n / M);
                            temp_taps[n] *= (float)val;
                        }
                        break;
                    }
            }
        }

        //dataIn.Lenght=dataOutLenght
        public void FiltrateSimple(int[] dataIn, int[] dataOut)
        {
            uint i, j;
            double result;
            //Go through the date with filter window of size taps
            for (j = 0; j < dataOut.Length; j++)
            {
                //Shift up
                for (i = M; i >= 1; i--)
                    m_sr[i] = m_sr[i - 1];

                //Add new data point to the beggining (after shift is empty)
                m_sr[0] = dataIn[j];

                result = 0;

                //Now apply the filter to the filter window
                for (i = 0; i < taps; i++)
                    result += m_sr[i] * f_taps[i];

                //And save result
                dataOut[j] = (int)result;
            }
        }

        //dataIn.Lenght=dataOutLenght
        public void FiltratePrimitiveFast(float[] dataIn, float[] dataOut, int size=0)
        {
            if (old_data == null) return;
            if (size == 0) size = dataIn.Length;
            //Extend the operation data of on tap beginning
            float[] extended_data = new float[size + taps + 1];

            //Add the old data first lenght of taps -1. -1 becouse the first point has to be from the new data set
            Array.ConstrainedCopy(old_data, 0, extended_data, 0, (int)taps);
            //Add the new data
            Array.ConstrainedCopy(dataIn, 0, extended_data, (int)taps, size);

            //Update the old data for next run
            Array.ConstrainedCopy(dataIn, size - (int)taps, old_data, 0, (int)taps);

            //Go through the date with filter window of size taps
            int degreeOfParallelism = Environment.ProcessorCount;

            int worker_lenght = size / degreeOfParallelism;

            Parallel.For(0, degreeOfParallelism, workerId =>
            {
                float result_re;
                int worker_position = worker_lenght * workerId;

                for (int j = worker_position; j < worker_position + worker_lenght; j++)
                {
                    int k;
                    result_re = 0;
                    //Now apply the filter to the filter window
                    for (int i = 0; i < taps; i++)
                    {
                        result_re += extended_data[k = j + i] * f_taps[i];
                    }

                    //And save result
                    dataOut[j] = result_re;
                }
            });
        }

        //dataIn.Lenght=dataOutLenght
        public void FiltratePrimitiveComplex(int[] dataIn, float[] dataOut)
        {
            uint i, j, r;
            float result_re;
            float result_im;
            //Go through the date with filter window of size taps
            for (j = 0; j < dataOut.Length; j += 2)
            {
                //Shift up
                Array.ConstrainedCopy(m_flo_re, 0, m_flo_re, 1, m_flo_re.Length - 1);
                Array.ConstrainedCopy(m_flo_im, 0, m_flo_im, 1, m_flo_im.Length - 1);
                //Add new data point to the beggining (after shift is empty)
                m_flo_re[0] = dataIn[j];
                m_flo_im[0] = dataIn[r = j + 1];

                result_re = result_im = 0;

                //Now apply the filter to the filter window
                for (i = 0; i < taps; i++)
                {
                    result_re += m_flo_re[i] * f_taps[i];
                    result_im += m_flo_im[i] * f_taps[i];
                }
                //And save result
                dataOut[j] = result_re;
                dataOut[r] = result_im;
            }
        }

        //dataIn.Lenght=dataOutLenght
        public void FiltratePrimitiveComplexFast(float[] dataIn, float[] dataOut)
        {

            //Extend the operation data of on tap beginning
            float[] extended_data = new float[dataIn.Length + two_taps + 1];

            //Add the old data first lenght of taps -1. -1 becouse the first point has to be from the new data set
            Array.ConstrainedCopy(old_data, 0, extended_data, 0, two_taps);
            //Add the new data
            Array.ConstrainedCopy(dataIn, 0, extended_data, two_taps, dataIn.Length);

            //Update the old data for next run
            Array.ConstrainedCopy(dataIn, dataIn.Length - two_taps, old_data, 0, two_taps);

            //Go through the date with filter window of size taps
            int degreeOfParallelism = Environment.ProcessorCount;

            int worker_lenght = dataOut.Length / degreeOfParallelism;

            Parallel.For(0, degreeOfParallelism, workerId =>
            {
                float result_re;
                float result_im;

                int worker_position = worker_lenght * workerId;


                for (int j = worker_position; j < worker_position + worker_lenght; j += 2)
                {
                    int k;
                    result_re = result_im = 0;
                    //Now apply the filter to the filter window
                    for (int i = 0; i < taps; i++)
                    {
                        result_re += extended_data[k = j + i] * f_taps[i];
                        result_im += extended_data[k + 1] * f_taps[i];
                    }

                    //And save result
                    dataOut[j] = (int)result_re;
                    dataOut[j + 1] = (int)result_im;
                }

            });

        }

        public void FiltratePrimitiveComplexFastInt(int[] dataIn, int[] dataOut)
        {

            //Extend the operation data of on tap beginning
           int[] extended_data = new int[dataIn.Length + two_taps + 1];

            //Add the old data first lenght of taps -1. -1 becouse the first point has to be from the new data set
            Array.ConstrainedCopy(old_dataInt, 0, extended_data, 0, two_taps);
            //Add the new data
            Array.ConstrainedCopy(dataIn, 0, extended_data, two_taps, dataIn.Length);

            //Update the old data for next run
            Array.ConstrainedCopy(dataIn, dataIn.Length - two_taps, old_dataInt, 0, two_taps);

            //Go through the date with filter window of size taps
            int degreeOfParallelism = Environment.ProcessorCount;

            int worker_lenght = dataOut.Length / degreeOfParallelism;

            Parallel.For(0, degreeOfParallelism, workerId =>
            {
                int result_re;
                int result_im;
                int k;

                int worker_position = worker_lenght * workerId;

                for (int j = worker_position; j < worker_position + worker_lenght; j += 2)
                {

                    result_re = result_im = 0;
                    //Now apply the filter to the filter window
                    for (int i = 0; i < taps; i++)
                    {
                        result_re += extended_data[k = j + i] * i_taps[i];
                        result_im += extended_data[k + 1] * i_taps[i];
                    }
                    //And save result
                    dataOut[j] = result_re;
                    dataOut[j + 1] = result_im;
                }
            });

        }


    }

}
