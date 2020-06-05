using System;
using System.Collections.Generic;

namespace PLANETAVERDE_API.Models
{
    public partial class Categoria
    {
        public int IdCategoria { get; set; }
        public string NbCategoria { get; set; }
        public string DeCategoria { get; set; }
        public string TpCategoria { get; set; }
        public DateTime? FhRegistro { get; set; }
        public string UsRegistro { get; set; }
        public string NbCategoriaHeader { get; set; }
    }
}
