using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorHub.DataAccess.Entities;

namespace TutorHub.DataAccess.IRepositories
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedule>?> GetAllAsync();

        Task<Schedule?> GetByIdAsync(int id);

        Task<IEnumerable<Schedule>> GetByStudentTeacherIdAsync(int studentTeacherId);

        Task<IEnumerable<Schedule>> GetByTeacherIdAsync(int teacherId);

        Task<IEnumerable<Schedule>> GetByStudentIdAsync(int studentId);

        Task<Schedule> AddAsync(Schedule schedule);

        Schedule Update(Schedule schedule);

        Task DeleteAsync(int id);

        Task<bool> IsScheduleSlotTakenAsync(int teacherId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime, int? excludeScheduleId = null);
    }
}
