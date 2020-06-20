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
    public class NoticiaDetallesController : ControllerBase
    {
        private readonly PLANETAVERDEWEBContext _context = new PLANETAVERDEWEBContext();

        public NoticiaDetallesController()
        {
          
        }

        // GET: api/NoticiaDetalles
        [HttpGet]
        public dynamic GetNoticiaDetalle()
        {
            dynamic jsonResponse = new JObject();
            try
            {
                var noticiadetalle= _context.NoticiaDetalle.ToList();
                JArray Jarray = new JArray();

                for (int i = 0; i < noticiadetalle.Count; i++)
                {
                    Jarray.Add(JToken.FromObject(
            new NoticiaDetalle
            {
                IdNoticiaHeader = noticiadetalle[i].IdNoticiaHeader,
                TxNoticia = noticiadetalle[i].TxNoticia,
                FhRegistro = noticiadetalle[i].FhRegistro,
                UsRegistro = noticiadetalle[i].UsRegistro
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

        // GET: api/NoticiaDetalles/5
        [HttpGet("{id}")]
        public dynamic GetNoticiaDetalle(string id)
        {
            dynamic jsonResponse = new JObject();
            try
            {
                var noticiadetalle = _context.NoticiaDetalle.Include(x => x.IdNoticiaHeaderNavigation).ToList().Find(x => x.IdNoticiaHeader == id);

                if (noticiadetalle == null)
                {
                    jsonResponse.code = 400;
                    jsonResponse.data = null;
                    jsonResponse.msj = "No se encontraron datos";
                    return jsonResponse;
                }
                jsonResponse.code = 200;
                jsonResponse.msj = "";
                jsonResponse.data = JToken.FromObject(new NoticiaDetalle
                {
                    IdNoticiaHeader = noticiadetalle.IdNoticiaHeader,
                    TxNoticia = noticiadetalle.TxNoticia,
                    FhRegistro = noticiadetalle.FhRegistro,
                    UsRegistro = noticiadetalle.UsRegistro,
                    IdNoticiaHeaderNavigation= noticiadetalle.IdNoticiaHeaderNavigation,
                });
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

        // POST: api/NoticiaDetalles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public dynamic PostNoticiaDetalle([FromBody]JObject Noticia)
        {
            dynamic jsonResponse = new JObject();
            try
            {
                var idNoticiaHeader = Noticia.Value<string>("idNoticiaHeader");
                var nbNoticia = Noticia.Value<string>("nbNoticia");
                var deNoticia = Noticia.Value<string>("deNoticia");
                var vlImage = Noticia.Value<string>("vlImage");
                var txNoticia = Noticia.Value<string>("txNoticia");
                var usRegistro = Noticia.Value<string>("usRegistro");

                var respuestaDB=_context.Database.ExecuteSqlRaw($"SP_ADD_NOTICIA '{idNoticiaHeader}','{nbNoticia}','{deNoticia}','{vlImage}','{txNoticia}','{usRegistro}'");
                if (respuestaDB==0)
                {
                    jsonResponse.code = 400;
                    jsonResponse.msj = "No se inserto el registro";
                    jsonResponse.data = null;
                    return jsonResponse;
                }
                jsonResponse.code = 200;
                jsonResponse.msj = "";
                jsonResponse.data = null;

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


        [HttpPost("/Categoria")]
        public dynamic NoticiaCategoria([FromBody]JObject Noti_Cat) {
            dynamic jsonResponse = new JObject();
            try
            {
                var accion = Noti_Cat.Value<string>("accion");
                var idCategoria = Noti_Cat.Value<int>("idCategoria");
                var idNoticiaHeader = Noti_Cat.Value<string>("idNoticiaHeader");
                var usRegistro = Noti_Cat.Value<string>("usRegistro");

                if (accion!="ADD"||accion!="DELETE")
                {
                    jsonResponse.code = 400;
                    jsonResponse.msj = "Error de parametros";
                    jsonResponse.data = null;
                    return jsonResponse;
                }

                var respuestaDB = _context.Database.ExecuteSqlRaw($"SP_ADD_CATEGORIA_NOTICIA '{accion}','{idCategoria}','{idNoticiaHeader}','{usRegistro}'");
                if (respuestaDB == 0)
                {
                    jsonResponse.code = 400;
                    jsonResponse.msj = "No se inserto el registro";
                    jsonResponse.data = null;
                    return jsonResponse;
                }
                jsonResponse.code = 200;
                jsonResponse.msj = "";
                jsonResponse.data = null;

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
        private bool NoticiaDetalleExists(string id)
        {
            return _context.NoticiaDetalle.Any(e => e.IdNoticiaHeader == id);
        }
    }
}
