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
    public class CategoriasController : ControllerBase
    {
        private readonly PLANETAVERDEWEBContext _context = new PLANETAVERDEWEBContext();

        public CategoriasController()
        {
            
        }

        /// <summary>
        /// Devuelve la lista de categorias.
        /// </summary>
        /// <remarks>
        /// Tipo: articulo / noticia / all
        /// </remarks>
        /// <param name="tipo"></param> 
        [HttpGet]
        public dynamic GetCategoria(string tipo)
        {
            dynamic jsonResponse = new JObject();
            try
            {
                JArray Jarray = new JArray();
                jsonResponse.code = 200;
                jsonResponse.msj = "";

                List<Categoria> categoria=null;

                if (tipo.ToLower() == "articulo")
                {
                    categoria= _context.Categoria.Where(n => n.TpCategoria == "ARTICULO").ToList();
                }
                else if (tipo.ToLower() == "noticia")
                {
                    categoria= _context.Categoria.Where(n => n.TpCategoria == "NOTICIA").ToList();
                }
                else if (tipo.ToLower() == "all")
                {
                    categoria= _context.Categoria.ToList();
                }else
                {

                    jsonResponse.code = 400;
                    jsonResponse.data = null;
                    jsonResponse.msj = "No se encontraron registros";

                    return jsonResponse;
                }

                for (int i = 0; i < categoria.Count; i++)
                {
                    Jarray.Add(JToken.FromObject(
            new Categoria
            {
                IdCategoria = categoria[i].IdCategoria,
                NbCategoria = categoria[i].NbCategoria,
                DeCategoria = categoria[i].DeCategoria,
                TpCategoria = categoria[i].TpCategoria,
                FhRegistro = categoria[i].FhRegistro,
                UsRegistro = categoria[i].UsRegistro,
                NbCategoriaHeader = categoria[i].NbCategoriaHeader,
            }));
                }
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

        /// <summary>
        /// Devuelve la lista de categorias que pertenecen a una noticia especifica.
        /// </summary>
        /// <param name="idNoticiaHeader"></param> 
        [HttpGet("{id}")]
        public dynamic GetCategoriaByNoticia(string idNoticiaHeader)
        {
            dynamic jsonResponse = new JObject();
            try
            {
                var categoria = _context.Categoria.FromSqlRaw($"SP_GETCATEGORIA_BY_NBNOTICIA '{idNoticiaHeader}'").ToList();
                
                JArray Jarray = new JArray();
                jsonResponse.code = 200;
                jsonResponse.msj = "";

                if (categoria.Count == 0)
                {
                    jsonResponse.code = 400;
                    jsonResponse.data = null;
                    jsonResponse.msj = "No se encontraron categorias";
                    return jsonResponse;
                }

                for (int i = 0; i < categoria.Count; i++)
                {
                    Jarray.Add(JToken.FromObject(
            new Categoria
            {
                IdCategoria = categoria[i].IdCategoria,
                NbCategoria = categoria[i].NbCategoria,
                DeCategoria = categoria[i].DeCategoria,
                TpCategoria = categoria[i].TpCategoria,
                FhRegistro = categoria[i].FhRegistro,
                UsRegistro = categoria[i].UsRegistro,
                NbCategoriaHeader = categoria[i].NbCategoriaHeader,
            }));
                }
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


        private bool CategoriaExists(int id)
        {
            return _context.Categoria.Any(e => e.IdCategoria == id);
        }
    }
}
