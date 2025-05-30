using TutorHub.DataAccess.Enums;

namespace TutorHub.BusinessLogic.FilterPipe;

public class TeacherFilter
{
    public List<Subject>? Subjects { get; set; }

    public decimal? MinHourlyRate { get; set; }

    public decimal? MaxHourlyRate { get; set; }

    public bool? Online { get; set; }

    public bool? Offline { get; set; }

    public int? MinAge { get; set; }

    public int? MaxAge { get; set; }

    public int? MinRating { get; set; }

    public VerificationStatus? VerificationStatus { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
