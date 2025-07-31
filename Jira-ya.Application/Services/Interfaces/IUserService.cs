using Jira_ya.Application.Common;
using Jira_ya.Application.DTOs;

namespace Jira_ya.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(Guid id);
        Task<Result<UserDto>> CreateAsync(CreateUserRequest dto);
        Task<Result<UserDto>> UpdateAsync(Guid id, CreateUserRequest dto);
        Task<Result<bool>> DeleteAsync(Guid id);
        Task<Result<IEnumerable<UserDto>>> CreateRandomUsersAsync(int amount, string userNameMask, string randomKey);
    }
}
