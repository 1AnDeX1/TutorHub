
namespace TutorHub.BusinessLogic.Models.User.Interfaces;

public interface IUserCreateModel
{
    string UserName { get; set; }
    string Email { get; set; }
    string? Password { get; set; }
}
