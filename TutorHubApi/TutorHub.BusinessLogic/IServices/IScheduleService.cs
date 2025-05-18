using TutorHub.BusinessLogic.Models.Schedules;

namespace TutorHub.BusinessLogic.IServices
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleModel>> GetAllAsync();

        Task<ScheduleModel> GetByIdAsync(int id);

        Task<IEnumerable<ScheduleModel>> GetByStudentTeacherIdAsync(int id);

        Task<IEnumerable<ScheduleModel>> GetByTeacherIdAsync(int teacherId);

        Task<IEnumerable<ScheduleModel>> GetByStudentIdAsync(int studentId);

        Task<ScheduleModel> CreateAsync(int teacherId, int studentId, ScheduleSimpleModel scheduleSimpleModel);

        Task<ScheduleModel> UpdateAsync(ScheduleModel scheduleModel);

        Task DeleteAsync(int id);
    }
}
