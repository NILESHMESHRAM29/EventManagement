using EventManagement.Data;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SectionController(AppDbContext context)
        {
            _context = context;
        }

        // ================================================
        // GET: api/section
        // ================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sections = await _context.Sections
                .Include(s => s.AddedByUser)
                .Include(s => s.SectionVolunteers)
                .Where(s => !s.IsDelete)
                .ToListAsync();

            return Ok(sections);
        }

        // ================================================
        // GET: api/section/{id}
        // ================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var section = await _context.Sections
                .Include(s => s.AddedByUser)
                .Include(s => s.SectionVolunteers)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDelete);

            if (section == null)
                return NotFound(new { message = "Section not found" });

            return Ok(section);
        }

        // ================================================
        // POST: api/section
        // ================================================
        [HttpPost]
        public async Task<IActionResult> Create(Section model)
        {
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.Sections.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Section created successfully",
                data = model
            });
        }

        // ================================================
        // PUT: api/section/{id}
        // ================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, Section model)
        {
            var section = await _context.Sections.FindAsync(id);

            if (section == null || section.IsDelete)
                return NotFound(new { message = "Section not found" });

            section.Name = model.Name;
            section.AddedBy = model.AddedBy;
            section.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Section updated successfully",
                data = section
            });
        }

        // ================================================
        // DELETE (SOFT DELETE): api/section/{id}
        // ================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var section = await _context.Sections.FindAsync(id);

            if (section == null || section.IsDelete)
                return NotFound(new { message = "Section not found" });

            section.IsDelete = true;
            section.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Section deleted (soft delete) successfully" });
        }
    }
}
