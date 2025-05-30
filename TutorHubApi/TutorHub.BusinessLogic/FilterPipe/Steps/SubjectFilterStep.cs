using TutorHub.DataAccess.Entities;

namespace TutorHub.BusinessLogic.FilterPipe.Steps;

public class SubjectFilterStep : ITeacherFilterStep
{
    public IQueryable<Teacher> Apply(IQueryable<Teacher> query, TeacherFilter filter)
    {
        if (filter.Subjects != null && filter.Subjects.Any())
        {
            query = query.Where(t => t.Subjects.Any(s => filter.Subjects.Contains(s)));
        }

        return query;
    }
}
