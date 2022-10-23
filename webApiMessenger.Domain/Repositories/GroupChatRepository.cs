using webApiMessenger.Domain.Entities;

namespace webApiMessenger.Domain.Repositories;

public class GroupChatRepository
{
    private static List<GroupChat> _groupChats = new();

    static GroupChatRepository()
    {
        
    }
}