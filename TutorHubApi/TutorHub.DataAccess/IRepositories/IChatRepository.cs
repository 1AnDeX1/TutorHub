using TutorHub.DataAccess.Entities;

namespace TutorHub.DataAccess.IRepositories;

public interface IChatRepository
{
    Task<IEnumerable<Chat>> GetAllAsync();

    Task<Chat?> GetByIdAsync(int id);

    Task<IEnumerable<Chat>> GetAllByTeacherIdAsync(int teacherId);

    Task<IEnumerable<Chat>> GetAllByStudentIdAsync(int studentId);

    Task<Chat> AddAsync(Chat chat);

    Task DeleteByTeacherIdAsync(int teacherId);

    Task DeleteByStudentIdAsync(int teacherId);

    Task DeleteAsync(int teacherId, int studentId);
}
