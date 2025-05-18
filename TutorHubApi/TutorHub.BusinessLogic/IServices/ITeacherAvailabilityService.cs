using TutorHub.BusinessLogic.Models.Schedules;

namespace TutorHub.BusinessLogic.IServices
{
    public interface ITeacherAvailabilityService
    {
        Task<IEnumerable<TeacherAvailabilityModel>> GetByTeacherIdAsync(int teacherId);
        Task AddAsync(int teacherId, TeacherAvailabilityRequest request);
        Task UpdateAsync(int id, UpdateAvailabilityRequest request);
        Task RemoveAsync(int id);
    }
}
