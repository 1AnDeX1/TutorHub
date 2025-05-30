using TutorHub.DataAccess.Entities;

namespace TutorHub.BusinessLogic.FilterPipe;

public interface ITeacherFilterStep
{
    IQueryable<Teacher> Apply(IQueryable<Teacher> query, TeacherFilter filter);
}
