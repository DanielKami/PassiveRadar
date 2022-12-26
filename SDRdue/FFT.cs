using System;
using System.Threading.Tasks;


namespace PasiveRadar
{
    public class FFT
    {
         uint nn;


        //  public Complex[][] FactorF;
        Complex[] Multiplier_forward;
        Complex[] Multiplier_backward;

        public uint[] Reverse;
        float sq;

        public FFT()
        {

        }

        public void Init(uint n)
        {
            nn = n;
            //FactorF = new Complex[nn][];
            //for (int i = 0; i < nn; i++)
            //{
            //    FactorF[i] = new SDRdue.Complex[nn];
            //}
            Multiplier_forward = new Complex[nn];
            Multiplier_backward = new Complex[nn];

            Reverse = new uint[nn];
            PrepareFFT();

        }

        public void PrepareFFT()
        {
            double sign = 1;
            double delta;
            double Sine;

            sq = (float)(1.0 / Math.Sqrt(nn));


            for (uint Step = 1; Step < nn; Step <<= 1)
            {
                //   Angle increment
                delta = Math.PI / Step * sign;
                //   Auxiliary sin(delta / 2)
                Sine = Math.Sin(delta * .5);
                //   Multiplier for trigonometric recurrence              
                Multiplier_forward[Step].Re = (float)(-2.0f * Sine * Sine);
                Multiplier_forward[Step].Im = (float)Math.Sin(delta);
            }

            //Backward FFT coeficient table
            sign = -1;
            for (uint Step = 1; Step < nn; Step <<= 1)
            {
                //   Angle increment
                delta = Math.PI / Step * sign;
                //   Auxiliary sin(delta / 2)
                Sine = Math.Sin(delta * .5);
                //   Multiplier for trigonometric recurrence              
                Multiplier_backward[Step].Re = (float)(-2.0f * Sine * Sine);
                Multiplier_backward[Step].Im = (float)Math.Sin(delta);
            }

            uint n, m, j, i;

            n = nn << 1;
            j = 1;// data[0] is not used

            for (i = 0; i < n; i += 2)
            {
                if (j > i)
                    Reverse[i / 2] = j;
                else
                    Reverse[i / 2] = 0;

                m = n >> 1;
                while (m >= 2 && j > m)
                {
                    j -= m;
                    m >>= 1;
                }
                j += m;
            }

            for (i = 0; i < nn; i++)
                Reverse[i]/=2;

                ////FactorF
                //Complex Factor = new Complex(0, 0), Product = new Complex(0, 0);

                ////   Iteration through dyads, quadruples, octads and so on...
                //for (uint Step = 1; Step < nn; Step <<= 1)
                //{
                //    //   Start value for transform factor, fi = 0
                //    // Factor=1;
                //    Factor.Re = 1;
                //    Factor.Im = 0;
                //    FactorF[Step][0] = Factor;
                //    //   Iteration through groups of different transform factor
                //    for (uint Group = 0; Group < Step; ++Group)
                //    {
                //        FactorF[Step][Group] = Factor = Multiplier[Step] * Factor + Factor;
                //    }
                //}
            }

        ////Forward_Backward FFT calculation flag
        public void CalcFFT(Complex[] Data_In, ref Complex[] Data_Out, bool Forward_Backward = true)
        {

            uint j, i;

            for (i = 0; i < nn; i++)
            {
                j = Reverse[i];

                if (j > 0)
                {
                    Data_Out[j] = Data_In[i];
                    Data_Out[i] = Data_In[j];
                }
            }


            if (Forward_Backward)
            {
                PerformFFTforward(Data_Out);
                for (i = 0; i < nn; i++)
                    Data_Out[i] *= sq;
            }
            else
                PerformFFTbackward(Data_Out);
        }


        private void PerformFFTforward(Complex[] Data)
        {
            Complex Factor = new Complex(0, 0);
            Complex Product = new Complex(0, 0);

            uint Step, Pair, Jump, Match;
            uint Group;

            //   Iteration through dyads, quadruples, octads and so on...
            for (Step = 1; Step < nn; Step <<= 1)
            {
                //   Jump to the next entry of the same transform factor
                Jump = Step << 1;

                //Start value for transform factor, fi = 0
                Factor.Re = 1;
                Factor.Im = 0;

                //   Iteration through groups of different transform factor
                for (Group = 0; Group < Step; ++Group)
                {
                    //Factor = FactorF[Step][Group];
                    
                    //   Iteration within group 
                    for (Pair = Group; Pair < nn; Pair += Jump)
                    {
                        unsafe
                        {
                            //   Match position
                            //   Second term of two-point transform
                            Product = Factor * Data[Match = Pair + Step];
                            //   Transform for fi + pi
                            Data[Match] = Data[Pair] - Product;
                            //   Transform for fi
                            Data[Pair] += Product;
                        }
                    }
                    //Successive transform factor via trigonometric recurrence
                    //FactorF[Step, Group] = Factor = Multiplier[Step] * Factor + Factor;
                    Factor = Multiplier_forward[Step] * Factor + Factor;
                }
            }
        }

        private void PerformFFTbackward(Complex[] Data)
        {
            Complex Factor = new Complex(0, 0);
            Complex Product = new Complex(0, 0);

            uint Step, Pair, Jump, Match;
            uint Group;

            //   Iteration through dyads, quadruples, octads and so on...
            for (Step = 1; Step < nn; Step <<= 1)
            {
                //   Jump to the next entry of the same transform factor
                Jump = Step << 1;

                //Start value for transform factor, fi = 0
                Factor.Re = 1;
                Factor.Im = 0;

                //   Iteration through groups of different transform factor
                for (Group = 0; Group < Step; ++Group)
                {
                    //Factor = FactorF[Step][Group];

                    //   Iteration within group 
                    for (Pair = Group; Pair < nn; Pair += Jump)
                    {
                        //   Match position
                        //   Second term of two-point transform
                        Product = Factor * Data[Match = Pair + Step];
                        //   Transform for fi + pi
                        Data[Match] = Data[Pair] - Product;
                        //   Transform for fi
                        Data[Pair] += Product;
                    }
                    //Successive transform factor via trigonometric recurrence
                    //FactorF[Step, Group] = Factor = Multiplier[Step] * Factor + Factor;
                    Factor = Multiplier_backward[Step] * Factor + Factor;
                }
            }
        }



    }


}