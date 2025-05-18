using AutoMapper;
using TutorHub.BusinessLogic.Models.Schedules;
using TutorHub.BusinessLogic.Models.StudentTeacher;
using TutorHub.BusinessLogic.Models.StudentTeachers;
using TutorHub.BusinessLogic.Models.User;
using TutorHub.BusinessLogic.Models.User.Student;
using TutorHub.BusinessLogic.Models.User.Teacher;
using TutorHub.DataAccess.Entities;

namespace TutorHub.BusinessLogic.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //User
            CreateMap<RegistrationModel, User>().ReverseMap();

            //Teacher
            CreateMap<Teacher, TeacherModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null ? src.User.Email : string.Empty))
                .ReverseMap();
            CreateMap<TeacherCreateModel, Teacher>().ReverseMap();
            CreateMap<TeacherCreateModel, RegistrationModel>().ReverseMap();
            CreateMap<TeacherCreateModel, User>().ReverseMap();

            //Student
            CreateMap<Student, StudentModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null ? src.User.Email : string.Empty))
                .ReverseMap();
            CreateMap<StudentCreateModel, RegistrationModel>().ReverseMap();
            CreateMap<StudentCreateModel, User>().ReverseMap();
            CreateMap<StudentCreateModel, Student>().ReverseMap();

            //TeacherAvailability
            CreateMap<TeacherAvailabilityModel, TeacherAvailability>().ReverseMap();
            CreateMap<TeacherAvailabilityRequest, TeacherAvailability>().ReverseMap();
            CreateMap<UpdateAvailabilityRequest, TeacherAvailability>().ReverseMap();

            //StudentTeacher
            CreateMap<StudentTeacherRequestModel, StudentTeacher>().ReverseMap();
            CreateMap<StudentTeacherSimpleModel, StudentTeacher>().ReverseMap();

            //Lesson
            CreateMap<Schedule, ScheduleSimpleModel>().ReverseMap();
            CreateMap<Schedule, ScheduleModel>().ReverseMap();
            CreateMap<Schedule, ScheduleCreateModel>().ReverseMap();
        }
    }
}
