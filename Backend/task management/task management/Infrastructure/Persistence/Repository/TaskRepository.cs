using Microsoft.EntityFrameworkCore;
using Npgsql;
using task_management.Application.Dtos.Request;
using task_management.Application.Dtos.Response;
using task_management.Domain.Entities;
using task_management.Domain.Interfaces;
using task_management.Infrastructure.Persistence.DBContext;

namespace task_management.Infrastructure.Persistence.Repository
{
    public class TaskRepository : ITaskRepository
    {

        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }


        public async Task<int> CreateAsync(TasksEntity task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task.id ?? 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<TasksEntity>()
                .FirstOrDefaultAsync(t => t.id == id && t.state == "A");

            if (entity == null)
                return false;

            entity.state = "I";
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<TasksEntity>> GetAllAsync()
        {
            return await _context.Set<TasksEntity>()
                        .Where(t => t.state == "A")
                        .ToListAsync();
        }

        public async Task<TasksEntity?> GetByIdAsync(int id)
        {
            return await _context.Set<TasksEntity>()
                        .FirstOrDefaultAsync(t => t.state == "A" && t.id == id);
        }

        public async Task<bool> UpdateAsync(TasksEntity task)
        {
            task.update_at = DateTime.Now;
            _context.Tasks.Update(task);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}
