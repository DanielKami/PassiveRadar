using System;
using System.Threading;
using System.Windows.Forms;

namespace PasiveRadar
{

    public unsafe partial class Form1
    {

        Thread thread_calculate_draw_data = null;
        volatile bool calculate_draw_data_exit = false;
        volatile bool calculate_draw_data_exited = false;

        Thread thread_calculate_radar = null;
        volatile bool calculate_radar_exit = false;
        volatile bool calculate_radar_exited = false;

        private readonly Object LockRadar = new Object();
        public AutoResetEvent DrawEvent2 = new AutoResetEvent(false);
        public AutoResetEvent DrawEvent = new AutoResetEvent(false);


        private bool resizing;

        WindowWave[] window_wave;
        WindowFlow[] window_flow;
        double[][] dataRadio;


        float[] dataRadar;
        double[] dataDifference;
        float[] PostProc;

        Complex[][] DataIn;

        Complex[][] DataOut;


        bool[] MouseDownPanel;
        int[] oldX_position;
        double[] MHz_perPixel;
        bool FilterMove = false;
        bool RightBandMove = false;
        bool LeftBandMove = false;


        public void StartDraw()
        {
            //Draw radar only the scale and crosses
            if (flags.showRadar0)
                windowRadar.RenderRadar(PostProc, true);

            calculate_draw_data_exit = false;
            calculate_draw_data_exited = false;

            if (thread_calculate_draw_data == null)
            {
                thread_calculate_draw_data = new Thread(new ThreadStart(CalculateDataForScenes));
                thread_calculate_draw_data.Priority = System.Threading.ThreadPriority.Normal;
                thread_calculate_draw_data.Start();
            }

            calculate_radar_exit = false;
            calculate_radar_exited = false;

            if (thread_calculate_radar == null)
            {
                thread_calculate_radar = new Thread(new ThreadStart(CalculateRadarScene));
                thread_calculate_radar.Priority = System.Threading.ThreadPriority.Normal;
                thread_calculate_radar.Start();
            }
        }

        public void StopDraw()
        {
            //Stop thread calculate data for scenes
            if (thread_calculate_draw_data != null)
            {
                calculate_draw_data_exit = true;
                while (!calculate_draw_data_exited) ;

                thread_calculate_draw_data = null;
            }

            ////Stop thread calculate data for radar
            if (thread_calculate_radar != null)
            {
                calculate_radar_exit = true;
                while (!calculate_radar_exited) ;
                thread_calculate_radar = null;
            }
        }

        private void CalculateDataForScenes()
        {

            DataIn = new Complex[Flags.MAX_DONGLES][];
            for (int i = 0; i < Flags.MAX_DONGLES; i++)
                DataIn[i] = new Complex[flags.BufferSizeRadio[i]];//+ flags.Negative + flags.Positive

            DataOut = new Complex[Flags.MAX_DONGLES][];
            for (int i = 0; i < Flags.MAX_DONGLES; i++)
                DataOut[i] = new Complex[flags.BufferSizeRadio[i]];


            while (!calculate_draw_data_exit)
            {
                DrawAll();
            }
            calculate_draw_data_exited = true;
        }

        //Calculate the scenes and draw them
        void DrawAll()
        {
            int i;

            //Slow it down a bit 
            if (flags.refresh_delay > 0)
                System.Threading.Thread.Sleep(flags.refresh_delay);


            //Start FFT calculations
            for (i = 0; i < Flags.MAX_DONGLES; i++)
            {
                if (rd[i].Visible & (flags.showRadioWave[i] | flags.showRadioFlow[i]))
                {
                    calculate[i].CopyToComplex(radio[i].dataIQ, ref DataIn[i], false);
                    calculate[i].FFT(DataIn[i], DataOut[i]);
                }
            }

            //Now the trick to calculate both FFT in the same time 
            //Wait until completed FFT calculations
            for (i = 0; i < Flags.MAX_DONGLES; i++)
                if (i == 0 || i == 1)
                {
                    if (rd[i].Visible & (flags.showRadioWave[i] || flags.showRadioFlow[i]))
                        calculate[i].FFTWaitToComplete();
                }

            //Start calculations  FFT data stream
            for (i = 0; i < Flags.MAX_DONGLES; i++)
                if (i == 0 || i == 1)
                {
                    if (rd[i].Visible & (flags.showRadioWave[i] || flags.showRadioFlow[i]))
                        calculate[i].Start(DataOut[i], dataRadio[i], flags.Cumulation[i], flags.Amplification[i]);
                }

            //Wait until completed calculations for streams
            for (i = 0; i < Flags.MAX_DONGLES; i++)
            {
                if (rd[i].Visible & (flags.showRadioWave[i] || flags.showRadioFlow[i]))
                    calculate[i].WaitToFinischCalc();
            }

            //Draw scenes waves
            for (i = 0; i < Flags.MAX_DONGLES; i++)
                if (rd[i].Visible & flags.showRadioWave[i])
                    window_wave[i].RenderWave(dataRadio[i]);

            //Draw scenes flows
            for (i = 0; i < Flags.MAX_DONGLES; i++)
                if (rd[i].Visible & flags.showRadioFlow[i])
                    window_flow[i].RenderFlow(dataRadio[i]);


        }

