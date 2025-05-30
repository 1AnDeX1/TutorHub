using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.IRepositories;
using TutorHub.DataAccess.IRepositories.IUserRepositories;

namespace TutorHub.DataAccess.Repositories;
public class UnitOfWork(ApplicationDbContext context,
    IStudentRepository studentRepository,
    ITeacherRepository teacherRepository,
    IUserRepository userRepository,
    IScheduleRepository scheduleRepository,
    IStudentTeacherRepository studentTeacherRepository,
    ITeacherAvailabilityRepository teacherAvailabilityRepository,
    ITeacherRatingRepository teacherRatingRepository,
    IChatRepository chatRepository,
    IChatMessageRepository chatMessageRepository) : IUnitOfWork

{
    public IStudentRepository Students => studentRepository;

    public ITeacherRepository Teachers => teacherRepository;

    public IUserRepository Users => userRepository;

    public IScheduleRepository Schedules => scheduleRepository;

    public IStudentTeacherRepository StudentTeachers => studentTeacherRepository;

    public ITeacherAvailabilityRepository TeacherAvailabilities => teacherAvailabilityRepository;

    public ITeacherRatingRepository TeacherRatings => teacherRatingRepository;

    public IChatRepository Chats => chatRepository;

    public IChatMessageRepository ChatMessages => chatMessageRepository;

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}
