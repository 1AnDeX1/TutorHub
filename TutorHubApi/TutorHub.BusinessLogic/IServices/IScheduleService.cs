using TutorHub.BusinessLogic.Models.Schedules;

namespace TutorHub.BusinessLogic.IServices;

public interface IScheduleService
{
    Task<IEnumerable<ScheduleModel>> GetAllAsync();

    Task<ScheduleModel> GetByIdAsync(int id);

    Task<IEnumerable<ScheduleModel>> GetByStudentTeacherIdAsync(int id);

    Task<IEnumerable<TeacherScheduleModel>> GetByTeacherIdAsync(int teacherId);

    Task<IEnumerable<StudentScheduleModel>> GetByStudentIdAsync(int studentId);

    Task<ScheduleModel> CreateAsync(ScheduleCreateModel scheduleCreateModel);

    Task<ScheduleModel> UpdateAsync(ScheduleModel scheduleModel);

    Task DeleteAsync(int id);
}
