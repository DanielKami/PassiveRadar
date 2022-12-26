#define EXTENDED_RTLSDR

using System;
using System.Threading;

namespace PasiveRadar
{
    public class Radio : IDisposable
    {
        //    private readonly AutoResetEvent _signal = new AutoResetEvent();
        public int BufferSize;
        public int frequency;
        public int FreqCorrection;
        public uint rate;
        public int dev_number;//in usb port
        public bool status;//it is open or close
        public int item;

        //USB dongle data
        public string manufact;
        public string product;
        public string serial;
        public int dongle_type;

        // Size of dongle buffer, it is smaller than buffer. Data are cumulated to generate full buffer. 
        int Radio_buffer_size = 1024 * 8;

        //Gains of amlplifier
        public int[] tuner_gain_list;
        public int Number_of_gains;

        public int number_of_stages;
        public int[,] stage_gains_list;
        public int[] nr_of_gains_in_stage;

        Dll dll;
        IntPtr dev = IntPtr.Zero;
        public byte[] data_dongle = null;
        public byte[] dataIQ = null;


        Thread thread = null;
        private readonly Object _Lock = new Object();
        volatile bool exit = false;
        volatile bool exited = false; //Flag is tru when the thread is exited

        public AutoResetEvent autoVisualEvent;

        public Radio()
        {
            dll = new Dll();

            //Gains
            tuner_gain_list = new int[256];
            stage_gains_list = new int[32, 256];
            nr_of_gains_in_stage = new int[32];
        }

        public void InitBuffers(Flags flags)
        {
            Stop();
            BufferSize = (int)flags.BufferSize;
            Radio_buffer_size = (int)Math.Pow(2, flags.Radio_buffer_size) * 1024;
            dll.InitBuffer((int)flags.BufferSize);
            dataIQ = new byte[(int)flags.BufferSize];

            data_dongle = new byte[Radio_buffer_size];


            if (dev != null)
                dll.reset_buffer(dev);
        }

        public void Start()
        {
            if (dev == IntPtr.Zero) return;
            exit = false;
            exited = false;

            if (thread == null)
            {
                thread = new Thread(new ThreadStart(Read));
                thread.Priority = System.Threading.ThreadPriority.Highest;
                thread.Start();
            }
        }

        public void Stop()
        {
            if (thread != null)
            {
                exit = true;

                while (!exited) ;
                thread = null;

                Array.Clear(data_dongle, 0, Radio_buffer_size);
                Array.Clear(dataIQ, 0, BufferSize);
            }
        }

        public int Open()
        {
            int r = dll.open(ref dev, dev_number);

            if (r == 0)
            {
                int r1 = dll.get_usb_strings(dev, ref manufact, ref product, ref serial);
                dongle_type = dll.get_tuner_type(dev);
                //Tuner gains in stages
                dll.get_tuner_gains(dev, tuner_gain_list);
                CheckNumberOfGains();

                FindStageGainList();
                SetCentralFreq();
                dll.reset_buffer(dev);
                dll.set_direct_sampling(dev, 0);
                dll.set_agc_mode(dev, 0);
                GainMode(true);
                status = true;
            }

            return r;
        }

        public void Close()
        {
            if (dev == IntPtr.Zero) return;
            dll.close(dev);
            dev = IntPtr.Zero;
            status = false;
        }

        public int SetDithering(bool on)
        {
            int r = 0;
#if EXTENDED_RTLSDR
            if (dev != IntPtr.Zero)
            {
                if (on)
                    r = dll.set_dithering(dev, 1);
                else
                    r = dll.set_dithering(dev, 0);
            }
#endif
            return r;
        }

        public int SetDirectSampling(int on)
        {
            int r = 0;

            if (dev != IntPtr.Zero)
            {
                r = dll.set_direct_sampling(dev, on);
            }
            return r;
        }



        public int GainMode(bool on)
        {
            int r = 0;

            if (dev != IntPtr.Zero)
            {
                if (on)
                    r = dll.set_tuner_gain_mode(dev, 1);
                else
                    r = dll.set_tuner_gain_mode(dev, 0);
            }
            return r;
        }

        public int AGCMode(bool on)
        {
            int r = 0;

            if (dev != IntPtr.Zero)
            {
                if (on)
                    r = dll.set_agc_mode(dev, 1);
                else
                    r = dll.set_agc_mode(dev, 0);
            }
            return r;
        }



        private void CheckNumberOfGains()
        {
            if (dev == IntPtr.Zero) return;

            Number_of_gains = 0;
            for (int i = 1; i < 256; i++)
            {
                if (tuner_gain_list[i] == 0)
                {
                    Number_of_gains = i;
                    break;
                }
            }
        }

        private void CheckNumberOfGainsInStage(uint stage)
        {
            if (dev == IntPtr.Zero) return;

            nr_of_gains_in_stage[stage] = 0;
            for (int i = 1; i < 256; i++)
            {
                if (stage_gains_list[stage, i] == 0)
                {
                    nr_of_gains_in_stage[stage] = i;
                    break;
                }
            }
        }

