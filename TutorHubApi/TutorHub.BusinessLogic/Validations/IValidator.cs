
using TutorHub.BusinessLogic.Models.StudentTeacher;
using TutorHub.BusinessLogic.Models.User;
using TutorHub.BusinessLogic.Models.User.Student;
using TutorHub.BusinessLogic.Models.User.Teacher;
using TutorHub.DataAccess.Entities;

namespace TutorHub.BusinessLogic.Validations;
public interface IValidator
{
    void ValidateStudentCreateModel(StudentCreateModel studentCreateModel);

    void ValidateTeacherCreateModel(TeacherCreateModel teacherCreateModel);

    void ValidateAdminCreateModel(RegistrationModel studentCreateModel);

    Task ValidateStudentTeacherRequestModel(StudentTeacherRequestModel request);

    Task ValidateAvailabilityForApprove(StudentTeacher studentTeacher);

    Task ValidateScheduleCreateUpdate(Schedule schedule);
}
