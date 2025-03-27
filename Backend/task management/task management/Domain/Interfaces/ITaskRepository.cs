using task_management.Application.Dtos.Request;
using task_management.Application.Dtos.Response;
using task_management.Domain.Entities;

namespace task_management.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TasksEntity>> GetAllAsync();
        Task<TasksEntity> GetByIdAsync(int id);
        Task<int> CreateAsync(TasksEntity task);
        Task<bool> UpdateAsync(TasksEntity task);
        Task<bool> DeleteAsync(int id);
    }
}
