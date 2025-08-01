using Jira_ya.Application.Services;
using Jira_ya.Domain.Interfaces;
using Moq;

namespace Jira_ya.UnitTests.Entities.TaskTests
{
    public class TaskServiceFixture : IDisposable
    {
        public Mock<ITaskRepository> TaskRepoMock { get; } = new();
        public Mock<INotificationService> NotificationMock { get; } = new();
        public Mock<IUserRepository> UserRepoMock { get; } = new();
        public Mock<AutoMapper.IMapper> MapperMock { get; } = new();
        public TaskService Service { get; }

        public TaskServiceFixture()
        {
            Service = new TaskService(TaskRepoMock.Object, NotificationMock.Object, UserRepoMock.Object, MapperMock.Object);
        }

        public void ResetMocks()
        {
            TaskRepoMock.Reset();
            NotificationMock.Reset();
            UserRepoMock.Reset();
            MapperMock.Reset();
        }

        public void Dispose() { }
    }
}
