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
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INotificationService, NotificationService>(); 
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IUserService, UserService>();

            services.AddControllers();
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<Application.Validators.CreateRandomUsersRequestValidator>();

            return services;
        }
    }
}
