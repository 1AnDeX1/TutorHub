using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories.UserInterfaces;

namespace TutorHub.DataAccess.Repositories.UserRepositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext _context;

        public TeacherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Teacher> teachers, int teachersCount)> GetAllAsync(int page, int pageSize)
        {
            var query = _context.Teachers
                .AsNoTracking()
                .Include(t => t.User);

            var teachersCount = await query.CountAsync();

            var teachers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (teachers, teachersCount);
        }


        public async Task<(IEnumerable<Teacher> teachers, int teachersCount)> GetAvailableTeachersAsync(int page, int pageSize)
        {
            var query = _context.Teachers
                .AsNoTracking()
                .Include(t => t.User)
                .Include(t => t.TeacherAvailabilities)
                .Where(t => t.TeacherAvailabilities.Any());

            var teachersCount = await query.CountAsync();

            var teachers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (teachers, teachersCount);
        }

        public async Task<(IEnumerable<Teacher> teachers, int teachersCount)> GetAllByNameAsync(string name, int page, int pageSize)
        {
            var query = _context.Teachers
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

        public async Task<(IEnumerable<Teacher> teachers, int teachersCount)> GetAvailableTeachersByNameAsync(string name, int page, int pageSize)
        {
            var query = _context.Teachers
                .AsNoTracking()
                .Include(t => t.User)
                .Include(t => t.TeacherAvailabilities)
                .Where(t => t.User!.UserName!.Contains(name))
                .Where(t => t.TeacherAvailabilities.Any());

            var teachersCount = await query.CountAsync();

            var teachers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (teachers, teachersCount);
        }


        public async Task<(IEnumerable<Teacher> teachers, int teachersCount)> GetWithPendingVerificationRequests(int page, int pageSize)
        {
            var query = _context.Teachers
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
            var query = _context.Teachers
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
            return await _context.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Teacher> AddAsync(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
            return teacher;
        }

        public Teacher Update(Teacher teacher)
        {
            _context.Teachers.Update(teacher);
            return teacher;
        }

        public async Task DeleteAsync(int id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);

            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
            }
        }
    }
}
