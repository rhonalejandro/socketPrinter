using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SocketListener.Logic
{
    class tscAirboxLabel
    {
        private XmlDocument _xml;
        private string _nombreImpresora;
        private string _guia;
        private List<List<string>> _ItemsEnvio;
        private string _date;
        private string _documen;
        private string _encargado;
        private string _freight;
        private string _nombre_user;
        private string _num_pack;
        private string _proveedor;
        private string _referido;
        private string _tracking;
        private string _user_id;
        private string _weight;
        private XmlNodeList _nodos;
        private XmlNode _nodo;

        public void Imprimir(XmlDocument xml, string nombreImpresora)
        {
            _xml = xml;
            
            _nombreImpresora = getText("nombreImpresora");
            _guia = getText("guia");

            _nodos = getNodes("root/items/item");

            _ItemsEnvio = new List<List<string>>(); 
            foreach (XmlNode node in _nodos)
            {
                List<string> itm = new List<string>();
                if (node != null)
                {
                    itm.Add(node["row1"].InnerText);
                    itm.Add(node["row2"].InnerText);
                    itm.Add(node["row3"].InnerText);
                    itm.Add(node["row4"].InnerText);
                    itm.Add(node["row5"].InnerText);
                    _ItemsEnvio.Add(itm);
                }
            }

            _nodo = getNode("encabezado");
            _date = getText("date", _nodo);
            _documen = getText("documen", _nodo);
            _encargado = getText("encargado", _nodo);
            _freight = getText("freight", _nodo);
            _nombre_user = getText("nombre_user", _nodo);
            _num_pack = getText("num_pack", _nodo);
            _proveedor = getText("proveedor", _nodo);
            _referido = getText("referido", _nodo);
            _tracking = getText("tracking", _nodo);
            _user_id = getText("user_id", _nodo);
            _weight = getText("weight", _nodo);

            imprimeEtiqueta();
        }

        private string getText(string _nodeText, XmlNode nodoSelected = null)
        {
            string texto = "";
            if (nodoSelected != null)
            {
                texto = nodoSelected[_nodeText].InnerText;
            }
            else { 
                var node = _xml.GetElementsByTagName(_nodeText)[0];
                if (node != null)
                {
                    texto = node.InnerText;
                }
            }

            return texto;
        }

        private XmlNodeList getNodes(string _nodeName)
        {
            XmlNodeList node = _xml.SelectNodes(_nodeName);
            if (node != null)
            {
                return node;
            }
            else
            { 
                return null;
            }
        }

        private XmlNode getNode(string _nodeName)
        {
            XmlNode node = _xml.SelectSingleNode(_nodeName);
            if (node != null)
            {
                return node;
            }
            else
            {
                return null;
            }
        }


        private void imprimeEtiqueta()
        {
            int cant = int.Parse(_num_pack);
            int i = 1;

            for (i = 1; i <= cant; i++)
            {
                Console.WriteLine(i);
                byte[] text;

                byte status = TSCLIB_DLL.usbportqueryprinter();
                TSCLIB_DLL.openport(_nombreImpresora);
                TSCLIB_DLL.sendcommand("SIZE 100 mm, 150 mm");
                TSCLIB_DLL.sendcommand("SPEED 4");
                TSCLIB_DLL.sendcommand("DENSITY 12");
                TSCLIB_DLL.sendcommand("DIRECTION 1");
                TSCLIB_DLL.sendcommand("SET TEAR ON");
                TSCLIB_DLL.sendcommand("CODEPAGE UTF-8");
                TSCLIB_DLL.clearbuffer();

                TSCLIB_DLL.sendcommand("BOX 20, 10, 800, 1150, 8");

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes("GUIA AEREA");
                TSCLIB_DLL.windowsfontUnicode(35, 40, 32, 0, 0, 0, "Arial", text);

                TSCLIB_DLL.barcode("230", "30", "128", "100", "1", "0", "3", "1", _guia);

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes(_documen);
                TSCLIB_DLL.windowsfontUnicode(680, 50, 68, 0, 0, 0, "Arial", text);

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes("SHIPPER "+ _proveedor);
                TSCLIB_DLL.windowsfontUnicode(35, 250, 35, 0, 0, 0, "Arial", text);

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes("CONSIGNEE: "+ _nombre_user);
                TSCLIB_DLL.windowsfontUnicode(35, 290, 35, 0, 0, 0, "Arial", text);

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes("DESCRIPTION: "+ _ItemsEnvio[0][0]);
                TSCLIB_DLL.windowsfontUnicode(35, 330, 35, 0, 0, 0, "Arial", text);

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes("TRACKING: "+ _tracking);
                TSCLIB_DLL.windowsfontUnicode(35, 370, 35, 0, 0, 0, "Arial", text);

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes("DECLARE VALUE: $"+ _ItemsEnvio[0][3]);
                TSCLIB_DLL.windowsfontUnicode(35, 410, 35, 0, 0, 0, "Arial", text);

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes("WEIGHT: "+ _weight);
                TSCLIB_DLL.windowsfontUnicode(35, 450, 35, 0, 0, 0, "Arial", text);

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes("REFERENCE: "+ _ItemsEnvio[0][4]);
                TSCLIB_DLL.windowsfontUnicode(35, 490, 35, 0, 0, 0, "Arial", text);

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes("ORIGEN");
                TSCLIB_DLL.windowsfontUnicode(50, 570, 42, 0, 0, 0, "Arial", text);

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes("DESTINATION");
                TSCLIB_DLL.windowsfontUnicode(270, 570, 42, 0, 0, 0, "Arial", text);

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes("BULTOS");
                TSCLIB_DLL.windowsfontUnicode(570, 570, 42, 0, 0, 0, "Arial", text);

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes("MIAMI");
                TSCLIB_DLL.windowsfontUnicode(70, 610, 42, 0, 0, 0, "Arial", text);

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes("SAN JOSE");
                TSCLIB_DLL.windowsfontUnicode(290, 610, 42, 0, 0, 0, "Arial", text);

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes(i.ToString() + "/" + cant.ToString());
                TSCLIB_DLL.windowsfontUnicode(590, 610, 42, 0, 0, 0, "Arial", text);

                text = System.Text.Encoding.GetEncoding("utf-16").GetBytes("INSUUED MIAMI "+_date);
                TSCLIB_DLL.windowsfontUnicode(100, 1100, 32, 0, 0, 0, "Arial", text);

                TSCLIB_DLL.printlabel("1", "1");
                TSCLIB_DLL.closeport();
              
            }
        }

    }
}
