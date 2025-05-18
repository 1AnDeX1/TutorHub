namespace TutorHub.BusinessLogic.Models.Schedules
{
    public class UpdateAvailabilityRequest
    {
        public int TeacherId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
