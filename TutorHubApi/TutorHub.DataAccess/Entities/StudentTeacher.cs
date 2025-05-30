using TutorHub.DataAccess.Enums;

namespace TutorHub.DataAccess.Entities;

public class StudentTeacher
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int TeacherId { get; set; }

    public Student Student { get; set; }
    
    public Teacher Teacher { get; set; }

    public ICollection<Schedule>? Schedules { get; set; } = new List<Schedule>();

    public StudentTeacherStatus Status { get; set; } = StudentTeacherStatus.Pending;
}
