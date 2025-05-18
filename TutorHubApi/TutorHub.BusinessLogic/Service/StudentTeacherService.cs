using AutoMapper;
using TutorHub.BusinessLogic.Exceptions;
using TutorHub.BusinessLogic.IServices;
using TutorHub.BusinessLogic.Models.Schedules;
using TutorHub.BusinessLogic.Models.StudentTeacher;
using TutorHub.BusinessLogic.Models.StudentTeachers;
using TutorHub.BusinessLogic.Models.User.Student;
using TutorHub.BusinessLogic.Models.User.Teacher;
using TutorHub.BusinessLogic.Validations;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.Enums;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.BusinessLogic.Service
{
    public class StudentTeacherService(
        IUnitOfWork unitOfWork,
        IValidator validator,
        IMapper mapper) : IStudentTeacherService
    {
        public async Task<IEnumerable<StudentTeacherSimpleModel>> GetAllAsync()
        {
            var studentTeachers = await unitOfWork.StudentTeachers.GetAllAsync();

            return mapper.Map<IEnumerable<StudentTeacherSimpleModel>>(studentTeachers);
        }

        public async Task<IEnumerable<TeacherModel>> GetTeachersOfStudentAsync(int studentId, StudentTeacherStatus status)
        {
            var teachers = await unitOfWork.StudentTeachers.GetTeachersByStudentIdAsync(studentId, status);
            return mapper.Map<IEnumerable<TeacherModel>>(teachers);
        }

        public async Task<IEnumerable<StudentModel>> GetStudentsOfTeacherAsync(int teacherId, StudentTeacherStatus status)
        {
            var students = await unitOfWork.StudentTeachers.GetStudentsByTeacherIdAsync(teacherId, status);
            return mapper.Map<IEnumerable<StudentModel>>(students);
        }

        public async Task<IEnumerable<ScheduleSimpleModel>> GetSchedulesAsync(int studentTeacherId)
        {
            var schedules = await unitOfWork.Schedules.GetByStudentTeacherIdAsync(studentTeacherId);
            return mapper.Map<IEnumerable<ScheduleSimpleModel>>(schedules);
        }

        public async Task RequestStudentToTeacher(StudentTeacherRequestModel request)
        {
            await validator.ValidateStudentTeacherRequestModel(request);

            var newstudentTeacher = mapper.Map<StudentTeacher>(request);
            newstudentTeacher.Status = StudentTeacherStatus.Pending;

            // All schedules will already be mapped and attached to the studentTeacher
            foreach (var schedule in newstudentTeacher.Schedules!)
            {
                schedule.Status = ScheduleStatus.Requested;
            }

            await unitOfWork.StudentTeachers.AddAsync(newstudentTeacher);
            await unitOfWork.SaveAsync();
        }


        public async Task ApproveRequestAsync(int teacherId, int studentId)
        {
            var studentTeacher = await unitOfWork.StudentTeachers.GetByTeacherAndStudentAsync(teacherId, studentId)
                ?? throw new NotFoundException("Cannot find this request");

            await validator.ValidateAvailabilityForApprove(studentTeacher);

            studentTeacher.Status = StudentTeacherStatus.Confirmed;

            unitOfWork.StudentTeachers.Update(studentTeacher);

            var schedules = studentTeacher.Schedules!;

            foreach (var schedule in schedules)
            {
                schedule.Status = ScheduleStatus.Confirmed;
                unitOfWork.Schedules.Update(schedule);

                // Find overlapping availability
                var availabilities = await unitOfWork.TeacherAvailabilities.GetAvailabilitiesAsync(
                    studentTeacher.TeacherId, schedule.DayOfWeek, schedule.StartTime, schedule.EndTime);

                foreach (var availability in availabilities)
                {
                    await unitOfWork.TeacherAvailabilities.DeleteAsync(availability.Id);
                }
            }

            await unitOfWork.SaveAsync();
        }


        public async Task RejectRequestAsync(int teacherId, int studentId)
        {
            var studentTeacher = await unitOfWork.StudentTeachers.GetByTeacherAndStudentAsync(teacherId, studentId)
                ?? throw new NotFoundException("Cannot find this request");

            if (studentTeacher.Status != StudentTeacherStatus.Pending)
                throw new ValidationException("This connection is already confirmed or rejected");

            studentTeacher.Status = StudentTeacherStatus.Rejected;            

            foreach (var schedule in studentTeacher.Schedules)
            {
                await unitOfWork.Schedules.DeleteAsync(schedule.Id);
            }

            unitOfWork.StudentTeachers.Update(studentTeacher);

            await unitOfWork.SaveAsync();
        }

        public async Task UpdateStatusAsync(int studentTeacherId, StudentTeacherStatus status)
        {
            var studentTeacher = await unitOfWork.StudentTeachers.GetByIdAsync(studentTeacherId)
                ?? throw new NotFoundException("No such request");

            studentTeacher.Status = status;

            unitOfWork.StudentTeachers.Update(studentTeacher);

            await unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int teacherId, int studentId)
        {
            var studentTeacher = await unitOfWork.StudentTeachers.GetByTeacherAndStudentAsync(teacherId, studentId)
                ?? throw new NotFoundException($"This connection wasn't found.");

            await unitOfWork.StudentTeachers.DeleteWithSchedulesAsync(studentTeacher.Id);

            await unitOfWork.SaveAsync();
        }
    }
}
