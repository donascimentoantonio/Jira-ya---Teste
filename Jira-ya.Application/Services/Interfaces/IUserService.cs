using Jira_ya.Application.DTOs;

namespace Jira_ya.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(Guid id);
        Task<UserDto> CreateAsync(CreateUserRequest dto);
        Task<UserDto> UpdateAsync(Guid id, CreateUserRequest dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
