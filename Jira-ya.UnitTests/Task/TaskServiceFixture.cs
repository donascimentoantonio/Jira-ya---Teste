using Jira_ya.Application.Services;
using Jira_ya.Domain.Interfaces;
using Moq;

namespace Jira_ya.UnitTests.Tasks
{
    public class TaskServiceFixture : IDisposable
    {
        public Mock<ITaskRepository> TaskRepoMock { get; } = new();
        public Mock<INotificationService> NotificationMock { get; } = new();
        public Mock<IUserRepository> UserRepoMock { get; } = new();
        public Mock<AutoMapper.IMapper> MapperMock { get; } = new();
        public Mock<Application.MessageBus.IMessageBusPublisher> MessageBusMock { get; } = new();
        public TaskService Service { get; }

        public TaskServiceFixture()
        {
            Service = new TaskService(TaskRepoMock.Object, NotificationMock.Object, UserRepoMock.Object, MapperMock.Object, MessageBusMock.Object);
        }

        public void ResetMocks()
        {
            TaskRepoMock.Reset();
            NotificationMock.Reset();
            UserRepoMock.Reset();
            MapperMock.Reset();
            MessageBusMock.Reset();
        }

        public void Dispose() { }
    }
}
