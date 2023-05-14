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

    public async Task CreateGroupChat(int user1id, List<int> usersId)
    {
        await _groupChatRepository.AddGroupChat(user1id, usersId);
    }
    
    public async Task<int> CreateDialogue(int user1id, int user2id)
    {
        return await _groupChatRepository.AddDialogue(user1id, user2id);
    }

    public async Task AddMember(int groupChatid, int user1id)
    {
        await _groupChatRepository.AddMember(groupChatid, user1id);
    }
    
    public async Task DeleteMember(int groupChatId, int removeUserId)
    {
        await _groupChatRepository.DeleteMemberAndTryDeleteGroupChat(groupChatId, removeUserId);
    }
    
    public async Task<IEnumerable<GroupChat>> GetGroupChats(int? userId = null)
    {
        return await _groupChatRepository.GetGroupChats(userId);
    }

    public async Task<bool> GroupChatContainUser(int groupChatId, int userId)
    {
        return await _groupChatRepository.GroupChatContainUser(groupChatId, userId);
    }
}