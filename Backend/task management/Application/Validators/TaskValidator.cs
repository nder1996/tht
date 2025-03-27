using FluentValidation;
using task_management.Application.Dtos.Request;

namespace task_management.Application.Validators
{
    public class TaskValidator : AbstractValidator<TaskRequest>
    {
        public TaskValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("El título es obligatorio")
                .MaximumLength(100).WithMessage("El título no puede exceder los 100 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("El estado de la tarea es obligatorio");

            RuleFor(x => x.state)
                .NotEmpty().WithMessage("El estado es obligatorio")
                .Length(1).WithMessage("El estado debe ser un solo carácter");

            RuleFor(x => x.due_date)
                .NotEmpty().WithMessage("La fecha de vencimiento es obligatoria")
                .Must(date => date > DateTime.Now)
                .WithMessage("La fecha de vencimiento debe ser posterior a la fecha actual");
        }
    }
} 