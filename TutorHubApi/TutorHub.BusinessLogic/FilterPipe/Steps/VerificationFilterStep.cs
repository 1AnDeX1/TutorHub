using TutorHub.DataAccess.Entities;

namespace TutorHub.BusinessLogic.FilterPipe.Steps;

public class VerificationFilterStep : ITeacherFilterStep
{
    public IQueryable<Teacher> Apply(IQueryable<Teacher> query, TeacherFilter filter)
    {
        if (filter.VerificationStatus.HasValue)
        {
            query = query.Where(t => t.VerificationStatus == filter.VerificationStatus.Value);
        }

        return query;
    }
}
