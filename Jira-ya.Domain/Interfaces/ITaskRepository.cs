namespace Jira_ya.Domain.Interfaces
{
    using DomainTask = Entities.Task;
    public interface ITaskRepository
    {
        DomainTask GetById(int id);
        IEnumerable<DomainTask> GetAll();
        void Add(DomainTask task);
        void Update(DomainTask task);
        void Delete(int id);
    }
}
