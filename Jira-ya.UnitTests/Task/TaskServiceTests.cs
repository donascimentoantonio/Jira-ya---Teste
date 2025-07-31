using Jira_ya.Application.DTOs;
using Moq;
using DomainTask = Jira_ya.Domain.Entities.DomainTask;
using Xunit;
using Jira_ya.Domain.Interfaces;

namespace Jira_ya.UnitTests.Tasks
{
    public class TaskServiceTests : IClassFixture<TaskServiceFixture>
    {
        private readonly TaskServiceFixture _fixture;
        private Mock<ITaskRepository> _taskRepoMock => _fixture.TaskRepoMock;
        private Mock<INotificationService> _notificationMock => _fixture.NotificationMock;
        private Mock<IUserRepository> _userRepoMock => _fixture.UserRepoMock;
        private Mock<AutoMapper.IMapper> _mapperMock => _fixture.MapperMock;
        private Jira_ya.Application.Services.TaskService _service => _fixture.Service;

        public TaskServiceTests(TaskServiceFixture fixture)
        {
            _fixture = fixture;
            _fixture.ResetMocks();
        }

        [Fact]
        public async Task CreateAsync_ReturnsOk_WhenTaskIsCreated()
        {
            var dto = new CreateTaskRequest { Title = "Task", DueDate = DateTime.Now.AddDays(1), AssignedUserId = Guid.NewGuid() };
            var user = new Jira_ya.Domain.Entities.User { Id = dto.AssignedUserId, Username = "user", Email = "user@email.com" };
            var task = new DomainTask { Id = Guid.NewGuid(), Title = dto.Title, DueDate = dto.DueDate, AssignedUserId = dto.AssignedUserId };
            var taskDto = new TaskDto { Id = task.Id, Title = task.Title, DueDate = task.DueDate, AssignedUserId = task.AssignedUserId };
            _userRepoMock.Setup(r => r.GetById(dto.AssignedUserId)).Returns(user);
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
            var dto = new CreateTaskRequest { Title = "Task", DueDate = DateTime.Now.AddDays(1), AssignedUserId = Guid.NewGuid() };
            _userRepoMock.Setup(r => r.GetById(dto.AssignedUserId)).Returns((Jira_ya.Domain.Entities.User)null);

            var result = await _service.CreateAsync(dto);

            Assert.False(result.Success);
            Assert.Equal("Usuário não existe para o guid informado, logo não será possível inserir esta task", result.Error);
        }

        [Fact]
        public async Task CreateAsync_ReturnsFail_WhenExceptionThrown()
        {
            var dto = new CreateTaskRequest { Title = "Task", DueDate = DateTime.Now.AddDays(1), AssignedUserId = Guid.NewGuid() };
            var user = new Jira_ya.Domain.Entities.User { Id = dto.AssignedUserId, Username = "user", Email = "user@email.com" };
            _userRepoMock.Setup(r => r.GetById(dto.AssignedUserId)).Returns(user);
            _mapperMock.Setup(m => m.Map<DomainTask>(dto)).Throws(new Exception("Erro de mapeamento"));

            var result = await _service.CreateAsync(dto);

            Assert.False(result.Success);
            Assert.Equal("Erro ao salvar a tarefa no banco de dados.", result.Error);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFail_WhenTaskNotFound()
        {
            var id = Guid.NewGuid();
            _taskRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((DomainTask)null);
            var dto = new TaskDto { Id = id, Title = "Task", DueDate = DateTime.Now.AddDays(1), AssignedUserId = Guid.NewGuid() };

            var result = await _service.UpdateAsync(id, dto);

            Assert.False(result.Success);
            Assert.Equal("Tarefa não encontrada.", result.Error);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsOk_WhenTaskIsDeleted()
        {
            var id = Guid.NewGuid();
            var task = new DomainTask { Id = id, Title = "Task", DueDate = DateTime.Now.AddDays(1), AssignedUserId = Guid.NewGuid() };
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
            var id = Guid.NewGuid();
            _taskRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((DomainTask)null);

            var result = await _service.DeleteAsync(id);

            Assert.False(result.Success);
            Assert.Equal("Tarefa não encontrada.", result.Error);
        }
    }
}
