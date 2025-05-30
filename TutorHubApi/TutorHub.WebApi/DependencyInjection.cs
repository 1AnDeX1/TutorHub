using TutorHub.BusinessLogic.Service.UserServices;
using TutorHub.DataAccess.IRepositories.IUserRepositories;
using TutorHub.DataAccess.Repositories.UserRepositories;
using TutorHub.DataAccess.Repositories;
using TutorHub.BusinessLogic.IServices;
using TutorHub.BusinessLogic.Service;
using TutorHub.DataAccess.IRepositories;
using TutorHub.BusinessLogic.IServices.IUserServices;
using TutorHub.BusinessLogic.Validations;

namespace TutorHub.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStudentTeacherRepository, StudentTeacherRepository>();
        services.AddScoped<ITeacherAvailabilityRepository, TeacherAvailabilityRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<ITeacherRatingRepository, TeacherRatingRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IChatMessageRepository, ChatMessageRepository>();

        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITeacherService, TeacherService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IStudentTeacherService, StudentTeacherService>();
        services.AddScoped<ITeacherAvailabilityService, TeacherAvailabilityService>();
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IChatService, ChatService>();

        services.AddScoped<IValidator, Validator>();

        return services;
    }
}
