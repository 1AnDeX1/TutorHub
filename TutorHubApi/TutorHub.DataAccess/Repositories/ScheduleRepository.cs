using Microsoft.EntityFrameworkCore;
using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.DataAccess.Repositories;

public class ScheduleRepository(ApplicationDbContext context) : IScheduleRepository
{
    public async Task<IEnumerable<Schedule>?> GetAllAsync()
    {
        return await context.Schedules.AsNoTracking().ToListAsync();
    }

    public async Task<Schedule?> GetByIdAsync(int id)
    {
        var lesson = await context.Schedules
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);

        return lesson;
    }

    public async Task<IEnumerable<Schedule>> GetByStudentTeacherIdAsync(int studentTeacherId)
    {
        var lessons = await context.Schedules
            .AsNoTracking()
            .Where(l => l.StudentTeacherId == studentTeacherId)
            .ToListAsync();

        return lessons;
    }

    public async Task<IEnumerable<Schedule>> GetByTeacherIdAsync(int teacherId)
    {
        var lessons = await context.Schedules
            .AsNoTracking()
            .Include(s => s.StudentTeacher)
            .Where(s => s.StudentTeacher.TeacherId == teacherId)
            .OrderBy(s => s.StartTime)
            .ToListAsync();

        return lessons;
    }

    public async Task<IEnumerable<Schedule>> GetByStudentIdAsync(int studentId)
    {
        var lessons = await context.Schedules
            .AsNoTracking()
            .Include(s => s.StudentTeacher)
            .Where(s => s.StudentTeacher.StudentId == studentId)
            .OrderBy(s => s.StartTime)
            .ToListAsync();

        return lessons;
    }

    public async Task<Schedule> AddAsync(Schedule schedule)
    {
        await context.Schedules.AddAsync(schedule);
        return schedule;
    }

    public Schedule Update(Schedule schedule)
    {
        context.Schedules.Update(schedule);

        return schedule;
    }

    public async Task DeleteAsync(int id)
    {
        var lesson = await context.Schedules.FirstOrDefaultAsync(l => l.Id == id);

        if (lesson != null)
        {
            context.Schedules.Remove(lesson);
        }
    }

    public async Task<bool> IsScheduleSlotTakenAsync(int teacherId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime, int? excludeLessonId = null)
    {
        return await context.Schedules
            .Where(l => l.StudentTeacher.TeacherId == teacherId && l.DayOfWeek == day)
            .Where(l => excludeLessonId == null || l.Id != excludeLessonId)
            .AnyAsync(l =>
                (startTime >= l.StartTime && startTime < l.EndTime) ||
                (endTime > l.StartTime && endTime <= l.EndTime) ||
                (startTime <= l.StartTime && endTime >= l.EndTime)
            );
    }
}