        public int SetGain(int nr)
        {
            if (dev == IntPtr.Zero) return -1;
            // 0 error
            if (nr < 0 || nr >= Number_of_gains)
                return 0;

            return dll.set_tuner_gain(dev, tuner_gain_list[nr]);
        }

        public int GetGain()
        {
            if (dev == IntPtr.Zero) return -1;

            return dll.get_tuner_gain(dev);
        }

        public int get_device_usb_strings(ref string str)
        {
            return dll.get_device_usb_strings(0, ref str);

        }

        public int SetCentralFreq()
        {
            if (dev != IntPtr.Zero)
                return dll.set_center_freq(dev, frequency);

            return -1;
        }

        public int GetCentralFreq()
        {
            if (dev != IntPtr.Zero)
                frequency = (int)dll.get_center_freq(dev);

            return -1;
        }

        public int SetFreqCorrection(int freq)
        {
            if (dev != IntPtr.Zero)
                FreqCorrection = dll.set_freq_correction(dev, freq);

            return -1;
        }

        public int GetFreqCorrection()
        {
            if (dev != IntPtr.Zero)
                FreqCorrection = dll.get_freq_correction(dev);

            return -1;
        }

        public int SetSampleRate(uint ratex)
        {
            rate = ratex;
            int r = -1; //0- succes
            if (dev != IntPtr.Zero)
                r = dll.set_sample_rate(dev, rate);

            return r;
        }

        public int GetSampleRate()
        {
            if (dev == IntPtr.Zero)
                rate = dll.get_sample_rate(dev);
            return -1;
        }

        private void FindStageGainList()
        {
#if EXTENDED_RTLSDR
            if (dev != IntPtr.Zero)
            {
                number_of_stages = dll.get_tuner_stage_count(dev);
                if (number_of_stages == 0) return;

                //string descryption = "";
                int[] list = new int[256];

                for (uint i = 0; i < number_of_stages; i++)
                {
                    dll.get_tuner_stage_gains(dev, i, ref list);

                    for (int j = 0; j < 256; j++)
                    {
                        stage_gains_list[i, j] = list[j];
                    }

                    CheckNumberOfGainsInStage(i);
                }
            }
#endif
        }

        public int SetTunerStageGain(uint stage, int nr)
        {
            if (dev == IntPtr.Zero)
                return -1;//No device found

            if (nr < 0 || nr > nr_of_gains_in_stage[stage]) return -2; // wrong number of stages

            return dll.set_tuner_stage_gain(dev, stage, stage_gains_list[stage, nr]);

        }

        public int GetTunerStageGain(uint stage)
        {
            if (dev == IntPtr.Zero)
                return -1;
            return dll.get_tuner_stage_gain(dev, stage);

        }

        private void CheckNumberOfGainsInStages()
        {
            if (dev == IntPtr.Zero)
                return;


            for (int stage = 0; stage < number_of_stages; stage++)
            {
                nr_of_gains_in_stage[stage] = 0;
                for (int i = 1; i < 256; i++)
                {
                    if (stage_gains_list[stage, i] == 0)
                    {
                        nr_of_gains_in_stage[stage] = i;
                        break;
                    }
                }
            }
        }

        //it works as a thread
        //If the buffer change the tread restarts
        public void Read()
        {
            int readed = 0;
            if (dev == IntPtr.Zero) return;

            int count = 0;
            int buffer_multiplication = BufferSize / Radio_buffer_size - 1;//-1 because copy offset and the radio buffer must be smaller than BufferSize
            byte[] temp_buffer = new byte[BufferSize];
            int reduced_buffer_size = BufferSize - Radio_buffer_size;

            while (!exit)
            {
                try
                {
                    if (status)
                        dll.read_sync(dev, ref data_dongle, Radio_buffer_size, ref readed);//direct data from dongle
                }
                catch (Exception ex)
                {
                    String str = "Error open device. " + ex.ToString();
                    // MessageBox.Show(str);
                    continue;
                }

                rotate_180(data_dongle);//fast

                //first fill the temp buffer
                if (count < buffer_multiplication)
                {
                    Buffer.BlockCopy(data_dongle, 0, temp_buffer, Radio_buffer_size * buffer_multiplication, Radio_buffer_size);
                    count++;
                }

                //Make a place for new data
                Buffer.BlockCopy(temp_buffer, Radio_buffer_size, temp_buffer, 0, reduced_buffer_size);

                //Add to the end of temp_buffer new data (fast)
                Buffer.BlockCopy(data_dongle, 0, temp_buffer, reduced_buffer_size, Radio_buffer_size); //    Array src,    int srcOffset,    Array dst,    int dstOffset,    int count

                //Protct reading data
                lock (_Lock)
                {
                    //copy temp to dataIQ must be protected, acces by other threads
                    Buffer.BlockCopy(temp_buffer, 0, dataIQ, 0, BufferSize);
                }
            }
            exited = true;
        }


        public string GetName()
        {
            if (dev == IntPtr.Zero)
                return "";
            return dll.get_device_name(dev);
        }



        void rotate_180(byte[] buf)
        /* 180 rotation is 1+0j, 
               0  1   2   3   4  5   6   7
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

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources

            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
