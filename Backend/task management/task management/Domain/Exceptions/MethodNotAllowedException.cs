namespace task_management.Domain.Exceptions
{
    public class MethodNotAllowedException : ApiException
    {
        public MethodNotAllowedException(string message = "Método no permitido para este recurso")
           : base(message, "METHOD_NOT_ALLOWED", 405)
        {
        }
    }
}
