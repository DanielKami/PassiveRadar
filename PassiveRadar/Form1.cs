using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PasiveRadar
{
    public unsafe partial class Form1 : Form
    {
        public int cumulation, cumulation_max;
        private readonly Object LockMem = new Object();


        Flags flags;
        Find find;
        Radio[] radio;
        Calculate[] calculate;
        Correlate correlate;
        RadarCumulate radar_cumulate;
        Ambiguity ambiguity;
        ClassRegresion Regresion;
        Settings[] set;
        WindowsRadio[] rd;
        Window windowRadar, windowBackground, windowCorrelateWave, windowCorrelateFlow;


        int[] dataOutRadio0;
        int[] dataOutRadio1;

        bool FormsReady = false;
        bool runing = false;

        //Sending flags info to all clases
        public delegate void MyDelegate(Flags flags);
        public static event MyDelegate FlagsDelegate;

        public Form1()
        {
            InitializeComponent();
            //           UsbNotification.RegisterUsbDeviceNotification(this.Handle);

            //Initialize all flags and values for controls
            flags = new Flags();
            flags.ColorThemeTable = new Microsoft.Xna.Framework.Color[Draw.ColorTableSize];
            flags.Read();
            flags.AMDdriver = FindAMD();//Detect AMD video driver



            ButtonFreqEqual(); //Set the correct icone and values for frequency
            RadarMode();

            //Mouse position parameters
            MouseDownPanel = new bool[Flags.MAX_DONGLES];
            oldX_position = new int[Flags.MAX_DONGLES];
            MHz_perPixel = new double[Flags.MAX_DONGLES];


            if (flags.FREQUENCY_EQUAL)
                for (int i = 1; i < Flags.MAX_DONGLES; i++)
                    flags.frequency[i] = flags.frequency[0];

            string[] str = new string[16];
            find = new Find();
            find.Device();

            radio = new Radio[Flags.MAX_DONGLES];
            for (int i = 0; i < Flags.MAX_DONGLES; i++)
                radio[i] = new Radio();



            #region Windows
            windowRadar = new Window(panelViewport2, 0);
            windowBackground = new Window(panelViewport3, 0);

            windowCorrelateWave = new Window(panelViewport6, 1);
            windowCorrelateFlow = new Window(panelViewport7, 1);

            window_wave = new WindowWave[Flags.MAX_DONGLES];
            window_flow = new WindowFlow[Flags.MAX_DONGLES];

            rd = new WindowsRadio[Flags.MAX_DONGLES];

            for (int i = 0; i < Flags.MAX_DONGLES; i++)
            {
                rd[i] = new WindowsRadio(i);
                rd[i].Show();
                rd[i].tuningNumber.frequency = (int)flags.frequency[i];
                rd[i].tuningNumber.Update_(false);
                rd[i].Initialize(flags);

                window_wave[i] = new WindowWave(rd[i].panelRadioWave, i);
                window_flow[i] = new WindowFlow(rd[i].panelRadioFlow, i);
            }
            #endregion
            //No dongles no two dongles that is easy
            flags.TwoDonglesMode = false;

            radar_cumulate = new RadarCumulate();
            ambiguity = new Ambiguity();

            calculate = new Calculate[Flags.MAX_DONGLES];
            for (int i = 0; i < Flags.MAX_DONGLES; i++)
                calculate[i] = new Calculate();

            Regresion = new ClassRegresion();

            correlate = new Correlate();
            InitBuffers();

            #region Setting windows
            set = new Settings[Flags.MAX_DONGLES];
            for (int i = 0; i < Flags.MAX_DONGLES; i++)
                set[i] = new Settings(i);

            for (int i = 0; i < Flags.MAX_DONGLES; i++)
            {
                if (radio[i] != null)
                {
                    radio[i].rate = (uint)flags.rate[i];
                    set[i].SetSettings(radio[i]);
                }
            }

            #endregion

            #region Events

            //User control settings
            Settings.EventGain += new Settings.MyDelegateSettings(ReturnRadioSettings);
            DisplayControl.EventSettings += new DisplayControl.MyDelegate(DisplaySettings);
            RadarControl.RadarSettings += new RadarControl.MyDelegate(RadarSettings);
            CorrelateControl.EventSettings += new CorrelateControl.MyDelegate(CorrelationSettings);



            ///Radio data ready
            Settings.EventRadio += new Settings.MyDelegate(AddRadio);

            //WindowsRadio size changed
            WindowsRadio.SizeChangedx += new WindowsRadio.DelegateEvents(WindowsSizeCorection);

            //Radio Set pressed
            WindowsRadio.RadioSet += new WindowsRadio.DelegateEvents(WindowsSetPressed);

            //Windows radio mouse event
            WindowsRadio.RadioMouseDown += new WindowsRadio.DelegateMouse(CalculateMouseDown);
            WindowsRadio.RadioMouseMove += new WindowsRadio.DelegateMouse(CalculateMouseMove);
            WindowsRadio.RadioMouseUp += new WindowsRadio.DelegateMouse(CalculateMouseUp);

            //Window radio frequency changed
            WindowsRadio.RadioFrequencyChanged += new WindowsRadio.DelegateEvents(WindowRadioFrequencyChanged);

            //Window radio settings
            WindowsRadio.EventSettings += new WindowsRadio.MyDelegate(DisplaySettings2);

            //Color changes
            ColorForm.EventColor += new ColorForm.MyDelegate(SetCustomColor);

            //Resunchronise correation
            Correlate.Resynchronise += new Correlate.MyDelegate(Resynchronise);

            #endregion
            if (flags.TwoDonglesMode == false)
            {
                flags.showRadar0 = false;
                flags.showBackground = false;
            }

            //Update all user controls
            UpdateFrequencies(flags.LastActiveWindowRadio);
            if (FlagsDelegate != null)
                FlagsDelegate(flags);

            InitSiteMenu();
        }

        private void WindowRadioFrequencyChanged(int Nr)
        {
            flags.frequency[Nr] = rd[Nr].tuningNumber.frequency + (int)flags.FilterCentralFreq[Nr];
            if (flags.FREQUENCY_EQUAL)
            {
                //Update in windows
                for (int i = 0; i < Flags.MAX_DONGLES; i++)
                {
                    if (i != Nr)
                    {
                        flags.frequency[i] = flags.frequency[Nr];
                        rd[i].tuningNumber.frequency = (int)flags.frequency[Nr];
                        rd[i].tuningNumber.Update_(false);
                    }
                }
            }

            if (radio == null) return;

            for (int i = 0; i < Flags.MAX_DONGLES; i++)
            {
                if (radio[i] != null)
                {
                    radio[i].frequency = (int)flags.frequency[i];
                    radio[i].SetCentralFreq();
                }
            }

            WindowsUpdate();
        }

        private void SetCustomColor(System.Drawing.Color[] col)
        {
            for (int i = 0; i < Draw.ColorTableSize; i++)
            {
                //Convert colors from system to XNA
                flags.ColorThemeTable[i].R = col[i].R;
                flags.ColorThemeTable[i].G = col[i].G;
                flags.ColorThemeTable[i].B = col[i].B;
                flags.ColorThemeTable[i].A = 255;
            }
            //WindowsUpdate();
            windowRadar.Update(flags);
            windowBackground.Update(flags);
            windowCorrelateFlow.Update(flags);
            foreach (WindowFlow x in window_flow)
                if (x != null) x.Update(flags);
        }

        //Find AMD driver, if yes we have OpenCL support 
        bool FindAMD()
        {
            System.Management.SelectQuery query = new System.Management.SelectQuery("Win32_SystemDriver");
            query.Condition = "Name = 'amdkmafd'";//Name of AMD driver
            System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher(query);
            var drivers = searcher.Get();

            if (drivers.Count > 0) return true;
            else return false;
        }

        //USB support needs more selectivity a simple way
        protected override void WndProc(ref Message m)
        {
            int devType;

            base.WndProc(ref m);
            if (m.Msg == UsbNotification.WmDevicechange)
            {
                switch ((int)m.WParam)
                {
                    case UsbNotification.DbtDeviceremovecomplete:

                        devType = Marshal.ReadInt32(m.LParam, 4);
                        //MessageBox.Show("MEDIArem");
                        //No radio 0 and 1
                        StopDraw();

                        for (int i = 0; i < Flags.MAX_DONGLES; i++)
                        {
                            if (radio[i] != null)
                            {
                                radio[i].Stop();
                                radio[i].Close();
                            }
                            set[i].ComboBoxRadio_Update(ref find);
                        }

                        find.NrOfDevices = 1;


                        break;

                    case UsbNotification.DbtDevicearrival:
                        {
                            devType = Marshal.ReadInt32(m.LParam, 4);
                            if (devType == 5)
                            {
                                String str = "New usb device found";
                                MessageBox.Show(str);
                            }
                            //No radio 0 and 1
                            StopDraw();

                            for (int i = 0; i < Flags.MAX_DONGLES; i++)
                            {
                                if (radio[i] != null)
                                {
                                    radio[i].Stop();
                                    radio[i].Close();
                                }
                            }

                            if (!radio[0].status || !radio[1].status)
                            {
                                find.Device();
                                for (int i = 0; i < Flags.MAX_DONGLES; i++)
                                    set[i].ComboBoxRadio_Update(ref find);
                            }

                        }
                        break;
                }
            }
        }


        private void InitBuffers()
        {
            //lock (LockMem)
            {
                for (int i = 0; i < Flags.MAX_DONGLES; i++)
                {
                    if (radio[i] != null)
                    {
                        radio[i].InitBuffers(flags);
                    }
                }

                dataRadio = new double[Flags.MAX_DONGLES][];
                for (int i = 0; i < Flags.MAX_DONGLES; i++)
                    dataRadio[i] = new double[flags.BufferSize];

                dataRadar = new float[flags.Columns * flags.Rows + flags.Columns];
                dataDifference = new double[flags.BufferSizeRadio[0] + 1];

                for (int i = 0; i < Flags.MAX_DONGLES; i++)
                    calculate[i].Init(flags.BufferSizeRadio[i], flags.Cumulation[i]);


                radar_cumulate.Init(flags);
                //uint BufferNegPos = flags.BufferSize + (flags.Negative + flags.Positive) * 2;
                uint BufferNegPos = flags.BufferSize + (50000 + 50000) * 2;
              
                dataOutRadio0 = new int[BufferNegPos];
                dataOutRadio1 = new int[BufferNegPos];

                PostProc = new float[flags.Columns * flags.Rows];
                
                ambiguity.Release(flags);  
                ambiguity.Prepare(flags);
                Regresion.Initiate(flags);
            }
        }

        private void Resynchronise()
        {
            flags.Resynchronisation = true;
            StopAllRadios();
            // System.Threading.Thread.Sleep(10);
            StartAllRadios();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (find.NrOfDevices == 0) return;
            if (runing == false)
            {
                radarControl1.ActiveDeactivateColumnsControll(false);
                //init buffers just in case
                InitBuffers();
 
                if (CheckTheCorrelationRadarRate())
                {
                    runing = true;
                    StartAllRadios();
                    StartDraw();
                    button1.ImageIndex = 1;
                }
            }
            else
            {
                if (find.NrOfDevices == 0) return;
                radarControl1.ActiveDeactivateColumnsControll(true);
                StopAllThreads();
                StopAllRadios();
                button1.ImageIndex = 0;
                runing = false;
            }
        }

        void StopAllRadios()
        {
            for (int i = 0; i < Flags.MAX_DONGLES; i++)
            {
                if (radio[i] != null)
                {
                    radio[i].Stop();
                }
            }
        }

        void StartAllRadios()
        {
            for (int i = 0; i < Flags.MAX_DONGLES; i++)
            {
                if (radio[i] != null)
                {
                    radio[i].Start();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Windwos position
            SaveState();

            StopDraw();
            ambiguity.Release(flags); //OpenCL AMD clear and close

            if (find.NrOfDevices == 0) return;
            if (radio != null)
                for (int i = 0; i < Flags.MAX_DONGLES; i++)
                {
                    if (radio[i] != null)
                    {
                        radio[i].Stop();
                        radio[i].Close();
                    }
                }

            windowRadar.service.ResetingDevice();
            windowBackground.service.ResetingDevice();
            windowCorrelateWave.service.ResetingDevice();
            windowCorrelateFlow.service.ResetingDevice();

            foreach (WindowWave x in window_wave)
                x.service.ResetingDevice();
            foreach (WindowFlow x in window_flow)
                x.service.ResetingDevice();

            flags.Save();
        }

        #region Windwos position
        private void SaveState()
        {
            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.MainFormLocation = Location;
                Properties.Settings.Default.MainFormSize = Size;
            }
            else
            {
                Properties.Settings.Default.MainFormLocation = RestoreBounds.Location;
                Properties.Settings.Default.MainFormSize = RestoreBounds.Size;
            }
            Properties.Settings.Default.MainFormState = WindowState;
            Properties.Settings.Default.Save();

            //Radio windows
            Properties.Settings.Default.ListFormLocation = "";
            Properties.Settings.Default.ListFormSize = "";
            Properties.Settings.Default.ListFormState = "";
            Properties.Settings.Default.ListFormShow = "";

            for (int i = 0; i < Flags.MAX_DONGLES; i++)
            {
                if (rd[i].WindowState == FormWindowState.Normal)
                {
                    Properties.Settings.Default.ListFormLocation += rd[i].Location.X + "," + rd[i].Location.Y + ",";
                    Properties.Settings.Default.ListFormSize += rd[i].Size.Width + "," + rd[i].Size.Height + ",";
                }
                else
                {
                    Properties.Settings.Default.ListFormLocation += rd[i].RestoreBounds.X + "," + rd[i].RestoreBounds.Y + ",";
                    Properties.Settings.Default.ListFormSize += rd[i].RestoreBounds.Size.Width + "," + rd[i].RestoreBounds.Size.Height + ",";
                }
                Properties.Settings.Default.ListFormState += rd[i].WindowState + ",";
                Properties.Settings.Default.ListFormShow += rd[i].Visible + ",";
            }
            Properties.Settings.Default.Save();
        }

        private void RestoreState()
        {
            if (Properties.Settings.Default.MainFormSize == new Size(0, 0))
            {
                return; // state has never been saved
            }
            StartPosition = FormStartPosition.Manual;
            Location = Properties.Settings.Default.MainFormLocation;
            Size = Properties.Settings.Default.MainFormSize;
            // I don't like an app to be restored minimized, even if I closed it that way
            WindowState = Properties.Settings.Default.MainFormState ==
              FormWindowState.Minimized ? FormWindowState.Normal : Properties.Settings.Default.MainFormState;

            //Radio Windows
            //Position
            List<Point> PositionList = ConvertStringToPoint(Properties.Settings.Default.ListFormLocation);
            if (PositionList.Count > 0)
                for (int i = 0; i < Flags.MAX_DONGLES; i++)
                {
                    if (i < PositionList.Count)
                    {
                        rd[i].StartPosition = FormStartPosition.Manual;
                        rd[i].Location = PositionList[i];
                    }
                }

            //Size
            List<Size> SizeList = ConvertStringToSize(Properties.Settings.Default.ListFormSize);
            if (SizeList.Count > 0)
                for (int i = 0; i < Flags.MAX_DONGLES; i++)
                {
                    if (i < SizeList.Count)
                    {
                        rd[i].Size = SizeList[i];
                    }
                }

            //State
            List<FormWindowState> StateList = ConvertStringToFormWindowState(Properties.Settings.Default.ListFormState);
            if (StateList.Count > 0)
                for (int i = 0; i < Flags.MAX_DONGLES; i++)
                {
                    if (i < StateList.Count)
                    {
                        rd[i].WindowState = StateList[i];
                    }
                }

            List<bool> ShowList = ConvertStringToBolean(Properties.Settings.Default.ListFormShow);
            if (ShowList.Count > 0)
                for (int i = 0; i < Flags.MAX_DONGLES; i++)
                {
                    if (i < ShowList.Count)
                    {
                        rd[i].Visible = ShowList[i];
                    }
                }
        }

        List<bool> ConvertStringToBolean(string Str)
        {
            List<bool> S = new List<bool>();

            if (Str != null && Str.Length > 0)
            {
                foreach (string txt in Str.Split(','))
                    if (txt != "") S.Add(bool.Parse(txt));
            }
            return S;

        }

        List<Point> ConvertStringToPoint(string Str)
        {
            List<Point> P = new List<Point>();
            List<int> arr = new List<int>();

            if (Str != null && Str.Length > 1)
            {
                foreach (string txt in Str.Split(','))
                    if (txt != "") arr.Add(int.Parse(txt));

                for (int i = 0; i < Flags.MAX_DONGLES; i++)
                    if (i * 2 < arr.Count - 1)
                        P.Add(new Point(arr[i * 2], arr[i * 2 + 1]));
            }
            return P;
        }

        List<Size> ConvertStringToSize(string Str)
        {
            List<Size> S = new List<Size>();
            List<int> arr = new List<int>();

            if (Str != null && Str.Length > 1)
            {
                foreach (string txt in Str.Split(','))
                    if (txt != "") arr.Add(int.Parse(txt));
                //Now use ints to create List of Sizes
                for (int i = 0; i < Flags.MAX_DONGLES; i++)
                    if (i * 2 < arr.Count - 1)
                        S.Add(new Size(arr[i * 2], arr[i * 2 + 1]));
            }
            return S;
        }

        List<FormWindowState> ConvertStringToFormWindowState(string Str)
        {
            List<FormWindowState> S = new List<FormWindowState>();

            if (Str != null && Str.Length > 0)
            {
                foreach (string txt in Str.Split(','))
                    if (txt != "") S.Add((FormWindowState)Enum.Parse(typeof(FormWindowState), txt));
            }
            return S;
        }
        #endregion

        private void panelViewport_MouseLeave(object sender, EventArgs e)
        {
            windowRadar.Location(-1);
            foreach (WindowWave x in window_wave)
                x.Location(-1, -1, -1);
            foreach (WindowFlow x in window_flow)
                x.Location(-1);
        }

        public void DisplaySettings2(int Nr, Flags LocalFlags)
        {
            flags.showRadioWave[Nr] = LocalFlags.showRadioWave[Nr];
            flags.showRadioFlow[Nr] = LocalFlags.showRadioFlow[Nr];
            flags.Amplification[Nr] = LocalFlags.Amplification[Nr];
            flags.Cumulation[Nr] = LocalFlags.Cumulation[Nr];
            flags.Level[Nr] = LocalFlags.Level[Nr];

            if (flags.BufferSizeRadio != LocalFlags.BufferSizeRadio)
            {
                StopDraw();
                StopAllRadios();
                flags.BufferSizeRadio = LocalFlags.BufferSizeRadio;
                InitBuffers();
                if (runing)
                {
                    StartAllRadios();
                    StartDraw();
                }
            }

            WindowsUpdate();
        }

        public void DisplaySettings(Flags LocalFlags)
        {
            flags.showRadar0 = LocalFlags.showRadar0;
            flags.showBackground = LocalFlags.showBackground;
            flags.showCorrelateWave0 = LocalFlags.showCorrelateWave0;
            flags.showCorrelateFlow0 = LocalFlags.showCorrelateFlow0;

            flags.ColorTheme = LocalFlags.ColorTheme;
            flags.refresh_delay = LocalFlags.refresh_delay;
            flags.Radio_buffer_size = LocalFlags.Radio_buffer_size;
            displayControl1.UpdateTable(flags);


            WindowsUpdate();
        }

        bool CheckTheCorrelationRadarRate()
        {
            bool res = true;
            if ((flags.showRadar0 || flags.showCorrelateFlow0 || flags.showCorrelateWave0) && flags.rate[0] != flags.rate[1])
            {
                flags.showRadar0 = false;
                flags.showCorrelateWave0 = false;
                flags.showCorrelateFlow0 = false;
                displayControl1.FalseCheckBoxes();
                res = false;
                StopAllThreads();
                StopAllRadios();
                MessageBox.Show("The radio 0 rate is different than the radio 1. Make them equal to start correlation or /and radar. ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                button1.ImageIndex = 0;
                runing = false;
            }
            return res;
        }

        /// <summary>
        /// Apply the radar settings from Radar settings panel
        /// </summary>
        /// <param name="LocalFlags"></param>
        private void RadarSettings(Flags LocalFlags)
        {
           // lock (LockMem)
            {
                flags.PasiveGain = LocalFlags.PasiveGain;
                flags.remove_symetrics = LocalFlags.remove_symetrics;
                flags.average = LocalFlags.average;
                flags.CorrectBackground = LocalFlags.CorrectBackground;
                flags.ColectEvery = LocalFlags.ColectEvery;
                flags.CorectionWeight = LocalFlags.CorectionWeight;
                flags.DistanceShift = LocalFlags.DistanceShift;
                flags.FreezeBackground = LocalFlags.FreezeBackground;
                flags.DopplerZoom = LocalFlags.DopplerZoom;
                flags.NrCorrectionPoints = LocalFlags.NrCorrectionPoints;
                flags.scale_type = LocalFlags.scale_type;
                flags.alpha = LocalFlags.alpha;


                if (runing == true)  //on running
                {

                    if (flags.BufferSize != LocalFlags.BufferSize ||
                        flags.Columns != LocalFlags.Columns ||
                        flags.Rows != LocalFlags.Rows ||
                        flags.TwoDonglesMode != LocalFlags.TwoDonglesMode ||                        
                        flags.NrCorrectionPoints != LocalFlags.NrCorrectionPoints
                        )
                    {
                        StopDraw();
                        radio[0].Stop();
                        radio[1].Stop();

                        flags.Columns = LocalFlags.Columns;
                        flags.Rows = LocalFlags.Rows;
                        flags.TwoDonglesMode = LocalFlags.TwoDonglesMode;
                        flags.NrCorrectionPoints = LocalFlags.NrCorrectionPoints;
                        flags.BufferSize = LocalFlags.BufferSize;

                        InitBuffers();

                        radio[0].Start();
                        radio[1].Start();
                        StartDraw();
                    }

                }
                else
                {
                    if (flags.BufferSize != LocalFlags.BufferSize ||
                        flags.Columns != LocalFlags.Columns ||
                        flags.Rows != LocalFlags.Rows ||
                        flags.DopplerZoom != LocalFlags.DopplerZoom ||
                        flags.TwoDonglesMode != LocalFlags.TwoDonglesMode ||
                        flags.OpenCL != LocalFlags.OpenCL ||
                        flags.NrCorrectionPoints != LocalFlags.NrCorrectionPoints)
                    {
                        StopAllThreads();
                        radio[0].Stop();
                        radio[1].Stop();

                        flags.Columns = LocalFlags.Columns;
                        flags.Rows = LocalFlags.Rows;
                        flags.DopplerZoom = LocalFlags.DopplerZoom;           
                        flags.TwoDonglesMode = LocalFlags.TwoDonglesMode;
                        flags.NrCorrectionPoints = LocalFlags.NrCorrectionPoints;
                        flags.scale_type = LocalFlags.scale_type;
                        flags.BufferSize = LocalFlags.BufferSize;
                        InitBuffers();
                    }
                }
                WindowsUpdate();//To correct the scale and so on
            }
        }

        private void RadarMode()
        {
            if (flags.showRadar0 || flags.showCorrelateWave0 || flags.showCorrelateFlow0)
            {
                //Reduce bandwith to the the highest working one
                if (flags.rate[0] > 2560000) flags.rate[0] = 2560000;
                //Set 1 and 2 radios to the same bandwith and buffer size
                if (flags.rate[1] != flags.rate[0] || flags.frequency[1] != flags.frequency[0])
                {
                    //  StopAllThreads();
                    flags.rate[1] = flags.rate[0];
                    flags.frequency[1] = flags.frequency[0];
                }
            }
        }

        private int RadioCount()
        {
            int count = 0;

            if (radio[0].status)
                count++;
            if (radio[1].status)
                count++;
            label3.Text = "Nr. recivers: " + count;

            if (count == 0)
                label3.Text = "Select reciver! ";

            flags.Nr_active_radio = count;//must  be before update all windows
            if (FlagsDelegate != null)
                FlagsDelegate(flags);

            return count;
        }

        private void CorrelationSettings(Flags LocalFlags)
        {
            StopDraw();
            lock (LockMem)
            {
                flags.Negative = LocalFlags.Negative;
                flags.Positive = LocalFlags.Positive;
                flags.AcceptedLevel = LocalFlags.AcceptedLevel;
                flags.CorrelateAmplitude = LocalFlags.CorrelateAmplitude;
                flags.CorrelateLevel = LocalFlags.CorrelateLevel;
                flags.AutoCorrelate = LocalFlags.AutoCorrelate;
            }
            WindowsUpdate();//To correct the scale and so on
            if (runing)
                StartDraw();
        }

        void WindowsSetPressed(int Nr)
        {
            UpdateSet(Nr);
            //  flags.Nr_active_radio=RadioCount();

        }

        private void StopAllThreads()
        {
            StopDraw();
        }

        private void AddRadio(int WindowRadio, int Reciver)
        {
            string str = "None";
            lock (LockMem)
            {
                runing = false;
                button1.ImageIndex = 0;
                StopAllThreads();
                if (radio[WindowRadio] != null)
                {
                    radio[WindowRadio].Stop();
                    radio[WindowRadio].Close();
                    //First check if other Radio posses the dongle (item) and free it
                    for (int i = 0; i < Flags.MAX_DONGLES; i++)

                        if (radio[i].item == Reciver)
                        {
                            radio[i].Stop();
                            radio[i].Close();
                            find.StatusList[radio[i].item] = 0;
                            set[i].itm = 0;
                            radio[i].item = 0;
                            rd[i].label_Radio.Text = "None";
                        }

                    find.StatusList[radio[WindowRadio].item] = 0;//free the previously used dongle
                    radio[WindowRadio].item = Reciver;// change entry in radio to the currrent
                    radio[WindowRadio].dev_number = find.List[Reciver];
                }
                find.StatusList[Reciver] = 1;

                if (Reciver > 0)
                {
                    if (radio[WindowRadio] != null)
                    {
                        radio[WindowRadio].BufferSize = (int)flags.BufferSize;
                        radio[WindowRadio].frequency = (int)flags.frequency[WindowRadio];
                        radio[WindowRadio].rate = (uint)flags.rate[WindowRadio];
                        radio[WindowRadio].Open();

                        if (radio[WindowRadio].status)
                            str = "(" + Reciver + ") " + radio[WindowRadio].GetName();
                    }




                }
                RadioCount();
            }
            //Update lists in combo boxes
            for (int i = 0; i < Flags.MAX_DONGLES; i++)
            {
                set[i].ComboBoxRadio_Update(ref find);
                //Update parameters
                set[i].SetSettings(radio[i]);
            }
            //Set text in windowRadio
            rd[WindowRadio].label_Radio.Text = str;
        }

        void UpdateSet(int n)
        {

            if (set[n].Visible == false)
            {
                set[n].Show();
                set[n].ComboBoxRadio_Update(ref find);

            }
            else
                set[n].Visible = false;
        }

        void ReturnRadioSettings(int Nr, int gain, uint _rate, bool AGC, bool MGC, bool ShiftOn, int shift, int sampling, bool dithering, bool StagesFlag, int[] StageGain)
        {
            if (radio[Nr] != null)
            {
                if (StagesFlag)
                {
                    for (uint stage = 0; stage < 3; stage++)
                        radio[Nr].SetTunerStageGain(stage, StageGain[stage]);
                }
                else
                    radio[Nr].SetGain(gain);

                radio[Nr].GainMode(MGC);
                radio[Nr].AGCMode(AGC);
                radio[Nr].SetDithering(dithering);
                radio[Nr].SetSampleRate(_rate);

                flags.rate[Nr] = _rate;


                if (ShiftOn)
                    radio[Nr].SetFreqCorrection(shift);
                else
                    radio[Nr].SetFreqCorrection(0);

                radio[Nr].SetDirectSampling(sampling);
            }
            UpdateFrequencies(Nr);

            //     flags.Nr_active_radio = RadioCount();
        }

        void OnPowerChange(Object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case Microsoft.Win32.PowerModes.Suspend:
                    StopAllThreads();
                    break;
                case Microsoft.Win32.PowerModes.Resume:
                    UpdateAllScenesWhenRunning();
                    break;
            }
        }

        void UpdateFrequencies(int Nr)
        {
            if (radio == null) return;

            if (Nr == -1) Nr = flags.LastActiveWindowRadio;
            flags.LastActiveWindowRadio = Nr;

            if (flags.FREQUENCY_EQUAL)
            {
                for (int i = 0; i < Flags.MAX_DONGLES; i++)
                {
                    if (i != Nr) flags.frequency[i] = flags.frequency[Nr];
                    rd[i].tuningNumber.frequency = (int)flags.frequency[Nr];
                    rd[i].tuningNumber.Update_(false);
                    radio[Nr].SetSampleRate((uint)flags.rate[Nr]);
                }
                // flags.rate[1] =  rd[1].rate = rd[0].rate;

            }
            else
            {
                for (int i = 0; i < Flags.MAX_DONGLES; i++)
                {
                    rd[Nr].tuningNumber.frequency = (int)flags.frequency[Nr];
                    rd[Nr].tuningNumber.Update_(false);
                }
            }

            for (int i = 0; i < Flags.MAX_DONGLES; i++)
            {
                if (radio[i] != null)
                {
                    radio[i].frequency = (int)flags.frequency[i];
                    radio[i].SetCentralFreq();
                }
            }
            WindowsLocation(-1);
            WindowsUpdate();
        }

        private void WindowsUpdate()
        {
            windowRadar.Update(flags);
            windowBackground.Update(flags);
            windowCorrelateWave.Update(flags);
            windowCorrelateFlow.Update(flags);


            if (FlagsDelegate != null)
                FlagsDelegate(flags);

            foreach (WindowWave x in window_wave)
                if (x != null) x.Update(flags);
            foreach (WindowFlow x in window_flow)
                if (x != null) x.Update(flags);

            if (!runing)
                UpdateAllScenes();
        }


        private void trackBar5_Scroll(object sender, EventArgs e)
        {

        }



        private void button5_Click(object sender, EventArgs e)
        {
            if (flags.ButtonDisplay)
            {
                button5.ImageIndex = 1;
                flags.ButtonDisplay = false;
                displayControl1.Visible = false;
            }
            else
            {
                button5.ImageIndex = 0;
                flags.ButtonDisplay = true;
                displayControl1.Visible = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (flags.ButtonRadar)
            {
                button7.ImageIndex = 1;
                flags.ButtonRadar = false;
                radarControl1.Visible = false;
            }
            else
            {
                button7.ImageIndex = 0;
                flags.ButtonRadar = true;
                radarControl1.Visible = true;
            }
        }

        void InitSiteMenu()
        {
            if (flags.ButtonDisplay)
            {
                button5.ImageIndex = 1;
                flags.ButtonDisplay = false;
                displayControl1.Visible = false;
            }
            else
            {
                button5.ImageIndex = 0;
                flags.ButtonDisplay = true;
                displayControl1.Visible = true;
            }

            if (flags.ButtonRadar)
            {
                button7.ImageIndex = 1;
                flags.ButtonRadar = false;
                radarControl1.Visible = false;
            }
            else
            {
                button7.ImageIndex = 0;
                flags.ButtonRadar = true;
                radarControl1.Visible = true;
            }

            if (flags.ButtonCorrelation)
            {
                button9.ImageIndex = 1;
                flags.ButtonCorrelation = false;
                correlateControl1.Visible = false;
            }
            else
            {
                button9.ImageIndex = 0;
                flags.ButtonCorrelation = true;
                correlateControl1.Visible = true;
            }
        }

        private void buttonFrequencyEqual_Click(object sender, EventArgs e)
        {
            ButtonFreqEqual();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Flags.MAX_DONGLES; i++)
            {
                rd[i].Show();
            }
        }

        private void panelViewport2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radarControl1_Load(object sender, EventArgs e)
        {

        }

        //private void Form1_SizeChanged(object sender, EventArgs e)
        //{

        //}

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            resizing = false;
            UpdateAllScenesWhenRunning();
            // UpdateAllScenes();
        }

        //private void Form1_SizeChanged(object sender, EventArgs e)
        //{

        //}

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            resizing = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            resizing = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RestoreState();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            FormsReady = true;
            resizing = false;
            UpdateAllScenes();
        }

        private void ButtonFreqEqual()
        {
            if (flags.FREQUENCY_EQUAL == false)
            {
                buttonFrequencyEqual.ImageIndex = 0;
                flags.FREQUENCY_EQUAL = true;
                flags.rate[1] = flags.rate[0];
            }
            else
            {
                buttonFrequencyEqual.ImageIndex = 1;
                flags.FREQUENCY_EQUAL = false;//?    
            }
            UpdateFrequencies(flags.LastActiveWindowRadio);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (flags.ButtonCorrelation)
            {
                button9.ImageIndex = 1;
                flags.ButtonCorrelation = false;
                correlateControl1.Visible = false;
            }
            else
            {
                button9.ImageIndex = 0;
                flags.ButtonCorrelation = true;
                correlateControl1.Visible = true;
            }
        }

    }
}
