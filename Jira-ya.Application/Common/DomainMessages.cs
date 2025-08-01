namespace Jira_ya.Application.Common
{
    public static class DomainMessages
    {
        public const string TaskCreated = "Tarefa criada: {0}";
        public const string TaskUpdated = "Tarefa atualizada: {0}";
        public const string TaskRemoved = "Tarefa removida: {0}";
        public const string TaskAssigned = "Tarefa atribuída ao usuário {0}";
        public const string TaskNotificationCreated = "Você recebeu uma nova tarefa: {0}";
        public const string TaskNotificationUpdated = "Uma tarefa sua foi atualizada: {0}";
        public const string TaskNotificationDeleted = "Uma tarefa sua foi excluída: {0}";
        public const string TaskNotificationAssigned = "Uma tarefa foi atribuída a você.";

        public const string UserNotFound = "Usuário não encontrado.";
        public const string UserNotExistsForTask = "Usuário não existe para o guid informado, logo não será possível inserir esta task";
        public const string UserAmountGreaterThanZero = "A quantidade de usuários deve ser maior que zero.";
        public const string UserCreated = "Usuário criado: {0}";
        public const string UserUpdated = "Usuário atualizado: {0}";
        public const string UserRemovedMessage = "Usuário removido: {0}";
        public const string RandomUserKeyEmpty = "A chave aleatória não pode ser vazia.";
        public const string RandomUserCreateError = "Erro ao criar usuários aleatórios.";
    }
}
