using TutorHub.DataAccess.Entities;

namespace TutorHub.DataAccess.IRepositories;

public interface ITeacherRatingRepository
{
    Task<TeacherRating?> GetByStudentAndTeacherAsync(int studentId, int teacherId);

    Task<IEnumerable<TeacherRating>> GetByTeacherIdAsync(int teacherId);

    Task AddAsync(TeacherRating rating);

    void Update(TeacherRating rating);

    Task DeleteByTeacherIdAsync(int teacherId);

    Task DeleteByStudentIdAsync(int studentId);
}
