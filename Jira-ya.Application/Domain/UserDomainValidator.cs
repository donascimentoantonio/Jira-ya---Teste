using Jira_ya.Application.Common;
using System.Text.RegularExpressions;

namespace Jira_ya.Application.Domain
{
    public static class UserDomainValidator
    {
        public static Result<object> ValidateUserExists(object entity)
        {
            if (entity == null)
                return Result<object>.Fail("Usuário não encontrado para o id informado.");
            return Result<object>.Ok(entity);
        }

        public static Result<string> ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return Result<string>.Fail("Nome de usuário é obrigatório.");
            if (username.Length < 3 || username.Length > 30)
                return Result<string>.Fail("Nome de usuário deve ter entre 3 e 30 caracteres.");
            if (!Regex.IsMatch(username, "^[a-zA-Z0-9_]+$"))
                return Result<string>.Fail("Nome de usuário só pode conter letras, números e underline.");
            return Result<string>.Ok(username);
        }

        public static Result<string> ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result<string>.Fail("E-mail é obrigatório.");
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Result<string>.Fail("E-mail inválido.");
            return Result<string>.Ok(email);
        }

        public static Result<bool> ValidateUserIsActive(bool isActive)
        {
            if (!isActive)
                return Result<bool>.Fail("Usuário está inativo.");
            return Result<bool>.Ok(isActive);
        }
    }
}
