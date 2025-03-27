using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _task_repository;

        public TaskService(ITaskRepository task_repository)
        {
            _task_repository = task_repository;
        }

        public async Task<int> CreateTask(TaskDTO task)
        {
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

            int id = await this._task_repository.CreateAsync(taskEntity);

            return id;
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

        public async Task<int> UpdateTask(TaskDTO task)
        {
            var existingTask = await this._task_repository.GetAsync(task.id);

            if (existingTask == null)
            {
                throw new Exception("Task not found");
            }

            var taskEntity = new TasksEntity
            {
                id = task.id,
                title = task.Title,
                description = task.Description,
                due_date = DateTime.SpecifyKind(task.due_date, DateTimeKind.Utc),
                status = task.Status,
                state = task.state,
                update_at = DateTime.UtcNow,
                create_at = existingTask.create_at
            };

            return await this._task_repository.UpdateAsync(taskEntity);
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