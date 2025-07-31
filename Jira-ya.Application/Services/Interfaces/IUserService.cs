using Jira_ya.Application.DTOs;

namespace Jira_ya.Application.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAll();
        UserDto GetById(int id);
        UserDto Create(CreateUserRequest dto);
        UserDto Update(int id, CreateUserRequest dto);
        bool Delete(int id);
    }
}
