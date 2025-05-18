using System.ComponentModel.DataAnnotations;

namespace TutorHub.BusinessLogic.Models.User;
public class RegistrationModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}
