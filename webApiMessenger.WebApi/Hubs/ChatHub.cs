using Microsoft.AspNetCore.SignalR;
using webApiMessenger.Application.services;

namespace webApiMessenger.WebApi.Hubs;
// TODO: авторизация
public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly MessengerService _messengerService;

    public ChatHub(ILogger<ChatHub> logger, MessengerService messengerService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _messengerService = messengerService ?? throw new ArgumentNullException(nameof(messengerService));
    }

    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("Новое подключение");
        return base.OnConnectedAsync();
    }

    public async Task SendMessage(string groupChatId, string message)
    {
        await Clients.OthersInGroup(groupChatId).SendAsync("ReceiveMessage", message);
        _logger.LogInformation("Отправили сообщение {message} в группу {groupChatId}", message, groupChatId);
    }

    public async Task ReceiveMessage(string groupChatId, List<string> messages)
    {
        
    }

    public async Task Join(string groupChatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupChatId);
        await Clients.OthersInGroup(groupChatId).SendAsync("ReceiveMessage", "Новый пользователь в группе");
        _logger.LogInformation("Добавили в группу {groupChatId}", groupChatId);
    }

    public async Task SayHello(string name)
    {
        _logger.LogInformation("SayHello invoked");
        var message = $"Hello {name}";
        await Clients.Caller.SendAsync("ReceiveSayHello", message);
    }
}