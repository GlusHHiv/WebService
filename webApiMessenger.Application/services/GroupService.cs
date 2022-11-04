using webApiMessenger.Domain;
using webApiMessenger.Domain.Entities;
using webApiMessenger.Domain.Repositories;

namespace webApiMessenger.Application.services;

public class GroupService
{
    private GroupChatRepository _groupChatRepository;

    public GroupService(IDbContext dbContext)
    {
        _groupChatRepository = new GroupChatRepository(dbContext);
    }

    public void CreateGroupChat(int user1id, int user2id)
    {
        
        _groupChatRepository.AddGroupChat(user1id, user2id);
    }

    public void AddMember(int groupChatid, int user1id)
    {
        _groupChatRepository.AddMember(groupChatid, user1id);
    }
    
    public IEnumerable<GroupChat> GetGroupChats()
    {
        return _groupChatRepository.GetGroupChats();
    }
}