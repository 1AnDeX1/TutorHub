using TutorHub.DataAccess.Enums;

namespace TutorHub.DataAccess.Entities;

public class Teacher
{
    public int Id { get; set; }

    public required string UserId { get; set; }

    public IList<Subject> Subjects { get; set; }

    public decimal HourlyRate { get; set; }

    public double Rating { get; set; }

    public bool Online { get; set; }

    public bool Offline { get; set; }

    public int Age { get; set; }

    public string? Description { get; set; }

    public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.NotRequested;

    public User? User { get; set; }

    public ICollection<Schedule>? Lessons { get; set; }

    public ICollection<StudentTeacher>? Students { get; set; }

    public ICollection<TeacherAvailability>? TeacherAvailabilities { get; set; }

    public ICollection<TeacherRating>? Ratings { get; set; }

    public ICollection<ChatMessage>? ChatMessages { get; set; }
}
