using EventManagement.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventManagement.Data
{
    public class AppDbContext : DbContext

    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ImportBatch> ImportBatches { get; set; }
        public DbSet<Id_Card> Id_Cards { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            modelBuilder.Entity<Id_Card>(entity =>
            {
                entity.ToTable("id_cards");

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
        }

        public DbSet<scan> scans{get; set;}
        public DbSet<permission_role> permission_Roles{ get; set; }
        public DbSet<permissions> permissions{ get; set; }
        
    }
}
