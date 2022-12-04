using webApiMessenger.Domain.Entities;
using webApiMessenger.Persistence.Repositories;

namespace webApiMessenger.Application.services;

public class MessengerService
{
    private readonly MessageRepository _messageRepository;

    public MessengerService(MessageRepository messageRepository)
    {
        _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
    }

    public async Task SendMessage(int groupChatId, int senderId, string messageText)
    {
        await _messageRepository.AddMessage(groupChatId, senderId, messageText);
    }

    public async Task<IEnumerable<Message>> GetOldMessagesFromGroupChat(int groupChatId, int userId)
    {
        return await _messageRepository.GetOldMessagesFromGroupChat(groupChatId, userId);
    }
    
    public async  Task<IEnumerable<Message>> GetNewMessagesFromGroupChat(int groupChatId, int userId)
    {
        return  await _messageRepository.GetNewMessagesFromGroupChat(groupChatId, userId);
    }
}