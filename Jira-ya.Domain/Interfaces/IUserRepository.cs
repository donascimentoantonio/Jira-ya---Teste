using System.Collections.Generic;
using Jira_ya.Domain.Entities;

namespace Jira_ya.Domain.Interfaces
{
    public interface IUserRepository
    {

        Task<User?> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);

        Task<User?> GetByUsernameAsync(string username);
    }
}
