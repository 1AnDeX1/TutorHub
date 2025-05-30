using TutorHub.BusinessLogic.Models.Chat;

namespace TutorHub.BusinessLogic.IServices;

public interface IChatService
{
    Task<IEnumerable<ChatModel>> GetAllAsync();

    Task<ChatModel> GetByIdAsync(int id);

    Task<IEnumerable<ChatModel>> GetAllByTeacherIdAsync(int teacherId);

    Task<IEnumerable<ChatModel>> GetAllByStudentIdAsync(int studentId);
}