        private void CalculateRadarScene()
        {
            uint CorrelationShift = 0;
            float[] DataCorrelate = new float[flags.BufferSize + flags.Negative + flags.Positive];
            correlate.Init(flags.BufferSize);



            while (!calculate_radar_exit)
            {
                if (runing)
                {
                    //Copy
                    lock (LockRadar)
                    {
                        if (flags.BufferSize > dataOutRadio0.Length || flags.BufferSize > dataOutRadio1.Length)
                            return;

                        if (radio[0] != null)
                            for (int i = 0; i < flags.BufferSize; i++)
                                dataOutRadio0[i] = radio[0].dataIQ[i] - 128;

                        if (radio[1] != null & flags.TwoDonglesMode)
                            for (int i = 0; i < flags.BufferSize; i++)
                                dataOutRadio1[i] = radio[1].dataIQ[i] - 128;
                    }


                    //Correlate data streams
                    if (flags.TwoDonglesMode)
                        correlate.Begin(dataOutRadio0, dataOutRadio1, DataCorrelate, ref CorrelationShift, flags);


                    //Calculate ambiguity map extremally slow 10x
                    if (flags.showRadar0 & runing)
                        try
                        {
                            ambiguity.StartGPU(dataOutRadio0, dataOutRadio1, dataRadar, flags);
                            ambiguity.StopGPU();
                        }
                        catch (Exception ex)
                        {
                            String str = "Error ambiguity function. " + ex.ToString();
                            MessageBox.Show(str);
                            break;
                        }

                    //Average maps
                    if (flags.showRadar0)
                        radar_cumulate.Run(dataRadar, PostProc, flags.average);

                    if (flags.CorrectBackground)
                    {
                        if (!flags.FreezeBackground)
                            Regresion.Add(PostProc, flags.ColectEvery); //Add frames for regresion fit to find the best background correction, the second parameter describe how othen to add the element
                        Regresion.CorrectBackground(PostProc, flags.CorectionWeight);
                    }

                    //Draw radar
                    if (flags.showRadar0)
                        windowRadar.RenderRadar(PostProc, true);


                    //Correlate
                    if (flags.showCorrelateWave0)
                    {
                        windowCorrelateWave.RenderCorrelate(DataCorrelate, flags);
                        flags.Resynchronisation = false; //reset flag
                    }

                    if (flags.showCorrelateFlow0)
                    {
                        windowCorrelateFlow.RenderCorrelateFlow(DataCorrelate, CorrelationShift);
                    }

                    //Draw scene  radar background
                    if (flags.showBackground)
                    {
                        Regresion.Background(PostProc, flags);
                        windowBackground.RenderRadar(PostProc, false);
                    }
                }
                else
                    Thread.Sleep(100);
            }

            calculate_radar_exited = true; //Thread exit normally
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (WindowWave x in window_wave)
                x.OnLoadWindow();
            foreach (WindowFlow x in window_flow)
                x.OnLoadWindow();
            windowRadar.OnLoadWindow();
            windowBackground.OnLoadWindow();
            windowCorrelateWave.OnLoadWindow();
            windowCorrelateFlow.OnLoadWindow();
            resizing = false;
            UpdateAllScenesWhenRunning();

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            resizing = true;
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            // resizing = false;
            UpdateAllScenesWhenRunning();
        }

