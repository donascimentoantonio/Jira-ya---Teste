using AutoMapper;
using Jira_ya.Application.DTOs;
using Jira_ya.Application.Mapping;
using Jira_ya.Application.Services;
using Jira_ya.Domain.Entities;
using Jira_ya.Infrastructure.Persistence;
using Jira_ya.UnitTests.TestUtils;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Jira_ya.IntegrationTests.services
{
    public class TaskServiceIntegrationTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly AppDbContext _context;
        private readonly TaskRepository _taskRepo;
        private readonly UserRepository _userRepo;
        private readonly TaskService _service;
        private readonly IMapper _mapper;

        public TaskServiceIntegrationTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;
            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();

            _taskRepo = new TaskRepository(_context);
            _userRepo = new UserRepository(_context);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<TaskProfile>();
            });
            _mapper = config.CreateMapper();
            var notification = new FakeNotificationService();
            _service = new TaskService(_taskRepo, notification, _userRepo, _mapper);
        }

        [Fact]
        public async Task CreateAsync_PersistsTask_WhenValid()
        {
            var user = DomainTestDataFactory.CreateValidUser();

            _context.Users.Add(user);
            _context.SaveChanges();

            var dto = DomainTestDataFactory.CreateValidTaskRequest();

            var result = await _service.CreateAsync(dto);

            Assert.True(result.Success, result.Error);
            Assert.NotNull(await _context.Tasks.FirstOrDefaultAsync(t => t.Title == "Task"));
        }

        public void Dispose()
        {
            _context.Database.CloseConnection();
            _context.Dispose();
            _connection.Dispose();
        }
    }

    public class FakeNotificationService : Jira_ya.Domain.Interfaces.INotificationService
    {
        public Task NotifyAsync(string message, Guid userId) => Task.CompletedTask;
    }
}
