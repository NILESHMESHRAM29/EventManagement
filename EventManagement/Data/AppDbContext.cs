using EventManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Data
{
    public class AppDbContext : DbContext

    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ImportBatch> ImportBatches { get; set; }
        public DbSet<IdCard> IdCards { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<PermissionRole> PermissionRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<SectionVolunteer> SectionVolunteers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========== USERS ==========
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.RoleId);
                entity.HasIndex(u => u.EmailVerifiedAt);

                entity.Property(u => u.IsSend).HasDefaultValue(false);
                entity.Property(u => u.IsIdCardDownloaded).HasDefaultValue(false);
                entity.Property(u => u.Dept).HasDefaultValue(0);
                entity.Property(u => u.IsDelete).HasDefaultValue(false);
                entity.Property(u => u.IsApproved).HasDefaultValue(false);

                // timestamps
                entity.Property(u => u.CreatedAt).HasColumnName("created_at");
                entity.Property(u => u.UpdatedAt).HasColumnName("updated_at");

                // foreign key
                entity.HasOne(u => u.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(u => u.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ========== IMPORT BATCHES ==========
            modelBuilder.Entity<ImportBatch>(entity =>
            {
                entity.ToTable("import_batches");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UploadedBy).HasColumnName("uploaded_by");
                entity.Property(e => e.TotalRows).HasColumnName("total_rows").HasDefaultValue(0);
                entity.Property(e => e.ProcessedRows).HasColumnName("processed_rows").HasDefaultValue(0);
                entity.Property(e => e.IsCompleted).HasColumnName("is_completed").HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                      .HasColumnName("updated_at")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            });

            // ========== ID CARDS ==========
            modelBuilder.Entity<IdCard>(entity =>
            {
                entity.ToTable("idcards");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.StudentId)
                    .HasColumnName("student_id");

                entity.Property(e => e.GeneratedBy)
                    .HasColumnName("generated_by");

                entity.Property(e => e.FilePath)
                    .HasColumnName("file_path");

                entity.Property(e => e.IsDelete)
                    .HasColumnName("is_delete")
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

                // Foreign Key: student_id → students.id
                entity.HasOne(e => e.Student)
                    .WithMany(s => s.Id_Cards)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ========== STUDENTS ==========
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permissions");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.HasIndex(e => e.Name)
                      .IsUnique();

                entity.Property(e => e.IsDelete)
                      .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            });

            // ========== PERMISSION ROLES ==========
            modelBuilder.Entity<PermissionRole>(entity =>
            {
                entity.ToTable("permission_role");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.IsDelete).HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

                entity.HasOne(e => e.Role)
                      .WithMany(r => r.PermissionRoles)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Permission)
                      .WithMany(p => p.PermissionRoles)
                      .HasForeignKey(e => e.PermissionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ========== ROLES ==========
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .HasColumnType("varchar(255)")
                      .IsRequired();

                entity.Property(e => e.IsDelete)
                      .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            });

            // ========== EVENTS ==========
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("events");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                      .HasColumnType("varchar(255)")
                      .IsRequired();

                entity.Property(e => e.Description)
                      .HasColumnType("text")
                      .IsRequired(false);

                entity.Property(e => e.EventDate)
                      .HasColumnType("date")
                      .IsRequired();

                entity.Property(e => e.IsDelete)
                      .HasDefaultValue(false);

                entity.Property(e => e.AddedBy)
                      .HasColumnType("bigint")
                      .IsRequired(false);

                entity.Property(e => e.UpdatedBy)
                      .HasColumnType("bigint")
                      .IsRequired(false);

                entity.Property(e => e.CreatedAt)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            });

            // ========== SECTIONS ==========
            modelBuilder.Entity<Section>(entity =>
            {
                entity.ToTable("sections");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .HasColumnType("varchar(255)")
                      .IsRequired();

                entity.Property(e => e.IsDelete)
                      .HasDefaultValue(false);

                entity.Property(e => e.AddedBy)
                      .HasColumnType("bigint")
                      .IsRequired(false);

                entity.Property(e => e.CreatedAt)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            });

            // ========== SECTION VOLUNTEERS ==========
            modelBuilder.Entity<SectionVolunteer>(entity =>
            {
                entity.ToTable("section_volunteer"); // Laravel table name

                entity.HasKey(e => e.Id);

                // Foreign key: section_id
                entity.HasOne(e => e.Section)
                      .WithMany(s => s.SectionVolunteers)
                      .HasForeignKey(e => e.SectionId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Foreign key: user_id
                entity.HasOne(e => e.User)
                      .WithMany(u => u.SectionVolunteers)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.IsActive)
                      .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                      .HasColumnName("updated_at")
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            });

        }
    }
}
