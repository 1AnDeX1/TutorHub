using Microsoft.EntityFrameworkCore;
using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.DataAccess.Repositories;

public class TeacherAvailabilityRepository(ApplicationDbContext context) : ITeacherAvailabilityRepository
{
    public async Task<IEnumerable<TeacherAvailability>> GetByTeacherIdAsync(int teacherId)
    {
        return await context.TeacherAvailabilities
            .AsNoTracking()
            .Where(a => a.TeacherId == teacherId)
            .ToListAsync();
    }

    public async Task<TeacherAvailability?> GetAvailabilityByIdAsync(int availabilityId)
    {
        return await context.TeacherAvailabilities.AsNoTracking().FirstOrDefaultAsync(a => a.Id == availabilityId);
    }

    public async Task<IEnumerable<TeacherAvailability>> GetAvailabilitiesAsync(
        int teacherId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime)
    {
        return await context.TeacherAvailabilities
            .Where(a =>
                a.TeacherId == teacherId &&
                a.DayOfWeek == day &&
                startTime < a.EndTime &&
                endTime > a.StartTime)
            .ToListAsync();
    }

    public async Task AddAsync(TeacherAvailability availability)
    {
        await context.TeacherAvailabilities.AddAsync(availability);
    }

    public void Update(TeacherAvailability availability)
    {
        context.TeacherAvailabilities.Update(availability);
    }

    public async Task DeleteAsync(int availabilityId)
    {
        var availability = await context.TeacherAvailabilities.FindAsync(availabilityId);
        if (availability != null)
        {
            context.TeacherAvailabilities.Remove(availability);
        }
    }

    public async Task<bool> IsSlotAvailableAsync(int teacherId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime)
    {
        return await context.TeacherAvailabilities
            .AsNoTracking()
            .AnyAsync(a =>
                a.TeacherId == teacherId &&
                a.DayOfWeek == day &&
                startTime < a.EndTime &&
                endTime > a.StartTime);
    }

    public async Task<bool> IsSlotAvailableForUpdateAsync(int teacherId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime, int excludeAvailabilityId)
    {
        return !await context.TeacherAvailabilities
            .AsNoTracking()
            .Where(a => a.TeacherId == teacherId && a.DayOfWeek == day)
            .Where(a => a.Id != excludeAvailabilityId)
            .AnyAsync(a => startTime < a.EndTime && endTime > a.StartTime);
    }
}
