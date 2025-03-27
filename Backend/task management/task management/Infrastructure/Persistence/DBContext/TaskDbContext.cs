using Microsoft.EntityFrameworkCore;
using task_management.Domain.Entities;


namespace task_management.Infrastructure.Persistence.DBContext
{
    public class TaskDbContext : DbContext
    {


        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
        }

        public DbSet<TasksEntity> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de tablas, relaciones, índices, etc.
            modelBuilder.Entity<TasksEntity>(entity =>
            {
                entity.ToTable("tasks", "public");

                entity.HasKey(e => e.id);

                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.state).HasColumnName("state");
                entity.Property(e => e.title).HasColumnName("title").IsRequired();
                entity.Property(e => e.description).HasColumnName("description");
                entity.Property(e => e.due_date).HasColumnName("due_date");
                entity.Property(e => e.status).HasColumnName("status").IsRequired();
                entity.Property(e => e.create_at).HasColumnName("create_at");
                entity.Property(e => e.update_at).HasColumnName("update_at");
                

                // Define otras relaciones si existen
                // entity.HasOne(e => e.User).WithMany().HasForeignKey("user_id");
            });

            // Agrega configuraciones para otras entidades
        }

    }
}