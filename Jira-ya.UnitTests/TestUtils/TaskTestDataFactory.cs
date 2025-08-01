using Jira_ya.Application.DTOs;
using Jira_ya.Domain.Common;

namespace Jira_ya.UnitTests.TestUtils
{
    public static class TaskTestDataFactory
    {
        public static Jira_ya.Domain.Entities.DomainTask CreateValidDomainTask(Guid? id = null, Guid? assignedUserId = null, string? title = null)
        {
            return new Jira_ya.Domain.Entities.DomainTask
            {
                Id = id ?? IdGenerator.New(),
                Title = title ?? "Task",
                Description = "Descrição teste",
                DueDate = DateTime.Now.AddDays(1),
                Status = Jira_ya.Domain.Enum.enumTaskStatus.Pending,
                AssignedUserId = assignedUserId ?? IdGenerator.New()
            };
        }

        public static Jira_ya.Application.DTOs.TaskDto CreateValidTaskDto(Guid? id = null, Guid? assignedUserId = null, string? title = null)
        {
            return new Jira_ya.Application.DTOs.TaskDto
            {
                Id = id ?? IdGenerator.New(),
                Title = title ?? "Task",
                Description = "Descrição teste",
                DueDate = DateTime.Now.AddDays(1),
                Status = Jira_ya.Domain.Enum.enumTaskStatus.Pending,
                AssignedUserId = assignedUserId ?? IdGenerator.New()
            };
        }

        public static CreateTaskRequest CreateValidTaskRequest(Guid? assignedUserId = null)
        {
            return new CreateTaskRequest
            {
                Title = "Task",
                Description = "Descrição teste",
                DueDate = DateTime.Now.AddDays(1),
                AssignedUserId = assignedUserId ?? IdGenerator.New()
            };
        }
    }
}
