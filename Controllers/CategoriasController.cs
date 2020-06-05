using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<Categoria>> GetCategoria(string tipo)
        {
            if (tipo.ToLower()=="articulo")
            {
                return  _context.Categoria.ToList().Where(n => n.TpCategoria == "ARTICULO");
            }
            else if (tipo.ToLower() == "noticia")
            {
                return _context.Categoria.ToList().Where(n => n.TpCategoria == "NOTICIA");
            }else if (tipo.ToLower() == "all")
            {
                return await _context.Categoria.ToListAsync();
            }
            return null;
        }

        // GET: api/Categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
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
