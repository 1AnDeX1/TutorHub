using AutoMapper;
using TutorHub.BusinessLogic.Exceptions;
using TutorHub.BusinessLogic.IServices;
using TutorHub.BusinessLogic.Models.Schedules;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.BusinessLogic.Service
{
    public class TeacherAvailabilityService : ITeacherAvailabilityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeacherAvailabilityService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TeacherAvailabilityModel>> GetByTeacherIdAsync(int teacherId)
        {
            var teacher = await _unitOfWork.TeacherAvailabilities.GetByTeacherIdAsync(teacherId);

            return _mapper.Map<IEnumerable<TeacherAvailabilityModel>>(teacher);
        }

        public async Task AddAsync(int teacherId, TeacherAvailabilityRequest request)
        {
            if (!await _unitOfWork.TeacherAvailabilities.IsSlotAvailableAsync(teacherId, request.DayOfWeek, request.StartTime, request.EndTime))
            {
                var availability = _mapper.Map<TeacherAvailability>(request);
                availability.TeacherId = teacherId;

                await _unitOfWork.TeacherAvailabilities.AddAsync(availability);
                await _unitOfWork.SaveAsync();
            }
            else
                throw new ValidationException("This time slot is already occupied."); 
        }

        public async Task UpdateAsync(int id, UpdateAvailabilityRequest request)
        {
            var availability = await _unitOfWork.TeacherAvailabilities.GetAvailabilityByIdAsync(id)
                ?? throw new NotFoundException("Availability not found.");

            if (await _unitOfWork.TeacherAvailabilities
                .IsSlotAvailableForUpdateAsync(availability.TeacherId, request.DayOfWeek, request.StartTime, request.EndTime, id))
            {
                var updatedAvailability = _mapper.Map<TeacherAvailability>(request);

                updatedAvailability.Id = availability.Id;
                updatedAvailability.TeacherId = availability.TeacherId;

                _unitOfWork.TeacherAvailabilities.Update(updatedAvailability);
                await _unitOfWork.SaveAsync();
            }
            else 
                throw new ValidationException("This time slot is already occupied.");
        }

        public async Task RemoveAsync(int id)
        {
            _ = await _unitOfWork.TeacherAvailabilities.GetAvailabilityByIdAsync(id)
                ?? throw new NotFoundException($"Availability with Id {id} not found.");

            await _unitOfWork.TeacherAvailabilities.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
