using TutorHub.BusinessLogic.Exceptions;
using TutorHub.BusinessLogic.Models.StudentTeacher;
using TutorHub.BusinessLogic.Models.User;
using TutorHub.BusinessLogic.Models.User.Student;
using TutorHub.BusinessLogic.Models.User.Teacher;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.Enums;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.BusinessLogic.Validations;

public class Validator(IUnitOfWork unitOfWork) : IValidator
{
    // Student
    public void ValidateStudentCreateModel(StudentCreateModel studentCreateModel)
    {
        if (string.IsNullOrWhiteSpace(studentCreateModel.UserName))
        {
            throw new ValidationException("UserName is required");
        }

        IsValidEmail(studentCreateModel.Email);
        
        if (string.IsNullOrWhiteSpace(studentCreateModel.Password))
        {
            throw new ValidationException("Password is required");
        }

        if (studentCreateModel.Age < 0)
        {
            throw new ValidationException("Age cannot be less than 0");
        }

        if (studentCreateModel.Grade != null 
            && studentCreateModel.Grade < 0
            && studentCreateModel.Grade > 11)
        {
            throw new ValidationException("Grage must be between 1 and 11");
        }
    }

    // Teacher
    public void ValidateTeacherCreateModel(TeacherCreateModel teacherCreateModel)
    {
        if (string.IsNullOrWhiteSpace(teacherCreateModel.UserName))
        {
            throw new ValidationException("User Name is required");
        }

        IsValidEmail(teacherCreateModel.Email);

        if (string.IsNullOrWhiteSpace(teacherCreateModel.Password))
        {
            throw new ValidationException("Password is required");
        }

        if (teacherCreateModel.Subjects == null)
        {
            throw new ValidationException("Subjects are required");
        }

        if (teacherCreateModel.HourlyRate < 0)
        {
            throw new ValidationException("Hourly rate must be greater than or equal to 0");
        }

        if (teacherCreateModel.Age < 0)
        {
            throw new ValidationException("Age cannot be less than 0");
        }
    }

    public void ValidateAdminCreateModel(RegistrationModel studentCreateModel)
    {
        if (string.IsNullOrWhiteSpace(studentCreateModel.UserName))
        {
            throw new ValidationException("UserName is required");
        }

        IsValidEmail(studentCreateModel.Email);

        if (string.IsNullOrWhiteSpace(studentCreateModel.Password))
        {
            throw new ValidationException("Password is required");
        }
    }

    // StudentTeacher
    public async Task ValidateStudentTeacherRequestModel(StudentTeacherRequestModel request)
    {
        _ = await unitOfWork.Students.GetByIdAsync(request.StudentId)
            ?? throw new NotFoundException($"Student with ID {request.StudentId} not found.");

        _ = await unitOfWork.Teachers.GetByIdAsync(request.TeacherId)
            ?? throw new NotFoundException($"Teacher with ID {request.TeacherId} not found.");

        var existingStudentTeacher = await unitOfWork.StudentTeachers
            .GetByStudentAndTeacherIdAsync(request.StudentId, request.TeacherId, false);

        if (existingStudentTeacher != null)
            throw new ValidationException("You already have connection to this teacher");

        if (request.Schedules == null || !request.Schedules.Any())
        {
            throw new ValidationException("At least one schedule must be provided.");
        }

        foreach (var schedule in request.Schedules)
        {
            if (schedule.StartTime >= schedule.EndTime)
            {
                throw new ValidationException($"Schedule start time must be earlier than end time: {schedule.StartTime} - {schedule.EndTime}");
            }

            var isAvailable = await unitOfWork.TeacherAvailabilities.IsSlotAvailableAsync(
                request.TeacherId, schedule.DayOfWeek, schedule.StartTime, schedule.EndTime);

            if (!isAvailable)
            {
                throw new ValidationException($"Selected time slot {schedule.StartTime} - {schedule.EndTime} on {schedule.DayOfWeek} is not available.");
            }
        }
    }

    public async Task ValidateAvailabilityForApprove(StudentTeacher studentTeacher)
    {
        if (studentTeacher.Status != StudentTeacherStatus.Pending)
            throw new ValidationException("This connection is already confirmed or rejected");

        if (studentTeacher.Schedules == null || !studentTeacher.Schedules.Any())
            throw new ValidationException("There is nothing to approve.");

        var existingStudentTeacher = await unitOfWork.StudentTeachers
            .GetByStudentAndTeacherIdAsync(studentTeacher.StudentId, studentTeacher.TeacherId, true);

        if (existingStudentTeacher != null)
            throw new ValidationException("You already have connection to this student");

        foreach (var schedule in studentTeacher.Schedules)
        {
            var hasConflict = await unitOfWork.Schedules.IsScheduleSlotTakenAsync(
                studentTeacher.TeacherId, schedule.DayOfWeek, schedule.StartTime, schedule.EndTime, schedule.Id);

            if (hasConflict)
            {
                throw new ValidationException($"Teacher already has a schedule scheduled during {schedule.StartTime} - {schedule.EndTime} on {schedule.DayOfWeek}.");
            }

            var isAvailable = await unitOfWork.TeacherAvailabilities.IsSlotAvailableAsync(
                studentTeacher.TeacherId, schedule.DayOfWeek, schedule.StartTime, schedule.EndTime);

            if (!isAvailable)
            {
                throw new ValidationException($"Selected time slot {schedule.StartTime} - {schedule.EndTime} on {schedule.DayOfWeek} is not available.");
            }
        }
    }

    // Schedule
    public async Task ValidateScheduleCreateUpdate(Schedule schedule)
    {
        var studentTeacher = await unitOfWork.StudentTeachers.GetByIdAsync(schedule.StudentTeacherId)
            ?? throw new NotFoundException($"Schedule with studentTeacherId {schedule.StudentTeacherId} not found");

        if (schedule.StartTime >= schedule.EndTime)
        {
            throw new ValidationException($"Schedule start time must be earlier than end time: {schedule.StartTime} - {schedule.EndTime}");
        }

        var hasConflict = await unitOfWork.Schedules.IsScheduleSlotTakenAsync(
        studentTeacher.TeacherId, schedule.DayOfWeek, schedule.StartTime, schedule.EndTime, schedule.Id);

        if (hasConflict)
        {
            throw new ValidationException($"Teacher already has a schedule scheduled during {schedule.StartTime} - {schedule.EndTime} on {schedule.DayOfWeek}.");
        }
    }

    private static void IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ValidationException("Email address is required");
        }

        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            throw new ValidationException("Email address must not end with a dot (.)");
        }

        try
        {
            _ = new System.Net.Mail.MailAddress(trimmedEmail);
        }
        catch
        {
            throw new ValidationException("Invalid email address format");
        }
    }
}
