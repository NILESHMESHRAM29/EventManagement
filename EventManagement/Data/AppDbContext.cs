using EventManagement.DTOs;
using EventManagement.Models;
using Microsoft.EntityFrameworkCore;


namespace EventManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Scan> Scans { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ImportBatch> ImportBatches { get; set; }
        public DbSet<IdCard> IdCards { get; set; }
        public DbSet<PermissionRole> PermissionRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<SectionVolunteer> SectionVolunteers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USERS
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.HasOne(u => u.Role)
                    .WithMany()
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PasswordResetToken>()
                .ToTable("PasswordResetTokens");
            
            modelBuilder.Entity<Session>()
                .ToTable("Sessions");

            modelBuilder.Entity<Session>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .IsRequired(false);

            modelBuilder.Entity<Session>()
                .HasIndex(s => s.LastActivity);

            // STUDENTS
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(e => e.UniqueId)
                    .IsUnique();

                entity.HasIndex(e => e.QrCodePath)
                    .IsUnique(false);

                entity.HasOne(s => s.User)
                    .WithMany(u => u.Students)
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.AddedByUser)
                    .WithMany()
                    .HasForeignKey(s => s.AddedBy)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(s => s.UpdateByUser)
                    .WithMany()
                    .HasForeignKey(s => s.UpdateBy)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.Property(e => e.IsDelete)
                    .HasDefaultValue(false);

                // Force plain datetime (no precision) for compatibility with older MySQL
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // SCANS
            modelBuilder.Entity<Scan>(entity =>
            {
                    entity.HasOne(s => s.Student)
                        .WithMany()
                        .HasForeignKey(s => s.StudentId)
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.HasOne(s => s.User)
                        .WithMany()
                        .HasForeignKey(s => s.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.HasOne(s => s.Section)
                        .WithMany()
                        .HasForeignKey(s => s.SectionId)
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.Property(e => e.IsDelete)
                        .HasDefaultValue(false);

                    entity.Property(e => e.CreatedAt)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // ROLES
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.IsDelete)
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });



            // IMPORT BATCHES
            modelBuilder.Entity<ImportBatch>(entity =>
            {
                entity.HasOne(b => b.UploadedByUser)
                    .WithMany()
                    .HasForeignKey(b => b.UploadedBy)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.TotalRows)
                    .HasDefaultValue(0);

                entity.Property(e => e.ProcessedRows)
                    .HasDefaultValue(0);

                entity.Property(e => e.IsCompleted)
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });



            // ID CARDS
            modelBuilder.Entity<IdCard>(entity =>
            {
                entity.HasOne(i => i.Student)
                    .WithMany()
                    .HasForeignKey(i => i.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.IsDelete)
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // PERMISSION ROLE
            modelBuilder.Entity<PermissionRole>(entity =>
            {
                entity.ToTable("permission_role");

                entity.HasOne(pr => pr.Role)
                    .WithMany()
                    .HasForeignKey(pr => pr.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pr => pr.Permission)
                    .WithMany()
                    .HasForeignKey(pr => pr.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.IsDelete)
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // PREMISSIONS
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.IsDelete)
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // EVENTS
            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.IsDelete)
                    .HasDefaultValue(false);

                entity.HasOne(e => e.AddedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.AddedBy)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.UpdatedBy)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });


            // SECTIONS
            modelBuilder.Entity<Section>(entity =>
            {
                entity.ToTable("sections");

                entity.Property(e => e.IsDelete)
                    .HasDefaultValue(false);

                entity.HasOne(s => s.AddedByUser)
                    .WithMany()
                    .HasForeignKey(s => s.AddedBy)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // SECTION VOLUNTEERS
            modelBuilder.Entity<SectionVolunteer>(entity =>
            {
                entity.ToTable("section_volunteer");

                entity.HasOne(sv => sv.Section)
                    .WithMany()
                    .HasForeignKey(sv => sv.SectionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sv => sv.User)
                    .WithMany()
                    .HasForeignKey(sv => sv.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}