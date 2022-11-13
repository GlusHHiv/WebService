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

    public MessageController(MessengerService messengerService)
    {
        _messengerService = messengerService ?? throw new ArgumentNullException(nameof(messengerService));
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<MessageDTO>> GetMessages(int groupChatId)
    {
        var messages = _messengerService.GetMessagesFromGroupChat(groupChatId);
        var dto = messages.Adapt<IEnumerable<MessageDTO>>();
        // Если пользователь не состоит в групп чате, отдаем Forbid()
        // return Forbid();
        
        
        // Пример как можно написать конфиг в текущем месте 
        // messages.Adapt<IEnumerable<MessageDTO>>(
        //     new TypeAdapterConfig()
        //         .NewConfig<Message, MessageDTO>()
        //         .Map(m => m.SenderNick, source => source.Sender.Nick)
        //         .Config
        //     );
        return Ok(dto);
    }
    
    [HttpPost]
    public void SendMessage([FromBody] SendMessageDTO sendMessageDto)
    {
        _messengerService.SendMessage(sendMessageDto.GroupChatId, sendMessageDto.SenderId, sendMessageDto.Text);
    }
}