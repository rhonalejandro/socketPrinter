using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SocketListener.Logic
{
    class ImprimirVoucher
    {
        public void imprimir(string documento)
        {
 
            bool exists = Directory.Exists(@"C:\voucher\");

            if (!exists)
                Directory.CreateDirectory(@"C:\voucher\");

            string fileName = @"C:\voucher\VoucherFactura.txt";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (StreamWriter sw = File.CreateText(fileName))
            {
                documento = documento.Replace("x001bi0d", "\x001bi\0d");
                sw.WriteLine(documento);
                sw.WriteLine("\x001bi\0d");
            }
            string arguments = $@"/C Type {fileName} > \\{GlobalHelpers.impresora.Ip}\{GlobalHelpers.impresora.Name}";

            Process.Start("cmd.exe", arguments);
        }  

        public void imprimirSoloDoc(string documento)
        {

            bool exists = Directory.Exists(@"C:\voucher\");

            if (!exists)
                Directory.CreateDirectory(@"C:\voucher\");

            string fileName = @"C:\voucher\VoucherFactura.txt";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (StreamWriter sw = File.CreateText(fileName))
            {
                documento = documento.Replace("x001bi0d", "\x001bi\0d");
                sw.WriteLine(documento);
                sw.WriteLine("\x001bi\0d");
            }
            string arguments = $@"/C Type {fileName} > \\{GlobalHelpers.impresora.Ip}\{GlobalHelpers.impresora.Name}";

            Process.Start("cmd.exe", arguments);
        }
    }
}
