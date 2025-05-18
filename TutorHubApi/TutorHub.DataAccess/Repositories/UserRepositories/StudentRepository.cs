using Microsoft.EntityFrameworkCore;
using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories.UserInterfaces;

namespace TutorHub.DataAccess.Repositories.UserRepositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Student> students, int studentsCount)> GetAllAsync(int page, int pageSize)
        {
            var students = await _context.Students
                .AsNoTracking()
                .Include(t => t.User)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var studentsCount = await _context.Students.CountAsync();

            return (students, studentsCount);
        }

        public async Task<(IEnumerable<Student> students, int studentsCount)> GetAllByNameAsync(string name, int page, int pageSize)
        {
            var students = await _context.Students
                .AsNoTracking()
                .Include(t => t.User)
                .Where(t => t.User!.UserName!.Contains(name))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var studentsCount = await _context.Students
                .Where(t => t.User!.UserName!.Contains(name))
                .CountAsync();

            return (students, studentsCount);
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Student> AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            return student;
        }

        public Student Update(Student student)
        {
            _context.Students.Update(student);
            return student;
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(t => t.Id == id);

            if (student != null)
            {
                _context.Students.Remove(student);
            }
        }
    }
}
