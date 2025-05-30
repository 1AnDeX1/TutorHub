using TutorHub.DataAccess.Entities;

namespace TutorHub.DataAccess.IRepositories.IUserRepositories;

public interface IUserRepository
{
    Task<(IEnumerable<User> users, int usersCount)> GetUsersAsync(int page, int pageSize);

    Task<(IEnumerable<User> users, int usersCount)> GetUsersByNameAsync(string username, int page, int pageSize);

    Task<User?> GetByIdAsync(string id);

    Task UpdateAsync(User user);

    Task DeleteAsync(string id);
}
