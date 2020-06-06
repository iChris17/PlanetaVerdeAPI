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
    public class NoticiaDetallesController : ControllerBase
    {
        private readonly PLANETAVERDEWEBContext _context = new PLANETAVERDEWEBContext();

        public NoticiaDetallesController()
        {
          
        }

        // GET: api/NoticiaDetalles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoticiaDetalle>>> GetNoticiaDetalle()
        {
            return await _context.NoticiaDetalle.ToListAsync();
        }

        // GET: api/NoticiaDetalles/5
        [HttpGet("{id}")]
        public ActionResult<NoticiaDetalle> GetNoticiaDetalle(string id)
        {
            var noticiaDetalle =  _context.NoticiaDetalle.Include(x=>x.IdNoticiaHeaderNavigation).ToList().Find(x=>x.IdNoticiaHeader==id);

            if (noticiaDetalle == null)
            {
                return NotFound();
            }

            return Ok(noticiaDetalle);
        }

        // PUT: api/NoticiaDetalles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNoticiaDetalle(string id, NoticiaDetalle noticiaDetalle)
        {
            if (id != noticiaDetalle.IdNoticiaHeader)
            {
                return BadRequest();
            }

            _context.Entry(noticiaDetalle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoticiaDetalleExists(id))
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

        // POST: api/NoticiaDetalles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<NoticiaDetalle>> PostNoticiaDetalle(NoticiaDetalle noticiaDetalle)
        {
            _context.NoticiaDetalle.Add(noticiaDetalle);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NoticiaDetalleExists(noticiaDetalle.IdNoticiaHeader))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetNoticiaDetalle", new { id = noticiaDetalle.IdNoticiaHeader }, noticiaDetalle);
        }

        // DELETE: api/NoticiaDetalles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NoticiaDetalle>> DeleteNoticiaDetalle(string id)
        {
            var noticiaDetalle = await _context.NoticiaDetalle.FindAsync(id);
            if (noticiaDetalle == null)
            {
                return NotFound();
            }

            _context.NoticiaDetalle.Remove(noticiaDetalle);
            await _context.SaveChangesAsync();

            return noticiaDetalle;
        }

        private bool NoticiaDetalleExists(string id)
        {
            return _context.NoticiaDetalle.Any(e => e.IdNoticiaHeader == id);
        }
    }
}
