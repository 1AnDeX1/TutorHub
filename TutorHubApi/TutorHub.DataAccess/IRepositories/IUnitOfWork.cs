using TutorHub.DataAccess.IRepositories;
using TutorHub.DataAccess.IRepositories.UserInterfaces;

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

    Task SaveAsync();
}