        //Size correction only for radio windows
        private void WindowsSizeCorection(int Nr)
        {
            StopDraw();//Thread stop
            if (rd[Nr].radio_resize == false)
            {
                window_wave[Nr].SizeChange();
                window_flow[Nr].SizeChange();
                UpdateAllScenes();

            }
            if (runing)
                StartDraw();//Thread start
        }

        //Size correction for all
        public void UpdateAllScenesWhenRunning()
        {
            StopDraw();//Thread stop
            if (resizing == false)
            {
                if (windowRadar != null) windowRadar.SizeChange();
                if (windowBackground != null) windowBackground.SizeChange();
                if (windowCorrelateWave != null) windowCorrelateWave.SizeChange();
                if (windowCorrelateFlow != null) windowCorrelateFlow.SizeChange();

                if (window_wave != null)
                    foreach (WindowWave x in window_wave)
                        if (x != null) x.SizeChange();

                if (window_flow != null)
                    foreach (WindowFlow x in window_flow)
                        if (x != null) x.SizeChange();

                //For scenes update when is not running
                UpdateAllScenes();
            }
            if (runing)
                StartDraw();//Thread start
        }

        //Scenes update when is not running
        void UpdateAllScenes()
        {
            if (resizing == false)
                if (FormsReady)
                {
                    StartDraw();
                    System.Threading.Thread.Sleep(20);
                    StopDraw();
                }
        }


        private void panelViewport3_MouseMove(object sender, MouseEventArgs e)
        {
            CalculateMouseMove(0, e.Location.X);
        }

        #region Mouse

        private void CalculateMouseUp(int Nr, int x)
        {
            MouseDownPanel[Nr] = false;
            UpdateFrequencies(Nr);
            FilterMove = false;
            LeftBandMove = false;
            RightBandMove = false;
            window_wave[Nr].panelViewport.Cursor = Cursors.Cross;
            UpdateFiltersAndScale(Nr);
        }

        private void CalculateMouseDown(int Nr, int x)
        {
            MouseDownPanel[Nr] = true;
            oldX_position[Nr] = x;
            //Find how many MHz is one pixel
            MHz_perPixel[Nr] = 1.0 / ((rd[Nr].panelRadioWave.Width - DrawWave.LeftMargin - DrawWave.RightMargin) / flags.rate[Nr]);

            float pos = (float)((x - DrawWave.LeftMargin) * MHz_perPixel[Nr] + flags.rate[Nr] / 2);
            float leftBand = (float)(flags.rate[Nr] - flags.FilterCentralFreq[Nr] - flags.RadioHalfBandwith);
            float rightBand = (float)(flags.rate[Nr] - flags.FilterCentralFreq[Nr] + flags.RadioHalfBandwith);
            if (pos > leftBand && pos < rightBand)
                FilterMove = true;
            if (pos > leftBand - 20000 && pos < leftBand)
                LeftBandMove = true;
            if (pos < rightBand + 20000 && pos > rightBand)
                RightBandMove = true;
        }

