using EventManagement.Data;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route ("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        private RoleController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetRole()
        {
            var roles = await _context.Roles.Where(r => !r.IsDelete).ToListAsync();
            return Ok(roles);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] Role role)
        {
            if (role == null || string.IsNullOrWhiteSpace(role.Name))
            {
                return BadRequest("Role name is required.");
            }
            role.CreatedAt = DateTime.UtcNow;
            role.UpdatedAt = DateTime.UtcNow;

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);

        }
}
