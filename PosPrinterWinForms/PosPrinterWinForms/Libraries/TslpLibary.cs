using System;
using System.Runtime.InteropServices;

namespace SocketListener.Libraries
{
    class TslpLibary
    {
        [DllImport("TSPL_SDK")]
        public static extern int FormatError(int error_no, int langid, byte[] buf, int pos, int bufSize);
        [DllImport("TSPL_SDK")]
        public static extern int PrinterCreator(ref IntPtr printer, string model);

        [DllImport("TSPL_SDK")]
        public static extern IntPtr PrinterCreatorS(string model);

        [DllImport("TSPL_SDK")]
        public static extern int PortOpen(IntPtr printer, string portSetting);

        [DllImport("TSPL_SDK")]
        public static extern int PortClose(IntPtr printer);

        [DllImport("TSPL_SDK")]
        public static extern int PrinterDestroy(IntPtr printer);
        [DllImport("TSPL_SDK")]
        public static extern int DirectIO(IntPtr printer, byte[] writeData, int writenum, byte[] readData, int readNum, ref int readedNum);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_SelfTest(IntPtr printer);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_BitMap(IntPtr printer, int xPos, int yPos, int mode, string fileName);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_Setup(IntPtr printer, int labelWidth, int labelHeight, int speed, int density, int type, int gap, int offset);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_ClearBuffer(IntPtr printer);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_BarCode(IntPtr printer, int xPos, int yPos, int codeType, int height, int readable, int rotation, int narrow, int wide, string data);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_QrCode(IntPtr printer, int xPos, int yPos, int eccLevel, int width, int mode, int rotation, int model, int mask, string data);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_Text(IntPtr printer, int xPos, int yPos, int font, int rotation, int xMultiplication, int yMultiplication, byte[] data);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_FormFeed(IntPtr printer);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_Box(IntPtr printer, int x_start, int y_start, int x_end, int y_end, int thinckness);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_SetTear(IntPtr printer, int on);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_SetRibbon(IntPtr printer, int ribbon);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_Offset(IntPtr printer, int distance);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_Direction(IntPtr printer, int direction);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_Feed(IntPtr printer, int len, int xPos, int yPos);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_Home(IntPtr printer);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_Print(IntPtr printer, int num, int copies);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_GetPrinterStatus(IntPtr printer, ref int status);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_SetCodePage(IntPtr printer, int codepage);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_Reverse(IntPtr printer, int xPos, int yPos, int width, int height);

        [DllImport("TSPL_SDK")]
        public static extern int TSPL_GapDetect(IntPtr printer, int paperLength, int gapLength);


        public IntPtr printer;
    }
}
