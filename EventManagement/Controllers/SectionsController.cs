using AutoMapper;
using EventManagement.Data;
using EventManagement.DTOs;
using EventManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EventManagement.Controllers
{
    [Route("api/sections")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SectionsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/sections
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SectionDto>>> GetSections()
        {
            var sections = await _context.Sections.ToListAsync();
            var sectionDtos = _mapper.Map<List<SectionDto>>(sections);

            return Ok(sectionDtos);
        }

        // GET: api/sections/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<SectionDto>> GetSection(int id)
        {
            var section = await _context.Sections.FindAsync(id);

            if (section == null)
            {
                return NotFound();
            }

            var sectionDto = _mapper.Map<SectionDto>(section);
            return Ok(sectionDto);
        }

        // PUT: api/sections/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSection(int id, SectionDto sectionDto)
        {
            var section = await _context.Sections.FindAsync(id);

            if (section == null)
            {
                return NotFound();
            }

            _mapper.Map(sectionDto, section);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/sections
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> PostSection(SectionDto sectionDto)
        {
            var section = _mapper.Map<Section>(sectionDto);

            _context.Sections.Add(section);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSection), new { id = section.Id }, sectionDto);
        }

        // DELETE: api/sections/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSection(int id)
        {
            var section = await _context.Sections.FindAsync(id);

            if (section == null)
            {
                return NotFound();
            }

            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
