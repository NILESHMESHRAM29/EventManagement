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
    [Route("api/permissionroles")]
    [ApiController]
    public class PermissionRolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PermissionRolesController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: api/PermissionRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionRole>>> GetPermissionRoles()
        {
            return await _context.PermissionRoles.ToListAsync();
        }
        [Authorize]
        // GET: api/PermissionRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionRole>> GetPermissionRole(int id)
        {
            var permissionRole = await _context.PermissionRoles.FindAsync(id);

            if (permissionRole == null)
            {
                return NotFound();
            }

            return permissionRole;
        }
        [Authorize]
        // PUT: api/PermissionRoles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermissionRole(int id, PermissionRole permissionRole)
        {
            if (id != permissionRole.Id)
            {
                return BadRequest();
            }

            _context.Entry(permissionRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermissionRoleExists(id))
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
        // POST: api/PermissionRoles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PermissionRole>> PostPermissionRole(PermissionRole permissionRole)
        {
            _context.PermissionRoles.Add(permissionRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPermissionRole", new { id = permissionRole.Id }, permissionRole);
        }
        [Authorize]
        // DELETE: api/PermissionRoles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermissionRole(int id)
        {
            var permissionRole = await _context.PermissionRoles.FindAsync(id);
            if (permissionRole == null)
            {
                return NotFound();
            }

            _context.PermissionRoles.Remove(permissionRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PermissionRoleExists(int id)
        {
            return _context.PermissionRoles.Any(e => e.Id == id);
        }
    }
}
