using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PLANETAVERDE_API.Models;

namespace PLANETAVERDE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiasController : ControllerBase
    {
        private readonly PLANETAVERDEWEBContext _context=new PLANETAVERDEWEBContext();

        public NoticiasController()
        {
        }

        // GET: api/Noticias
        [HttpGet]
        public dynamic GetNoticia()
        {
            dynamic jsonResponse = new JObject();
            try
            {
                JArray Jarray = new JArray();
              var noticia = _context.Noticia.OrderByDescending(n => n.FhRegistro).ToList();

                for (int i = 0; i < noticia.Count; i++)
                {
                    Jarray.Add(JToken.FromObject(
            new Noticia
            {
                IdNoticiaHeader = noticia[i].IdNoticiaHeader,
                NbNoticia = noticia[i].NbNoticia,
                DeNoticia = noticia[i].DeNoticia,
                VlImage = noticia[i].VlImage,
                FhRegistro = noticia[i].FhRegistro,
                UsRegistro = noticia[i].UsRegistro
            }));
                }
                jsonResponse.code = 200;
                jsonResponse.msj = "";
                jsonResponse.data = Jarray;

                return jsonResponse;
            }
            catch (Exception e)
            {
                jsonResponse.code = 500;
                jsonResponse.data = null;
                jsonResponse.msj = e.Message;
                return jsonResponse;
            }
           
        }

        // GET: api/Noticias/5
        [HttpGet("{id}")]
        public dynamic GetNoticia(string id,int seccion=0,string idnoticia="")
        {
            dynamic jsonResponse = new JObject();
            try
            {
                var categoria = _context.Categoria.ToList().Find(x => x.NbCategoriaHeader == id);

                if (categoria == null)
                {
                    jsonResponse.code = 400;
                    jsonResponse.data = null;
                    jsonResponse.msj = "No se encontraron datos";
                    return jsonResponse;
                }
                var noticia=_context.Noticia.FromSqlRaw($"SP_GETNOTICIA_BY_CATEGORIA '{id}','{seccion}','{idnoticia}'").ToList();
                JArray Jarray = new JArray();
                for (int i = 0; i < noticia.Count; i++)
                {
                    Jarray.Add(JToken.FromObject(
            new Noticia
            {
                IdNoticiaHeader = noticia[i].IdNoticiaHeader,
                NbNoticia = noticia[i].NbNoticia,
                DeNoticia = noticia[i].DeNoticia,
                VlImage = noticia[i].VlImage,
                FhRegistro = noticia[i].FhRegistro,
                UsRegistro = noticia[i].UsRegistro
            }));
                }
                jsonResponse.code = 200;
                jsonResponse.msj = "";
                jsonResponse.data = Jarray;

                return jsonResponse;
            }
            catch (Exception e)
            {
                jsonResponse.code = 500;
                jsonResponse.data = null;
                jsonResponse.msj = e.Message;
                return jsonResponse;
            }
           
        }

        [HttpGet("buscar/{id}")]
        public dynamic GetNoticia(string id)
        {
            dynamic jsonResponse = new JObject();
            try
            {
                var noticia = _context.Noticia.FromSqlRaw($"SP_BUSCARNOTICIA '{id}'").ToList();
                JArray Jarray = new JArray();
                for (int i = 0; i < noticia.Count; i++)
                {
                    Jarray.Add(JToken.FromObject(
            new Noticia
            {
                IdNoticiaHeader = noticia[i].IdNoticiaHeader,
                NbNoticia = noticia[i].NbNoticia,
                DeNoticia = noticia[i].DeNoticia,
                VlImage = noticia[i].VlImage,
                FhRegistro = noticia[i].FhRegistro,
                UsRegistro = noticia[i].UsRegistro
            }));
                }
                jsonResponse.code = 200;
                jsonResponse.msj = "";
                jsonResponse.data = Jarray;

                return jsonResponse;
            }
            catch (Exception e)
            {
                jsonResponse.code = 500;
                jsonResponse.data = null;
                jsonResponse.msj = e.Message;
                return jsonResponse;
            }

        }


        // POST: api/Noticias
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public dynamic PostNoticia([FromBody]JObject ini)
        {
            dynamic jsonResponse = new JObject();
            try
            {
                var inicial = ini.Value<string>("ini");
                var cant = ini.Value<string>("cant");
                var cat = ini.Value<string>("cat");
                var noticia = _context.Noticia.FromSqlRaw($"SP_GETNOTICIAS_PAGINATION '{cat}',{inicial},{cant}").ToList();
                JArray Jarray = new JArray();
                for (int i = 0; i < noticia.Count; i++)
                {
                    Jarray.Add(JToken.FromObject(
            new Noticia
            {
                IdNoticiaHeader = noticia[i].IdNoticiaHeader,
                NbNoticia = noticia[i].NbNoticia,
                DeNoticia = noticia[i].DeNoticia,
                VlImage = noticia[i].VlImage,
                FhRegistro = noticia[i].FhRegistro,
                UsRegistro = noticia[i].UsRegistro
            }));
                }
                jsonResponse.code = 200;
                jsonResponse.msj = "";
                jsonResponse.data = Jarray;

                return jsonResponse;
            }
            catch (Exception e)
            {
                jsonResponse.code = 500;
                jsonResponse.data = null;
                jsonResponse.msj = e.Message;
                return jsonResponse;
            }
        }


        private bool NoticiaExists(string id)
        {
            return _context.Noticia.Any(e => e.IdNoticiaHeader == id);
        }
    }
}
