namespace task_management.Domain.Exceptions
{
    public class BadRequestException : ApiException
    {
        public BadRequestException(string message, string errorCode = "SOLICITUD_INVALIDA")
           : base(message, errorCode, 400)
        {
        }
    }
}
