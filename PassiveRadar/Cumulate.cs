namespace PasiveRadar
{
    class RadarCumulate
    {
        uint cumulateIndex;
        uint Columns;
        uint Rows;
        uint MaxAverage;

        float[] CumulateBuffer;

        public void Init(Flags flags)
        {
            Columns = flags.Columns;
            Rows = flags.Rows;
            MaxAverage = flags.MaxAverage;
            CumulateBuffer = new float[(MaxAverage + 1) * (Columns * Rows)];
        }

        public void Run(float[] data, float[] PostProc, int average)
        {
            cumulateIndex++;
            if (cumulateIndex >= average) cumulateIndex = 0;
            uint Frame = Columns * Rows;
            uint CCR = cumulateIndex * Frame;

            //Protection just in case
            if (CumulateBuffer.Length < cumulateIndex * Frame || data.Length < Frame) return;

            //Cumulation
            for (uint i = 0; i < Frame; i++)
                CumulateBuffer[CCR + i] = data[i];

            //Averaging
            float one_ave = 1.0f / average;

            //Clera the output
            System.Array.Clear(PostProc, 0, (int)Frame);

            for (uint j = 0; j < average; j++)
            {
                long jF = j * Frame;
                for (uint i = 0; i < Frame; i++)
                    PostProc[i] += CumulateBuffer[i + jF];
            }

            //It is faster to do one more loop
            for (uint i = 0; i < Frame; i++)
                PostProc[i] *=  255 * one_ave; //255 is for graphics normalisation

        }
    }
}
