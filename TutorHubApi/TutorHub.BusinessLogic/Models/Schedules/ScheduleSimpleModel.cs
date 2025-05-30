namespace TutorHub.BusinessLogic.Models.Schedules;

public class ScheduleSimpleModel
{
    public DayOfWeek DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }
}
