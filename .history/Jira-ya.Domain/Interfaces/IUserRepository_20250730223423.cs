using System.Collections.Generic;
using Jira_ya.Domain.Entities;

namespace Jira_ya.Domain.Interfaces
{
    public interface IUserRepository
    {
        User GetById(int id);
        IEnumerable<User> GetAll();
        void Add(User user);
        void Update(User user);
        void Delete(int id);
    }
}
