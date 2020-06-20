using System;
using System.Collections.Generic;

namespace PLANETAVERDE_API.Models
{
    public partial class NoticiaCategoria
    {
        public int IdCategoria { get; set; }
        public string IdNoticiaHeader { get; set; }
        public DateTime? FhRegistro { get; set; }
        public string UsRegistro { get; set; }
        public bool? InPrincipal { get; set; }
        public bool? InActivo { get; set; }

        public virtual Categoria IdCategoriaNavigation { get; set; }
        public virtual Noticia IdNoticiaHeaderNavigation { get; set; }
    }
}
