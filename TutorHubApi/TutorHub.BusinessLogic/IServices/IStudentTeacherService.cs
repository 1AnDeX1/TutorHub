using TutorHub.BusinessLogic.Models.Schedules;
using TutorHub.BusinessLogic.Models.StudentTeacher;
using TutorHub.BusinessLogic.Models.StudentTeachers;
using TutorHub.BusinessLogic.Models.User.Student;
using TutorHub.BusinessLogic.Models.User.Teacher;
using TutorHub.DataAccess.Enums;

namespace TutorHub.BusinessLogic.IServices
{
    public interface IStudentTeacherService
    {
        Task<IEnumerable<StudentTeacherSimpleModel>> GetAllAsync();

        Task<IEnumerable<TeacherModel>> GetTeachersOfStudentAsync(int studentId, StudentTeacherStatus status);

        Task<IEnumerable<StudentModel>> GetStudentsOfTeacherAsync(int teacherId, StudentTeacherStatus status);

        Task<IEnumerable<ScheduleSimpleModel>> GetSchedulesAsync(int studentTeacherId);

        Task RequestStudentToTeacher(StudentTeacherRequestModel request);

        Task ApproveRequestAsync(int teacherId, int studentId);

        Task RejectRequestAsync(int teacherId, int studentId);

        Task DeleteAsync(int teacherId, int studentId);
    }
}
