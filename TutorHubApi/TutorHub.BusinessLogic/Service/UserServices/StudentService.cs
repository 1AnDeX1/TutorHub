using AutoMapper;
using TutorHub.BusinessLogic.Exceptions;
using TutorHub.BusinessLogic.IServices.IUserServices;
using TutorHub.BusinessLogic.Models.User;
using TutorHub.BusinessLogic.Models.User.Student;
using TutorHub.BusinessLogic.Validations;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.BusinessLogic.Service.UserServices;

public class StudentService(
    IUnitOfWork unitOfWork,
    IUserService userService,
    IAuthService authService,
    IValidator validator,
    IMapper mapper) : IStudentService
{
    public async Task<(IEnumerable<StudentModel> students, int studentsCount)> GetAllAsync(string? name, int page, int pageSize)
    {
        var studentsObject = string.IsNullOrEmpty(name)
            ? await unitOfWork.Students.GetAllAsync(page, pageSize)
            : await unitOfWork.Students.GetAllByNameAsync(name, page, pageSize);

        return (mapper.Map<IEnumerable<StudentModel>>(studentsObject.students), studentsObject.studentsCount);
    }

    public async Task<StudentModel?> GetByIdAsync(int id)
    {
        var student = await unitOfWork.Students.GetByIdAsync(id)
            ?? throw new NotFoundException($"Student with ID {id} not found.");

        return mapper.Map<StudentModel>(student);
    }

    public async Task<StudentModel> CreateAsync(StudentCreateModel studentCreateModel)
    {
        validator.ValidateStudentCreateModel(studentCreateModel);

        var userModel = mapper.Map<RegistrationModel>(studentCreateModel);

        var (status, token, userId) = await authService.Registration(userModel, UserRoles.Student);
        if (status == 0)
        {
            throw new ValidationException(token);
        }

        var student = mapper.Map<Student>(studentCreateModel);

        student.UserId = userId;

        var createdStudent = await unitOfWork.Students.AddAsync(student);

        await unitOfWork.SaveAsync();

        return mapper.Map<StudentModel>(createdStudent);
    }

    public async Task<StudentModel> UpdateAsync(int id, StudentCreateModel studentCreateModel)
    {
        var existingStudent = await unitOfWork.Students.GetByIdAsync(id)
            ?? throw new NotFoundException($"Student with ID {id} not found.");

        validator.ValidateStudentCreateModel(studentCreateModel);

        //Update user
        await userService.UpdateUserAsync(existingStudent.UserId, studentCreateModel);

        //Update student
        var updatedStudent = mapper.Map(studentCreateModel, existingStudent);

        updatedStudent.Id = id;

        unitOfWork.Students.Update(updatedStudent);

        await unitOfWork.SaveAsync();

        return mapper.Map<StudentModel>(updatedStudent);
    }

    public async Task DeleteAsync(int id)
    {
        var student = await unitOfWork.Students.GetByIdAsync(id)
            ?? throw new NotFoundException($"Student with ID {id} not found.");

        var studentTeacherIds = await unitOfWork.StudentTeachers.GetStudentTeacherIdsByStudentId(id);

        foreach (var studentTeacherId in studentTeacherIds)
        {
            await unitOfWork.StudentTeachers.DeleteWithSchedulesAsync(studentTeacherId);
        }

        await unitOfWork.Chats.DeleteByStudentIdAsync(id);
        await userService.DeleteUserAsync(student.UserId);
        await unitOfWork.TeacherRatings.DeleteByStudentIdAsync(id);
        await unitOfWork.Students.DeleteAsync(id);

        await unitOfWork.SaveAsync();
    }
}
