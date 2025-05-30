using TutorHub.DataAccess.Entities;

namespace TutorHub.BusinessLogic.FilterPipe.Steps;

public class PaginationFilterStep : ITeacherFilterStep
{
    public IQueryable<Teacher> Apply(IQueryable<Teacher> query, TeacherFilter filter)
    {
        if (filter.PageSize == int.MaxValue)
        {
            return query;
        }

        int skip = (filter.Page - 1) * filter.PageSize;

        return query.Skip(skip).Take(filter.PageSize);
    }
}
