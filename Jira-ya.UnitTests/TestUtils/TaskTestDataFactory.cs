using Jira_ya.Application.DTOs;

namespace Jira_ya.UnitTests.TestUtils
{
    public static class TaskTestDataFactory
    {
        public static CreateTaskRequest CreateValidTaskRequest(Guid? assignedUserId = null)
        {
            return new CreateTaskRequest
            {
                Title = "Task",
                Description = "Descrição teste",
                DueDate = DateTime.Now.AddDays(1),
                AssignedUserId = assignedUserId ?? Guid.NewGuid()
            };
        }
    }
}
