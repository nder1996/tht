using task_management.Application.Dtos.Request;
using task_management.Application.Dtos.Response;

namespace task_management.Domain.Interfaces
{
    public interface ITaskService
    {
        Task<ApiResponse<IEnumerable<TaskResponse>>> GetTasks();
        Task<ApiResponse<TaskResponse>> GetTaskById(int id);
        Task<ApiResponse<string>> CreateTask(TaskRequest task);
        Task<ApiResponse<string>> UpdateTask(int id , TaskRequest task);
        Task<ApiResponse<string>> DeleteTask(int id);
    }
}
