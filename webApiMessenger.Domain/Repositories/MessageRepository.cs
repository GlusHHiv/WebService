using webApiMessenger.Domain.Entities;

namespace webApiMessenger.Domain.Repositories;

public class MessageRepository
{
    private static List<Message> _messages = new();

    static MessageRepository()
    {
        
    }
}