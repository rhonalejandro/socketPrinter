using System;
using System.Collections.Generic;
using System.Text;

namespace SocketListener.Entities
{
    class Encabezado
    {
        public string OrderType { get; set; }
        public string IdOrderType { get; set; }
        public string NombreEmpresa { get; set; }
        public string NumeroTracking { get; set; }
        public string CodeOffEmisora { get; set; }
        public string DirOffEmisora { get; set; }
        public string UbiOffEmisora { get; set; }
        public string RutOffEmisora { get; set; }
        public string ZonOffEmisora { get; set; }
        public string DirDestino { get; set; }
        public string UbiDestino { get; set; }
        public string IdUbiOffEmisora { get; set; }
        public string RutDestino { get; set; }
        public string ZonDestino { get; set; }
        public string CodeOffDestino { get; set; }
        public string DirOffDestino { get; set; }
        public string UbiOffDestino { get; set; }
        public string IdUbiOffDestino { get; set; }
        public string RutOffDestino { get; set; }
        public string ZonOffDestino { get; set; }
        public string Fecha_envio { get; set; }
        public string Url_envio { get; set; }

    }
}
