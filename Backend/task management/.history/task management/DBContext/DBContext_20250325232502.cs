namespace task_management.DBContext
{
    public class DBContext: DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; }
    }
}
