
using Jira_ya.Domain.Common;
using Jira_ya.Domain.Entities;

namespace Jira_ya.UnitTests.TestUtils
{
    public static class DomainTestDataFactory
    {
        public static User CreateValidUser(Guid? id = null, string? username = null, string? email = null)
        {
            return new User
            {
                Id = id ?? IdGenerator.New(),
                Username = username ?? "user",
                Email = email ?? "user@email.com"
            };
        }
    }
}
