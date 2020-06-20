using System;
using System.Collections.Generic;

namespace PLANETAVERDE_API.Models
{
    public partial class NoticiaDetalle
    {
        public string IdNoticiaHeader { get; set; }
        public string TxNoticia { get; set; }
        public DateTime? FhRegistro { get; set; }
        public string UsRegistro { get; set; }

        public virtual Noticia IdNoticiaHeaderNavigation { get; set; }
    }
}
