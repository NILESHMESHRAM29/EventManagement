using EventManagement.Data;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventController(AppDbContext context)
        {
            _context = context;
        }

        // =======================================
        // GET: api/event
        // =======================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _context.Events
                .Include(e => e.AddedByUser)
                .Include(e => e.UpdatedByUser)
                .Where(e => !e.IsDelete)
                .ToListAsync();

            return Ok(events);
        }

        // =======================================
        // GET: api/event/{id}
        // =======================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var ev = await _context.Events
                .Include(e => e.AddedByUser)
                .Include(e => e.UpdatedByUser)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDelete);

            if (ev == null)
                return NotFound(new { message = "Event not found" });

            return Ok(ev);
        }

        // =======================================
        // POST: api/event
        // =======================================
        [HttpPost]
        public async Task<IActionResult> Create(Event model)
        {
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.Events.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Event created successfully",
                data = model
            });
        }

        // =======================================
        // PUT: api/event/{id}
        // =======================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, Event model)
        {
            var ev = await _context.Events.FindAsync(id);

            if (ev == null || ev.IsDelete)
                return NotFound(new { message = "Event not found" });

            ev.Title = model.Title;
            ev.Description = model.Description;
            ev.EventDate = model.EventDate;
            ev.UpdatedBy = model.UpdatedBy;
            ev.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Event updated successfully",
                data = ev
            });
        }

        // =======================================
        // DELETE (Soft): api/event/{id}
        // =======================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var ev = await _context.Events.FindAsync(id);

            if (ev == null || ev.IsDelete)
                return NotFound(new { message = "Event not found" });

            ev.IsDelete = true;
            ev.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Event deleted (soft delete) successfully" });
        }
    }
}
