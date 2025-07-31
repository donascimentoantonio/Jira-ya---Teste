using FluentValidation;
using Jira_ya.Application.DTOs;

namespace Jira_ya.Application.Validators.Task
{
    public class TaskDtoValidator : AbstractValidator<TaskDto>
    {
        public TaskDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Título é obrigatório.")
                .Length(3, 100).WithMessage("Título deve ter entre 3 e 100 caracteres.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres.");

            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("DueDate não pode ser no passado.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status inválido.");
        }
    }
}
