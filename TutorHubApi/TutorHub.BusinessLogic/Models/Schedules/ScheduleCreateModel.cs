namespace TutorHub.BusinessLogic.Models.Schedules;

public class ScheduleCreateModel
{
    public int TeacherId { get; set; }

    public int StudentId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }
}
