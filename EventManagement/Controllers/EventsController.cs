using AutoMapper;
using EventManagement.Data;
using EventManagement.DTOs;
using EventManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Controllers
{
    [Authorize]
    [Route("api/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EventController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _context.Events
                .Where(e => !e.IsDelete)
                .ToListAsync();

            return Ok(_mapper.Map<List<EventResponseDto>>(events));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ev = await _context.Events.FindAsync(id);

            if (ev == null || ev.IsDelete)
                return NotFound(new { Message = "Event not found" });

            return Ok(_mapper.Map<EventResponseDto>(ev));
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventCreateDto dto)
        {
            var ev = _mapper.Map<Event>(dto);

            ev.CreatedAt = DateTime.UtcNow;
            ev.UpdatedAt = DateTime.UtcNow;

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<EventResponseDto>(ev));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EventUpdateDto dto)
        {
            var ev = await _context.Events.FindAsync(id);

            if (ev == null || ev.IsDelete)
                return NotFound(new { Message = "Event not found" });

            _mapper.Map(dto, ev);
            ev.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<EventResponseDto>(ev));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var ev = await _context.Events.FindAsync(id);

            if (ev == null || ev.IsDelete)
                return NotFound(new { Message = "Event not found" });

            ev.IsDelete = true;
            ev.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Event soft deleted" });
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var ev = await _context.Events.FindAsync(id);

            if (ev == null)
                return NotFound(new { Message = "Event not found" });

            ev.IsDelete = false;
            ev.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Event restored" });
        }
    }
}