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
    [Route("api/[controller]")]
    [ApiController]
    public class ImportBatchesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ImportBatchesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ImportBatches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportBatch>>> GetImportBatches()
        {
            return await _context.ImportBatches.ToListAsync();
        }

        // GET: api/ImportBatches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportBatch>> GetImportBatch(int id)
        {
            var importBatch = await _context.ImportBatches.FindAsync(id);

            if (importBatch == null)
            {
                return NotFound();
            }

            return importBatch;
        }

        // PUT: api/ImportBatches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImportBatch(int id, ImportBatch importBatch)
        {
            if (id != importBatch.Id)
            {
                return BadRequest();
            }

            _context.Entry(importBatch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImportBatchExists(id))
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

        // POST: api/ImportBatches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImportBatch>> PostImportBatch(ImportBatch importBatch)
        {
            _context.ImportBatches.Add(importBatch);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImportBatch", new { id = importBatch.Id }, importBatch);
        }

        // DELETE: api/ImportBatches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImportBatch(int id)
        {
            var importBatch = await _context.ImportBatches.FindAsync(id);
            if (importBatch == null)
            {
                return NotFound();
            }

            _context.ImportBatches.Remove(importBatch);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImportBatchExists(int id)
        {
            return _context.ImportBatches.Any(e => e.Id == id);
        }
    }
}
