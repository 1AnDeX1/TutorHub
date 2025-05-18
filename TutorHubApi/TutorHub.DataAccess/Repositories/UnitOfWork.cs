using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.IRepositories;
using TutorHub.DataAccess.IRepositories.UserInterfaces;

namespace TutorHub.DataAccess.Repositories;
public class UnitOfWork(ApplicationDbContext context,
    IStudentRepository studentRepository,
    ITeacherRepository teacherRepository,
    IUserRepository userRepository,
    IScheduleRepository scheduleRepository,
    IStudentTeacherRepository studentTeacherRepository,
    ITeacherAvailabilityRepository teacherAvailabilityRepository,
    ITeacherRatingRepository teacherRatingRepository) : IUnitOfWork

{
    public IStudentRepository Students => studentRepository;

    public ITeacherRepository Teachers => teacherRepository;

    public IUserRepository Users => userRepository;

    public IScheduleRepository Schedules => scheduleRepository;

    public IStudentTeacherRepository StudentTeachers => studentTeacherRepository;

    public ITeacherAvailabilityRepository TeacherAvailabilities => teacherAvailabilityRepository;

    public ITeacherRatingRepository TeacherRatings => teacherRatingRepository;

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}
