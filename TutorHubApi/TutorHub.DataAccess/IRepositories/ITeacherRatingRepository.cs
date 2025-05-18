using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorHub.DataAccess.Entities;

namespace TutorHub.DataAccess.IRepositories
{
    public interface ITeacherRatingRepository
    {
        Task<TeacherRating?> GetByStudentAndTeacherAsync(int studentId, int teacherId);

        Task<IEnumerable<TeacherRating>> GetByTeacherIdAsync(int teacherId);

        Task AddAsync(TeacherRating rating);

        void Update(TeacherRating rating);

        Task DeleteByTeacherIdAsync(int teacherId);

        Task DeleteByStudentIdAsync(int studentId);
    }
}
