using System.Security;

namespace EventManagement.Models
{
    public class PermissionRole
    {
        public int Id { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
