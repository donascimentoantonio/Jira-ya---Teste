using System.Collections.Generic;
using Jira_ya.Domain.Entities;

namespace Jira_ya.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task GetById(int id);
        IEnumerable<Task> GetAll();
        void Add(Task task);
        void Update(Task task);
        void Delete(int id);
    }
}
