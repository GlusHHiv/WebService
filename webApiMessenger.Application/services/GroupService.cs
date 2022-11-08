using webApiMessenger.Domain;
using webApiMessenger.Domain.Entities;
using webApiMessenger.Persistence.Repositories;

namespace webApiMessenger.Application.services;

public class GroupService
{
    private GroupChatRepository _groupChatRepository;

    public GroupService(GroupChatRepository groupChatRepository)
    {
        _groupChatRepository = groupChatRepository;
    }

    public void CreateGroupChat(int user1id, int user2id)
    {
        _groupChatRepository.AddGroupChat(user1id, user2id);
    }

    public void AddMember(int groupChatid, int user1id)
    {
        _groupChatRepository.AddMember(groupChatid, user1id);
    }
    
    public void DeleteMember(int groupChatId, int removeUserId)
    {
        _groupChatRepository.DeleteMemberAndTryDeleteGroupChat(groupChatId, removeUserId);
    }
    
    public IEnumerable<GroupChat> GetGroupChats()
    {
        return _groupChatRepository.GetGroupChats();
    }
}