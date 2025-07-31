using FluentValidation;
using Jira_ya.Application.DTOs;

namespace Jira_ya.Application.Validators.User
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome de usuário é obrigatório.")
                .Length(3, 50).WithMessage("Nome de usuário deve ter entre 3 e 50 caracteres.")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("Nome de usuário só pode conter letras, números e underline.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório.")
                .EmailAddress().WithMessage("Email inválido.");
        }
    }
}
