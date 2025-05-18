using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorHub.BusinessLogic.Models;
using TutorHub.BusinessLogic.Models.User.Teacher;
using TutorHub.DataAccess.Entities;

namespace TutorHub.BusinessLogic.IServices.IUserServices
{
    public interface ITeacherService
    {
        Task<(IEnumerable<TeacherModel> teachers, int teachersCount)> GetAllAsync(string? name, int page, int pageSize);

        Task<(IEnumerable<TeacherModel> teachers, int teachersCount)> GetAvailableTeachersAsync(string? name, int page, int pageSize);

        Task<TeacherModel?> GetByIdAsync(int id);

        Task<TeacherModel> CreateAsync(TeacherCreateModel teacherCreateModel);

        Task<TeacherModel> UpdateAsync(int id, TeacherCreateModel teacherCreateModel);

        Task SendVerificationRequestAsync(int id);

        Task RateTeacherAsync(TeacherRatingModel teacherRatingModel);

        Task DeleteAsync(int id);
    }
}
