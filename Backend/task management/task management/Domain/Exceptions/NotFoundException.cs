namespace task_management.Domain.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message, string errorCode = "RECURSO_NO_ENCONTRADO")
            : base(message, errorCode, 404)
        {
        }
    }
}
