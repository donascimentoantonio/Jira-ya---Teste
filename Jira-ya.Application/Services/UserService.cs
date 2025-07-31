
using Jira_ya.Application.DTOs;
using Jira_ya.Application.Services.Interfaces;
using Jira_ya.Domain.Entities;
using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Application.Services
{
    public class UserService(IUserRepository userRepository, INotificationService notificationService) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly INotificationService _notificationService = notificationService;


        public IEnumerable<UserDto> GetAll()
        {
            return _userRepository.GetAll().Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            });
        }

        public UserDto GetById(int id)
        {
            var u = _userRepository.GetById(id);
            if (u == null) return null;
            return new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            };
        }

        public UserDto Create(CreateUserRequest dto)
        {
            var entity = new User
            {
                Name = dto.Name,
                Email = dto.Email
            };
            _userRepository.Add(entity);
            _notificationService.Notify($"Usuário criado: {entity.Name}", entity.Id);
            return new UserDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email
            };
        }

        public UserDto Update(int id, CreateUserRequest dto)
        {
            var entity = _userRepository.GetById(id);
            if (entity == null) return null;
            entity.Name = dto.Name;
            entity.Email = dto.Email;
            _userRepository.Update(entity);
            _notificationService.Notify($"Usuário atualizado: {entity.Name}", entity.Id);
            return new UserDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email
            };
        }

        public bool Delete(int id)
        {
            var entity = _userRepository.GetById(id);
            if (entity == null) return false;
            _userRepository.Delete(id);
            _notificationService.Notify($"Usuário removido: {entity.Name}", entity.Id);
            return true;
        }
    }
}
