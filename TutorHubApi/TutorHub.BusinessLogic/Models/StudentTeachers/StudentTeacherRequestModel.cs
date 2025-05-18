using TutorHub.BusinessLogic.Models.Schedules;

namespace TutorHub.BusinessLogic.Models.StudentTeacher
{
    public class StudentTeacherRequestModel
    {
        public int StudentId { get; set; }

        public int TeacherId { get; set; }

        public required List<ScheduleSimpleModel> Schedules { get; set; }
    }
}
