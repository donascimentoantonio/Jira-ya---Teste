using Jira_ya.Application.DTOs;
using Jira_ya.Application.Services.Interfaces;
using Jira_ya.Domain.Entities;
using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public UserService(IUserRepository userRepository, INotificationService notificationService)
        {
            _userRepository = userRepository;
            _notificationService = notificationService;
        }


        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = _userRepository.GetAll();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Username,
                Email = u.Email
            });
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var u = _userRepository.GetById(id);
            if (u == null) return null;
            return new UserDto
            {
                Id = u.Id,
                Name = u.Username,
                Email = u.Email
            };
        }

        public async Task<UserDto> CreateAsync(CreateUserRequest dto)
        {
            var entity = new User
            {
                Id = Guid.NewGuid(),
                Username = dto.Name,
                Email = dto.Email
            };
            _userRepository.Add(entity);
            await _notificationService.NotifyAsync($"Usuário criado: {entity.Username}", entity.Id);
            return new UserDto
            {
                Id = entity.Id,
                Name = entity.Username,
                Email = entity.Email
            };
        }

        public async Task<UserDto> UpdateAsync(Guid id, CreateUserRequest dto)
        {
            var entity = _userRepository.GetById(id);
            if (entity == null) return null;
            entity.Username = dto.Name;
            entity.Email = dto.Email;
            _userRepository.Update(entity);
            await _notificationService.NotifyAsync($"Usuário atualizado: {entity.Username}", entity.Id);
            return new UserDto
            {
                Id = entity.Id,
                Name = entity.Username,
                Email = entity.Email
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = _userRepository.GetById(id);
            if (entity == null) return false;
            _userRepository.Delete(id);
            await _notificationService.NotifyAsync($"Usuário removido: {entity.Username}", entity.Id);
            return true;
        }

        public async Task<IEnumerable<UserDto>> CreateRandomUsersAsync(int amount, string userNameMask, string randomKey)
        {
            var users = new List<UserDto>();
            var rand = new Random();
            for (int i = 0; i < amount; i++)
            {
                var randomPart = randomKey + rand.Next(100000, 999999);
                var username = userNameMask.Replace("{{random}}", randomPart);
                var entity = new User
                {
                    Id = Guid.NewGuid(),
                    Username = username,
                    Email = $"{username}@example.com"
                };
                _userRepository.Add(entity);
                users.Add(new UserDto
                {
                    Id = entity.Id,
                    Name = entity.Username,
                    Email = entity.Email
                });
            }
            return users;
        }
    }
}
