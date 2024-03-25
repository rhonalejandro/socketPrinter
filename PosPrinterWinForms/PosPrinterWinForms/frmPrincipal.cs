using Microsoft.Win32;
using SocketListener.Logic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
namespace PosPrinterWinForms
{
    public partial class frmPrincipal : Form
    {
        const int PORT_NO = 4500;
        static Socket serverSocket;
        static private string guid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        private bool cerrar = false;
        private static readonly object _lock = new object();

        public frmPrincipal()
        {
            InitializeComponent();

            GlobalHelpers.cargarConfiguracion();
            
            startSoket();

            txtIp.Text = GlobalHelpers.impresora.Ip;
            txtNombre.Text = GlobalHelpers.impresora.Name;
            chbAutoStart.Checked = GlobalHelpers.configuracion.AutoStart;
        }

        private static void startSoket()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT_NO));
            serverSocket.Listen(1); //just one socket
            serverSocket.BeginAccept(null, 0, OnAccept, null);
        }

        #region Funciones del socket
        private static void OnAccept(IAsyncResult result)
        {
            
                byte[] buffer = new byte[4096000];
                try
                {
                    Socket client = null;
                    string headerResponse = "";
                    if (serverSocket != null && serverSocket.IsBound)
                    {
                        client = serverSocket.EndAccept(result);
                        int bytesRead = client.Receive(buffer); // Leer la cantidad real de bytes recibidos
                        headerResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    }
                    if (client != null)
                    {
                        var key = headerResponse.Replace("ey:", "`")
                                   .Split('`')[1]   
                                   .Replace("\r", "").Split('\n')[0]  
                                   .Trim();

                        var test1 = AcceptKey(ref key);

                        var newLine = "\r\n";

                        var response = "HTTP/1.1 101 Switching Protocols" + newLine
                             + "Upgrade: websocket" + newLine
                             + "Connection: Upgrade" + newLine
                             + "Sec-WebSocket-Accept: " + test1 + newLine + newLine
                             ;

                        client.Send(Encoding.UTF8.GetBytes(response));
                        var i = client.Receive(buffer);
                        string browserSent = GetDecodedData(buffer, i);

                        try
                        {
                            // Intenta deserializar el JSON
                            Body JsonString = JsonSerializer.Deserialize<Body>(browserSent);
                            var type = JsonString.type;
                            var printer = JsonString.printer;
                            IList<string> data = JsonString.data;

                            switch (type)
                            {
                                case "label-printer":
                                    ImprimirEtiqueta(data, printer);
                                    break;
                                case "pos-printer":
                                    ImprimirDocumentos(data, printer);
                                    break;
                                default:
                                    // Manejar otro tipo de objeto
                                    break;
                            }
                        }
                        catch (JsonException)
                        {
                            var JsonString = JsonSerializer.Deserialize<object>(browserSent);
                            ImprimirDocumento(JsonString.ToString());
                        }


                    
                    
                        System.Threading.Thread.Sleep(10000);
                    }
                }
                catch (SocketException exception)
                {
                
                    Console.WriteLine($"Exception: {exception.Message}");
                }
                catch (Exception ex)
                {
                    // Manejar otras excepciones
                    Console.WriteLine($"Exception: {ex.Message}");
                }
                finally
                {
                    if (serverSocket != null && serverSocket.IsBound)
                    {
                        serverSocket.BeginAccept(null, 0, OnAccept, null);
                    }
                }
          
        }

        public static T[] SubArray<T>(T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        private static string AcceptKey(ref string key)
        {
            string longKey = key + guid;
            byte[] hashBytes = ComputeHash(longKey);
            return Convert.ToBase64String(hashBytes);
        }

        static SHA1 sha1 = SHA1CryptoServiceProvider.Create();
        private static byte[] ComputeHash(string str)
        {
            return sha1.ComputeHash(Encoding.ASCII.GetBytes(str));
        }

        //Needed to decode frame
        public static string GetDecodedData(byte[] buffer, int length)
        {
            byte b = buffer[1];
            int dataLength = 0;
            int totalLength = 0;
            int keyIndex = 0;

            if (b - 128 <= 125)
            {
                dataLength = b - 128;
                keyIndex = 2;
                totalLength = dataLength + 6;
            }

            if (b - 128 == 126)
            {
                dataLength = BitConverter.ToInt16(new byte[] { buffer[3], buffer[2] }, 0);
                keyIndex = 4;
                totalLength = dataLength + 8;
            }

            if (b - 128 == 127)
            {
                dataLength = (int)BitConverter.ToInt64(new byte[] { buffer[9], buffer[8], buffer[7], buffer[6], buffer[5], buffer[4], buffer[3], buffer[2] }, 0);
                keyIndex = 10;
                totalLength = dataLength + 14;
            }

            if (totalLength > length)
                throw new Exception("The buffer length is small than the data length");

            byte[] key = new byte[] { buffer[keyIndex], buffer[keyIndex + 1], buffer[keyIndex + 2], buffer[keyIndex + 3] };

            int dataIndex = keyIndex + 4;
            int count = 0;
            for (int i = dataIndex; i < totalLength; i++)
            {
                buffer[i] = (byte)(buffer[i] ^ key[count % 4]);
                count++;
            }

            return Encoding.ASCII.GetString(buffer, dataIndex, dataLength);
        }

        public enum EOpcodeType
        {
            /* Denotes a continuation code */
            Fragment = 0,

            /* Denotes a text code */
            Text = 1,

            /* Denotes a binary code */
            Binary = 2,

            /* Denotes a closed connection */
            ClosedConnection = 8,

            /* Denotes a ping*/
            Ping = 9,

            /* Denotes a pong */
            Pong = 10
        }

        public static byte[] GetFrameFromString(string Message, EOpcodeType Opcode = EOpcodeType.Text)
        {
            byte[] response;
            byte[] bytesRaw = Encoding.Default.GetBytes(Message);
            byte[] frame = new byte[10];

            int indexStartRawData = -1;
            int length = bytesRaw.Length;

            frame[0] = (byte)(128 + (int)Opcode);
            if (length <= 125)
            {
                frame[1] = (byte)length;
                indexStartRawData = 2;
            }
            else if (length >= 126 && length <= 65535)
            {
                frame[1] = (byte)126;
                frame[2] = (byte)((length >> 8) & 255);
                frame[3] = (byte)(length & 255);
                indexStartRawData = 4;
            }
            else
            {
                frame[1] = (byte)127;
                frame[2] = (byte)((length >> 56) & 255);
                frame[3] = (byte)((length >> 48) & 255);
                frame[4] = (byte)((length >> 40) & 255);
                frame[5] = (byte)((length >> 32) & 255);
                frame[6] = (byte)((length >> 24) & 255);
                frame[7] = (byte)((length >> 16) & 255);
                frame[8] = (byte)((length >> 8) & 255);
                frame[9] = (byte)(length & 255);

                indexStartRawData = 10;
            }

            response = new byte[indexStartRawData + length];

            int i, reponseIdx = 0;

            //Add the frame bytes to the reponse
            for (i = 0; i < indexStartRawData; i++)
            {
                response[reponseIdx] = frame[i];
                reponseIdx++;
            }

            //Add the data bytes to the response
            for (i = 0; i < length; i++)
            {
                response[reponseIdx] = bytesRaw[i];
                reponseIdx++;
            }

            return response;
        }

        private static void ImprimirEtiqueta(IList<string> xmlimprimirsh, string printer)
        {
            GlobalHelpers helper = new GlobalHelpers();
            XmlDocument xml;
            ImprimirEtiqueta imprimirEtiqueta;
            foreach (var xmlimprimir in xmlimprimirsh)
            {
                switch (printer.ToLower()){
                    case "zebra":
                        zebraLabel zebra = new zebraLabel();
                        zebra.imprimir(helper.XmlToObject(xmlimprimir.ToString()));
                        break;
                    case "dymo":

                        break;
                    case "q5bt":
                        xml = helper.XmlToObject(xmlimprimir.ToString());
                        imprimirEtiqueta = new ImprimirEtiqueta();
                        imprimirEtiqueta.Imprimir(xml);
                        break;
                    case "tsc":
                        xml = helper.XmlToObject(xmlimprimir.ToString());
                        var nodoImpresora = xml.GetElementsByTagName("impresora")[0];
                        var nodoNombreImpresora = xml.GetElementsByTagName("nombreImpresora")[0];
                        var nodoEtiqueta = xml.GetElementsByTagName("etiqueta")[0];
                        var impresora = nodoImpresora.InnerText;
                        var nombreImpresora = nodoNombreImpresora.InnerText;
                        var etiqueta = nodoEtiqueta.InnerText;

                        if (etiqueta == "Airbox")
                        {
                            tscAirboxLabel airboxLabel = new tscAirboxLabel();
                            airboxLabel.Imprimir(xml, nombreImpresora);
                        }
                        break;
                        
                    default:
                        
                        break;
                }

                
            }
            
        }

        private static void ImprimirDocumentos(IList<string> documentos, string printer)
        {
            ImprimirVoucher imprimirVoucher = new ImprimirVoucher();

            foreach (var documento in documentos)
            {
                imprimirVoucher.imprimir(documento);
            }
        }

        private static void ImprimirDocumento(string documento)
        {
            ImprimirVoucher imprimirVoucher = new ImprimirVoucher();
            imprimirVoucher.imprimirSoloDoc(documento);
        }


        private static void ImprimirEtiquetaPrueba()
        {
            GlobalHelpers helper = new GlobalHelpers();
            XmlDocument xml = helper.XmlToObject(helper.xmlprueba);

            ImprimirEtiqueta imprimirEtiqueta = new ImprimirEtiqueta();
            imprimirEtiqueta.Imprimir(xml);
        }

        public static void ImprimirDocumentoPrueba()
        {
            GlobalHelpers helper = new GlobalHelpers();
            ImprimirVoucher imprimirVoucher = new ImprimirVoucher();
            imprimirVoucher.imprimir(helper.documentoprueba);
        }
        #endregion



        #region Componentes del Form

        public void escribirLog(string text)
        {
            txtLog.AppendText(text);
            txtLog.AppendText(Environment.NewLine);
        }

        private void btnPrueba_Click(object sender, EventArgs e)
        {
            
            try
            {
                escribirLog("-------- Printing Test -------- ");
                ImprimirDocumentoPrueba();
                if (GlobalHelpers.configuracion.LabelPrinter)
                { 
                    ImprimirEtiquetaPrueba();
                }
                escribirLog("----- Success -----");
            }
            catch (Exception ex)
            {
                escribirLog("------------------ Error ------------------");
                escribirLog(ex.StackTrace);
                escribirLog(ex.Message);
                notifyIcon1.Icon = SystemIcons.Warning;
                escribirLog("---------------- End Error ----------------");
            }
        }


      
        private void mostrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.BringToFront();
        }

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cerrarApp();
        }

     
        #endregion

        private void CerrarSocket_Click(object sender, EventArgs e)
        {
            cerrarApp();
        }
        private void MinimizeSocket_Click(object sender, EventArgs e)
        {
            minimize();
        }


        public void cerrarApp() {
            DialogResult result = MessageBox.Show(" Are you sure that you want close the Socket printer services?", "Close the Socket printer services", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                cerrar = true;
                notifyIcon1.Visible = false;
                this.Close();
            }
            else
            {
                cerrar = false;
            }

        }

        public void minimize()
        {
            this.Hide();
            notifyIcon1.Visible = true;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.BringToFront();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            AddOrUpdateAppSetting("printer:name", txtNombre.Text);
            AddOrUpdateAppSetting("printer:ip", txtIp.Text);

            MessageBox.Show("Settings updated success", "Printer updated");
            GlobalHelpers.cargarConfiguracion();
        }

        public static void AddOrUpdateAppSetting<T>(string sectionPathKey, T value)
        {
            try
            {
                var filePath = System.IO.Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                string json = System.IO.File.ReadAllText(filePath);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                SetValueRecursively(sectionPathKey, jsonObj, value);

                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(filePath, output);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing app settings | {0}", ex.Message);
            }
        }

        private static void SetValueRecursively<T>(string sectionPathKey, dynamic jsonObj, T value)
        {
            var remainingSections = sectionPathKey.Split(":", 2);

            var currentSection = remainingSections[0];
            if (remainingSections.Length > 1)
            {
                var nextSection = remainingSections[1];
                SetValueRecursively(nextSection, jsonObj[currentSection], value);
            }
            else
            {
                jsonObj[currentSection] = value;
            }
        }
      

        private void chbAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            AddOrUpdateAppSetting("settings:AutoStart", chbAutoStart.Checked);
            GlobalHelpers.configuracion.AutoStart = chbAutoStart.Checked;
        
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (GlobalHelpers.configuracion.AutoStart)
            {
                registryKey.SetValue("RDMPrinterSocket", Path.ChangeExtension(Application.ExecutablePath, ".exe").Replace("/", "\\"));
            }
            else
            {
                registryKey.DeleteValue("RDMPrinterSocket");
            }
        }

    }

    class Body
    {
        public string type { get; set; }
        public string printer { get; set; }

        //public string data { get; set; }
        public IList<string> data { get; set; }
    }

}
