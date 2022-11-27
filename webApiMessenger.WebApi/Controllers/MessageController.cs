using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webApiMessenger.Application.services;
using webApiMessenger.WebApi.DTOs;

namespace webApiMessenger.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class MessageController : Controller
{
    private readonly MessengerService _messengerService;
    private readonly GroupService _groupService;

    public MessageController(MessengerService messengerService, GroupService groupService)
    {
        _messengerService = messengerService ?? throw new ArgumentNullException(nameof(messengerService));
        _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
    }

    
    [HttpGet]
    public ActionResult<IEnumerable<GetMessagesDTO>> GetMessages(int groupChatId)
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type == "Id").Value);
        if (!_groupService.GroupChatContainUser(groupChatId, userId))
            return Forbid("Вы не можете получить сообщения группы в которой не состоите");
        
        var oldMessages = _messengerService.GetOldMessagesFromGroupChat(groupChatId, userId);
        var newMessages = _messengerService.GetNewMessagesFromGroupChat(groupChatId, userId);

        return Ok(new GetMessagesDTO
        {
            OldMessages = oldMessages.Adapt<IEnumerable<MessageDTO>>(),
            NewMessages = newMessages.Adapt<IEnumerable<MessageDTO>>()
        });

        // Пример как можно написать конфиг в текущем месте 
        // messages.Adapt<IEnumerable<MessageDTO>>(
        //     new TypeAdapterConfig()
        //         .NewConfig<Message, MessageDTO>()
        //         .Map(m => m.SenderNick, source => source.Sender.Nick)
        //         .Config);
    }
    
    [HttpPost]
    public void SendMessage([FromBody] SendMessageDTO sendMessageDto)
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
        _messengerService.SendMessage(sendMessageDto.GroupChatId, userId, sendMessageDto.Text);
    }
}