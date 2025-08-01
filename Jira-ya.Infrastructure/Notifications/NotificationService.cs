using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Infrastructure.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly Jira_ya.Application.MessageBus.IMessageBusPublisher _messageBusPublisher;
        private const string NotificationQueue = "user-notifications";

        public NotificationService(Jira_ya.Application.MessageBus.IMessageBusPublisher messageBusPublisher)
        {
            _messageBusPublisher = messageBusPublisher;
        }

        public async Task NotifyAsync(string message, Guid userId)
        {
            // Aqui pode-se adicionar lógica extra (ex: e-mail, push, etc)
            await _messageBusPublisher.PublishAsync(NotificationQueue, new {
                UserId = userId,
                Message = message
            });
        }
    }
}
