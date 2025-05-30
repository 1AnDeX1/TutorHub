using AutoMapper;
using TutorHub.BusinessLogic.IServices;
using TutorHub.BusinessLogic.Models.Chat;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.BusinessLogic.Service
{
    public class ChatService(
        IUnitOfWork unitOfWork,
        IMapper mapper) : IChatService
    {
        public async Task<IEnumerable<ChatModel>> GetAllAsync()
        {
            var chats = await unitOfWork.Chats.GetAllAsync();

            return mapper.Map<IEnumerable<ChatModel>>(chats);
        }

        public async Task<ChatModel> GetByIdAsync(int id)
        {
            var chat = await unitOfWork.Chats.GetByIdAsync(id);

            return mapper.Map<ChatModel>(chat);
        }

        public async Task<IEnumerable<ChatModel>> GetAllByTeacherIdAsync(int teacherId)
        {
            var chats = await unitOfWork.Chats.GetAllByTeacherIdAsync(teacherId);

            return mapper.Map<IEnumerable<ChatModel>>(chats);
        }

        public async Task<IEnumerable<ChatModel>> GetAllByStudentIdAsync(int studentId)
        {
            var chats = await unitOfWork.Chats.GetAllByStudentIdAsync(studentId);

            return mapper.Map<IEnumerable<ChatModel>>(chats);
        }
    }
}
