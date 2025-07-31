using Jira_ya.Domain.Enum;

namespace Jira_ya.Domain.Entities
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public enumTaskStatus Status { get; set; }
        public Guid AssignedUserId { get; set; }
    }
}
