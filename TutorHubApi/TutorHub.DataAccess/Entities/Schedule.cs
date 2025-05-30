using TutorHub.DataAccess.Enums;

namespace TutorHub.DataAccess.Entities;

public class Schedule
{
    public int Id { get; set; }

    public int StudentTeacherId { get; set; }
    
    public DayOfWeek DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public ScheduleStatus Status { get; set; } = ScheduleStatus.Requested;

    public StudentTeacher StudentTeacher { get; set; }

    public ICollection<Lesson>? LessonHistories { get; set; }
}
