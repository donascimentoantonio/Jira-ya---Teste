using Jira_ya.Application.Common;
using Jira_ya.Application.DTOs;
using Jira_ya.Application.Services.Interfaces;
using Jira_ya.Domain.Interfaces;
using Jira_ya.Application.Domain;
using Jira_ya.Application.MessageBus;

namespace Jira_ya.Application.Services
{
    using DomainTask = Jira_ya.Domain.Entities.DomainTask;

    public class TaskService(ITaskRepository taskRepository, INotificationService notificationService, IUserRepository userRepository, AutoMapper.IMapper mapper, IMessageBusPublisher messageBusPublisher) : ITaskService    
    {
        private readonly ITaskRepository _taskRepository = taskRepository;
        private readonly INotificationService _notificationService = notificationService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly AutoMapper.IMapper _mapper = mapper;
        private readonly IMessageBusPublisher _messageBusPublisher = messageBusPublisher;

        private const string NotificationQueue = "user-notifications";

        public async Task<IEnumerable<TaskDto>> GetAllAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task<TaskDto> GetByIdAsync(Guid id)
        {
            var t = await _taskRepository.GetByIdAsync(id);
            if (t == null) return null;
            return _mapper.Map<TaskDto>(t);
        }

        public async Task<Result<TaskDto>> CreateAsync(CreateTaskRequest dto)
        {
            var titleValidation = TaskDomainValidator.ValidateTitle(dto.Title);
            if (!titleValidation.Success) return Result<TaskDto>.Fail(titleValidation.Error);
            var dateValidation = TaskDomainValidator.ValidateDueDate(dto.DueDate);
            if (!dateValidation.Success) return Result<TaskDto>.Fail(dateValidation.Error);


            var user = await _userRepository.GetByIdAsync(dto.AssignedUserId);
            if (user == null)
                return Result<TaskDto>.Fail(Common.DomainMessages.UserNotExistsForTask);

            DomainTask entity;
            try
            {
                entity = _mapper.Map<DomainTask>(dto);
                entity.Id = Guid.NewGuid();
                await _taskRepository.AddAsync(entity);
                await _notificationService.NotifyAsync(string.Format(Common.DomainMessages.TaskCreated, entity.Title), entity.AssignedUserId);
                await _messageBusPublisher.PublishAsync(NotificationQueue, new {
                    UserId = entity.AssignedUserId,
                    Message = string.Format(Common.DomainMessages.TaskNotificationCreated, entity.Title)
                });
            }
            catch (Exception ex)
            {
                return ErrorHandler.HandleException<TaskDto>(ex, ErrorMessages.TaskSaveError);
            }
            var resultDto = _mapper.Map<TaskDto>(entity);
            return Result<TaskDto>.Ok(resultDto);
        }

        public async Task<Result<TaskDto>> UpdateAsync(Guid id, TaskDto dto)
        {
            var entity = await _taskRepository.GetByIdAsync(id);
            var existsValidation = TaskDomainValidator.ValidateTaskExists(entity);
            if (!existsValidation.Success) return Result<TaskDto>.Fail(existsValidation.Error);
            var titleValidation = TaskDomainValidator.ValidateTitle(dto.Title);
            if (!titleValidation.Success) return Result<TaskDto>.Fail(titleValidation.Error);
            var dateValidation = TaskDomainValidator.ValidateDueDate(dto.DueDate);
            if (!dateValidation.Success) return Result<TaskDto>.Fail(dateValidation.Error);
            _mapper.Map(dto, entity);
            try
            {
                await _taskRepository.UpdateAsync(entity);
                await _notificationService.NotifyAsync(string.Format(Common.DomainMessages.TaskUpdated, entity.Title), entity.AssignedUserId);
                await _messageBusPublisher.PublishAsync(NotificationQueue, new {
                    UserId = entity.AssignedUserId,
                    Message = string.Format(Common.DomainMessages.TaskNotificationUpdated, entity.Title)
                });
            }
            catch (Exception ex)
            {
                return ErrorHandler.HandleException<TaskDto>(ex, ErrorMessages.TaskUpdateError);
            }
            return Result<TaskDto>.Ok(_mapper.Map<TaskDto>(entity));
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            var entity = await _taskRepository.GetByIdAsync(id);
            var existsValidation = TaskDomainValidator.ValidateTaskExists(entity);
            if (!existsValidation.Success) return Result<bool>.Fail(existsValidation.Error);
            try
            {
                await _taskRepository.DeleteAsync(id);
                await _notificationService.NotifyAsync(string.Format(Common.DomainMessages.TaskRemoved, entity.Title), entity.AssignedUserId);
                await _messageBusPublisher.PublishAsync(NotificationQueue, new {
                    UserId = entity.AssignedUserId,
                    Message = string.Format(Common.DomainMessages.TaskNotificationDeleted, entity.Title)
                });
            }
            catch (Exception ex)
            {
                return ErrorHandler.HandleException<bool>(ex, ErrorMessages.TaskDeleteError);
            }
            return Result<bool>.Ok(true);
        }

        public async Task<IEnumerable<TaskDto>> GetByUserIdAsync(Guid userId)
        {
            var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }
        public async Task<bool> AssignTaskAsync(Guid taskId, Guid userId)
        {
            var result = await _taskRepository.AssignTaskAsync(taskId, userId);
            if (result)
            {
                await _notificationService.NotifyAsync(string.Format(Common.DomainMessages.TaskAssigned, userId), userId);
                await _messageBusPublisher.PublishAsync(NotificationQueue, new {
                    UserId = userId,
                    Message = Common.DomainMessages.TaskNotificationAssigned
                });
            }
            return result;
        }
    }
}
