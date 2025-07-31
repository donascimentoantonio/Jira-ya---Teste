using Jira_ya.Application.Common;

namespace Jira_ya.Application.Domain
{
    public static class TaskDomainValidator
    {
        public static Result<object> ValidateTaskExists(object entity)
        {
            if (entity == null)
                return Result<object>.Fail("Task não encontrada para o id informado.");
            return Result<object>.Ok(entity);
        }

        public static Result<string> ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result<string>.Fail("Título é obrigatório.");
            return Result<string>.Ok(title);
        }

        public static Result<DateTime> ValidateDueDate(DateTime dueDate)
        {
            if (dueDate < DateTime.Today)
                return Result<DateTime>.Fail("Data de vencimento não pode ser no passado.");
            return Result<DateTime>.Ok(dueDate);
        }
    }
}
