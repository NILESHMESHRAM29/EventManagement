using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManagement.Data;
using EventManagement.Models;

namespace EventManagement.Controllers
{
    [Route("api/scans")]
    [ApiController]
    public class ScansController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ScansController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Scans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Scan>>> GetScans()
        {
            return await _context.Scans.ToListAsync();
        }

        // GET: api/Scans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Scan>> GetScan(int id)
        {
            var scan = await _context.Scans.FindAsync(id);

            if (scan == null)
            {
                return NotFound();
            }

            return scan;
        }

        // PUT: api/Scans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScan(int id, Scan scan)
        {
            if (id != scan.Id)
            {
                return BadRequest();
            }

            _context.Entry(scan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScanExists(id))
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

        // POST: api/Scans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Scan>> PostScan(Scan scan)
        {
            _context.Scans.Add(scan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScan", new { id = scan.Id }, scan);
        }

        // DELETE: api/Scans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScan(int id)
        {
            var scan = await _context.Scans.FindAsync(id);
            if (scan == null)
            {
                return NotFound();
            }

            _context.Scans.Remove(scan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ScanExists(int id)
        {
            return _context.Scans.Any(e => e.Id == id);
        }
    }
}
