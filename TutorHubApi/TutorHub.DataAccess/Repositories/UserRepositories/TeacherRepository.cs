using Microsoft.EntityFrameworkCore;
using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories.IUserRepositories;

namespace TutorHub.DataAccess.Repositories.UserRepositories;

public class TeacherRepository(ApplicationDbContext context) : ITeacherRepository
{
    public async Task<(IEnumerable<Teacher> teachers, int teachersCount)> GetAllAsync(int page, int pageSize)
    {
        var query = context.Teachers
            .AsNoTracking()
            .Include(t => t.User);

        var teachersCount = await query.CountAsync();

        var teachers = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (teachers, teachersCount);
    }


    public IQueryable<Teacher> GetAvailableTeachers()
    {
        var baseQuery = context.Teachers
            .AsNoTracking()
            .Include(t => t.User)
            .Include(t => t.TeacherAvailabilities)
            .Where(t => t.TeacherAvailabilities.Any());

        return baseQuery;
    }

    public async Task<(IEnumerable<Teacher> teachers, int teachersCount)> GetAllByNameAsync(string name, int page, int pageSize)
    {
        var query = context.Teachers
            .AsNoTracking()
            .Include(t => t.User)
            .Where(t => t.User!.UserName!.Contains(name));

        var teachersCount = await query.CountAsync();

        var teachers = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (teachers, teachersCount);
    }

    public async Task<(IEnumerable<Teacher> teachers, int teachersCount)> GetWithPendingVerificationRequests(int page, int pageSize)
    {
        var query = context.Teachers
            .AsNoTracking()
            .Include(t => t.User)
            .Where(t => t.VerificationStatus == Enums.VerificationStatus.Pending);

        var teachersCount = await query.CountAsync();

        var teachers = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (teachers, teachersCount);
    }

    public async Task<(IEnumerable<Teacher> teachers, int teachersCount)> GetWithPendingVerificationRequestsByName(string name, int page, int pageSize)
    {
        var query = context.Teachers
            .AsNoTracking()
            .Include(t => t.User)
            .Where(t => t.User!.UserName!.Contains(name))
            .Where(t => t.VerificationStatus == Enums.VerificationStatus.Pending);

        var teachersCount = await query.CountAsync();

        var teachers = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (teachers, teachersCount);
    }

    public async Task<Teacher?> GetByIdAsync(int id)
    {
        return await context.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Teacher> AddAsync(Teacher teacher)
    {
        await context.Teachers.AddAsync(teacher);
        return teacher;
    }

    public Teacher Update(Teacher teacher)
    {
        context.Teachers.Update(teacher);
        return teacher;
    }

    public async Task DeleteAsync(int id)
    {
        var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.Id == id);

        if (teacher != null)
        {
            context.Teachers.Remove(teacher);
        }
    }
}
