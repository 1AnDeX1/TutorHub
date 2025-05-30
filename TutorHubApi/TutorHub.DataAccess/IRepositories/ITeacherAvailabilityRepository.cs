using TutorHub.DataAccess.Entities;

namespace TutorHub.DataAccess.IRepositories;

public interface ITeacherAvailabilityRepository
{
    Task<IEnumerable<TeacherAvailability>> GetByTeacherIdAsync(int teacherId);

    Task<TeacherAvailability?> GetAvailabilityByIdAsync(int availabilityId);

    Task<IEnumerable<TeacherAvailability>> GetAvailabilitiesAsync(
        int teacherId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime);

    Task AddAsync(TeacherAvailability availability);

    void Update(TeacherAvailability availability);

    Task DeleteAsync(int availabilityId);

    Task<bool> IsSlotAvailableAsync(int teacherId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime);

    Task<bool> IsSlotAvailableForUpdateAsync(int teacherId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime, int excludeAvailabilityId);
}
