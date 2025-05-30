using TutorHub.BusinessLogic.Models.User.Teacher;

namespace TutorHub.BusinessLogic.Models.User.Teachers;

public class PagedTeacherResult
{
    public IEnumerable<TeacherModel> Teachers { get; set; }

    public int TotalPages { get; set; }

    public int CurrentPage { get; set; }
}
