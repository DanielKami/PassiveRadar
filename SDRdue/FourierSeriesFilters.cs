//http://www.labbookpages.co.uk/audio/firWindowing.html
//https://en.wikipedia.org/wiki/Window_function

using System;

namespace SDRdue
{
    class FourierSeriesFilters
    {

        string m_filt_t;
        int taps;
        double m_Fs;
        double m_Fx;
        double m_Fu;
        double m_lambda;
        double m_phi;
        double[] m_taps;
        double[] m_sr;



        public string Ini(string filt_t, int num_taps, double Fs, double Fx)
        {

            m_filt_t = filt_t;
            taps = num_taps;
            m_Fs = Fs;
            m_Fx = Fx;
            m_lambda = Math.PI * Fx / (Fs / 2);

            if (Fx <= 0 || Fx >= Fs / 2) return "Cutting frequency outside the borders <0; F/2>";

            m_taps = new double[taps];
            m_sr = new double[taps];

            Reset();

            if (m_filt_t == "LowPassFilter") designLPF();
            else if (m_filt_t == "HighPassFilter") designHPF();

            return "Ok";
        }


        public string Ini(string filt_t, int num_taps, double SampleRate, double LowFreq,
                       double UpFreq)
        {

            m_filt_t = filt_t;
            taps = num_taps;
            m_Fs = SampleRate / 1000;
            m_Fx = LowFreq / 1000;
            m_Fu = UpFreq / 1000;
            m_lambda = 2.0 * Math.PI * LowFreq / SampleRate; // The normalised transition frequency
            m_phi = 2.0 * Math.PI * UpFreq / SampleRate; // The normalised transition frequency


            if (LowFreq >= UpFreq) LowFreq = UpFreq - 1;
            if (LowFreq <= 0) LowFreq = 1;
            if (LowFreq >= SampleRate / 2) LowFreq = SampleRate / 2;
            if (UpFreq <= 0) UpFreq = 0;
            if (UpFreq >= SampleRate / 2) UpFreq = SampleRate / 2;

            m_taps = new double[taps];
            m_sr = new double[taps];

            Reset();

            if (m_filt_t == "BandPassFilter") designBPF();
            FilterType(0);//"RECTANGULAR"

            return "Ok";
        }

        void Reset()
        {
            for (int i = 0; i < taps; i++)
                m_sr[i] = 0;
        }

        void designHPF()
        {
            int n;
            double mm;

            for (n = 0; n < taps; n++)
            {
                mm = n - (taps - 1.0) / 2.0;
                if (mm == 0.0) m_taps[n] = 1.0 - m_lambda / Math.PI;
                else m_taps[n] = -Math.Sin(mm * m_lambda) / (mm * Math.PI);
            }
        }

        void designLPF()
        {
            int n;
            double mm;

            for (n = 0; n < taps; n++)
            {
                mm = n - (taps - 1.0) / 2.0;
                if (mm == 0.0) m_taps[n] = m_lambda / Math.PI;
                else m_taps[n] = Math.Sin(mm * m_lambda) / (mm * Math.PI);
            }
        }

        void designBPF()//RECTANGULAR
        {
            int n;
            double mm;

            for (n = 0; n < taps; n++)
            {
                mm = n - (taps - 1.0) / 2.0;
                if (mm == 0.0) m_taps[n] = (m_phi - m_lambda) / Math.PI;

                else m_taps[n] = (Math.Sin(mm * m_phi) -
                                    Math.Sin(mm * m_lambda)) / (mm * Math.PI);
            }

        }

        //m_num_taps = Lenght
        void FilterType(int type)
        {
            int n;
            int M = taps - 1;//M - This is the filter order, it is always equal to the number of taps minus 1
            switch (type)
            {
                case 0: //RECTANGULAR
                    for (n = 0; n < taps; n++)
                    {
                        m_taps[n] *= 1.0;
                    }
                    break;

                case 1: //BARTLETT
                    for (n = 0; n < taps; n++)
                    {
                        double tmp = (double)n - (double)M / 2;
                        double val = 1.0 - (2.0 * Math.Abs(tmp)) / M;
                        m_taps[n] *= val;

                    }
                    break;

                case 2: // HANNING:
                    for (n = 0; n < taps; n++)
                    {
                        double val = 0.5 - 0.5 * Math.Cos(2.0 * Math.PI * n / M);
                        m_taps[n] *= val;
                    }
                    break;

                case 3: // HAMMING:
                    for (n = 0; n < taps; n++)
                    {
                        double val = 0.54 - 0.46 * Math.Cos(2.0 * Math.PI * n / M);
                        m_taps[n] *= val;
                    }
                    break;

                case 4: // BLACKMAN:
                    for (n = 0; n < taps; n++)
                    {
                        double val = 0.42 - 0.5 * Math.Cos(2.0 * Math.PI * n / M) + 0.08 * Math.Cos(4.0 * Math.PI * n / M);
                        m_taps[n] *= val;
                    }
                    break;
            }       
        }



    public void Exequte(int[] dataIn,  int[] dataOut)
        {
            int i, j;
            double result;

            for (j = 0; j < dataOut.Length; j++)
            {
                for (i = taps - 1; i >= 1; i--)
                {
                    m_sr[i] = m_sr[i - 1];
                }
                m_sr[0] = dataIn[j];

                result = 0;
                for (i = 0; i < taps; i++)
                    result += m_sr[i] * m_taps[i];

                dataOut[j] = (int)result;
            }

        }

    }
}
