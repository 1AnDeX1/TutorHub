using TutorHub.DataAccess.Enums;

namespace TutorHub.BusinessLogic.Models.User.Teacher;

public class TeacherModel
{
    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public IList<Subject> Subjects { get; set; }

    public decimal HourlyRate { get; set; }

    public double Rating { get; set; }

    public bool Online { get; set; }

    public bool Offline { get; set; }

    public int Age { get; set; }

    public string? Description { get; set; }

    public VerificationStatus VerificationStatus { get; set; }
}
