using Jira_ya.Domain.Enum;
using System.Text.Json.Serialization;

namespace Jira_ya.Application.DTOs
{
    public class CreateTaskRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public enumTaskStatus Status { get; set; }
        [JsonIgnore]
        public Guid AssignedUserId { get; set; }
    }
}
