namespace TutorHub.BusinessLogic.Models.Schedules;

public class ScheduleModel
{
    public int Id { get; set; }

    public int StudentTeacherId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }
}
