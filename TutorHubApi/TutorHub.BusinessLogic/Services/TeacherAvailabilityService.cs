using AutoMapper;
using TutorHub.BusinessLogic.Exceptions;
using TutorHub.BusinessLogic.IServices;
using TutorHub.BusinessLogic.Models.Schedules;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.BusinessLogic.Service;

public class TeacherAvailabilityService(
    IUnitOfWork unitOfWork,
    IMapper mapper) : ITeacherAvailabilityService
{
    public async Task<IEnumerable<TeacherAvailabilityModel>> GetByTeacherIdAsync(int teacherId)
    {
        var teacher = await unitOfWork.TeacherAvailabilities.GetByTeacherIdAsync(teacherId);

        return mapper.Map<IEnumerable<TeacherAvailabilityModel>>(teacher);
    }

    public async Task AddAsync(int teacherId, TeacherAvailabilityRequest request)
    {
        if (!await unitOfWork.TeacherAvailabilities.IsSlotAvailableAsync(teacherId, request.DayOfWeek, request.StartTime, request.EndTime) &&
            !await unitOfWork.Schedules.IsScheduleSlotTakenAsync(teacherId, request.DayOfWeek, request.StartTime, request.EndTime))
        {
            var availability = mapper.Map<TeacherAvailability>(request);
            availability.TeacherId = teacherId;

            await unitOfWork.TeacherAvailabilities.AddAsync(availability);
            await unitOfWork.SaveAsync();
        }
        else
            throw new ValidationException("This time slot is already occupied."); 
    }

    public async Task UpdateAsync(int id, UpdateAvailabilityRequest request)
    {
        var availability = await unitOfWork.TeacherAvailabilities.GetAvailabilityByIdAsync(id)
            ?? throw new NotFoundException("Availability not found.");

        if (await unitOfWork.TeacherAvailabilities
            .IsSlotAvailableForUpdateAsync(availability.TeacherId, request.DayOfWeek, request.StartTime, request.EndTime, id) &&
            !await unitOfWork.Schedules.IsScheduleSlotTakenAsync(availability.TeacherId, request.DayOfWeek, request.StartTime, request.EndTime))
        {
            var updatedAvailability = mapper.Map<TeacherAvailability>(request);

            updatedAvailability.Id = availability.Id;
            updatedAvailability.TeacherId = availability.TeacherId;

            unitOfWork.TeacherAvailabilities.Update(updatedAvailability);
            await unitOfWork.SaveAsync();
        }
        else 
            throw new ValidationException("This time slot is already occupied.");
    }

    public async Task RemoveAsync(int id)
    {
        _ = await unitOfWork.TeacherAvailabilities.GetAvailabilityByIdAsync(id)
            ?? throw new NotFoundException($"Availability with Id {id} not found.");

        await unitOfWork.TeacherAvailabilities.DeleteAsync(id);
        await unitOfWork.SaveAsync();
    }
}
