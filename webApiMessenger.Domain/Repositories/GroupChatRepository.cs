using Microsoft.EntityFrameworkCore;
using webApiMessenger.Domain.Entities;

namespace webApiMessenger.Domain.Repositories;

public class GroupChatRepository
{

    private readonly IDbContext _dbContext;
    
    public GroupChatRepository(IDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    
    
    public void AddGroupChat(int user1id, int user2id)
    {
        var groupChat = new GroupChat();
        _dbContext.GroupChats.Add(groupChat);
        _dbContext.SaveChanges();
        groupChat = _dbContext.GroupChats.Include(groupChat => groupChat.Members).FirstOrDefault(g => g.Id == groupChat.Id);
        var findUser1 = _dbContext.Users.FirstOrDefault(u => u.Id == user1id);
        var findUser2 = _dbContext.Users.FirstOrDefault(u => u.Id == user2id);
        if (findUser1 == null )
        {
            throw new ArgumentException($"Пользователь с id {user1id} не существует!");
        }
        if (findUser2 == null )
        {
            throw new ArgumentException($"Пользователь с id {user2id} не существует!");
        }
        groupChat.Members.Add(findUser1);
        groupChat.Members.Add(findUser2);
        _dbContext.SaveChanges();
    } 
      
    public void AddMember(int groupChatid, int user1id)
    {           
        var findGroup = _dbContext.GroupChats.Include(groupChat => groupChat.Members).FirstOrDefault(g => g.Id == groupChatid);
        var findUser1 = _dbContext.Users.FirstOrDefault(u => u.Id == user1id);
        if (findGroup == null)
        {
            throw new ArgumentException($"Группа с id {groupChatid} не существует!");
        }                
        if (findUser1 == null )
        {
            throw new ArgumentException($"Пользователь с id {user1id} не существует!");
        }
            
        findGroup.Members.Add(findUser1);
        _dbContext.SaveChanges();
    } 
    
    public void DeleteMember(int groupChatId, int removeUserId)
    {
        var findGroup = _dbContext.GroupChats.Include(groupChat => groupChat.Members).FirstOrDefault(g => g.Id == groupChatId);
        var removeUser = findGroup.Members.FirstOrDefault(u => u.Id == removeUserId);
        if (findGroup == null)
        {
            throw new ArgumentException($"Группа с id {groupChatId} не существует!");
        }         
        if (removeUser == null)
        {
            throw new ArgumentException($"Пользователь с id {removeUserId} не является участником группы!");
        }        
        findGroup.Members.Remove(removeUser);
        _dbContext.SaveChanges();        
        
        
    }
    
    public List<GroupChat> GetGroupChats()
    {
        return _dbContext.GroupChats.Include(groupChat => groupChat.Members).ToList();
    }    
    
}