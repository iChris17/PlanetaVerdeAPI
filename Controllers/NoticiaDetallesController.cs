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
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiaDetallesController : ControllerBase
    {
        private readonly PLANETAVERDEWEBContext _context = new PLANETAVERDEWEBContext();

        public NoticiaDetallesController()
        {
          
        }

        /// <summary>
        /// Obtener una noticia especifica.
        /// </summary>
        /// <param name="idNoticiaHeader"></param> 
        // GET: api/NoticiaDetalles/5
        [HttpGet("{id}")]
        public dynamic GetNoticiaDetalle(string idNoticiaHeader)
        {
            dynamic jsonResponse = new JObject();
            try
            {
                var noticiadetalle = _context.NoticiaDetalle.Include(x => x.IdNoticiaHeaderNavigation).ToList().Find(x => x.IdNoticiaHeader == idNoticiaHeader);

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

        /// <summary>
        /// Inserta una noticia y su detalle.
        /// </summary>
        /// <remarks>
        /// registro de noticia:
        ///
        ///     POST /noticiadetalles
        ///     {
        ///	        "idNoticiaHeader":"Prueba",
        ///	        "nbNoticia":"nombre",
        ///	        "deNoticia":"desc",
        ///	        "vlImage":"base64",
        ///	        "usRegistro":"CACEVEDO",
        ///	        "txNoticia":"noticia"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Se registro correctamente</response>
        /// <response code="400">Ocurrio un error</response>  
        /// /// <response code="500">Ocurrio un error de servidor</response>  

    [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Agregar categoria a una noticia.
        /// </summary>
        /// <remarks>
        /// Añadir categoria:
        ///
        ///     POST /noticiadetalles
        ///     {
        ///	        "accion":"ADD",
        ///	        "idCategoria":0,
        ///	        "idNoticiaHeader":"noticia-prueba",
        ///	        "usRegistro":"CACEVEDO",
        ///     }
        ///
        /// Eliminar Categoria:
        ///
        ///     POST /noticiadetalles
        ///     {
        ///	        "accion":"DELETE",
        ///	        "idCategoria":0,
        ///	        "idNoticiaHeader":"noticia-prueba",
        ///	        "usRegistro":"CACEVEDO",
        ///     }
        ///
        /// </remarks>

        [HttpPost("Categoria")]
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
