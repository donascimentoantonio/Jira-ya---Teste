using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Infrastructure.Persistence
{
    using DomainTask = Jira_ya.Domain.Entities.Task;

    public class TaskRepository(AppDbContext context) : ITaskRepository
    {
        public DomainTask GetById(int id)
        {
            return context.Tasks.Find(id);
        }

        public IEnumerable<DomainTask> GetAll()
        {
            return [.. context.Tasks];
        }

        public void Add(DomainTask task)
        {
            context.Tasks.Add(task);
            context.SaveChanges();
        }

        public void Update(DomainTask task)
        {
            context.Tasks.Update(task);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var task = context.Tasks.Find(id);
            if (task != null)
            {
                context.Tasks.Remove(task);
                context.SaveChanges();
            }
        }
    }
}
