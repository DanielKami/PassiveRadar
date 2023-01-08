using System.Threading.Tasks;

namespace PasiveRadar
{

    class Correlate
    {
        public uint BufferSize;
        uint negative;
        uint positive;
        uint NegPos;

        const int MaxAcceptanceFeilCount = 5;
        uint AcceptanceFeilCount = 0;

        const int MaxCumulateCorrelateLevel = 20;
        double[] ArrayCumulateCorrelateLevel;
        int CumulateLevelNr = 0;


        public delegate void MyDelegate();
        static public event MyDelegate Resynchronise;

        uint size;
        FFT fft;
        Complex[] f;
        Complex[] Ff;
        Complex[] g;
        Complex[] Fg;
        Complex[] H;
        Complex[] rH;
        public Correlate()
        {
            ArrayCumulateCorrelateLevel = new double[MaxCumulateCorrelateLevel];
            fft = new FFT();
        }

        public void Init(uint size_)
        {
            size = size_;
            fft.Init(size);
            f = new Complex[size];
            Ff = new Complex[size];
            g = new Complex[size];
            Fg = new Complex[size];
            H = new Complex[size];
            rH = new Complex[size];

        }

        public bool Begin(int[] Data1, int[] Data2, float[] CorrelateArray, ref uint CorrelationShift, Flags flags)
        {
            CorrelationShift = 0;
            float Max = 0;

            BufferSize = flags.BufferSize;
            negative = flags.Negative;
            positive = flags.Positive;
            NegPos = negative + positive;
            if (NegPos < 1) NegPos = 1;

            ///////////////////////////////////////////////////////////////////////////////////////////////////////
            //If there is no value for corelation do the full corelation
            if (Data1.Length < BufferSize + NegPos || Data2.Length < BufferSize + NegPos)
                return false;

            ThreadCorelate(Data1, Data2, CorrelateArray);
            //FourierCorelate(Data1, Data2, CorrelateArray);//much faster

            //Find the maximum 
            for (uint i = 0; i < NegPos; i++)
            {
                if (CorrelateArray[i] > Max)
                {
                    Max = CorrelateArray[CorrelationShift = i];
                }
            }

            //Normalize corelate to max
            float one_max = 1.0f / Max;
            float average = 0;
            for (int i = 0; i < NegPos; i++)
            {
                average += CorrelateArray[i] = CorrelateArray[i] * one_max;
            }
            average /= NegPos;

            double AverageLevel = 0;
            //Auto correlation level
            if (flags.AutoCorrelate)
            {
                CumulateLevelNr++;
                if (CumulateLevelNr >= MaxCumulateCorrelateLevel) CumulateLevelNr = 0;

                ArrayCumulateCorrelateLevel[CumulateLevelNr] = average;

                for (int i = 0; i < MaxCumulateCorrelateLevel; i++)
                    AverageLevel += ArrayCumulateCorrelateLevel[i];
                AverageLevel /= MaxCumulateCorrelateLevel;

                if (AverageLevel < 0.2) AverageLevel = 0.2;
                if (AverageLevel > 0.96) AverageLevel = 0.96;
                flags.AcceptedLevel = AverageLevel * 1.2;
                if (flags.AcceptedLevel > 1) flags.AcceptedLevel = 1;

            }

            //If the average is higher than acceptance level than correlate is not valid
            if (average > flags.AcceptedLevel || AverageLevel > 0.95)
            {
                AcceptanceFeilCount++;
                if (AcceptanceFeilCount > MaxAcceptanceFeilCount)
                {
                    //resynchronise dongles
                    Resynchronise.Invoke();
                    AcceptanceFeilCount = 0;
                }
                return false;
            }

            //No feils in synchronisation so reset the dlag and go on
            AcceptanceFeilCount = 0;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Shift the second data string to the begining 
            uint neg_2 = negative * 2; //because the complex number is interleved
            uint corcor = CorrelationShift * 2;
            for (int i = 0; i < BufferSize; i++)
            {
                Data1[i] = Data1[i + neg_2];
                Data2[i] = Data2[i + corcor];
            }

            return true;
            //So now Data2 is shifted to the correct position of correlation
        }


