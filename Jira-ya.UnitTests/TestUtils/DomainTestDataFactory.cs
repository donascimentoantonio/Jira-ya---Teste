
namespace Jira_ya.UnitTests.TestUtils
{
    using DomainUser = Jira_ya.Domain.Entities.User;

    public static class DomainTestDataFactory
    {
        public static DomainUser CreateValidUser(Guid? id = null, string? username = null, string? email = null)
        {
            return new DomainUser
            {
                Id = id ?? Guid.NewGuid(),
                Username = username ?? "user",
                Email = email ?? "user@email.com"
            };
        }
    }
}
