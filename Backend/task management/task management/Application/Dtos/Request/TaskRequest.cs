using System.ComponentModel.DataAnnotations;
using task_management.Application.Dtos.Response;

namespace task_management.Application.Dtos.Request
{
    public class TaskRequest
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(100, ErrorMessage = "El título no puede exceder los 100 caracteres")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El estado de la tarea es obligatorio")]
        [RegularExpression("^(PENDIENTE|EN PROGRESO|COMPLETADO)$",
                   ErrorMessage = "El estado solo puede ser 'PENDIENTE', 'EN PROGRESO' o 'COMPLETADO'")]
        public string Status { get; set; }

        [StringLength(1, MinimumLength = 1, ErrorMessage = "El estado debe ser exactamente 1 carácter")]
        [RegularExpression("^[AI]$", ErrorMessage = "El estado solo puede ser 'A' o 'I'")]
        public string state { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "La fecha de vencimiento debe ser posterior a la fecha actual")]
        public DateTime due_date { get; set; }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null)
                return true;

            return (DateTime)value > DateTime.Now;
        }
    }
}
