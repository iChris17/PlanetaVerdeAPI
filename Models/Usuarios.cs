using System;
using System.Collections.Generic;

namespace PLANETAVERDE_API.Models
{
    public partial class Usuarios
    {
        public int IdUsuario { get; set; }
        public string NbUsuario { get; set; }
        public string VlContraseña { get; set; }
        public DateTime? FhRegistro { get; set; }
    }
}
