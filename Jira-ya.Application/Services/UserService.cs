using Jira_ya.Application.Common;
using Jira_ya.Application.DTOs;
using Jira_ya.Application.Services.Interfaces;
using Jira_ya.Domain.Entities;
using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Application.Services
{
    public class UserService(IUserRepository userRepository, INotificationService notificationService, AutoMapper.IMapper mapper) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly INotificationService _notificationService = notificationService;
        private readonly AutoMapper.IMapper _mapper = mapper;

        private Task<User?> GetUserOrNull(Guid id)
        {
            return _userRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var u = await _userRepository.GetByIdAsync(id);
            if (u == null) return null;
            return _mapper.Map<UserDto>(u);
        }

        public async Task<Result<UserDto>> CreateAsync(CreateUserRequest dto)
        {
            try
            {
                var entity = _mapper.Map<User>(dto);
                entity.Id = Guid.NewGuid();
                await _userRepository.AddAsync(entity);
                await _notificationService.NotifyAsync($"Usuário criado: {entity.Username}", entity.Id);
                return Result<UserDto>.Ok(_mapper.Map<UserDto>(entity));
            }
            catch (Exception ex)
            {
                return ErrorHandler.HandleException<UserDto>(ex, ErrorMessages.UserSaveError);
            }
        }

        public async Task<Result<UserDto>> UpdateAsync(Guid id, CreateUserRequest dto)
        {
            var entity = await GetUserOrNull(id);
            if (entity == null) return Result<UserDto>.Fail(Common.DomainMessages.UserNotFound);
            _mapper.Map(dto, entity);
            try
            {
                await _userRepository.UpdateAsync(entity);
                await _notificationService.NotifyAsync($"Usuário atualizado: {entity.Username}", entity.Id);
                return Result<UserDto>.Ok(_mapper.Map<UserDto>(entity));
            }
            catch (Exception ex)
            {
                return ErrorHandler.HandleException<UserDto>(ex, ErrorMessages.UserUpdateError);
            }
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            var entity = await GetUserOrNull(id);
            if (entity == null)
                return Result<bool>.Fail(Common.DomainMessages.UserNotFound);
            try
            {
                await _userRepository.DeleteAsync(id);
                await _notificationService.NotifyAsync(string.Format(Common.DomainMessages.UserRemovedMessage, entity.Username), entity.Id);
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return ErrorHandler.HandleException<bool>(ex, ErrorMessages.UserDeleteError);
            }
        }

        public async Task<Result<IEnumerable<UserDto>>> CreateRandomUsersAsync(int amount, string userNameMask, string randomKey)
        {

            if (amount <= 0)
                return Result<IEnumerable<UserDto>>.Fail(Common.DomainMessages.UserAmountGreaterThanZero);

            if (string.IsNullOrWhiteSpace(randomKey))
                return Result<IEnumerable<UserDto>>.Fail(Common.DomainMessages.RandomUserKeyEmpty);

            var users = new List<UserDto>();
            var rand = new Random();
            try
            {
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

                    _userRepository.AddAsync(entity);
                    users.Add(new UserDto
                    {
                        Id = entity.Id,
                        Name = entity.Username,
                        Email = entity.Email
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException<bool>(ex, Common.DomainMessages.RandomUserCreateError);
                return Result<IEnumerable<UserDto>>.Fail(Common.DomainMessages.RandomUserCreateError);
            }
            return Result<IEnumerable<UserDto>>.Ok(users);
        }
    }
}
