using Microsoft.EntityFrameworkCore;
using task_management.Model;

namespace task_management.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tasks> Tasks { get; set; }
    }
} 