        //Data strings from two dongles are shifted in time due to number of reasons. This has to be corrected prior to the ambiguity function. 
        void ThreadCorelate(int[] Data1, int[] Data2, float[] CorelateArray)
        {
            //                  negative                      positive                 scale of autocorrelation j index
            //          |--------------------------|--------------------------|-------------------------------------------|
            //                                     |==========================================|
            //                                                      Data1
            //          |=========================================|
            //                              Data2
            //          {                    j index              }




            //Divade the task on threads (so expensive method)

            uint neg2 = negative;
            //Scan for corelation beginning 
            Parallel.For(0, NegPos / 2, new ParallelOptions { MaxDegreeOfParallelism = 8 }, i =>
               {


                   long sq;
                   long temp = 0;

                   long sq2;
                   long temp2 = 0;

                   long aa;
                   long b;
                   //We start Data1 from i+BufferSize to investigate the corelation also in backward direction )if i and j-0) there would be only forward direction)
                   //Do corelation of two strings the second string has scaned position. The area to corelate is defined by CorrSize (smaller faster and so on) 
                   for (uint t = 0; t < 1024 * 4; t += 1) //propably part of te full matrix is enough so divide by 2
                   {
                       sq = (aa = Data1[t + negative]) * Data2[b = (t + i)];// + Data1[jn+1] * Data2[ij+1];
                       temp += sq * sq;
                       sq2 = aa * Data2[b + NegPos / 2];// + Data1[jn+1] * Data2[ij+1];
                       temp2 += sq2 * sq2;
                   }
                   CorelateArray[i] = temp;
                   CorelateArray[i + NegPos / 2] = temp2;
               });
        }

        //Init(uint size) must be initialized first!
        void FourierCorelate(int[] Data1, int[] Data2, float[] CorelateArray)
        {

            //Copy data
            uint a;
            for (uint i = 0; i < size / 2 - 1; i++)
            {
                f[i].Re = Data1[a = i * 2];
                f[i].Im = Data1[a + 1];

                g[i].Re = Data2[a];
                g[i].Im = Data2[a + 1];
            }

            //First fourier transform  both data1 and data2
            //f = Data1(t),   g = Data2(t)
            //F(f)   and  F(g)
            //Multiply H = F(f) * F(g) in frequency domain
            //Reverse Fourier transform of the product F^-1 (H)

            fft.CalcFFT(f, ref Ff, true);
            fft.CalcFFT(g, ref Fg, true);


            //Perform two tasks in parallel on the source array
            // Parallel.Invoke(
            //     () =>
            //     {
            //         uint a;
            //         for (uint i = 0; i < size / 2 - 1; i++)
            //         {
            //             f[i].Re = Data1[a = i * 2];
            //             f[i].Im = Data1[a + 1];
            //         }
            //         fft.CalcFFT(f, ref Ff, true);
            //     },

            //     () =>
            //     {
            //         uint a;
            //         for (uint i = 0; i < size / 2 - 1; i++)
            //         {
            //             g[i].Re = Data2[a = i * 2];
            //             g[i].Im = Data2[a + 1];
            //         }
            //         fft.CalcFFT(g, ref Fg, true);
            //     }

            //); //close parallel.invoke


            //Multiply both transforms and place it in product H
            // (a + ib)(c + id) = (ac - bd) + i(ad + bc).
            for (int i = 0; i < size; i++)
            {
                H[i].Re = Ff[i].Re * Fg[i].Re - Ff[i].Im * Fg[i].Im;
                H[i].Im = Ff[i].Re * Fg[i].Im + Ff[i].Im * Fg[i].Re;
            }

            //Reverse fft for H
            fft.CalcFFT(H, ref rH, false);

            //Copy data module
            for (int i = 0; i < size; i++)
            {
                CorelateArray[i] = rH[i].Re * rH[i].Re;
            }

        }

    }
}
