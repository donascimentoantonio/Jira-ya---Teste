using Jira_ya.Application.DTOs;
using Jira_ya.Domain.Common;
using System;

namespace Jira_ya.UnitTests.TestUtils
{
    public static class UserDtoTestDataFactory
    {
        public static UserDto CreateValidUserDto(Guid? id = null, string? name = null, string? email = null)
        {
            return new UserDto
            {
                Id = id ?? IdGenerator.New(),
                Name = name ?? "user",
                Email = email ?? "user@email.com"
            };
        }
    }
}
