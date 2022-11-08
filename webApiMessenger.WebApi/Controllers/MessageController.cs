using Mapster;
using Microsoft.AspNetCore.Mvc;
using webApiMessenger.Application.services;
using webApiMessenger.Domain.Entities;
using webApiMessenger.WebApi.DTOs;

namespace webApiMessenger.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MessageController : Controller
{
    private readonly MessengerService _messengerService;

    public MessageController(MessengerService messengerService)
    {
        _messengerService = messengerService ?? throw new ArgumentNullException(nameof(messengerService));
    }
    
    [HttpGet]
    public IEnumerable<MessageDTO> GetMessages(int groupChatId)
    {
        var messages = _messengerService.GetMessagesFromGroupChat(groupChatId);
        var dto = messages.Adapt<IEnumerable<MessageDTO>>();
        
        // Пример как можно написать конфиг в текущем месте 
        // messages.Adapt<IEnumerable<MessageDTO>>(
        //     new TypeAdapterConfig()
        //         .NewConfig<Message, MessageDTO>()
        //         .Map(m => m.SenderNick, source => source.Sender.Nick)
        //         .Config
        //     );
        return dto;
    }
    
    [HttpPost]
    public void SendMessage([FromBody] SendMessageDTO sendMessageDto)
    {
        _messengerService.SendMessage(sendMessageDto.GroupChatId, sendMessageDto.SenderId, sendMessageDto.Text);
    }
}