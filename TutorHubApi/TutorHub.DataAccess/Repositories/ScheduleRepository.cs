using Microsoft.EntityFrameworkCore;
using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.DataAccess.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public ScheduleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Schedule>?> GetAllAsync()
        {
            return await _context.Schedules.AsNoTracking().ToListAsync();
        }

        public async Task<Schedule?> GetByIdAsync(int id)
        {
            var lesson = await _context.Schedules
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);

            return lesson;
        }

        public async Task<IEnumerable<Schedule>> GetByStudentTeacherIdAsync(int studentTeacherId)
        {
            var lessons = await _context.Schedules
                .AsNoTracking()
                .Where(l => l.StudentTeacherId == studentTeacherId)
                .ToListAsync();

            return lessons;
        }

        public async Task<IEnumerable<Schedule>> GetByTeacherIdAsync(int teacherId)
        {
            var lessons = await _context.Schedules
                .AsNoTracking()
                .Include(l => l.StudentTeacher)
                .Where(l => l.StudentTeacher.TeacherId == teacherId)
                .ToListAsync();

            return lessons;
        }

        public async Task<IEnumerable<Schedule>> GetByStudentIdAsync(int studentId)
        {
            var lessons = await _context.Schedules
                .AsNoTracking()
                .Include(l => l.StudentTeacher)
                .Where(l => l.StudentTeacher.StudentId == studentId)
                .ToListAsync();

            return lessons;
        }

        public async Task<Schedule> AddAsync(Schedule schedule)
        {
            await _context.Schedules.AddAsync(schedule);
            return schedule;
        }

        public Schedule Update(Schedule schedule)
        {
            _context.Schedules.Update(schedule);

            return schedule;
        }

        public async Task DeleteAsync(int id)
        {
            var lesson = await _context.Schedules.FirstOrDefaultAsync(l => l.Id == id);

            if (lesson != null)
            {
                _context.Schedules.Remove(lesson);
            }
        }

        public async Task<bool> IsScheduleSlotTakenAsync(int teacherId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime, int? excludeLessonId = null)
        {
            return await _context.Schedules
                .Where(l => l.StudentTeacher.TeacherId == teacherId && l.DayOfWeek == day)
                .Where(l => excludeLessonId == null || l.Id != excludeLessonId)
                .AnyAsync(l =>
                    (startTime >= l.StartTime && startTime < l.EndTime) ||
                    (endTime > l.StartTime && endTime <= l.EndTime) ||
                    (startTime <= l.StartTime && endTime >= l.EndTime)
                );
        }
    }
}
