using Microsoft.EntityFrameworkCore;
using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories.IUserRepositories;

namespace TutorHub.DataAccess.Repositories.UserRepositories;

public class StudentRepository(ApplicationDbContext context) : IStudentRepository
{
    public async Task<(IEnumerable<Student> students, int studentsCount)> GetAllAsync(int page, int pageSize)
    {
        var students = await context.Students
            .AsNoTracking()
            .Include(t => t.User)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var studentsCount = await context.Students.CountAsync();

        return (students, studentsCount);
    }

    public async Task<(IEnumerable<Student> students, int studentsCount)> GetAllByNameAsync(string name, int page, int pageSize)
    {
        var students = await context.Students
            .AsNoTracking()
            .Include(t => t.User)
            .Where(t => t.User!.UserName!.Contains(name))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var studentsCount = await context.Students
            .Where(t => t.User!.UserName!.Contains(name))
            .CountAsync();

        return (students, studentsCount);
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        return await context.Students.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Student> AddAsync(Student student)
    {
        await context.Students.AddAsync(student);
        return student;
    }

    public Student Update(Student student)
    {
        context.Students.Update(student);
        return student;
    }

    public async Task DeleteAsync(int id)
    {
        var student = await context.Students.FirstOrDefaultAsync(t => t.Id == id);

        if (student != null)
        {
            context.Students.Remove(student);
        }
    }
}
