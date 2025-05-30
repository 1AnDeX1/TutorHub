using TutorHub.DataAccess.Entities;

namespace TutorHub.BusinessLogic.FilterPipe.Steps;

public class AgeFilterStep : ITeacherFilterStep
{
    public IQueryable<Teacher> Apply(IQueryable<Teacher> query, TeacherFilter filter)
    {
        if (filter.MinAge.HasValue)
        {
            query = query.Where(t => t.Age >= filter.MinAge.Value);
        }

        if (filter.MaxAge.HasValue)
        {
            query = query.Where(t => t.Age <= filter.MaxAge.Value);
        }

        return query;
    }
}
