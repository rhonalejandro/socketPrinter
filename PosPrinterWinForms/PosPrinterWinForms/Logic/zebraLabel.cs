using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;

namespace SocketListener.Logic
{
    class zebraLabel
    {

        public void imprimir(XmlDocument documento)
        {

            var printers = UsbDiscoverer.GetZebraUsbPrinters();

            while (printers.Count <1)
            {
                Thread.Sleep(10);
            }
            string destinationDevice = printers[0].Address;
            string templateFilename = "EtiquetaMyS.zpl";
            string defaultQuantityString = "1";
            bool verbose = true;
            string printexample = GetSampleXmlData();

            try {
                MemoryStream xmlToprint = new MemoryStream(Encoding.UTF8.GetBytes(documento.OuterXml.ToString()));
                using (Stream sourceStream = xmlToprint)
                {
                    XmlPrinter.Print(destinationDevice, sourceStream, templateFilename, defaultQuantityString, null, verbose);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }


        }

        private static string GetSampleXmlData()
        {
            string sampleXmlData =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                    + "<file _FORMAT=\"XmlExamp.zpl\">"
                        + " <label>\n"
                            + "     <variable name=\"tracking\">HE-B1-220221-000000057</variable>"
                            + "     <variable name=\"recibofisico\">13123231</variable>"
                            + "     <variable name=\"nombrecliente\">Rhonald Alejandro Brito Querales</variable>"
                            + "     <variable name=\"identificacioncliente\">Dimex: 186200826115</variable>"
                            + "     <variable name=\"telefonocliente\">Telefono: +506 85418284</variable>"
                            + "     <variable name=\"emailcliente\">Email: Rhonalejandro@gmail.com</variable>"
                            + "     <variable name=\"tipopaquete\">A</variable>"
                            + "     <variable name=\"desdehasta\">SJ-B2 > HE-B1</variable>"
                            + "     <variable name=\"destino\">Heredia SAN ROQUE Costa Rica SAN ROQUE Costa Rica asda dasd sada</variable>"
                            + "     <variable name=\"destinol2\">asdasdasdasd asd sadasddHeredia SAN ROQUE Costa Rica SAN ROQUE Costa </variable>"
                            + "     <variable name=\"destinol3\">Heredia asdasdasd asd SAN ROQUE Costa Rica SAN ROQUE </variable>"
                            + "     <variable name=\"nombrereceptor\">Mario Gerardo Fallas Serrano</variable>"
                            + "     <variable name=\"identificacionreceptor\">CI: 108810883</variable>"
                            + "     <variable name=\"telefonoreceptor\">+506 85859652</variable>"
                            + "     <variable name=\"descripcionpaquete\">Caja vacia</variable>"
                            + "     <variable name=\"cantidadpaquete\">Paquete 1 de 2</variable>"
                            + "     <variable name=\"destinocodigo\">HE-B1</variable>"
                            + "     <variable name=\"rutawaze\">iksdjsoiadij asdoaisdj asda osdjo aisjdoajdaoidja odasjdo asjdoasd asoidjas doasjdi asjdaso</variable>"
                            + "  </label>\n"
                            + "</file>";
            
            
            string sampleDataMario =
                "<?xml version=\"1.0\" ?>"
                + "<file>"
                + "<label>"
                + "<variable name=\"tracking\">PUN-1-220711-000015051</variable>"
                + "<variable name=\"recibofisico\">Recibo: </variable>"
                + "<variable name=\"nombrecliente\">MARIO GABRIEL FALLAS FALLAS</variable>"
                + "<variable name=\"identificacioncliente\">C.I 118120255</variable>"
                + "<variable name=\"telefonocliente\">Telefono: no_8</variable>"
                + "<variable name=\"emailcliente\">Email:</variable>"
                + "<variable name=\"tipopaquete\">M</variable>"
                + "<variable name=\"desdehasta\">SJ-B2 -> PUN-1</variable>"
                + "<variable name=\"destino\">Heredia, SAN ROQUE, De la iglesia central, 50m oeste, 150 norte, dia</variable>"
                + "<variable name=\"destinol2\">gonal a la cruz roja. Casa amarilla, con un arbol de limones dulces j</variable>"
                + "<variable name=\"destinol3\">usto a la par. Verjas negras y un cartel que dice Perro bravisimo, t</variable>"
                + "<variable name=\"nombrereceptor\">MARIO GABRIEL FALLAS FALLAS</variable>"
                + "<variable name=\"identificacionreceptor\">CI 118120255</variable>"
                + "<variable name=\"telefonoreceptor\">12345678</variable>"
                + "<variable name=\"descripcionpaquete\">Bultox1, </variable>"
                + "<variable name=\"cantidadpaquete\">Paquete 1 de 1 </variable>"
                + "<variable name=\"destinocodigo\">PUN-1</variable>"
                + "<variable name=\"rutawaze\"> _fjslak;dfjkasld</variable>"
                + "</label>"
                + "</file>";


            return sampleDataMario;
        }
    }


}