        private void CalculateMouseMove(int Nr, int x)
        {
            float half_rate = (float)flags.rate[Nr] / 2;
            MHz_perPixel[Nr] = 1.0 / ((rd[Nr].panelRadioWave.Width - DrawWave.LeftMargin - DrawWave.RightMargin) / flags.rate[Nr]);
            double PointedFrequency = (flags.frequency[Nr] - half_rate);
            float FrequencyAtCursorPosition = (float)(PointedFrequency + (x - DrawWave.LeftMargin) * MHz_perPixel[Nr]) / 1000000;

            if (MouseDownPanel[Nr] == false)
            {
                WindowsLocation(FrequencyAtCursorPosition);

                float pos = (float)((x - DrawWave.LeftMargin) * MHz_perPixel[Nr] + half_rate);
                float leftBand = (float)(flags.rate[Nr] - flags.FilterCentralFreq[Nr] - flags.RadioHalfBandwith);
                float rightBand = (float)(flags.rate[Nr] - flags.FilterCentralFreq[Nr] + flags.RadioHalfBandwith);

                if (pos > leftBand - 20000 && pos < leftBand)
                    window_wave[Nr].panelViewport.Cursor = Cursors.VSplit;
                else
                if (pos < rightBand + 20000 && pos > rightBand)
                    window_wave[Nr].panelViewport.Cursor = Cursors.VSplit;
                else
                    window_wave[Nr].panelViewport.Cursor = Cursors.Cross;
            }
            else
            {
                int delta = x - oldX_position[Nr];
                double s = delta * MHz_perPixel[Nr];

                if (FilterMove)
                {
                    flags.FilterCentralFreq[Nr] -= (float)s;
                    float band = (float)(flags.FilterCentralFreq[Nr] + half_rate);
                    if (band < 1) flags.FilterCentralFreq[Nr] = 1 - half_rate;
                    else if (band > flags.rate[Nr]) flags.FilterCentralFreq[Nr] = half_rate;
                    if (band >= 1 && band <= flags.rate[Nr])
                        oldX_position[Nr] = x;
                    UpdateFiltersAndScale(Nr);
                }
                if (LeftBandMove)
                {
                    flags.RadioHalfBandwith -= (float)s;
                    if (flags.RadioHalfBandwith < 10) flags.RadioHalfBandwith = 10;
                    else if (flags.RadioHalfBandwith > 250000 / 2) flags.RadioHalfBandwith = 250000 / 2;
                    if (flags.RadioHalfBandwith >= 10 && flags.RadioHalfBandwith < 250000 / 2)
                    {
                        oldX_position[Nr] = x;
                        UpdateFiltersAndScale(Nr);
                        //Cursor
                        window_wave[Nr].panelViewport.Cursor = Cursors.VSplit;
                    }
                }
                if (RightBandMove)
                {
                    flags.RadioHalfBandwith += (float)s;
                    if (flags.RadioHalfBandwith < 10) flags.RadioHalfBandwith = 10;
                    else if (flags.RadioHalfBandwith > 250000 / 2) flags.RadioHalfBandwith = 250000 / 2;
                    if (flags.RadioHalfBandwith >= 10 / 2 && flags.RadioHalfBandwith < 250000 / 2)
                    {
                        oldX_position[Nr] = x;
                        UpdateFiltersAndScale(Nr);
                        //Cursor
                        window_wave[Nr].panelViewport.Cursor = Cursors.VSplit;
                    }
                }
                if (!FilterMove & !LeftBandMove & !RightBandMove)
                {
                    flags.frequency[Nr] -= s;

                    if (flags.FREQUENCY_EQUAL)
                        for (int i = 0; i < Flags.MAX_DONGLES; i++)
                            flags.frequency[i] = flags.frequency[Nr];

                    UpdateFrequencies(Nr);
                    oldX_position[Nr] = x;
                }
            }
        }

        void UpdateFiltersAndScale(int Nr)//Nr-active radio at the moment
        {
            for (int i = 0; i < Flags.MAX_DONGLES; i++)
            {
                if (Nr != -1 && flags.FREQUENCY_EQUAL) flags.FilterCentralFreq[i] = flags.FilterCentralFreq[Nr];
                window_wave[i].Location(-1, (float)(flags.rate[i] / 2 - (flags.FilterCentralFreq[i] + flags.RadioHalfBandwith)) / 1000000, (float)(flags.rate[i] / 2 - (flags.FilterCentralFreq[i] - flags.RadioHalfBandwith)) / 1000000);
                window_flow[i].Location(-1);
            }

        }

        void WindowsLocation(float FrequencyAtCursorPosition)
        {
            windowBackground.Location(FrequencyAtCursorPosition);

            for (int i = 0; i < Flags.MAX_DONGLES; i++)
            {
                window_wave[i].Location(FrequencyAtCursorPosition, (float)(flags.rate[i] / 2 - (flags.FilterCentralFreq[i] + flags.RadioHalfBandwith)) / 1000000, (float)(flags.rate[i] / 2 - (flags.FilterCentralFreq[i] - flags.RadioHalfBandwith)) / 1000000);
            }
            foreach (WindowFlow x in window_flow)
                x.Location(FrequencyAtCursorPosition);
        }
        #endregion

    }
}
