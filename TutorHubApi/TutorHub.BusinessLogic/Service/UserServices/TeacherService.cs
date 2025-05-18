using AutoMapper;
using TutorHub.BusinessLogic.Exceptions;
using TutorHub.BusinessLogic.IServices.IUserServices;
using TutorHub.BusinessLogic.Models;
using TutorHub.BusinessLogic.Models.User;
using TutorHub.BusinessLogic.Models.User.Teacher;
using TutorHub.BusinessLogic.Validations;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.Enums;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.BusinessLogic.Service.UserServices;

public class TeacherService(
    IUnitOfWork unitOfWork,
    IUserService userService,
    IAuthService authService,
    IValidator validator,
    IMapper mapper) : ITeacherService
{
    public async Task<(IEnumerable<TeacherModel> teachers, int teachersCount)> GetAllAsync(string? name, int page, int pageSize)
    {
        var teachersObject = string.IsNullOrEmpty(name)
            ? await unitOfWork.Teachers.GetAllAsync(page, pageSize)
            : await unitOfWork.Teachers.GetAllByNameAsync(name, page, pageSize);

        return (mapper.Map<IEnumerable<TeacherModel>>(teachersObject.teachers), teachersObject.teachersCount);
    }

    public async Task<(IEnumerable<TeacherModel> teachers, int teachersCount)> GetAvailableTeachersAsync(string? name, int page, int pageSize)
    {
        var teachersObject = string.IsNullOrEmpty(name)
            ? await unitOfWork.Teachers.GetAvailableTeachersAsync(page, pageSize)
            : await unitOfWork.Teachers.GetAvailableTeachersByNameAsync(name, page, pageSize);

        return (mapper.Map<IEnumerable<TeacherModel>>(teachersObject.teachers), teachersObject.teachersCount);
    }

    public async Task<TeacherModel?> GetByIdAsync(int id)
    {
        var teacher = await unitOfWork.Teachers.GetByIdAsync(id)
            ?? throw new NotFoundException($"Teacher with ID {id} not found.");

        return mapper.Map<TeacherModel>(teacher);
    }

    public async Task<TeacherModel> CreateAsync(TeacherCreateModel teacherCreateModel)
    {
        validator.ValidateTeacherCreateModel(teacherCreateModel);


        //Register user
        var userModel = mapper.Map<RegistrationModel>(teacherCreateModel);

        var (status, token, userId) = await authService.Registration(userModel, UserRoles.Teacher);
        if (status == 0)
        {
            throw new ValidationException(token);
        }


        var teacher = mapper.Map<Teacher>(teacherCreateModel);

        teacher.UserId = userId;

        var createdTeacher = await unitOfWork.Teachers.AddAsync(teacher);

        await unitOfWork.SaveAsync();

        return mapper.Map<TeacherModel>(createdTeacher);
    }

    public async Task<TeacherModel> UpdateAsync(int id, TeacherCreateModel teacherCreateModel)
    {
        var existingTeacher = await unitOfWork.Teachers.GetByIdAsync(id)
            ?? throw new NotFoundException($"Teacher with ID {id} not found.");

        validator.ValidateTeacherCreateModel(teacherCreateModel);

        //Update user
        await userService.UpdateUserAsync(existingTeacher.UserId, teacherCreateModel);

        //Update teacher
        var updatedTeacher = mapper.Map(teacherCreateModel, existingTeacher);

        updatedTeacher.Id = id;

        unitOfWork.Teachers.Update(updatedTeacher);

        await unitOfWork.SaveAsync();

        return mapper.Map<TeacherModel>(updatedTeacher);
    }

    public async Task SendVerificationRequestAsync(int id)
    {
        var teacher = await unitOfWork.Teachers.GetByIdAsync(id)
            ?? throw new NotFoundException($"Teacher with ID {id} not found.");

        if (teacher.VerificationStatus == VerificationStatus.Pending)
            throw new ValidationException("Verification already requested");

        teacher.VerificationStatus = VerificationStatus.Pending;

        unitOfWork.Teachers.Update(teacher);
        await unitOfWork.SaveAsync();
    }

    public async Task RateTeacherAsync(TeacherRatingModel teacherRatingModel)
    {
        if (teacherRatingModel.Value < 1 || teacherRatingModel.Value > 5)
            throw new ValidationException("Rating must be between 1 and 5");

        var teacher = await unitOfWork.Teachers.GetByIdAsync(teacherRatingModel.TeacherId)
            ?? throw new ValidationException($"Teacher with ID {teacherRatingModel.TeacherId} not found");

        var existingRating = await unitOfWork.TeacherRatings
            .GetByStudentAndTeacherAsync(teacherRatingModel.StudentId, teacherRatingModel.TeacherId);

        if (existingRating != null)
        {
            existingRating.Value = teacherRatingModel.Value;
            unitOfWork.TeacherRatings.Update(existingRating);
        }
        else
        {
            var rating = new TeacherRating
            {
                StudentId = teacherRatingModel.StudentId,
                TeacherId = teacherRatingModel.TeacherId,
                Value = teacherRatingModel.Value
            };
            await unitOfWork.TeacherRatings.AddAsync(rating);
        }
        await unitOfWork.SaveAsync();

        var allRatings = await unitOfWork.TeacherRatings.GetByTeacherIdAsync(teacherRatingModel.TeacherId);
             
        teacher.Rating = Math.Round(allRatings.Average(r => r.Value), 2);
        unitOfWork.Teachers.Update(teacher);

        await unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var teacher = await unitOfWork.Teachers.GetByIdAsync(id)
            ?? throw new NotFoundException($"Teacher with ID {id} not found.");

        await userService.DeleteUserAsync(teacher.UserId);
        await unitOfWork.TeacherRatings.DeleteByTeacherIdAsync(id);
        await unitOfWork.Teachers.DeleteAsync(id);
        await unitOfWork.SaveAsync();
    }
}
