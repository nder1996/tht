using Microsoft.VisualBasic;
using Serilog;
using System.Threading.Tasks;
using task_management.Application.Dtos.Request;
using task_management.Application.Dtos.Response;
using task_management.Domain.Entities;
using task_management.Domain.Interfaces;
using task_management.Infrastructure.Persistence.DBContext;
using task_management.Infrastructure.Persistence.Repository;

namespace task_management.Application.Service
{
    public class TaskService : ITaskService
    {

        private readonly ITaskRepository _task_repository;

        public TaskService(ITaskRepository _task_repository)
        {
            this._task_repository = _task_repository;
        }

        public async Task<ApiResponse<string>> CreateTask(TaskRequest task)
        {
            try
            {
                if (task == null)
                    return ResponseApiBuilderService.ErrorResponse<string>(400, "DATOS_REQUERIDOS", "Los datos de la tarea son requeridos");

                var taskEntity = new TasksEntity
                {
                    title = task.Title,
                    description = task.Description,
                    due_date = task.due_date = DateTime.SpecifyKind(task.due_date, DateTimeKind.Utc),
                    status = task.Status,
                    state = task.state,
                    create_at = DateTime.UtcNow
                };

                int id = await this._task_repository.CreateAsync(taskEntity);

                return ResponseApiBuilderService.SuccessResponse<string>(
                    "Tarea Creada con exito",
                    "SUCCESS"
                );
            }
            catch (Exception ex)
            {
                return ResponseApiBuilderService.ErrorResponse<string>(
                    500,
                    "ERROR_INTERNO",
                    $"Ocurrió un error: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponse<string>> DeleteTask(int id)
        {
            try
            {
                bool result = await this._task_repository.DeleteAsync(id);

                if (!result)
                {
                    return ResponseApiBuilderService.ErrorResponse<string>(
                        404,
                        "TASK_NOT_FOUND",
                        $"No se encontró la tarea con id {id}"
                    );
                }

                return ResponseApiBuilderService.SuccessResponse<string>(
                    "Tarea eliminada correctamente",
                    "SUCCESS"
                );
            }
            catch (Exception ex)
            {
                return ResponseApiBuilderService.ErrorResponse<string>(
                    500,
                    "ERROR_INTERNO",
                    $"Ocurrió un error: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponse<TaskResponse>> GetTaskById(int id)
        {
            try
            {
                var taskEntity = await this._task_repository.GetByIdAsync(id);

                if (taskEntity == null)
                {
                    return ResponseApiBuilderService.ErrorResponse<TaskResponse>(
                        404,
                        "TASK_NOT_FOUND",
                        $"No se encontró la tarea con id {id}"
                    );
                }

                var taskResponse = new TaskResponse
                {
                    Id = (int)taskEntity.id,
                    Title = taskEntity.title,
                    Description = taskEntity.description,
                    Status = taskEntity.status,
                    State = taskEntity.state,
                    CreatedAt = taskEntity.create_at ?? DateTime.MinValue, // Default fallback value
                    UpdatedAt = taskEntity.update_at ?? DateTime.Now // Current time as fallback
                };

                return ResponseApiBuilderService.SuccessResponse<TaskResponse>(
                    taskResponse,
                    "SUCCESS"
                );
            }
            catch (Exception ex)
            {
                return ResponseApiBuilderService.ErrorResponse<TaskResponse>(
                    500,
                    "ERROR_INTERNO",
                    $"Ocurrió un error: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponse<IEnumerable<TaskResponse>>> GetTasks()
        {
            try
            {
                Log.Information("Iniciando aplicación");
                var tasksEntities = await this._task_repository.GetAllAsync();

                // Convertir las entidades a respuestas, incluso si la lista está vacía
                var tasksResponse = tasksEntities.Select(task => new TaskResponse
                {
                    Id = (int) task.id,
                    Title = task.title,
                    Description = task.description,
                    Status = task.status,
                    State = task.state,
                    CreatedAt = task.create_at ?? DateTime.MinValue, // Default fallback value
                    UpdatedAt = task.update_at ?? DateTime.Now // Current time as fallback
                });

                // Devolver la respuesta exitosa con los datos (o una lista vacía)
                return ResponseApiBuilderService.SuccessResponse<IEnumerable<TaskResponse>>(
                    tasksResponse,
                    tasksEntities.Any() ? "SUCCESS" : "NO_TASKS_FOUND"
                );
            }
            catch (Exception ex)
            {
                return ResponseApiBuilderService.ErrorResponse<IEnumerable<TaskResponse>>(
                    500,
                    "ERROR_INTERNO",
                    $"Ocurrió un error: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponse<string>> UpdateTask(int id, TaskRequest task)
        {
            try
            {
                if (task == null)
                    return ResponseApiBuilderService.ErrorResponse<string>(400, "DATOS_REQUERIDOS", "Los datos de la tarea son requeridos");

                var existingTask = await _task_repository.GetByIdAsync(id);
                if (existingTask == null)
                    return ResponseApiBuilderService.ErrorResponse<string>(404, "TAREA_NO_ENCONTRADA", "La tarea no existe");

                var taskEntity = new TasksEntity
                {
                    id = id,
                    title = task.Title,
                    description = task.Description,
                    due_date = DateTime.SpecifyKind(task.due_date, DateTimeKind.Utc),
                    status = task.Status,
                    state = task.state,
                    update_at = DateTime.UtcNow
                };

                return await _task_repository.UpdateAsync(taskEntity)
                    ? ResponseApiBuilderService.SuccessResponse<string>("Tarea actualizada con éxito", "SUCCESS")
                    : ResponseApiBuilderService.ErrorResponse<string>(500, "ERROR_ACTUALIZACION", "No se pudo actualizar la tarea");
            }
            catch (Exception ex)
            {
                return ResponseApiBuilderService.ErrorResponse<string>(
                    500,
                    "ERROR_INTERNO",
                    $"Ocurrió un error: {ex.Message}"
                );
            }
        }
    }
}
