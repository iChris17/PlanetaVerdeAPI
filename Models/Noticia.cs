using System;
using System.Collections.Generic;

namespace PLANETAVERDE_API.Models
{
    public partial class Noticia
    {
        public string IdNoticiaHeader { get; set; }
        public int? IdCategoria { get; set; }
        public string NbNoticia { get; set; }
        public string DeNoticia { get; set; }
        public string VlImage { get; set; }
        public DateTime? FhRegistro { get; set; }
        public string UsRegistro { get; set; }

        public virtual NoticiaDetalle NoticiaDetalle { get; set; }
    }
}
