using Jira_ya.Application.Services;
using Jira_ya.Domain.Interfaces;
using Moq;

namespace Jira_ya.UnitTests.User
{
    public class UserServiceFixture : IDisposable
    {
        public Mock<IUserRepository> UserRepoMock { get; } = new();
        public Mock<INotificationService> NotificationMock { get; } = new();
        public Mock<AutoMapper.IMapper> MapperMock { get; } = new();
        public UserService Service { get; }

        public UserServiceFixture()
        {
            Service = new UserService(UserRepoMock.Object, NotificationMock.Object, MapperMock.Object);
        }

        public void ResetMocks()
        {
            UserRepoMock.Reset();
            NotificationMock.Reset();
            MapperMock.Reset();
        }

        public void Dispose() { }
    }
}
