using System.ComponentModel.DataAnnotations;

namespace TutorHub.BusinessLogic.Models.User;
public class LoginModel
{
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}
