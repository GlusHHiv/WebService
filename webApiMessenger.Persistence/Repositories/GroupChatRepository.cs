using Microsoft.EntityFrameworkCore;
using webApiMessenger.Domain;
using webApiMessenger.Domain.Entities;

namespace webApiMessenger.Persistence.Repositories;

public class GroupChatRepository
{
    private readonly IDbContext _dbContext;

    public GroupChatRepository(IDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task AddGroupChat(int user1id, int user2id)
    {
        var findUser1 = await _dbContext.Users.Include(user => user.Friends).FirstOrDefaultAsync(u => u.Id == user1id);
        var findUser2 = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user2id);
        
        if (findUser1 == null) throw new ArgumentException($"Пользователь с id {user1id} не существует!");
        if (findUser2 == null) throw new ArgumentException($"Пользователь с id {user2id} не существует!");
        if (findUser1.Friends.FirstOrDefault(f => f.Id == user2id) == null) 
            throw new ArgumentException($"Пользователь с id {user2id} не является вашим другом!");
        var groupChat = new GroupChat();
        groupChat.Members = new List<User> {findUser1, findUser2};
        await _dbContext.GroupChats.AddAsync(groupChat);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveGroupChat(int groupChatId)
    {
        var groupChat = await _dbContext.GroupChats.FirstOrDefaultAsync(g => g.Id == groupChatId);
        if (groupChat == null) throw new ArgumentException($"Группа с id {groupChatId} не существует!");

        _dbContext.GroupChats.Remove(groupChat);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddMember(int groupChatid, int user1id)
    {
        var findGroup = await _dbContext.GroupChats.Include(groupChat => groupChat.Members).FirstOrDefaultAsync(g => g.Id == groupChatid);
        var findUser1 = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user1id);
        
        if (findGroup == null) throw new ArgumentException($"Группа с id {groupChatid} не существует!");
        if (findUser1 == null) throw new ArgumentException($"Пользователь с id {user1id} не существует!");

        findGroup.Members.Add(findUser1);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteMemberAndTryDeleteGroupChat(int groupChatId, int removeUserId)
    {
        var findGroup = await _dbContext.GroupChats.Include(groupChat => groupChat.Members).FirstOrDefaultAsync(g => g.Id == groupChatId);
        if (findGroup == null) throw new ArgumentException($"Группа с id {groupChatId} не существует!");
        
        var removeUser =  findGroup.Members.FirstOrDefault(u => u.Id == removeUserId);
        if (removeUser == null) throw new ArgumentException($"Пользователь с id {removeUserId} не является участником группы!");
        
        findGroup.Members.Remove(removeUser);
        await _dbContext.SaveChangesAsync();

        if (findGroup.Members.Count == 0)
        {
            await RemoveGroupChat(findGroup.Id);
        }
    }

    public async Task <List<GroupChat>> GetGroupChats()
    {
        return await _dbContext.GroupChats.Include(groupChat => groupChat.Members).AsNoTracking().ToListAsync();
    }

    public async Task<bool>  GroupChatContainUser(int groupChatId, int userId)
    {
        var groupChat = await _dbContext.GroupChats.Include(g => g.Members).FirstOrDefaultAsync(g => g.Id == groupChatId);
        if (groupChat == null) throw new ArgumentException($"Группа с id {groupChatId} не существует!");
        return groupChat.Members.Exists(u => u.Id == userId);
    }
}