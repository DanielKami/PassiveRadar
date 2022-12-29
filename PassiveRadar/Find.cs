using System;
using System.Windows;

namespace PasiveRadar
{
    public class Find
    {
        Dll dll;
        IntPtr dev = IntPtr.Zero;
        public int[] StatusList;
        public string[] NameList;
        public int[] List; //List stores the index of device which can be opened
        public int NrOfDevices = 0;

        public Find()
        {
            dll = new Dll();
            NrOfDevices = 1;
            List = new int[32];
            NameList = new string[32];
            NameList[0] = "None";
            StatusList = new int[32];
            StatusList[0] = 1;
        }
        /*param index of device to be opened.
        * \return -1 if no device found at index
        * \return -2 if cannot open device.
        * \return various libusb errors if cannot open device or claim interface
        * \return various tuner init errors.
        * \return -10 if already open.*/
        public int Device()
        {
            int r = -1;
            string manufact = "";
            string product = "";
            string serial = "";

            NrOfDevices = 1;
            int pos = 0;

            //int nrdev=dll.;
            //String str = "Can't open rtlsdr dongle. " + nrdev;
            //MessageBox.Show(str);

            for (int i = 0; i < 16; i++)
            {
                dev = IntPtr.Zero;
                int res = dll.open(ref dev, i);
                //String str = "open result " + res + "poiner: " + dev;
                //MessageBox.Show(str);

                int type = dll.get_tuner_type(dev);
                // str = "typ " + type;
                //MessageBox.Show(str);
                if (res == 0 && (type > 0 && type < 7))
                {
                    int test = dll.get_usb_strings(dev, ref manufact, ref product, ref serial);
                    NameList[pos + 1] = "(" + i + ") " + manufact + " " + product + " " + serial;
                    List[pos + 1] = i;
                    pos++;
                    NrOfDevices++;

                    dll.close(dev);
                }

            }
            return r;
        }
    }
}
