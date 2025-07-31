using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Infrastructure.Persistence
{
    using Jira_ya.Domain.Entities;

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }


        public User GetById(Guid id)
        {
            return _context.Users.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return [.. _context.Users];
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
