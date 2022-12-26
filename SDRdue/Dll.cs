


using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;



namespace PasiveRadar
{
    public unsafe class Dll
    {
        private IntPtr pnt;

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern char* rtlsdr_get_device_name(int index);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_device_count();

[System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_device_usb_strings(uint devIndex, StringBuilder str1, StringBuilder str2, StringBuilder str3);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_index_by_serial(StringBuilder str1);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_open(ref IntPtr dev, int devIndex);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_close(IntPtr dev);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_xtal_freq(IntPtr dev, uint rtl_freq, uint tuner_freq);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_xtal_freq(IntPtr dev, ref int f1, ref int f2);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_usb_strings(IntPtr dev, StringBuilder manufact, StringBuilder product, StringBuilder serial);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_write_eeprom(IntPtr dev, uint[] data, uint offset, uint leen);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_read_eeprom(IntPtr dev, IntPtr data, uint offset, uint leen);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_center_freq(IntPtr dev, int f1);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint rtlsdr_get_center_freq(IntPtr dev);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_freq_correction(IntPtr dev, int ppm);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_freq_correction(IntPtr dev);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_tuner_type(IntPtr dev);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_tuner_gains(IntPtr dev, int[] gains);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_tuner_gain(IntPtr dev, int gain);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_tuner_gain(IntPtr dev);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_tuner_if_gain(IntPtr dev, int stage, int gain);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_tuner_stage_gains(IntPtr dev, uint stage, int[] gain, char[] description);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_tuner_stage_count(IntPtr dev);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_tuner_stage_gain(IntPtr dev, uint stage);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_tuner_stage_gain(IntPtr dev, uint stage, int gain);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_tuner_gain_mode(IntPtr dev, int manual);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_sample_rate(IntPtr dev, uint rate);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint rtlsdr_get_sample_rate(IntPtr dev);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint rtlsdr_get_corr_sample_rate(IntPtr dev);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_testmode(IntPtr dev, int a);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_agc_mode(IntPtr dev, int a);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_direct_sampling(IntPtr dev, int on);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_direct_sampling(IntPtr dev);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_offset_tuning(IntPtr dev, int on);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_offset_tuning(IntPtr dev);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_dithering(IntPtr dev, int dither);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_reset_buffer(IntPtr dev);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_read_sync(IntPtr dev, IntPtr str, int lenght, ref int readed);

        [System.Runtime.InteropServices.DllImport(@"rtlsdr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr rtlsdr_get_version();



        //c# accesed functions

        //Comment: it is unsave method because the poiter to the char array can be deleted after function exit. 
        public string get_device_name(IntPtr dev)
        {

            string[] rtlsdr_tuner = new string[]
            {
	            "UNKNOWN",
	            "E4000",
	            "FC0012",
	            "FC0013",
	            "FC2580",
	            "R820T",
	            "R828D"
            };

            int index = rtlsdr_get_tuner_type(dev);
            return rtlsdr_tuner[index];
        }

  
        public int get_device_count()
        {
            return rtlsdr_get_device_count();
        }

        /*checked
         * Get USB device strings.
         *
         * NOTE: The string arguments must provide space for up to 256 bytes.
         *
         * \param index the device index
         * \param manufact manufacturer name, may be NULL
         * \param product product name, may be NULL
         * \param serial serial number, may be NULL
         * \return 0 on success
         */
        public int get_device_usb_strings(uint devIndex, ref string str)
        {
            StringBuilder str1 = new StringBuilder(new String(' ', 256));
            StringBuilder str2 = new StringBuilder(new String(' ', 256));
            StringBuilder str3 = new StringBuilder(new String(' ', 256));

            int r = rtlsdr_get_device_usb_strings(devIndex, str1, str2, str3);
            str = str1 + " " + str2 + " " + str3;
            return r;
        }

        /*checked
         * Get device index by USB serial string descriptor.
         *
         * \param serial serial string of the device
         * \return device index of first device where the name matched
         * \return -1 if name is NULL
         * \return -2 if no devices were found at all
         * \return -3 if devices were found, but none with matching name
         */
        public int get_index_by_serial(uint devIndex, ref string str)
        {
            StringBuilder str1 = new StringBuilder(new String(' ', 256));

            str1.Append(str);
            int r = rtlsdr_get_index_by_serial(str1);
            str = "" + str1;
            return r;
        }

        /*checked
         * rtlsdr_open Open the device for use
         * \param index of device to be opened.
         * \return -1 if no device found at index
         * \return -2 if cannot open device.
         * \return various libusb errors if cannot open device or claim interface
         * \return various tuner init errors.
         * \return -10 if already open.
         */
        public int open(ref IntPtr dev, int devIndex)
        {
            try
            {
                return rtlsdr_open(ref dev, devIndex);
            }
            catch (Exception ex)
            {
                String str = "Can't open rtlsdr dongle. " + ex.ToString();
                MessageBox.Show(str);
                return -1;
            }
         
        }

        public int close(IntPtr dev)
        {
            if (dev != IntPtr.Zero)
            {
                return rtlsdr_close(dev);
            }
            return 0;
        }

        /* configuration functions */

        /*!
         * Set crystal oscillator frequencies used for the RTL2832 and the tuner IC.
         *
         * Usually both ICs use the same clock. Changing the clock may make sense if
         * you are applying an external clock to the tuner or to compensate the
         * frequency (and samplerate) error caused by the original (cheap) crystal.
         *
         * NOTE: Call this function only if you fully understand the implications.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param rtl_freq frequency value used to clock the RTL2832 in Hz
         * \param tuner_freq frequency value used to clock the tuner IC in Hz
         * \return 0 on success
         */
        public int set_xtal_freq(IntPtr dev, uint rtl_freq, uint tuner_freq)
        {
            return rtlsdr_set_xtal_freq(dev, rtl_freq, tuner_freq);
        }

        /*checked
         * Get crystal oscillator frequencies used for the RTL2832 and the tuner IC.
         *
         * Usually both ICs use the same clock.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param rtl_freq frequency value used to clock the RTL2832 in Hz
         * \param tuner_freq frequency value used to clock the tuner IC in Hz
         * \return 0 on success
         */
        public int get_xtal_freq(IntPtr dev, ref int f1, ref int f2)
        {
            return rtlsdr_get_xtal_freq(dev, ref f1, ref f2);
        }

        /*checked
         * Get USB device strings.
         *
         * NOTE: The string arguments must provide space for up to 256 bytes.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param manufact manufacturer name, may be NULL
         * \param product product name, may be NULL
         * \param serial serial number, may be NULL
         * \return 0 on success
         */
        public int get_usb_strings(IntPtr dev, ref string manufact, ref string product, ref string serial)
        {
            StringBuilder str1 = new StringBuilder(new String(' ', 256));
            StringBuilder str2 = new StringBuilder(new String(' ', 256));
            StringBuilder str3 = new StringBuilder(new String(' ', 256));

            int r = rtlsdr_get_usb_strings(dev, str1, str2, str3);

            manufact = "" + str1;
            product = "" + str2;
            serial = "" + str3;

            return r;
        }

        /*
         * Write the device EEPROM
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param data buffer of data to be written
         * \param offset address where the data should be written
         * \param len length of the data
         * \return 0 on success
         * \return -1 if device handle is invalid
         * \return -2 if EEPROM size is exceeded
         * \return -3 if no EEPROM was found
         */
        public int write_eeprom(IntPtr dev, uint[] data, uint offset, uint leen)
        {
            return rtlsdr_write_eeprom(dev, data, offset, leen);
        }

        /*not working
         * Read the device EEPROM
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param data buffer where the data should be written
         * \param offset address where the data should be read from
         * \param len length of the data
         * \return 0 on success
         * \return -1 if device handle is invalid
         * \return -2 if EEPROM size is exceeded
         * \return -3 if no EEPROM was found
         */
        public int read_eeprom(IntPtr dev, ref byte[] data, uint offset, uint Length)
        {

            int byte_lenght = Marshal.SizeOf(data[0]) * data.Length;
            IntPtr pnt = Marshal.AllocHGlobal(byte_lenght);
            Marshal.Copy(data, 0, pnt, data.Length);
            int r = rtlsdr_read_eeprom(dev, pnt, offset, Length);
            Marshal.Copy(pnt, data, 0, byte_lenght);
            return r;
        }

        public int set_center_freq(IntPtr dev, int f1)
        {
            return rtlsdr_set_center_freq(dev, f1);
        }

        /*checked
         * Get actual frequency the device is tuned to.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \return 0 on error, frequency in Hz otherwise
         */
        public uint get_center_freq(IntPtr dev)
        {
            return rtlsdr_get_center_freq(dev);
        }

        /*!
         * Set the frequency correction value for the device.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param ppm correction value in parts per million (ppm)
         * \return 0 on success
         */
        public int set_freq_correction(IntPtr dev, int ppm)
        {
            return rtlsdr_set_freq_correction(dev, ppm);
        }

        /*!
         * Get actual frequency correction value of the device.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \return correction value in parts per million (ppm)
         */
        public int get_freq_correction(IntPtr dev)
        {
            return rtlsdr_get_freq_correction(dev);
        }

        /*!
         * Get the tuner type.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \return RTLSDR_TUNER_UNKNOWN on error, tuner type otherwise
         * 	RTLSDR_TUNER_UNKNOWN = 0,
	        1 - RTLSDR_TUNER_E4000,
	        2 - RTLSDR_TUNER_FC0012,
	        3 - RTLSDR_TUNER_FC0013,
	        4 - RTLSDR_TUNER_FC2580,
	        5 - RTLSDR_TUNER_R820T,
	        6 - RTLSDR_TUNER_R828D
         */
        public int get_tuner_type(IntPtr dev)
        {
            if (dev != IntPtr.Zero)
                try
                {
                    return rtlsdr_get_tuner_type(dev);
                }
                catch
                {
                    return -1;
                }
            else return -1;
        }

        /*!
         * Get a list of gains supported by the tuner.
         *
         * NOTE: The gains argument must be preallocated by the caller. If NULL is
         * being given instead, the number of available gain values will be returned.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param gains array of gain values. In tenths of a dB, 115 means 11.5 dB.
         * \return <= 0 on error, number of available (returned) gain values otherwise
         */
        public int get_tuner_gains(IntPtr dev, int[] gains)
        {
            return rtlsdr_get_tuner_gains(dev, gains);
        }

        /*!
         * Set the gain for the device.
         * Manual gain mode must be enabled for this to work.
         *
         * Valid gain values (in tenths of a dB) for the E4000 tuner:
         * -10, 15, 40, 65, 90, 115, 140, 165, 190,
         * 215, 240, 290, 340, 420, 430, 450, 470, 490
         *
         * Valid gain values may be queried with \ref rtlsdr_get_tuner_gains function.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param gain in tenths of a dB, 115 means 11.5 dB.
         * \return 0 on success
         */
        public int set_tuner_gain(IntPtr dev, int gain)
        {
            return rtlsdr_set_tuner_gain(dev, gain);
        }

        /*!
         * Get actual gain the device is configured to.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \return 0 on error, gain in tenths of a dB, 115 means 11.5 dB.
         */
        public int get_tuner_gain(IntPtr dev)
        {
            return rtlsdr_get_tuner_gain(dev);
        }

        /*!
         * Set the intermediate frequency gain for the device.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param stage intermediate frequency gain stage number (1 to 6 for E4000)
         * \param gain in tenths of a dB, -30 means -3.0 dB.
         * \return 0 on success
         */
        public int set_tuner_if_gain(IntPtr dev, int stage, int gain)
        {
            return rtlsdr_set_tuner_if_gain(dev, stage, gain);
        }

        //  ADDED_STAGE_GAIN_MATERIAL
        /*!
         * Get a list of gains and description of the gain stages supported by the tuner.
         * NOTE: The gains argument must be preallocated by the caller. If NULL is
         * being given instead, the number of available gain settings will be returned.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param stage the stage to get the array of gain settings. If no such
         *   stage exists, return error
         * \gains array to hold the different gain settings for this stage
         *   - use NULL to get the size of the array returned by the function
         * \param description the textual description of the respective stage
         *   is copied into this string (description max. 256 chars)
         *   Optional: can be NULL
         * \return <= 0 on error, number of available (returned) gain values otherwise
         * \def DESCRIPTION_MAXLEN description max. 256 chars
         */
        public int get_tuner_stage_gains(IntPtr dev, uint stage, ref int[] gain)
        {
            char[] str = new char[256];

            int r = rtlsdr_get_tuner_stage_gains(dev, stage, gain, str);
            string s = new string(str);
            //description = s;
            return r;
        }

        /*checked
         * Get the number of stages for per stage gain control for this device.
         * \param dev the device handle given by rtlsdr_open()
         * \return <= 0 on error, number of available stages otherwise
         */
        public int get_tuner_stage_count(IntPtr dev)
        {
            return rtlsdr_get_tuner_stage_count(dev);
        }

        /*checked
         * Set the gain of a stage in the tuner
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param stage the stage to set gain for
         * \param in tenths of a dB, e.g. -30 means -3.0 dB.
         * \return in tenths of a dB, e.g. -30 means -3.0 dB. (0dB if inappropriate)
         */
        public int get_tuner_stage_gain(IntPtr dev, uint stage)
        {
            return rtlsdr_get_tuner_stage_gain(dev, stage);
        }

        /*checked
         * Set the gain of a stage in the tuner
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param stage the stage to set gain for
         * \param in tenths of a dB, e.g. -30 means -3.0 dB.
         * \return <= 0 on error, 0 on success
         */
        public int set_tuner_stage_gain(IntPtr dev, uint stage, int gain)
        {
            return rtlsdr_set_tuner_stage_gain(dev, stage, gain);
        }

        //	/ADDED_STAGE_GAIN_MATERIAL

        /*!
         * Set the gain mode (automatic/manual) for the device.
         * Manual gain mode must be enabled for the gain setter function to work.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param manual gain mode, 1 means manual gain mode shall be enabled.
         * \return 0 on success
         */
        public int set_tuner_gain_mode(IntPtr dev, int manual)
        {
            return rtlsdr_set_tuner_gain_mode(dev, manual);
        }

        /*checked
         * Set the sample rate for the device, also selects the baseband filters
         * according to the requested sample rate for tuners where this is possible.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param samp_rate the sample rate to be set, possible values are:
         * 		    225001 - 300000 Hz
         * 		    900001 - 3200000 Hz
         * 		    sample loss is to be expected for rates > 2400000
         * \return 0 on success, -EINVAL on invalid rate
         */
        public int set_sample_rate(IntPtr dev, uint rate)
        {
            return rtlsdr_set_sample_rate(dev, rate);
        }

        /*checked
         * Get actual sample rate the device is configured to.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \return 0 on error, sample rate in Hz otherwise
         */
        public uint get_sample_rate(IntPtr dev)
        {
            return rtlsdr_get_sample_rate(dev);
        }

        /*!
         * Get corrected sample rate the device is configured to.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \return 0 on error, sample rate in Hz otherwise
         */
        public uint get_corr_sample_rate(IntPtr dev)
        {
            return rtlsdr_get_corr_sample_rate(dev);
        }

        /*!
         * Enable test mode that returns an 8 bit counter instead of the samples.
         * The counter is generated inside the RTL2832.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param test mode, 1 means enabled, 0 disabled
         * \return 0 on success
         */
        public int set_testmode(IntPtr dev, int a)
        {
            return rtlsdr_set_testmode(dev, a);
        }

        /*!
         * Enable or disable the internal digital AGC of the RTL2832.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param digital AGC mode, 1 means enabled, 0 disabled
         * \return 0 on success
         */
        public int set_agc_mode(IntPtr dev, int on)
        {
            return rtlsdr_set_agc_mode(dev, on);
        }

        /*!
         * Enable or disable the direct sampling mode. When enabled, the IF mode
         * of the RTL2832 is activated, and rtlsdr_set_center_freq() will control
         * the IF-frequency of the DDC, which can be used to tune from 0 to 28.8 MHz
         * (xtal frequency of the RTL2832).
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param on 0 means disabled, 1 I-ADC input enabled, 2 Q-ADC input enabled
         * \return 0 on success
         */
        public int set_direct_sampling(IntPtr dev, int on)
        {
            return rtlsdr_set_direct_sampling(dev, on);
        }

        /*!
         * Get state of the direct sampling mode
         *
         * \param dev the device handle given by rtlsdr_open()
         * \return -1 on error, 0 means disabled, 1 I-ADC input enabled
         *	    2 Q-ADC input enabled
         */
        public int get_direct_sampling(IntPtr dev, int on)
        {
            return rtlsdr_set_direct_sampling(dev, on);
        }

        /*!
         * Enable or disable offset tuning for zero-IF tuners, which allows to avoid
         * problems caused by the DC offset of the ADCs and 1/f noise.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param on 0 means disabled, 1 enabled
         * \return 0 on success
         */
        public int set_offset_tuning(IntPtr dev, int on)
        {
            return rtlsdr_set_offset_tuning(dev, on);
        }

        /*!
         * Get state of the offset tuning mode
         *
         * \param dev the device handle given by rtlsdr_open()
         * \return -1 on error, 0 means disabled, 1 enabled
         */
        public int get_offset_tuning(IntPtr dev, int on)
        {
            return rtlsdr_get_offset_tuning(dev);
        }

        /*!
         * Enable or disable frequency dithering for r820t tuners.
         * Must be performed before freq_set().
         * Fails for other tuners.
         *
         * \param dev the device handle given by rtlsdr_open()
         * \param on 0 means disabled, 1 enabled
         * \return 0 on success
         */
        public int set_dithering(IntPtr dev, int dither)
        {
            return rtlsdr_set_dithering(dev, dither);
        }

        /* streaming functions
         * reset_buffer must be before read_sync to corectly initialize buffers
         */
        public int reset_buffer(IntPtr dev)
        {
            return rtlsdr_reset_buffer(dev);
        }

         public int read_sync(IntPtr dev, ref byte[] data, int lenght, ref int readed)
        {
            // StringBuilder str1 = new StringBuilder(new String(' ', lenght));
            int r = rtlsdr_read_sync(dev, pnt, lenght, ref readed);
            Marshal.Copy(pnt, data, 0, lenght);
            //  str = ""+str1;
            return r;
        }

        /*!
         * Return a version string
         *
         * \return a string in the format 1.2.3.4
         */
        public string read_sync()
        {
            int lenght = 256;
            char[] array = new char[lenght];
            int byte_lenght = Marshal.SizeOf(array[0]) * array.Length;
            IntPtr pnt = Marshal.AllocHGlobal(byte_lenght);
            pnt = rtlsdr_get_version();
            Marshal.Copy(pnt, array, 0, byte_lenght);
            string s = new string(array);
            return s;
        }

        public void InitBuffer(int lenght)
        {

            // int byte_lenght = Marshal.SizeOf(data[0]) * data.Length;
            //The main data buffer is in byte format so no worry about the size
            pnt = Marshal.AllocHGlobal(lenght);

        }
    }
}
