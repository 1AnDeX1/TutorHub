using Microsoft.EntityFrameworkCore;
using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.DataAccess.Repositories;

public class TeacherRatingRepository(ApplicationDbContext context) : ITeacherRatingRepository
{
    public async Task<TeacherRating?> GetByStudentAndTeacherAsync(int studentId, int teacherId)
    {
        return await context.TeacherRatings
            .FirstOrDefaultAsync(r => r.StudentId == studentId && r.TeacherId == teacherId);
    }

    public async Task<IEnumerable<TeacherRating>> GetByTeacherIdAsync(int teacherId)
    {
        return await context.TeacherRatings
            .Where(r => r.TeacherId == teacherId)
            .ToListAsync();
    }

    public async Task AddAsync(TeacherRating rating)
    {
        await context.TeacherRatings.AddAsync(rating);
    }

    public void Update(TeacherRating rating)
    {
        context.TeacherRatings.Update(rating);
    }

    public async Task DeleteByTeacherIdAsync(int teacherId)
    {
        var teacherRatings = await context.TeacherRatings
            .Where(t => t.TeacherId == teacherId)
            .ToListAsync();

        if (teacherRatings != null)
        {
            context.TeacherRatings.RemoveRange(teacherRatings);
        }
    }

    public async Task DeleteByStudentIdAsync(int studentId)
    {
        var teacherRatings = await context.TeacherRatings
            .Where(t => t.StudentId == studentId)
            .ToListAsync();

        if (teacherRatings != null)
        {
            context.TeacherRatings.RemoveRange(teacherRatings);
        }
    }
}
