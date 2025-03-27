namespace task_management.Application.Dtos.Response
{
    public record ApiResponse<T>(Meta? Meta = null, T? Data = default, ErrorDetails? Error = null);

    public record Meta(string? Message = null, int? StatusCode = null);

    public record ErrorDetails(string? Code = null, string? Description = null);
}
