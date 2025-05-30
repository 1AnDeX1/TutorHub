using AutoMapper;
using TutorHub.BusinessLogic.Exceptions;
using TutorHub.BusinessLogic.IServices;
using TutorHub.BusinessLogic.Models.Schedules;
using TutorHub.BusinessLogic.Validations;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.BusinessLogic.Service;
public class ScheduleService(
    IUnitOfWork unitOfWork,
    IValidator validator,
    IMapper mapper) : IScheduleService
{
    public async Task<IEnumerable<ScheduleModel>> GetAllAsync()
    {
        var schedules = await unitOfWork.Schedules.GetAllAsync();

        return mapper.Map<IEnumerable<ScheduleModel>>(schedules);
    }

    public async Task<ScheduleModel> GetByIdAsync(int id)
    {
        var schedules = await unitOfWork.Schedules.GetByIdAsync(id)
            ?? throw new NotFoundException($"Schedule with ID {id} not found");

        return mapper.Map<ScheduleModel>(schedules);
    }

    public async Task<IEnumerable<ScheduleModel>> GetByStudentTeacherIdAsync(int id)
    {
        var schedules = await unitOfWork.Schedules.GetByStudentTeacherIdAsync(id)
            ?? throw new NotFoundException($"Schedule with studentTeacherId {id} not found");

        return mapper.Map<IEnumerable<ScheduleModel>>(schedules);
    }

    public async Task<IEnumerable<TeacherScheduleModel>> GetByTeacherIdAsync(int teacherId)
    {
        var schedules = await unitOfWork.Schedules.GetByTeacherIdAsync(teacherId)
            ?? throw new NotFoundException($"Schedule with teacherId {teacherId} not found");

        var mappedSchedules = mapper.Map<IEnumerable<TeacherScheduleModel>>(schedules);

        return mappedSchedules;
    }

    public async Task<IEnumerable<StudentScheduleModel>> GetByStudentIdAsync(int studentId)
    {
        var schedules = await unitOfWork.Schedules.GetByStudentIdAsync(studentId)
            ?? throw new NotFoundException($"Schedule with studentId {studentId} not found");

        return mapper.Map<IEnumerable<StudentScheduleModel>>(schedules);
    }

    public async Task<ScheduleModel> CreateAsync(ScheduleCreateModel scheduleCreateModel)
    {
        var studentTeacher = await unitOfWork.StudentTeachers.GetByTeacherAndStudentAsync(scheduleCreateModel.TeacherId, scheduleCreateModel.StudentId)
            ?? throw new ValidationException("There is no such student");

        var schedule = mapper.Map<Schedule>(scheduleCreateModel);
        schedule.StudentTeacherId = studentTeacher.Id;

        await validator.ValidateScheduleCreateUpdate(schedule);

        var createdschedule = await unitOfWork.Schedules.AddAsync(schedule);

        await unitOfWork.SaveAsync();

        return mapper.Map<ScheduleModel>(createdschedule);
    }

    public async Task<ScheduleModel> UpdateAsync(ScheduleModel scheduleModel)
    {
        var schedule = mapper.Map<Schedule>(scheduleModel);

        await validator.ValidateScheduleCreateUpdate(schedule);

        var createdschedule = unitOfWork.Schedules.Update(schedule);

        await unitOfWork.SaveAsync();

        return mapper.Map<ScheduleModel>(createdschedule);
    }

    public async Task DeleteAsync(int id)
    {
        _ = await unitOfWork.Schedules.GetByIdAsync(id)
            ?? throw new NotFoundException("No such schedule");

        await unitOfWork.Schedules.DeleteAsync(id);
        await unitOfWork.SaveAsync();
    }
}
