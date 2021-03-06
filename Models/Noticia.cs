﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PLANETAVERDE_API.Models
{
    public partial class Noticia
    {
        public Noticia()
        {
            NoticiaCategoria = new HashSet<NoticiaCategoria>();
        }

        public string IdNoticiaHeader { get; set; }
        public string NbNoticia { get; set; }
        public string DeNoticia { get; set; }
        public string VlImage { get; set; }
        public DateTime? FhRegistro { get; set; }
        public string UsRegistro { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual NoticiaDetalle NoticiaDetalle { get; set; }
        public virtual ICollection<NoticiaCategoria> NoticiaCategoria { get; set; }
    }
}
