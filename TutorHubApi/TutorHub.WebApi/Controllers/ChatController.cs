using Microsoft.AspNetCore.Mvc;
using TutorHub.BusinessLogic.IServices;
using TutorHub.BusinessLogic.Models.Chat;

namespace TutorHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController(IChatService chatService): ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChatModel>>> GetAllChats()
    {
        var chats = await chatService.GetAllAsync();
        return Ok(chats);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetChatById(int id)
    {
        var chat = await chatService.GetByIdAsync(id);

        if (chat == null)
            return NotFound();

        return Ok(chat);
    }
}
