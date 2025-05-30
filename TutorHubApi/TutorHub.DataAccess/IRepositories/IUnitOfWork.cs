using TutorHub.DataAccess.IRepositories.IUserRepositories;
using TutorHub.DataAccess.IRepositories.IUserRepositories;

namespace TutorHub.DataAccess.IRepositories;
public interface IUnitOfWork
{
    IStudentRepository Students { get; }

    ITeacherRepository Teachers { get; }

    IUserRepository Users { get; }

    IScheduleRepository Schedules { get; }

    IStudentTeacherRepository StudentTeachers { get; }

    ITeacherAvailabilityRepository TeacherAvailabilities { get; }

    ITeacherRatingRepository TeacherRatings { get; }

    IChatRepository Chats { get; }

    IChatMessageRepository ChatMessages { get; }

    Task SaveAsync();
}
