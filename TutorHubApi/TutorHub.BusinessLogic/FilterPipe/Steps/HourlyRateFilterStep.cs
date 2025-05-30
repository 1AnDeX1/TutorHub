using TutorHub.DataAccess.Entities;

namespace TutorHub.BusinessLogic.FilterPipe.Steps;

public class HourlyRateFilterStep : ITeacherFilterStep
{
    public IQueryable<Teacher> Apply(IQueryable<Teacher> query, TeacherFilter filter)
    {
        if (filter.MinHourlyRate.HasValue)
        {
            query = query.Where(t => t.HourlyRate >= filter.MinHourlyRate.Value);
        }

        if (filter.MaxHourlyRate.HasValue)
        {
            query = query.Where(t => t.HourlyRate <= filter.MaxHourlyRate.Value);
        }

        return query;
    }
}
