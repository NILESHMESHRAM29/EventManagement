using EventManagement.Data;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectionVolunteerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SectionVolunteerController(AppDbContext context)
        {
            _context = context;
        }

        // ===========================================================

        // GET: api/sectionvolunteer
        // ===========================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.SectionVolunteers
                .Include(sv => sv.Section)
                .Include(sv => sv.User)
                .Where(sv => sv.IsActive)
                .ToListAsync();

            return Ok(list);
        }

        // ===========================================================
        // GET: api/sectionvolunteer/{id}
        // ===========================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var mapping = await _context.SectionVolunteers
                .Include(sv => sv.Section)
                .Include(sv => sv.User)
                .FirstOrDefaultAsync(sv => sv.Id == id);

            if (mapping == null)
                return NotFound(new { message = "Section-Volunteer mapping not found" });

            return Ok(mapping);
        }

        // ===========================================================
        // POST: api/sectionvolunteer
        // ASSIGN USER TO SECTION
        // ===========================================================
        [HttpPost]
        public async Task<IActionResult> Create(SectionVolunteer model)
        {
            // Optional validation (recommended)
            var sectionExists = await _context.Sections.AnyAsync(s => s.Id == model.SectionId && !s.IsDelete);
            var userExists = await _context.Users.AnyAsync(u => u.Id == model.UserId && !u.IsDelete);

            if (!sectionExists)
                return BadRequest(new { message = "Section does not exist" });

            if (!userExists)
                return BadRequest(new { message = "User does not exist" });

            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.SectionVolunteers.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Volunteer assigned to section successfully",
                data = model
            });
        }

        // ===========================================================
        // PUT: api/sectionvolunteer/{id}
        // UPDATE MAPPING (e.g. move volunteer to different section)
        // ===========================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, SectionVolunteer model)
        {
            var mapping = await _context.SectionVolunteers.FindAsync(id);

            if (mapping == null)
                return NotFound(new { message = "Section-Volunteer mapping not found" });

            mapping.SectionId = model.SectionId;
            mapping.UserId = model.UserId;
            mapping.IsActive = model.IsActive;
            mapping.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Mapping updated successfully",
                data = mapping
            });
        }

        // ===========================================================
        // DELETE (SOFT): api/sectionvolunteer/{id}
        // DEACTIVATE VOLUNTEER ASSIGNMENT
        // ===========================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var mapping = await _context.SectionVolunteers.FindAsync(id);

            if (mapping == null)
                return NotFound(new { message = "Section-Volunteer mapping not found" });

            mapping.IsActive = false;
            mapping.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Volunteer removed from section (soft delete) successfully" });
        }
    }
}
