using Microsoft.Extensions.Configuration;
using SocketListener.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace SocketListener.Logic
{
    class GlobalHelpers
    {
        public string xmlprueba = "<?xml version=\"1.0\"?><root><id_order_type>3</id_order_type><order_type>Envio Express</order_type><tracking>SJB2-210831-000000022</tracking><fecha_envio>2021-08-31 11:20:44</fecha_envio><nombre_receptor>DIEGO ALBERTO MORA ROSALES</nombre_receptor><phone_receptor>88270028</phone_receptor><email_receptor>diego@sngstore.com</email_receptor><documento_receptor>503240137</documento_receptor><t_documento_receptor>CI</t_documento_receptor><code_off_o_emisora>SJO-C</code_off_o_emisora><direccion_o_emisora>San José, 400 norte de la antigua Embajada Americana, edificio Ricardo Padilla</direccion_o_emisora><id_ubicacion_o_emisora>11</id_ubicacion_o_emisora><ubicacion_o_emisora>CENTRO SAN JOSE</ubicacion_o_emisora><ruta_o_emisora>SJB2</ruta_o_emisora><zona_o_emisora>San Jose</zona_o_emisora><direccion_destino>Coca-Cola Femsa S.A., Colonia Florida, San José, Calle Blancos, Costa Rica</direccion_destino><ubicacion_destino>CALLE BLANCOS</ubicacion_destino><ruta_destino>AB2</ruta_destino><zona_destino>San Jose</zona_destino><code_off_o_destino/><direccion_o_destino/><id_ubicacion_o_destino/><ubicacion_o_destino/><ruta_o_destino/><zona_o_destino/><paquete_descripcion>aaa</paquete_descripcion><paquete_alto>0.00</paquete_alto><paquete_ancho>3.00</paquete_ancho><paquete_largo>22.00</paquete_largo><paquete_peso>33.00</paquete_peso><latitude>9.945607989123092</latitude><longitude>-84.0675276517868</longitude><paquete_peso_volumetrico>0.00</paquete_peso_volumetrico><paquete_tipo_codigo>A</paquete_tipo_codigo><paquete_tipo>Paquete</paquete_tipo><nombre_empresa>SNG CONSULTORES SOCIEDAD ANONIMA</nombre_empresa><url_envio>https://waze.com/ul?ll=9.945607989123092,-84.0675276517868%26z=10</url_envio></root>";
        public string documentoprueba = "     SNG CONSULTORES SOCIEDAD ANONIMA    \n          Ced. Iden.: 3101647276         \n           info@deprixapro.site          \n         Oficina: San Jose Centro        \n San Jos\u00e9, 400 norte de la antigua Embaj\n ada Americana, edificio Ricardo Padilla \n Distrito:San Jose   Telefono: 22222222  \n ----------------------------------------\n Tiquete Electronico 00100001040000000345\n Fecha: 2021-06-24T15:00:32              \n Agente: RHONALD ALEJANDRO BRITO QUERALES\n Cliente: N/A                            \n ----------------------------------------\n         DESCRIPCION DEL PAQUETE         \n ----------------------------------------\n                                         \n Encomienda paquete ALJ-210-624-000-000-2\n 10 (2.00kg) (0.00Kg/m3)                 \n                                 3.500,00\n                                         \n ----------------------------------------\n                                         \n           SubTotal:             3.500,00\n          Descuento:                 0,00\n              Total:             3.500,00\n                IVA:               455,00\n  Total comprobante:             3.955,00\n                                         \n ----------------------------------------\n                                         \n     Clave del comprobante electonico    \n 5062406210031016472760010000104000000034\n                5185410925               \n                                         \n Autorizada mediante  resolucion No. DGT-\n   R-033-2019 del 20 de junio del 2019   \n                                         \n                                         \n                                         \n";
        public static Printer impresora;
        public static Settings configuracion;

        #region Truncar texto
        public string TruncaText(string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            var returnValue = value;
            if (value.Length > length)
            {
                var tmp = value.Substring(0, length);
                if (tmp.LastIndexOf(' ') > 0)
                    returnValue = tmp.Substring(0, tmp.LastIndexOf(' ')) + " ...";
            }
            return returnValue;
        }
        #endregion

        #region Normalizar textos quitar caracteres especiales
        public string normalizaTexto(string palabra)
        {

            string palabaSinTildes = Regex.Replace(palabra.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
            return palabaSinTildes;
        }
        #endregion

        #region Divide Texto
        public string[] DivideText(string text, int lineLength)
        {
            return Regex.Matches(text, ".{1," + lineLength + "}").Cast<Match>().Select(m => m.Value).ToArray();
        }
        #endregion


        public XmlDocument XmlToObject(String value)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(value);
            return doc;
        }

        public static void cargarConfiguracion()
        {
            //Obtiene una instancia del archivo de configuracion
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            var sectionImpresora = config.GetSection(nameof(Printer));
            impresora = sectionImpresora.Get<Printer>();

            var sectionConfig = config.GetSection(nameof(Settings));
            configuracion = sectionConfig.Get<Settings>();
        }

    }
}
