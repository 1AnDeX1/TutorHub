using TutorHub.DataAccess.Entities;

namespace TutorHub.DataAccess.IRepositories.IUserRepositories;

public interface IStudentRepository
{
    Task<(IEnumerable<Student> students, int studentsCount)> GetAllAsync(int page, int pageSize);

    Task<(IEnumerable<Student> students, int studentsCount)> GetAllByNameAsync(string name, int page, int pageSize);

    Task<Student?> GetByIdAsync(int id);

    Task<Student> AddAsync(Student students);

    Student Update(Student teacstudentsher);

    Task DeleteAsync(int id);
}
