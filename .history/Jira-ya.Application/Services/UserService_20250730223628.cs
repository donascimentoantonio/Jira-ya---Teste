using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public UserService(IUserRepository userRepository, INotificationService notificationService)
        {
            _userRepository = userRepository;
            _notificationService = notificationService;
        }
    }
}
