using System.ComponentModel.DataAnnotations;
using task_management.Application.Dtos.Response;

namespace task_management.Application.Dtos.Request
{
    public class TaskRequest
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(100, ErrorMessage = "El título no puede exceder los 100 caracteres")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "El estado de la tarea es obligatorio")]
        [StringLength(100, ErrorMessage = "El estado no puede exceder los 100 caracteres")]
        public string Status { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(1, ErrorMessage = "El estado debe ser un solo carácter")]
        [RegularExpression("^[AI]$", ErrorMessage = "El estado solo puede ser 'A' (Activo) o 'I' (Inactivo)")]
        public string state { get; set; }

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