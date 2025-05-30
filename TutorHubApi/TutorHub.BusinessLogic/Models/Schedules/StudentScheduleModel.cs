
namespace TutorHub.BusinessLogic.Models.Schedules;

public class StudentScheduleModel
{
    public int Id { get; set; }

    public int StudentTeacherId { get; set; }

    public int TeacherId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }
}
