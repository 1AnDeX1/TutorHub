using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TutorHub.BusinessLogic.Hub;
using TutorHub.BusinessLogic.Models.Chat;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatMessagesController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IChatMessageRepository _messageRepository;
    private readonly IMapper _mapper;

    public ChatMessagesController(
        IHubContext<ChatHub> hubContext,
        IChatMessageRepository messageRepository,
        IMapper mapper)
    {
        _hubContext = hubContext;
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] ChatMessageModel chatMessageModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Save the message to the database
        var chatMessage = _mapper.Map<ChatMessage>(chatMessageModel);
        await _messageRepository.AddAsync(chatMessage);

        // Notify SignalR group members
        await _hubContext.Clients.Group(chatMessageModel.ChatId.ToString())
            .SendAsync("ReceiveMessage", chatMessageModel);

        return Ok();
    }

    // Fetch all messages for a specific chat
    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetMessages(int chatId)
    {
        var messages = await _messageRepository.GetMessagesByChatIdAsync(chatId);
        var result = _mapper.Map<IEnumerable<ChatMessageModel>>(messages);

        return Ok(result);
    }
}
