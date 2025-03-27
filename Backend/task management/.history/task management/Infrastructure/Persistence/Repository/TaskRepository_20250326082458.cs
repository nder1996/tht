using Npgsql;
using task_management.Application.Dtos.Request;
using task_management.Application.Dtos.Response;
using task_management.Domain.Interfaces;

namespace task_management.Infrastructure.Persistence.Repository
{
    public class TaskRepository : ITaskRepository
    {
        public Task<int> CreateAsync(TaskRequest task)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TaskResponse>> GetAllAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Usar CreateCommand y ExecuteReaderAsync de Npgsql
                using (var cmd = new NpgsqlCommand(
                    "SELECT id, title, description, due_date, status, create_at as CreatedAt, update_at as UpdatedAt, state FROM public.tasks",
                    connection))
                {
                    var tasks = new List<TaskResponse>();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tasks.Add(new TaskResponse
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                title = reader.GetString(reader.GetOrdinal("title")),
                                description = reader.GetString(reader.GetOrdinal("description")),
                                due_date = reader.IsDBNull(reader.GetOrdinal("due_date")) ? null : reader.GetDateTime(reader.GetOrdinal("due_date")),
                                status = reader.GetString(reader.GetOrdinal("status")),
                                create_at = reader.GetDateTime(reader.GetOrdinal("create_at")),
                                update_at = reader.GetDateTime(reader.GetOrdinal("update_at")),
                                state = reader.GetString(reader.GetOrdinal("state"))
                            });
                        }
                    }

                    return tasks;
                }
            }
        }

        public Task<TaskResponse> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(TaskRequest task)
        {
            throw new NotImplementedException();
        }
    }
}
