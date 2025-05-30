using TutorHub.BusinessLogic.FilterPipe;
using TutorHub.BusinessLogic.Models;
using TutorHub.BusinessLogic.Models.User.Teacher;
using TutorHub.BusinessLogic.Models.User.Teachers;

namespace TutorHub.BusinessLogic.IServices.IUserServices;

public interface ITeacherService
{
    Task<(IEnumerable<TeacherModel> teachers, int teachersCount)> GetAllAsync(string? name, int page, int pageSize);

    Task<PagedTeacherResult> GetAvailableTeachersAsync(TeacherFilter filter);

    Task<PagedTeacherResult> GetBestTeachersAsync(TeacherFilter filter);

    Task<TeacherModel?> GetByIdAsync(int id);

    Task<TeacherModel> CreateAsync(TeacherCreateModel teacherCreateModel);

    Task<TeacherModel> UpdateAsync(int id, TeacherCreateModel teacherCreateModel);

    Task SendVerificationRequestAsync(int id);

    Task RateTeacherAsync(TeacherRatingModel teacherRatingModel);

    Task DeleteAsync(int id);
}
