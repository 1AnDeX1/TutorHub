namespace TutorHub.BusinessLogic.Models.Schedules
{
    public class TeacherAvailabilityRequest
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
