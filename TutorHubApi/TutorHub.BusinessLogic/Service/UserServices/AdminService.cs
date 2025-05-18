using AutoMapper;
using TutorHub.BusinessLogic.Exceptions;
using TutorHub.BusinessLogic.IServices.IUserServices;
using TutorHub.BusinessLogic.Models.User;
using TutorHub.BusinessLogic.Models.User.Student;
using TutorHub.BusinessLogic.Models.User.Teacher;
using TutorHub.BusinessLogic.Validations;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.Enums;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.BusinessLogic.Service.UserServices
{
    public class AdminService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IValidator validator,
        IMapper mapper) : IAdminService
    {
        public async Task<(IEnumerable<TeacherModel> teachers, int teachersCount)> GetTeachersWithPendingVerificationRequest(string? name, int page, int pageSize)
        {
            var teachersObject = string.IsNullOrEmpty(name)
                ? await unitOfWork.Teachers.GetWithPendingVerificationRequests(page, pageSize)
                : await unitOfWork.Teachers.GetWithPendingVerificationRequestsByName(name, page, pageSize);

            return (mapper.Map<IEnumerable<TeacherModel>>(teachersObject.teachers), teachersObject.teachersCount);
        }

        public async Task CreateAsync(RegistrationModel adminCreateModel)
        {
            validator.ValidateAdminCreateModel(adminCreateModel);

            var userModel = mapper.Map<RegistrationModel>(adminCreateModel);

            var (status, token, userId) = await authService.Registration(userModel, UserRoles.Admin);
            if (status == 0)
            {
                throw new ValidationException(token);
            }

            await unitOfWork.SaveAsync();
        }

        public async Task ApproveVerificationRequest(int teacherId) 
        {
            var teacher = await unitOfWork.Teachers.GetByIdAsync(teacherId)
                ?? throw new NotFoundException($"Teacher with ID {teacherId} not found.");

            teacher.VerificationStatus = VerificationStatus.Approved;

            unitOfWork.Teachers.Update(teacher);
            await unitOfWork.SaveAsync();
        }

        public async Task RejectVerificationRequest(int teacherId)
        {
            var teacher = await unitOfWork.Teachers.GetByIdAsync(teacherId)
                ?? throw new NotFoundException($"Teacher with ID {teacherId} not found.");

            teacher.VerificationStatus = VerificationStatus.Rejected;

            unitOfWork.Teachers.Update(teacher);
            await unitOfWork.SaveAsync();
        }
    }
}
