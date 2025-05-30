using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.Enums;

namespace TutorHub.DataAccess.IRepositories;

public interface IStudentTeacherRepository
{
    Task<IEnumerable<StudentTeacher>> GetAllAsync();

    Task<IEnumerable<Teacher>> GetTeachersByStudentIdAsync(int studentId, StudentTeacherStatus status);

    Task<IEnumerable<Student>> GetStudentsByTeacherIdAsync(int teacherId, StudentTeacherStatus status);

    Task<StudentTeacher?> GetByStudentAndTeacherIdAsync(int studentId, int teacherId, bool checkForApprove);

    Task<StudentTeacher?> GetByIdAsync(int studentTeacherId);

    Task<StudentTeacher?> GetByTeacherAndStudentAsync(int teacherId, int studentId);

    Task<List<int>> GetStudentTeacherIdsByTeacherId(int id);

    Task<List<int>> GetStudentTeacherIdsByStudentId(int id);

    Task<StudentTeacher> AddAsync(StudentTeacher studentTeacher);

    StudentTeacher Update(StudentTeacher studentTeacher);

    Task DeleteWithSchedulesAsync(int studentTeacherId);
}
