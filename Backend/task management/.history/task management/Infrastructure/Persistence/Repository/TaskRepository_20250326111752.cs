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


        public Task<int> CreateAsync(TaskRequest task)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TasksEntity>> GetAllAsync()
        {
            _context.ChangeTracker.Clear();
            return await _context.Set<TasksEntity>().ToListAsync();
        }

        public Task<TasksEntity> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(TaskRequest task)
        {
            throw new NotImplementedException();
        }
    }
}
