using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorHub.BusinessLogic.Models.User;
using TutorHub.BusinessLogic.Models.User.Teacher;

namespace TutorHub.BusinessLogic.IServices.IUserServices
{
    public interface IAdminService
    {
        Task<(IEnumerable<TeacherModel> teachers, int teachersCount)> GetTeachersWithPendingVerificationRequest(string? name, int page, int pageSize);

        Task CreateAsync(RegistrationModel adminCreateModel);

        Task ApproveVerificationRequest(int teacherId);

        Task RejectVerificationRequest(int teacherId);
    }
}
