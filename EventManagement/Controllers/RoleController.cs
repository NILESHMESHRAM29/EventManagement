using EventManagement.Data;
using EventManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoleController(AppDbContext context)
        {
            _context = context;
        }

        // ================================
        // GET: api/role
        // ================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _context.Roles
                .Where(r => !r.IsDelete)
                .ToListAsync();

            return Ok(roles);
        }

        // ================================
        // GET: api/role/{id}
        // ================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDelete);

            if (role == null)
                return NotFound(new { message = "Role not found" });

            return Ok(role);
        }

        // ================================
        // POST: api/role
        // ================================
        [HttpPost]
        public async Task<IActionResult> Create(Role model)
        {
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.Roles.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Role created successfully", data = model });
        }

        // ================================
        // PUT: api/role/{id}
        // ================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, Role model)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null || role.IsDelete)
                return NotFound(new { message = "Role not found" });

            role.Name = model.Name;
            role.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Role updated successfully", data = role });
        }

        // ================================
        // DELETE (SOFT DELETE): api/role/{id}
        // ================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null || role.IsDelete)
                return NotFound(new { message = "Role not found" });

            role.IsDelete = true;
            role.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Role deleted (soft) successfully" });
        }
    }
}
