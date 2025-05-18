using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorHub.DataAccess.Entities;

namespace TutorHub.DataAccess.IRepositories.UserInterfaces
{
    public interface IUserRepository
    {
        Task<(IEnumerable<User> users, int usersCount)> GetUsersAsync(int page, int pageSize);

        Task<(IEnumerable<User> users, int usersCount)> GetUsersByNameAsync(string username, int page, int pageSize);

        Task<User?> GetByIdAsync(string id);

        Task UpdateAsync(User user);

        Task DeleteAsync(string id);
    }
}
