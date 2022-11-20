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

    public void SendMessage(int groupChatId, int senderId, string messageText)
    {
        _messageRepository.AddMessage(groupChatId, senderId, messageText);
    }

    public IEnumerable<Message> GetOldMessagesFromGroupChat(int groupChatId, int userId)
    {
        return _messageRepository.GetOldMessagesFromGroupChat(groupChatId, userId);
    }
    
    public IEnumerable<Message> GetNewMessagesFromGroupChat(int groupChatId, int userId)
    {
        return _messageRepository.GetNewMessagesFromGroupChat(groupChatId, userId);
    }
}