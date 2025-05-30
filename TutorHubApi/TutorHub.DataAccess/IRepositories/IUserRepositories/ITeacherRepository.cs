using TutorHub.DataAccess.Entities;

namespace TutorHub.DataAccess.IRepositories.IUserRepositories;

public interface ITeacherRepository
{
    Task<(IEnumerable<Teacher> teachers, int teachersCount)> GetAllAsync(int page, int pageSize);

    IQueryable<Teacher> GetAvailableTeachers();

    Task<(IEnumerable<Teacher> teachers, int teachersCount)> GetAllByNameAsync(string name, int page, int pageSize);

    Task<(IEnumerable<Teacher> teachers, int teachersCount)> GetWithPendingVerificationRequests(int page, int pageSize);

    Task<(IEnumerable<Teacher> teachers, int teachersCount)> GetWithPendingVerificationRequestsByName(string name, int page, int pageSize);

    Task<Teacher?> GetByIdAsync(int id);

    Task<Teacher> AddAsync(Teacher teacher);

    Teacher Update(Teacher teacher);

    Task DeleteAsync(int id);
}
