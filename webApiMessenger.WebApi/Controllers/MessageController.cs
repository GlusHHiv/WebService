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
    public ActionResult<IEnumerable<GetMessagesDTO>> GetMessages(int groupChatId)
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type == "Id").Value);
        var oldMessages = _messengerService.GetOldMessagesFromGroupChat(groupChatId, userId);
        var newMessages = _messengerService.GetNewMessagesFromGroupChat(groupChatId, userId);
        // Если пользователь не состоит в групп чате, отдаем Forbid()
        // return Forbid();

        return Ok(new GetMessagesDTO
        {
            OldDTO = oldMessages.Adapt<IEnumerable<MessageDTO>>(),
            NewDTO = newMessages.Adapt<IEnumerable<MessageDTO>>()
        });
        
        
        
        
        // Пример как можно написать конфиг в текущем месте 
        // messages.Adapt<IEnumerable<MessageDTO>>(
        //     new TypeAdapterConfig()
        //         .NewConfig<Message, MessageDTO>()
        //         .Map(m => m.SenderNick, source => source.Sender.Nick)
        //         .Config
        //     );
        
        
    }
    
    [HttpPost]
    public void SendMessage([FromBody] SendMessageDTO sendMessageDto)
    {
        _messengerService.SendMessage(sendMessageDto.GroupChatId, sendMessageDto.SenderId, sendMessageDto.Text);
    }
}