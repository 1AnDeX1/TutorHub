using Microsoft.EntityFrameworkCore;
using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories.UserInterfaces;

namespace TutorHub.DataAccess.Repositories.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<User> users, int usersCount)> GetUsersAsync(int page, int pageSize)
        {
            var users = await _context.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var usersCount = await _context.Users.CountAsync();

            return (users, usersCount);
        }

        public async Task<(IEnumerable<User> users, int usersCount)> GetUsersByNameAsync(string username, int page, int pageSize)
        {
            var users = await _context.Users
                .Where(t => t.UserName!.Contains(username))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var usersCount = await _context.Users.Where(t => t.UserName!.Contains(username)).CountAsync();

            return (users, usersCount);
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }

}
