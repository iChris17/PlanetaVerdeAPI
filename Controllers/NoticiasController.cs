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
    public class NoticiasController : ControllerBase
    {
        private readonly PLANETAVERDEWEBContext _context=new PLANETAVERDEWEBContext();

        public NoticiasController()
        {
        }

        // GET: api/Noticias
        [HttpGet]
        public ActionResult<IEnumerable<Noticia>> GetNoticia()
        {
            return Ok(_context.Noticia.ToList().OrderByDescending(n=>n.FhRegistro));
        }

        // GET: api/Noticias/5
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Noticia>> GetNoticia(string id,int seccion=0)
        {
            var categoria = _context.Categoria.ToList().Find(x => x.NbCategoriaHeader == id);

            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(_context.Noticia.FromSqlRaw($"SP_GETNOTICIA_BY_CATEGORIA '{id}','{seccion}'").ToList());
        }

        // PUT: api/Noticias/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNoticia(string id, Noticia noticia)
        {
            if (id != noticia.IdNoticiaHeader)
            {
                return BadRequest();
            }

            _context.Entry(noticia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoticiaExists(id))
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

        // POST: api/Noticias
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Noticia>> PostNoticia(Noticia noticia)
        {
            _context.Noticia.Add(noticia);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NoticiaExists(noticia.IdNoticiaHeader))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetNoticia", new { id = noticia.IdNoticiaHeader }, noticia);
        }

        // DELETE: api/Noticias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Noticia>> DeleteNoticia(string id)
        {
            var noticia = await _context.Noticia.FindAsync(id);
            if (noticia == null)
            {
                return NotFound();
            }

            _context.Noticia.Remove(noticia);
            await _context.SaveChangesAsync();

            return noticia;
        }

        private bool NoticiaExists(string id)
        {
            return _context.Noticia.Any(e => e.IdNoticiaHeader == id);
        }
    }
}
