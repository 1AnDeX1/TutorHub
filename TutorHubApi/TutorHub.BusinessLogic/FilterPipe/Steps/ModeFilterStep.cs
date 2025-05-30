using TutorHub.DataAccess.Entities;

namespace TutorHub.BusinessLogic.FilterPipe.Steps;

public class ModeFilterStep : ITeacherFilterStep
{
    public IQueryable<Teacher> Apply(IQueryable<Teacher> query, TeacherFilter filter)
    {
        if (filter.Online == true && filter.Offline == false)
        {
            query = query.Where(t => t.Online || (t.Online && t.Offline));
        }

        if (filter.Online == false && filter.Offline == true)
        {
            query = query.Where(t => t.Offline || (t.Online && t.Offline) );
        }

        return query;
    }
}
