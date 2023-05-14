using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using webApiMessenger.Application.services;
using webApiMessenger.Shared.DTOs;

namespace webApiMessenger.WebApi.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly MessengerService _messengerService;
    private readonly GroupService _groupService;

    private int UserId => int.Parse(Context.User.Claims.FirstOrDefault(claim => claim.Type == "Id").Value);

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

    public async Task SendMessage(int groupChatId, string messageText)
    {
        var message = await _messengerService.SendMessage(groupChatId, UserId, messageText);
        var messageDto = message.Adapt<MessageDTO>();
        await Clients.Caller.SendAsync("ReceiveMessage", messageDto);
        await Clients.OthersInGroup(groupChatId.ToString()).SendAsync("ReceiveNewMessages", new [] { messageDto });
        _logger.LogInformation("Пользователь {UserId} отправил сообщение {message} в группу {groupChatId}", UserId, message, groupChatId);
    }

    public async Task GetOldMessages(int groupChatId)
    {
        var messages = await _messengerService.GetOldMessagesFromGroupChat(groupChatId, UserId);
        var messageDtos = messages.Adapt<IEnumerable<MessageDTO>>();
        await Clients.Caller.SendAsync("ReceiveOldMessages", messageDtos);
    }

    public async Task GetNewMessages(int groupChatId)
    {
        var messages = await _messengerService.GetNewMessagesFromGroupChat(groupChatId, UserId);
        var messageDtos = messages.Adapt<IEnumerable<MessageDTO>>();
        await Clients.Caller.SendAsync("ReceiveNewMessages", messageDtos);
    }

    public async Task Join(int groupChatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupChatId.ToString());
        await Clients.OthersInGroup(groupChatId.ToString()).SendAsync("ReceiveMessage", "Новый пользователь в группе");
        _logger.LogInformation("Добавили в группу {groupChatId}", groupChatId);

        await GetOldMessages(groupChatId);
        await GetNewMessages(groupChatId);
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
        var userGroupChats = await _groupService.GetGroupChats(UserId);
        var groupChatDTOs = userGroupChats.Adapt<List<GroupChatDTO>>();
        await Clients.Caller.SendAsync("ReceiveMyGroupChats", groupChatDTOs);
    }
}