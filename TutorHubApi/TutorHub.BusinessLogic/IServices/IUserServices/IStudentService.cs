using TutorHub.BusinessLogic.Models.User.Student;

namespace TutorHub.BusinessLogic.IServices.IUserServices;

public interface IStudentService
{
    Task<(IEnumerable<StudentModel> students, int studentsCount)> GetAllAsync(string? name, int page, int pageSize);

    Task<StudentModel?> GetByIdAsync(int id);

    Task<StudentModel> CreateAsync(StudentCreateModel studentCreateModel);

    Task<StudentModel> UpdateAsync(int id, StudentCreateModel studentCreateModel);

    Task DeleteAsync(int id);
}
