using Jira_ya.Application.DTOs;
using System;

namespace Jira_ya.UnitTests.TestUtils
{
    public static class UserTestDataFactory
    {
        public static CreateUserRequest CreateValidUserRequest(string? name = null, string? email = null)
        {
            return new CreateUserRequest
            {
                Name = name ?? "user",
                Email = email ?? "user@email.com"
            };
        }
    }
}
