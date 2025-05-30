using Microsoft.EntityFrameworkCore;
using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.Enums;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.DataAccess.Repositories;

public class StudentTeacherRepository(ApplicationDbContext context) : IStudentTeacherRepository
{
    public async Task<IEnumerable<StudentTeacher>> GetAllAsync()
    {
        return await context.StudentTeachers.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Teacher>> GetTeachersByStudentIdAsync(int studentId, StudentTeacherStatus status)
    {
        return await context.StudentTeachers
            .AsNoTracking()
            .Where(st => st.StudentId == studentId)
            .Where(st => st.Status == status)
            .Include(st => st.Teacher!.User)
            .Select(st => st.Teacher!)
            .ToListAsync();
    }

    public async Task<IEnumerable<Student>> GetStudentsByTeacherIdAsync(int teacherId, StudentTeacherStatus status)
    {
        var students = await context.StudentTeachers
            .AsNoTracking()
            .Where(st => st.TeacherId == teacherId)
            .Where(st => st.Status == status)
            .Include(st => st.Student!.User)
            .Select(st => st.Student!)
            .ToListAsync();

        return students;
    }

    public async Task<StudentTeacher?> GetByStudentAndTeacherIdAsync(int studentId, int teacherId, bool checkForApprove)
    {
        StudentTeacher? studentTeacher;
        if (checkForApprove)
        {
            studentTeacher = await context.StudentTeachers
            .AsNoTracking()
            .FirstOrDefaultAsync(st => st.TeacherId == teacherId
                && st.StudentId == studentId
                && st.Status != StudentTeacherStatus.Rejected
                && st.Status != StudentTeacherStatus.Pending);
        }
        else
        {
            studentTeacher = await context.StudentTeachers
            .AsNoTracking()
            .FirstOrDefaultAsync(st => st.TeacherId == teacherId
                && st.StudentId == studentId
                && st.Status != StudentTeacherStatus.Rejected);
        }
        
        return studentTeacher;
    }

    public async Task<StudentTeacher?> GetByIdAsync(int studentTeacherId)
    {
        var studentTeacher = await context.StudentTeachers
            .AsNoTracking()
            .Include(st => st.Schedules)
            .FirstOrDefaultAsync(st => st.Id == studentTeacherId);

        return studentTeacher;
    }

    public async Task<StudentTeacher?> GetByTeacherAndStudentAsync(int teacherId, int studentId)
    {
        var studentTeacher = await context.StudentTeachers
            .AsNoTracking()
            .Include(st => st.Schedules)
            .FirstOrDefaultAsync(st => st.TeacherId == teacherId 
                && st.StudentId == studentId);

        return studentTeacher;
    }

    public async Task<List<int>> GetStudentTeacherIdsByTeacherId(int id)
    {
        var studentTeacherIds = await context.StudentTeachers
            .Where(st => st.TeacherId == id)
            .Select(st => st.Id)
            .ToListAsync();

        return studentTeacherIds;
    }

    public async Task<List<int>> GetStudentTeacherIdsByStudentId(int id)
    {
        var studentTeacherIds = await context.StudentTeachers
            .Where(st => st.StudentId == id)
            .Select(st => st.Id)
            .ToListAsync();

        return studentTeacherIds;
    }

    public async Task<StudentTeacher> AddAsync(StudentTeacher studentTeacher)
    {
        await context.StudentTeachers.AddAsync(studentTeacher);

        return studentTeacher;
    }

    public StudentTeacher Update(StudentTeacher studentTeacher)
    {
        context.StudentTeachers.Update(studentTeacher);

        return studentTeacher;
    }

    public async Task DeleteWithSchedulesAsync(int studentTeacherId)
    {
        var studentTeacher = await context.StudentTeachers
            .Include(st => st.Schedules)
            .FirstOrDefaultAsync(st => st.Id == studentTeacherId);

        if (studentTeacher != null)
        {
            if (studentTeacher.Schedules != null)
                context.Schedules.RemoveRange(studentTeacher.Schedules);

            context.StudentTeachers.Remove(studentTeacher);
        }
    }
}
