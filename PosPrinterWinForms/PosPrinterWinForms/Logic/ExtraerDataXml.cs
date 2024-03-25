using SocketListener.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SocketListener.Logic
{
    class ExtraerDataXml
    {
        private static XmlDocument xml;
       
        public ExtraerDataXml(XmlDocument _xml)
        {
            xml = _xml;
        }

        public Receptor GetReceptor()
        {
            Receptor receptor = new Receptor();
            receptor.Nombre = xml.GetElementsByTagName("nombre_receptor")[0].InnerText;
            receptor.Correo = xml.GetElementsByTagName("email_receptor")[0].InnerText;
            receptor.Documento = xml.GetElementsByTagName("documento_receptor")[0].InnerText;
            receptor.Telefono = xml.GetElementsByTagName("phone_receptor")[0].InnerText;
            receptor.TipoDocumento = xml.GetElementsByTagName("t_documento_receptor")[0].InnerText;
            return receptor;
        }

        public Encabezado GetEncabezado()
        {
            Encabezado encabezado = new Encabezado();
            encabezado.OrderType = xml.GetElementsByTagName("order_type")[0].InnerText;
            encabezado.IdOrderType = xml.GetElementsByTagName("id_order_type")[0].InnerText;
            encabezado.NumeroTracking = xml.GetElementsByTagName("tracking")[0].InnerText;
            encabezado.CodeOffEmisora = xml.GetElementsByTagName("code_off_o_emisora")[0].InnerText;
            encabezado.DirOffEmisora = xml.GetElementsByTagName("direccion_o_emisora")[0].InnerText;
            encabezado.UbiOffEmisora = xml.GetElementsByTagName("ubicacion_o_emisora")[0].InnerText;
            encabezado.IdUbiOffEmisora = xml.GetElementsByTagName("id_ubicacion_o_emisora")[0].InnerText;
            encabezado.RutOffEmisora = xml.GetElementsByTagName("ruta_o_emisora")[0].InnerText;
            encabezado.ZonOffEmisora = xml.GetElementsByTagName("zona_o_emisora")[0].InnerText;
            encabezado.DirDestino = xml.GetElementsByTagName("direccion_destino")[0].InnerText;
            encabezado.UbiDestino = xml.GetElementsByTagName("ubicacion_destino")[0].InnerText;
            encabezado.RutDestino = xml.GetElementsByTagName("ruta_destino")[0].InnerText;
            encabezado.ZonDestino = xml.GetElementsByTagName("zona_destino")[0].InnerText;
            encabezado.CodeOffDestino = xml.GetElementsByTagName("code_off_o_destino")[0].InnerText;
            encabezado.DirOffDestino = xml.GetElementsByTagName("direccion_o_destino")[0].InnerText;
            encabezado.UbiOffDestino = xml.GetElementsByTagName("ubicacion_o_destino")[0].InnerText;
            encabezado.IdUbiOffDestino = xml.GetElementsByTagName("id_ubicacion_o_destino")[0].InnerText;
            encabezado.RutOffDestino = xml.GetElementsByTagName("ruta_o_destino")[0].InnerText;
            encabezado.ZonOffDestino = xml.GetElementsByTagName("zona_o_destino")[0].InnerText;
            encabezado.NombreEmpresa = xml.GetElementsByTagName("nombre_empresa")[0].InnerText;
            encabezado.Url_envio = xml.GetElementsByTagName("url_envio")[0].InnerText;
            encabezado.Fecha_envio = xml.GetElementsByTagName("fecha_envio")[0].InnerText;

            return encabezado;
        }

        public Paquete GetPaquete() {
            Paquete paquete = new Paquete();
            paquete.Paquete_descripcion = xml.GetElementsByTagName("paquete_descripcion")[0].InnerText;
            paquete.Paquete_alto = xml.GetElementsByTagName("paquete_alto")[0].InnerText;
            paquete.Paquete_ancho = xml.GetElementsByTagName("paquete_ancho")[0].InnerText;
            paquete.Paquete_largo = xml.GetElementsByTagName("paquete_largo")[0].InnerText;
            paquete.Paquete_peso = xml.GetElementsByTagName("paquete_peso")[0].InnerText;
            paquete.Paquete_peso_volumetrico = xml.GetElementsByTagName("paquete_peso_volumetrico")[0].InnerText;
            paquete.Paquete_tipo_codigo = xml.GetElementsByTagName("paquete_tipo_codigo")[0].InnerText;
            paquete.Paquete_tipo = xml.GetElementsByTagName("paquete_tipo")[0].InnerText;
            return paquete;
        }
    }
}
