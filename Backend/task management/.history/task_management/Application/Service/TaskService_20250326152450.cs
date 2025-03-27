using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Application.Services.Response;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _task_repository;

        public TaskService(ITaskRepository task_repository)
        {
            _task_repository = task_repository;
        }

        public async Task<ApiResponse<string>> CreateTask(TaskRequest task)
        {
            try
            {
                if (task == null)
                    return ResponseApiBuilderService.ErrorResponse<string>(400, "DATOS_REQUERIDOS", "Los datos de la tarea son requeridos");

                if (string.IsNullOrEmpty(task.Title))
                    return ResponseApiBuilderService.ErrorResponse<string>(400, "CAMPO_REQUERIDO", "El título es obligatorio");
                
                if (string.IsNullOrEmpty(task.Status))
                    return ResponseApiBuilderService.ErrorResponse<string>(400, "CAMPO_REQUERIDO", "El estado de la tarea es obligatorio");
                
                if (string.IsNullOrEmpty(task.state))
                    return ResponseApiBuilderService.ErrorResponse<string>(400, "CAMPO_REQUERIDO", "El estado es obligatorio");
                
                if (task.state != "A" && task.state != "I")
                    return ResponseApiBuilderService.ErrorResponse<string>(400, "VALOR_INVALIDO", "El estado solo puede ser 'A' (Activo) o 'I' (Inactivo)");
                
                if (task.due_date <= DateTime.Now)
                    return ResponseApiBuilderService.ErrorResponse<string>(400, "FECHA_INVALIDA", "La fecha de vencimiento debe ser posterior a la fecha actual");

                var taskEntity = new TasksEntity
                {
                    title = task.Title,
                    description = task.Description,
                    due_date = DateTime.SpecifyKind(task.due_date, DateTimeKind.Utc),
                    status = task.Status,
                    state = task.state,
                    create_at = DateTime.UtcNow,
                    update_at = null
                };

                try
                {
                    int id = await this._task_repository.CreateAsync(taskEntity);

                    return ResponseApiBuilderService.SuccessResponse<string>(
                        "Tarea Creada con éxito",
                        "SUCCESS"
                    );
                }
                catch (DbUpdateException dbEx)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException)
                {
                    throw;
                }

                return ResponseApiBuilderService.ErrorResponse<string>(
                    500,
                    "ERROR_INTERNO",
                    $"Ocurrió un error: {ex.Message}"
                );
            }
        }

        public async Task<TaskDTO> GetTask(int id)
        {
            var taskEntity = await this._task_repository.GetAsync(id);

            if (taskEntity == null)
            {
                throw new Exception("Task not found");
            }

            return new TaskDTO
            {
                id = taskEntity.id,
                Title = taskEntity.title,
                Description = taskEntity.description,
                due_date = taskEntity.due_date,
                Status = taskEntity.status,
                state = taskEntity.state
            };
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
                    create_at = existingTask.create_at,
                    update_at = DateTime.UtcNow
                };

                var success = await _task_repository.UpdateAsync(taskEntity);
                return success
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

        public async Task<bool> DeleteTask(int id)
        {
            return await this._task_repository.DeleteAsync(id);
        }

        public async Task<List<TaskDTO>> GetAllTasks()
        {
            var taskEntities = await this._task_repository.GetAllAsync();

            var tasks = new List<TaskDTO>();

            foreach (var taskEntity in taskEntities)
            {
                tasks.Add(new TaskDTO
                {
                    id = taskEntity.id,
                    Title = taskEntity.title,
                    Description = taskEntity.description,
                    due_date = taskEntity.due_date,
                    Status = taskEntity.status,
                    state = taskEntity.state
                });
            }

            return tasks;
        }
    }
} 