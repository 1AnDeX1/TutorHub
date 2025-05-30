using TutorHub.BusinessLogic.FilterPipe.Steps;
using TutorHub.DataAccess.Entities;

namespace TutorHub.BusinessLogic.FilterPipe;

public class TeacherFilterPipeline
{
    private readonly List<ITeacherFilterStep> _steps;

    public TeacherFilterPipeline()
    {
        _steps = new List<ITeacherFilterStep>
        {
            new SubjectFilterStep(),
            new HourlyRateFilterStep(),
            new AgeFilterStep(),
            new ModeFilterStep(),
            new VerificationFilterStep(),
            new PaginationFilterStep()
        };
    }

    public IQueryable<Teacher> Apply(IQueryable<Teacher> query, TeacherFilter filter)
    {
        foreach (var step in _steps)
        {
            query = step.Apply(query, filter);
        }
        return query;
    }
}
