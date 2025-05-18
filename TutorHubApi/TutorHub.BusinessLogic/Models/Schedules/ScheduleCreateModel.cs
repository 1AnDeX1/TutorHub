namespace TutorHub.BusinessLogic.Models.Schedules
{
    public class ScheduleCreateModel
    {
        public int StudentTeacherId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
    }
}
