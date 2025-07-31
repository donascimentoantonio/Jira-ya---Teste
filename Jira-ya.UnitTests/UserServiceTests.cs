using Jira_ya.Application.Common;
using Jira_ya.Application.DTOs;
using Jira_ya.Application.Services;
using Jira_ya.Domain.Entities;
using Jira_ya.Domain.Interfaces;
using Moq;
using Xunit;

namespace Jira_ya.UnitTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<INotificationService> _notificationMock = new();
        private readonly Mock<AutoMapper.IMapper> _mapperMock = new();
        private readonly UserService _service;

        public UserServiceTests()
        {
            _service = new UserService(_userRepoMock.Object, _notificationMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ReturnsOk_WhenUserIsCreated()
        {
            var dto = new CreateUserRequest { Username = "user", Email = "user@email.com" };
            var user = new User { Id = Guid.NewGuid(), Username = dto.Username, Email = dto.Email };
            var userDto = new UserDto { Id = user.Id, Name = user.Username, Email = user.Email };
            _mapperMock.Setup(m => m.Map<User>(dto)).Returns(user);
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            var result = await _service.CreateAsync(dto);

            Assert.True(result.Success);
            Assert.Equal(userDto, result.Data);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFail_WhenUserNotFound()
        {
            _userRepoMock.Setup(r => r.GetById(It.IsAny<Guid>())).Returns((User)null);
            var dto = new CreateUserRequest { Username = "user", Email = "user@email.com" };

            var result = await _service.UpdateAsync(Guid.NewGuid(), dto);

            Assert.False(result.Success);
            Assert.Equal("Usuário não encontrado.", result.Error);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFail_WhenUserNotFound()
        {
            _userRepoMock.Setup(r => r.GetById(It.IsAny<Guid>())).Returns((User)null);

            var result = await _service.DeleteAsync(Guid.NewGuid());

            Assert.False(result.Success);
            Assert.Equal("Usuário não encontrado.", result.Error);
        }
    }
}
