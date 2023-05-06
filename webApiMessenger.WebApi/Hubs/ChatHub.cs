using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using webApiMessenger.Application.services;
using webApiMessenger.Domain.Entities;
using webApiMessenger.Shared.DTOs;

namespace webApiMessenger.WebApi.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly MessengerService _messengerService;
    private readonly GroupService _groupService;

    public ChatHub(ILogger<ChatHub> logger, MessengerService messengerService, GroupService groupService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _messengerService = messengerService ?? throw new ArgumentNullException(nameof(messengerService));
        _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
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

    public async Task GetAllGroupChats()
    {
        var allGroupChats = await _groupService.GetGroupChats();
        var groupChatDTOs = allGroupChats.Adapt<List<GroupChatDTO>>();
        await Clients.Caller.SendAsync("ReceiveAllGroupChats", groupChatDTOs);
    }

    public async Task GetMyGroupChats()
    {
        var userId = Context.User.Claims.FirstOrDefault(claim => claim.Type == "Id").Value;
        var userGroupChats = await _groupService.GetGroupChats(int.Parse(userId));
        var groupChatDTOs = userGroupChats.Adapt<List<GroupChatDTO>>();
        await Clients.Caller.SendAsync("ReceiveMyGroupChats", groupChatDTOs);
    }
}