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

        // GET: api/Categorias
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

        // GET: api/Categorias/5
        [HttpGet("{id}")]
        public dynamic GetCategoria(string id,int n)
        {
            dynamic jsonResponse = new JObject();
            try
            {
                var categoria = _context.Categoria.FromSqlRaw($"SP_GETCATEGORIA_BY_NBNOTICIA '{id}'").ToList();
                
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

        // PUT: api/Categorias/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, Categoria categoria)
        {
            if (id != categoria.IdCategoria)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Categorias
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
        {
            _context.Categoria.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoria", new { id = categoria.IdCategoria }, categoria);
        }

        // DELETE: api/Categorias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Categoria>> DeleteCategoria(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categoria.Remove(categoria);
            await _context.SaveChangesAsync();

            return categoria;
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categoria.Any(e => e.IdCategoria == id);
        }
    }
}
