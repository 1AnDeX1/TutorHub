using Microsoft.EntityFrameworkCore;
using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories.IUserRepositories;

namespace TutorHub.DataAccess.Repositories.UserRepositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<(IEnumerable<User> users, int usersCount)> GetUsersAsync(int page, int pageSize)
    {
        var users = await context.Users
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var usersCount = await context.Users.CountAsync();

        return (users, usersCount);
    }

    public async Task<(IEnumerable<User> users, int usersCount)> GetUsersByNameAsync(string username, int page, int pageSize)
    {
        var users = await context.Users
            .Where(t => t.UserName!.Contains(username))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var usersCount = await context.Users.Where(t => t.UserName!.Contains(username)).CountAsync();

        return (users, usersCount);
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await context.Users
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var user = await context.Users.FindAsync(id);
        if (user != null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}
