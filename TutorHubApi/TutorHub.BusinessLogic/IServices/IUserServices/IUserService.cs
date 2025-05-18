using TutorHub.BusinessLogic.Models.User;
using TutorHub.BusinessLogic.Models.User.Interfaces;

namespace TutorHub.BusinessLogic.IServices.IUserServices
{
    public interface IUserService
    {
        Task<UserModel?> GetUserByIdAsync(string id);

        Task UpdateUserAsync<T>(string id, T model) where T : IUserCreateModel;

        Task DeleteUserAsync(string id);
    }
}
