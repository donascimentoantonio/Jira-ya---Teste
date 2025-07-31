using FluentValidation;
using Jira_ya.Application.DTOs;

namespace Jira_ya.Application.Validators.User
{
    public class CreateRandomUsersRequestValidator : AbstractValidator<CreateRandomUsersRequest>
    {
        public CreateRandomUsersRequestValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount deve ser maior que zero.");

            RuleFor(x => x.UserNameMask)
                .NotEmpty().WithMessage("UserNameMask é obrigatório.")
                .Must(mask => mask.Contains("{{random}}"))
                .WithMessage("A máscara deve conter '{{random}}'.");
        }
    }
}
