using SocketListener.Entities;
using SocketListener.Libraries;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace SocketListener.Logic
{ 
    class ImprimirEtiqueta
    {
        public void Imprimir(XmlDocument xml)
        { 
            ExtraerDataXml extraerDataXml = new ExtraerDataXml(xml);
            Encabezado encabezado = extraerDataXml.GetEncabezado(); 
            Receptor receptor = extraerDataXml.GetReceptor();
            Paquete paquete = extraerDataXml.GetPaquete();
            GlobalHelpers helper = new GlobalHelpers();

            int num = 0;
            byte[] buf = new byte[0x200];
            tspl_dll _dll = new tspl_dll();
            num = tspl_dll.PrinterCreator(ref _dll.printer, "R42");
            if (num == 0)
            {
                num = tspl_dll.PortOpen(_dll.printer, "USB");
                if (num == 0)
                {
                    int status = 0;
                    num = tspl_dll.TSPL_GetPrinterStatus(_dll.printer, ref status);
                    if (9 == status)
                    {
                        Console.WriteLine("head opened and out of ribbon");
                    }
                    else if (12 == status)
                    {
                        Console.WriteLine("Out of ribbon and out of paper");
                    }
                    else if (0x20 == status)
                    {
                        Console.WriteLine("Printing");
                    }
                    else
                    {
                        tspl_dll.TSPL_ClearBuffer(_dll.printer);
                        tspl_dll.TSPL_Setup(_dll.printer, 75, 130, 2, 6, 1, 1, 1);

                        Encoding Texto = Encoding.GetEncoding("UTF-8");
                        byte[] data;

     

                        #region Codigo Direccion Envio QR Listo
                        num = tspl_dll.TSPL_Box(_dll.printer, 20, 40, 260, 280, 8); // Codigo Direccion Envio
                        num = tspl_dll.TSPL_QrCode(_dll.printer, 35, 55, 3, 10, 0, 0, 0, 7, "\"" + encabezado.RutDestino + "\"");
                        #endregion

                        #region Informacion ruta

                        #region Ruta Emisora -> Ruta Receptora
                        num = tspl_dll.TSPL_Box(_dll.printer, 253, 40, 580, 280, 8); // Cuadro rectangular
                        data = Texto.GetBytes(encabezado.RutOffEmisora + "->" + encabezado.RutDestino); //Tipo de mercancia
                        num = tspl_dll.TSPL_Text(_dll.printer, 270, 65, 9, 0, 2, 2, data);
                        #endregion

                        #region Codigo de Barra Ruta destino
                        num = tspl_dll.TSPL_Box(_dll.printer, 255, 135, 580, 280, 4); // Cuadro rectangular
                        num = tspl_dll.TSPL_BarCode(_dll.printer, 350, 155, 8, 90, 1, 0, 2, 2, encabezado.RutDestino);
                        #endregion

                        #endregion

                        #region Main Principal
                          num = tspl_dll.TSPL_Box(_dll.printer, 20, 40, 580, 1030, 8); //main principal
                          num = tspl_dll.TSPL_Box(_dll.printer, 480, 373, 580, 273, 8); //Cuadro de tipo
                          data = Texto.GetBytes(paquete.Paquete_tipo_codigo); //Tipo de mercancia
                          num = tspl_dll.TSPL_Text(_dll.printer, 515, 290, 9, 0, 3, 3, data);

                        #region Datos del Rceptor
                        data = Texto.GetBytes(helper.TruncaText(helper.normalizaTexto(receptor.Nombre), 36));
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 290, 9, 0, 1, 1, data);
                        data = Texto.GetBytes(receptor.TipoDocumento + ": " + receptor.Documento);
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 320, 9, 0, 1, 1, data);
                        data = Texto.GetBytes("Email: "+receptor.Correo);
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 350, 9, 0, 1, 1, data);
                        data = Texto.GetBytes("Telefono: "+receptor.Telefono);
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 380, 9, 0, 1, 1, data);
                        #endregion

                        #region Direccion de Envio
                        string direccionEnvio = "";
                        string Domicilio = "";
                        if (encabezado.IdOrderType == "2")
                        {
                            direccionEnvio += encabezado.DirOffDestino;
                        }
                        else if (encabezado.IdOrderType == "1" || encabezado.IdOrderType == "3")
                        {
                            direccionEnvio += encabezado.DirDestino;
                            Domicilio += "A Domicilio";
                        }

                       
                        var direccion = helper.DivideText(helper.normalizaTexto(direccionEnvio), 35);
                        int space = 0;
                        int maxLineas = 4;
                        foreach (string dir in direccion)
                        {
                            if (maxLineas > 0)
                            {
                                data = Texto.GetBytes(dir.Trim());
                                num = tspl_dll.TSPL_Text(_dll.printer, 50, 440 + space, 9, 0, 1, 1, data);
                                space = space + 30;
                            }
                            maxLineas = maxLineas - 1;
                        }
                        #endregion

                        #region Datos del paquete
                        data = Texto.GetBytes("Descripcion: " + helper.normalizaTexto(paquete.Paquete_descripcion));
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 520, 9, 0, 1, 1, data);
                        data = Texto.GetBytes("Alto: " + paquete.Paquete_alto + "cm, Ancho: " + paquete.Paquete_ancho + "cm, Largo: " + paquete.Paquete_largo + "cm");
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 550, 9, 0, 1, 1, data);
                        data = Texto.GetBytes("Peso: " + paquete.Paquete_peso + "kg, Peso Volumetrico: " + paquete.Paquete_peso_volumetrico + "Kg/m3");
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 580, 9, 0, 1, 1, data);
                        
                        data = Texto.GetBytes(Domicilio); //Tipo de mercancia
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 630, 9, 0, 2, 2, data);


                        num = tspl_dll.TSPL_BitMap(_dll.printer, 75, 700, 0, "logoSmall.png");


                        if (Domicilio.Length > 1)
                        { 
                            num = tspl_dll.TSPL_QrCode(_dll.printer, 350, 630, 3, 5, 0, 0, 0, 7, "\"" + encabezado.Url_envio + "\"");
                        }
                        #endregion

                        #endregion

                        #region Footer Principal Listo
                        num = tspl_dll.TSPL_Box(_dll.printer, 20, 860, 580, 1030, 8); //footer principal
                        num = tspl_dll.TSPL_BarCode(_dll.printer, 50, 880, 8, 100, 1, 0, 2, 2, encabezado.NumeroTracking);
                        #endregion


                        num = tspl_dll.TSPL_Print(_dll.printer, 1, 1);
                        num = tspl_dll.PortClose(_dll.printer);
                        num = tspl_dll.PrinterDestroy(_dll.printer);
                    }
                }
                else
                {
                    num = tspl_dll.FormatError(num, 1, buf, 0, 0x200);
                    Console.WriteLine(Encoding.Default.GetString(buf));
                }
            }
            else
            {
                num = tspl_dll.FormatError(num, 1, buf, 0, 0x200);
                Console.WriteLine(Encoding.Default.GetString(buf));
            }
        }

        public void Imprimir2(XmlDocument xml)
        {
            ExtraerDataXml extraerDataXml = new ExtraerDataXml(xml);
            Encabezado encabezado = extraerDataXml.GetEncabezado();
            Receptor receptor = extraerDataXml.GetReceptor();
            Paquete paquete = extraerDataXml.GetPaquete();
            GlobalHelpers helper = new GlobalHelpers();

            int num = 0;
            byte[] buf = new byte[0x200];
            tspl_dll _dll = new tspl_dll();
            num = tspl_dll.PrinterCreator(ref _dll.printer, "R42");
            if (num == 0)
            {
                num = tspl_dll.PortOpen(_dll.printer, "USB");
                if (num == 0)
                {
                    int status = 0;
                    num = tspl_dll.TSPL_GetPrinterStatus(_dll.printer, ref status);
                    if (9 == status)
                    {
                        Console.WriteLine("head opened and out of ribbon");
                    }
                    else if (12 == status)
                    {
                        Console.WriteLine("Out of ribbon and out of paper");
                    }
                    else if (0x20 == status)
                    {
                        Console.WriteLine("Printing");
                    }
                    else
                    {
                        tspl_dll.TSPL_ClearBuffer(_dll.printer);
                        tspl_dll.TSPL_Setup(_dll.printer, 100, 150, 2, 6, 1, 1, 1);

                        Encoding Texto = Encoding.GetEncoding("UTF-8");
                        byte[] data;

                        #region header Principal Listo
                        num = tspl_dll.TSPL_Box(_dll.printer, 20, 40, 730, 1200, 4); //Header principal
                        num = tspl_dll.TSPL_BitMap(_dll.printer, 38, 68, 0, "icon.jpg");
                        data = Texto.GetBytes(encabezado.NombreEmpresa);
                        num = tspl_dll.TSPL_Text(_dll.printer, 120, 68, 9, 0, 1, 1, data);
                        data = Texto.GetBytes(encabezado.RutDestino);
                        num = tspl_dll.TSPL_Text(_dll.printer, 590, 50, 9, 0, 3, 3, data);
                        #endregion

                        #region Codigo Direccion Envio QR Listo
                        num = tspl_dll.TSPL_Box(_dll.printer, 20, 120, 280, 400, 8); // Codigo Direccion Envio
                        num = tspl_dll.TSPL_QrCode(_dll.printer, 40, 158, 3, 10, 0, 0, 0, 7, "\"" + encabezado.RutDestino + "\"");
                        #endregion

                        #region Informacion del receptor

                        #region Datos del Receptor
                        num = tspl_dll.TSPL_Box(_dll.printer, 272, 120, 730, 400, 8); // Cuadro rectangular
                        data = Texto.GetBytes(helper.TruncaText(helper.normalizaTexto(receptor.Nombre), 36));
                        num = tspl_dll.TSPL_Text(_dll.printer, 288, 138, 9, 0, 1, 1, data);
                        data = Texto.GetBytes(receptor.TipoDocumento + ": " + receptor.Documento);
                        num = tspl_dll.TSPL_Text(_dll.printer, 288, 168, 9, 0, 1, 1, data);
                        data = Texto.GetBytes(receptor.Correo);
                        num = tspl_dll.TSPL_Text(_dll.printer, 288, 198, 9, 0, 1, 1, data);
                        data = Texto.GetBytes(receptor.Telefono);
                        num = tspl_dll.TSPL_Text(_dll.printer, 288, 228, 9, 0, 1, 1, data);
                        #endregion

                        #region direccion de Recepcion
                        string direccionEnvio = "";
                        string Domicilio = "";
                        if (encabezado.IdOrderType == "2")
                        {
                            direccionEnvio += encabezado.DirOffDestino;
                        }
                        else if (encabezado.IdOrderType == "1" || encabezado.IdOrderType == "3")
                        {
                            direccionEnvio += encabezado.DirDestino;
                            Domicilio += "A Domicilio";
                        }

                        num = tspl_dll.TSPL_Box(_dll.printer, 272, 255, 730, 400, 4); // Cuadro rectangular
                        var direccion = helper.DivideText(helper.normalizaTexto(direccionEnvio), 35);
                        int space = 0;
                        int maxLineas = 4;
                        foreach (string dir in direccion)
                        {
                            if (maxLineas > 0)
                            {
                                data = Texto.GetBytes(dir.Trim());
                                num = tspl_dll.TSPL_Text(_dll.printer, 288, 270 + space, 9, 0, 1, 1, data);
                                space = space + 30;
                            }
                            maxLineas = maxLineas - 1;
                        }
                        #endregion

                        #endregion

                        #region Main Principal falta
                        num = tspl_dll.TSPL_Box(_dll.printer, 20, 120, 730, 1000, 8); //main principal
                        num = tspl_dll.TSPL_Box(_dll.printer, 630, 500, 730, 392, 8); //Cuadro de tipo
                        data = Texto.GetBytes(paquete.Paquete_tipo_codigo); //Tipo de mercancia
                        num = tspl_dll.TSPL_Text(_dll.printer, 665, 415, 9, 0, 3, 3, data);

                        data = Texto.GetBytes(encabezado.RutOffEmisora + " -> " + encabezado.RutDestino); //Tipo de mercancia
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 415, 9, 0, 2, 2, data);
                        num = tspl_dll.TSPL_BarCode(_dll.printer, 50, 850, 8, 100, 1, 0, 2, 2, encabezado.RutDestino);

                        data = Texto.GetBytes("Descripcion: " + helper.normalizaTexto(paquete.Paquete_descripcion));
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 545, 9, 0, 1, 1, data);
                        data = Texto.GetBytes("Alto: " + paquete.Paquete_alto + "cm, Ancho: " + paquete.Paquete_ancho + "cm, Largo: " + paquete.Paquete_largo + "cm");
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 575, 9, 0, 1, 1, data);
                        data = Texto.GetBytes("Peso: " + paquete.Paquete_peso + "kg, Peso Volumetrico: " + paquete.Paquete_peso_volumetrico + "Kg/m3");
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 605, 9, 0, 1, 1, data);

                        data = Texto.GetBytes(Domicilio); //Tipo de mercancia
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 705, 9, 0, 2, 2, data);

                        if (Domicilio.Length > 1)
                        {
                            num = tspl_dll.TSPL_QrCode(_dll.printer, 500, 760, 3, 5, 0, 0, 0, 7, "\"" + encabezado.Url_envio + "\"");
                        }
                        #endregion

                        #region Footer Principal Listo
                        num = tspl_dll.TSPL_Box(_dll.printer, 20, 992, 730, 1200, 8); //footer principal
                        data = Texto.GetBytes("NUMERO DE TRACKING");
                        num = tspl_dll.TSPL_Text(_dll.printer, 150, 1009, 9, 0, 2, 1, data);
                        num = tspl_dll.TSPL_BarCode(_dll.printer, 130, 1050, 8, 100, 1, 0, 2, 2, encabezado.NumeroTracking);
                        #endregion


                        num = tspl_dll.TSPL_Print(_dll.printer, 1, 1);
                        num = tspl_dll.PortClose(_dll.printer);
                        num = tspl_dll.PrinterDestroy(_dll.printer);
                    }
                }
                else
                {
                    num = tspl_dll.FormatError(num, 1, buf, 0, 0x200);
                    Console.WriteLine(Encoding.Default.GetString(buf));
                }
            }
            else
            {
                num = tspl_dll.FormatError(num, 1, buf, 0, 0x200);
                Console.WriteLine(Encoding.Default.GetString(buf));
            }
        }
       
        public void PrintLabel(XmlDocument xml)
        {
            ExtraerDataXml extraerDataXml = new ExtraerDataXml(xml);
            Encabezado encabezado = extraerDataXml.GetEncabezado();
            Receptor receptor = extraerDataXml.GetReceptor();
            Paquete paquete = extraerDataXml.GetPaquete();
            GlobalHelpers helper = new GlobalHelpers();

            int num = 0;
            byte[] buf = new byte[0x200];
            tspl_dll _dll = new tspl_dll();
            num = tspl_dll.PrinterCreator(ref _dll.printer, "R42");
            if (num == 0)
            {
                num = tspl_dll.PortOpen(_dll.printer, "USB");
                if (num == 0)
                {
                    int status = 0;
                    num = tspl_dll.TSPL_GetPrinterStatus(_dll.printer, ref status);
                    if (9 == status)
                    {
                        Console.WriteLine("head opened and out of ribbon");
                    }
                    else if (12 == status)
                    {
                        Console.WriteLine("Out of ribbon and out of paper");
                    }
                    else if (0x20 == status)
                    {
                        Console.WriteLine("Printing");
                    }
                    else
                    {
                        tspl_dll.TSPL_ClearBuffer(_dll.printer);
                        tspl_dll.TSPL_Setup(_dll.printer, 100, 150, 2, 6, 1, 1, 1);

                        Encoding Texto = Encoding.GetEncoding("UTF-8");
                        byte[] data;

                        #region header Principal Listo
                        num = tspl_dll.TSPL_Box(_dll.printer, 20, 40, 730, 1200, 4); //Header principal
                        num = tspl_dll.TSPL_BitMap(_dll.printer, 38, 68, 0, "icon.jpg");
                        data = Texto.GetBytes(encabezado.NombreEmpresa);
                        num = tspl_dll.TSPL_Text(_dll.printer, 120, 68, 9, 0, 1, 1, data);
                        data = Texto.GetBytes(encabezado.RutDestino);
                        num = tspl_dll.TSPL_Text(_dll.printer, 590, 50, 9, 0, 3, 3, data);
                        #endregion

                        #region Codigo Direccion Envio QR Listo
                        num = tspl_dll.TSPL_Box(_dll.printer, 20, 120, 280, 400, 8); // Codigo Direccion Envio
                        num = tspl_dll.TSPL_QrCode(_dll.printer, 40, 158, 3, 10, 0, 0, 0, 7, "\"" + encabezado.RutDestino + "\"");
                        #endregion

                        #region Informacion del receptor

                        #region Datos del Receptor
                        num = tspl_dll.TSPL_Box(_dll.printer, 272, 120, 730, 400, 8); // Cuadro rectangular
                        data = Texto.GetBytes(helper.TruncaText(helper.normalizaTexto(receptor.Nombre), 36));
                        num = tspl_dll.TSPL_Text(_dll.printer, 288, 138, 9, 0, 1, 1, data);
                        data = Texto.GetBytes(receptor.TipoDocumento + ": " + receptor.Documento);
                        num = tspl_dll.TSPL_Text(_dll.printer, 288, 168, 9, 0, 1, 1, data);
                        data = Texto.GetBytes(receptor.Correo);
                        num = tspl_dll.TSPL_Text(_dll.printer, 288, 198, 9, 0, 1, 1, data);
                        data = Texto.GetBytes(receptor.Telefono);
                        num = tspl_dll.TSPL_Text(_dll.printer, 288, 228, 9, 0, 1, 1, data);
                        #endregion

                        #region direccion de Recepcion
                        string direccionEnvio = "";
                        string Domicilio = "";
                        if (encabezado.IdOrderType == "2")
                        {
                            direccionEnvio += encabezado.DirOffDestino;
                        }
                        else if (encabezado.IdOrderType == "1" || encabezado.IdOrderType == "3")
                        {
                            direccionEnvio += encabezado.DirDestino;
                            Domicilio += "Door to door";
                        }

                        num = tspl_dll.TSPL_Box(_dll.printer, 272, 255, 730, 400, 4); // Cuadro rectangular
                        var direccion = helper.DivideText(helper.normalizaTexto(direccionEnvio), 35);
                        int space = 0;
                        int maxLineas = 4;
                        foreach (string dir in direccion)
                        {
                            if (maxLineas > 0)
                            {
                                data = Texto.GetBytes(dir.Trim());
                                num = tspl_dll.TSPL_Text(_dll.printer, 288, 270 + space, 9, 0, 1, 1, data);
                                space = space + 30;
                            }
                            maxLineas = maxLineas - 1;
                        }
                        #endregion

                        #endregion

                        #region Main Principal falta
                        num = tspl_dll.TSPL_Box(_dll.printer, 20, 120, 730, 1000, 8); //main principal
                        num = tspl_dll.TSPL_Box(_dll.printer, 630, 500, 730, 392, 8); //Cuadro de tipo
                        data = Texto.GetBytes(paquete.Paquete_tipo_codigo); //Tipo de mercancia
                        num = tspl_dll.TSPL_Text(_dll.printer, 665, 415, 9, 0, 3, 3, data);

                        data = Texto.GetBytes(encabezado.RutOffEmisora + " -> " + encabezado.RutDestino); //Tipo de mercancia
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 415, 9, 0, 2, 2, data);
                        num = tspl_dll.TSPL_BarCode(_dll.printer, 50, 850, 8, 100, 1, 0, 2, 2, encabezado.RutDestino);

                        data = Texto.GetBytes("Description: " + helper.normalizaTexto(paquete.Paquete_descripcion));
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 545, 9, 0, 1, 1, data);
                        data = Texto.GetBytes("High: " + paquete.Paquete_alto + "cm, Width: " + paquete.Paquete_ancho + "cm, Long: " + paquete.Paquete_largo + "cm");
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 575, 9, 0, 1, 1, data);
                        data = Texto.GetBytes("Weight: " + paquete.Paquete_peso + "kg, Volumetric Weight: " + paquete.Paquete_peso_volumetrico + "Kg/m3");
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 605, 9, 0, 1, 1, data);

                        data = Texto.GetBytes(Domicilio); //Tipo de mercancia
                        num = tspl_dll.TSPL_Text(_dll.printer, 50, 705, 9, 0, 2, 2, data);

                        if (Domicilio.Length > 1)
                        {
                            num = tspl_dll.TSPL_QrCode(_dll.printer, 500, 760, 3, 5, 0, 0, 0, 7, "\"" + encabezado.Url_envio + "\"");
                        }
                        #endregion

                        #region Footer Principal Listo
                        num = tspl_dll.TSPL_Box(_dll.printer, 20, 992, 730, 1200, 8); //footer principal
                        data = Texto.GetBytes("NUMBER OF TRACKING");
                        num = tspl_dll.TSPL_Text(_dll.printer, 150, 1009, 9, 0, 2, 1, data);
                        num = tspl_dll.TSPL_BarCode(_dll.printer, 130, 1050, 8, 100, 1, 0, 2, 2, encabezado.NumeroTracking);
                        #endregion


                        num = tspl_dll.TSPL_Print(_dll.printer, 1, 1);
                        num = tspl_dll.PortClose(_dll.printer);
                        num = tspl_dll.PrinterDestroy(_dll.printer);
                    }
                }
                else
                {
                    num = tspl_dll.FormatError(num, 1, buf, 0, 0x200);
                    Console.WriteLine(Encoding.Default.GetString(buf));
                }
            }
            else
            {
                num = tspl_dll.FormatError(num, 1, buf, 0, 0x200);
                Console.WriteLine(Encoding.Default.GetString(buf));
            }
        }


        public class tspl_dll
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



}
