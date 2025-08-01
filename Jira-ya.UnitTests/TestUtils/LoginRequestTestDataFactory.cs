using Jira_ya.Application.DTOs;
using System;

namespace Jira_ya.UnitTests.TestUtils
{
    public static class LoginRequestTestDataFactory
    {
        public static LoginRequest CreateValidLoginRequest(string? username = null, string? password = null)
        {
            return new LoginRequest
            {
                Username = username ?? "user",
                Password = password ?? "123"
            };
        }
    }
}
