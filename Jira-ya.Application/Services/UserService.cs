using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Application.Services
{
    public class UserService(IUserRepository userRepository, INotificationService notificationService)
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly INotificationService _notificationService = notificationService;
    }
}
