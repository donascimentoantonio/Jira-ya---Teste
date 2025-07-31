namespace Jira_ya.Domain.Interfaces
{
    public interface INotificationService
    {
        void Notify(string message, int userId);
    }
}
