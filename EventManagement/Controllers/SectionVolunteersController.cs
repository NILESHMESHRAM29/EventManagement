using EventManagement.Data;
using EventManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionVolunteersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SectionVolunteersController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: api/SectionVolunteers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SectionVolunteer>>> GetSectionVolunteers()
        {
            return await _context.SectionVolunteers.ToListAsync();
        }
        [Authorize]
        // GET: api/SectionVolunteers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SectionVolunteer>> GetSectionVolunteer(int id)
        {
            var sectionVolunteer = await _context.SectionVolunteers.FindAsync(id);

            if (sectionVolunteer == null)
            {
                return NotFound();
            }

            return sectionVolunteer;
        }
        [Authorize]
        // PUT: api/SectionVolunteers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSectionVolunteer(int id, SectionVolunteer sectionVolunteer)
        {
            if (id != sectionVolunteer.Id)
            {
                return BadRequest();
            }

            _context.Entry(sectionVolunteer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SectionVolunteerExists(id))
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
        [Authorize]
        // POST: api/SectionVolunteers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SectionVolunteer>> PostSectionVolunteer(SectionVolunteer sectionVolunteer)
        {
            _context.SectionVolunteers.Add(sectionVolunteer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSectionVolunteer", new { id = sectionVolunteer.Id }, sectionVolunteer);
        }
        [Authorize]
        // DELETE: api/SectionVolunteers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSectionVolunteer(int id)
        {
            var sectionVolunteer = await _context.SectionVolunteers.FindAsync(id);
            if (sectionVolunteer == null)
            {
                return NotFound();
            }

            _context.SectionVolunteers.Remove(sectionVolunteer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SectionVolunteerExists(int id)
        {
            return _context.SectionVolunteers.Any(e => e.Id == id);
        }
    }
}
