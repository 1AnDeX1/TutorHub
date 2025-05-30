namespace TutorHub.DataAccess.Entities;

public class TeacherAvailability
{
    public int Id { get; set; }

    public int TeacherId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public Teacher Teacher { get; set; }
}
