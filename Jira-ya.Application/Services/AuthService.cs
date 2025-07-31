using Jira_ya.Application.DTOs;
using Jira_ya.Domain.Entities;
using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> AuthenticateAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user != null /* && user.Password == request.Password */)
                return user;
            return null;
        }
    }
}
