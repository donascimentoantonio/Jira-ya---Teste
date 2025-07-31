using Jira_ya.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jira_ya.Infrastructure.Persistence
{
    using DomainTask = Domain.Entities.DomainTask;

    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<DomainTask> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
