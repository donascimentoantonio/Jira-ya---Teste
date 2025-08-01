namespace Jira_ya.Application.MessageBus
{
    public interface IMessageBusPublisher
    {
        Task PublishAsync(string queue, object message);
    }
}