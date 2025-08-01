using FluentValidation;
using FluentValidation.AspNetCore;
using Jira_ya.Application.Services;
using Jira_ya.Application.Services.Interfaces;
using Jira_ya.Domain.Interfaces;
using Jira_ya.Infrastructure.Notifications;
using Jira_ya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Jira_ya.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            #region AutoMapper
            services.AddAutoMapper(typeof(Jira_ya.Application.Mapping.TaskProfile).Assembly);
            services.AddAutoMapper(typeof(Jira_ya.Application.Mapping.UserProfile).Assembly);
            #endregion
            
            #region Message Bus
            services.AddScoped<Jira_ya.Application.MessageBus.IMessageBusPublisher, Jira_ya.Infrastructure.MessageBus.RabbitMqPublisher>();
            services.AddScoped<INotificationService, NotificationService>();
            #endregion

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            #region Repository
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            #endregion

            #region Services
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IUserService, UserService>();
            #endregion

            #region Validatores
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<Application.Validators.User.CreateRandomUsersRequestValidator>();
            #endregion

            services.AddControllers();

            return services;
        }
    }
}
