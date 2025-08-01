using Moq;
using Jira_ya.UnitTests.TestUtils;
using Jira_ya.Application.DTOs;
using Jira_ya.Domain.Entities;
using Jira_ya.Domain.Common;

namespace Jira_ya.UnitTests.Entities.TaskTests
{
public class TaskServiceTests : IClassFixture<TaskServiceFixture>
    {
        private readonly TaskServiceFixture _fixture;
        private Mock<Domain.Interfaces.ITaskRepository> _taskRepoMock => _fixture.TaskRepoMock;
        private Mock<Domain.Interfaces.INotificationService> _notificationMock => _fixture.NotificationMock;
        private Mock<Domain.Interfaces.IUserRepository> _userRepoMock => _fixture.UserRepoMock;
        private Mock<AutoMapper.IMapper> _mapperMock => _fixture.MapperMock;
        private Mock<Application.MessageBus.IMessageBusPublisher> _messageBusMock => _fixture.MessageBusMock;
        private Application.Services.TaskService _service => _fixture.Service;

        public TaskServiceTests(TaskServiceFixture fixture)
        {
            _fixture = fixture;
            _fixture.ResetMocks();
            _messageBusMock.Setup(m => m.PublishAsync(It.IsAny<string>(), It.IsAny<object>())).Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task CreateAsync_ReturnsOk_WhenTaskIsCreated()
        {
            var dto = TaskTestDataFactory.CreateValidTaskRequest();
            var user = DomainTestDataFactory.CreateValidUser(id: dto.AssignedUserId);
            var task = TaskTestDataFactory.CreateValidDomainTask(assignedUserId: dto.AssignedUserId, title: dto.Title);
            var taskDto = TaskTestDataFactory.CreateValidTaskDto(id: task.Id, assignedUserId: task.AssignedUserId, title: task.Title);
            _userRepoMock.Setup(r => r.GetByIdAsync(dto.AssignedUserId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<DomainTask>(dto)).Returns(task);
            _mapperMock.Setup(m => m.Map<TaskDto>(task)).Returns(taskDto);
            _taskRepoMock.Setup(r => r.AddAsync(It.IsAny<DomainTask>())).Returns(Task.CompletedTask);
            _notificationMock.Setup(n => n.NotifyAsync(It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            Assert.True(result.Success);
            Assert.Equal(taskDto, result.Data);
        }

        [Fact]
        public async Task CreateAsync_ReturnsFail_WhenUserNotFound()
        {
            var dto = TaskTestDataFactory.CreateValidTaskRequest();
            _userRepoMock.Setup(r => r.GetByIdAsync(dto.AssignedUserId)).ReturnsAsync((User?)null);

            var result = await _service.CreateAsync(dto);

            Assert.False(result.Success);
            Assert.Equal("Usuário não existe para o guid informado, logo não será possível inserir esta task", result.Error);
        }

        [Fact]
        public async Task CreateAsync_ReturnsFail_WhenExceptionThrown()
        {
            var dto = TaskTestDataFactory.CreateValidTaskRequest();
            var user = DomainTestDataFactory.CreateValidUser(id: dto.AssignedUserId);
            _userRepoMock.Setup(r => r.GetByIdAsync(dto.AssignedUserId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<DomainTask>(dto)).Throws(new Exception("Erro de mapeamento"));

            var result = await _service.CreateAsync(dto);

            Assert.False(result.Success);
            Assert.Equal("Erro ao salvar a tarefa no banco de dados.", result.Error);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFail_WhenTaskNotFound()
        {
            var id = IdGenerator.New();
            _taskRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((DomainTask?)null);
            var dto = TaskTestDataFactory.CreateValidTaskDto(id: id);

            var result = await _service.UpdateAsync(id, dto);

            Assert.False(result.Success);
            Assert.Equal("Tarefa não encontrada.", result.Error);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsOk_WhenTaskIsDeleted()
        {
            var id = IdGenerator.New();
            var task = TaskTestDataFactory.CreateValidDomainTask(id: id);
            _taskRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(task);
            _taskRepoMock.Setup(r => r.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            _notificationMock.Setup(n => n.NotifyAsync(It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.CompletedTask);

            var result = await _service.DeleteAsync(id);

            Assert.True(result.Success);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFail_WhenTaskNotFound()
        {
            var id = IdGenerator.New();
            _taskRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((DomainTask?)null);

            var result = await _service.DeleteAsync(id);

            Assert.False(result.Success);
            Assert.Equal("Tarefa não encontrada.", result.Error);
        }
    }
}
