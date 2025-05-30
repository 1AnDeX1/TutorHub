using TutorHub.BusinessLogic.Models.User.Interfaces;
using TutorHub.DataAccess.Enums;

namespace TutorHub.BusinessLogic.Models.User.Teacher;

public class TeacherCreateModel : IUserCreateModel
{
    public required string UserName { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public IList<Subject> Subjects { get; set; }

    public decimal HourlyRate { get; set; }

    public bool Online { get; set; }

    public bool Offline { get; set; }

    public int Age { get; set; }

    public string? Description { get; set; }
}
