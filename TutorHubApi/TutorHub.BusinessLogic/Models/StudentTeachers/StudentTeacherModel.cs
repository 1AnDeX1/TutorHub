using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.Enums;

namespace TutorHub.BusinessLogic.Models.StudentTeacher
{
    public class StudentTeacherModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
        public StudentTeacherStatus Status { get; set; }
        public required List<Schedule> Schedules { get; set; }
    }
}
