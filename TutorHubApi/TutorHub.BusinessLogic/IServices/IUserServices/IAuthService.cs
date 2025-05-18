using TutorHub.BusinessLogic.Models.User;
using TutorHub.BusinessLogic.Models.User.Teacher;

namespace TutorHub.BusinessLogic.IServices.IUserServices
{
    public interface IAuthService
    {
        Task<(int, string, string)> Registration(RegistrationModel model, string role);

        Task<(int, string, string?)> Login(LoginModel model);
    }
}
