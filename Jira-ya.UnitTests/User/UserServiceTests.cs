using Jira_ya.UnitTests.TestUtils;
using Jira_ya.Application.DTOs;
using Jira_ya.Domain.Interfaces;
using Moq;
using DomainUser = Jira_ya.Domain.Entities.User;

namespace Jira_ya.UnitTests.User
{
    public class UserServiceTests : IClassFixture<UserServiceFixture>
    {
        private readonly UserServiceFixture _fixture;
        private Mock<IUserRepository> _userRepoMock => _fixture.UserRepoMock;
        private Mock<AutoMapper.IMapper> _mapperMock => _fixture.MapperMock;
        private Jira_ya.Application.Services.UserService _service => _fixture.Service;

        public UserServiceTests(UserServiceFixture fixture)
        {
            _fixture = fixture;
            _fixture.ResetMocks();
        }

        [Fact]
        public async Task CreateAsync_ReturnsOk_WhenUserIsCreated()
        {
            var dto = UserTestDataFactory.CreateValidUserRequest();
            var user = DomainTestDataFactory.CreateValidUser(username: dto.Name, email: dto.Email);
            var userDto = UserDtoTestDataFactory.CreateValidUserDto(id: user.Id, name: user.Username, email: user.Email);
            _mapperMock.Setup(m => m.Map<DomainUser>(dto)).Returns(user);
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            var result = await _service.CreateAsync(dto);

            Assert.True(result.Success);
            Assert.Equal(userDto, result.Data);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFail_WhenUserNotFound()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((DomainUser)null);
            var dto = UserTestDataFactory.CreateValidUserRequest();

            var result = await _service.UpdateAsync(Guid.NewGuid(), dto);

            Assert.False(result.Success);
            Assert.Equal("Usuário não encontrado.", result.Error);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFail_WhenUserNotFound()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((DomainUser)null);

            var result = await _service.DeleteAsync(Guid.NewGuid());

            Assert.False(result.Success);
            Assert.Equal("Usuário não encontrado.", result.Error);
        }

        [Fact]
        public async Task CreateAsync_ReturnsFail_WhenExceptionThrown()
        {
            var dto = UserTestDataFactory.CreateValidUserRequest();
            _mapperMock.Setup(m => m.Map<DomainUser>(dto)).Throws(new System.Exception("Erro de mapeamento"));

            var result = await _service.CreateAsync(dto);

            Assert.False(result.Success);
            Assert.Equal("Erro ao salvar o usuário no banco de dados.", result.Error);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsOk_WhenUserIsUpdated()
        {
            var dto = UserTestDataFactory.CreateValidUserRequest();
            var user = DomainTestDataFactory.CreateValidUser(username: dto.Name, email: dto.Email);
            var userDto = UserDtoTestDataFactory.CreateValidUserDto(id: user.Id, name: user.Username, email: user.Email);
            _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map(dto, user));
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            var result = await _service.UpdateAsync(user.Id, dto);

            Assert.True(result.Success);
            Assert.Equal(userDto, result.Data);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsOk_WhenUserIsDeleted()
        {
            var user = DomainTestDataFactory.CreateValidUser();
            _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);

            var result = await _service.DeleteAsync(user.Id);

            Assert.True(result.Success);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task CreateRandomUsersAsync_ReturnsFail_WhenAmountIsZero()
        {
            var result = await _service.CreateRandomUsersAsync(0, "mask-{{random}}", "RND");
            Assert.False(result.Success);
            Assert.Equal("A quantidade de usuários deve ser maior que zero.", result.Error);
        }

        [Fact]
        public async Task CreateRandomUsersAsync_ReturnsOk_WhenValid()
        {
            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<DomainUser>())).Returns(Task.CompletedTask);
            var result = await _service.CreateRandomUsersAsync(2, "mask-{{random}}", "RND");
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count());
        }
    }
}
