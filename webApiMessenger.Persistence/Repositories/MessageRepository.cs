using Microsoft.EntityFrameworkCore;
using webApiMessenger.Domain;
using webApiMessenger.Domain.Entities;

namespace webApiMessenger.Persistence.Repositories;

public class MessageRepository
{
    private readonly IDbContext _dbContext;

    public MessageRepository(IDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public void AddMessage(int groupChatId, int senderId, string messageText)
    {
        if (string.IsNullOrEmpty(messageText) || 
            string.IsNullOrWhiteSpace(messageText)) throw new ArgumentException("Текст сообщения не может быть пустым");
        
        var groupChat = _dbContext.GroupChats.Include(g => g.Members).FirstOrDefault(g => g.Id == groupChatId);
        if (groupChat == null) throw new ArgumentException($"Группа с id {groupChatId} не существует!");

        var sender = _dbContext.Users.FirstOrDefault(u => u.Id == senderId);
        if (sender == null) throw new ArgumentException($"Пользователь с id {senderId} не существует!");

        if (!groupChat.Members.Exists(m => m.Id == senderId)) 
            throw new ArgumentException($"Пользователь с id {senderId} не состоит в группе с id {groupChatId}");
        
        _dbContext.Messages.Add(new Message
        {
            Sender = sender,
            GroupChat = groupChat,
            Text = messageText
        });
        _dbContext.SaveChanges();
    }
    
    
    public IEnumerable<Message> GetOldMessagesFromGroupChat(int groupChatId, int memberId)
    {
        var groupChat = _dbContext.GroupChats
            .Include(g => g.Messages).FirstOrDefault(g => g.Id == groupChatId);
        var member = _dbContext.Users.Include(u => u.LastReadMessagesInGroupChat)
            .FirstOrDefault(u => u.Id == memberId);
        var lastReaded = member.LastReadMessagesInGroupChat.FirstOrDefault(m => m.GroupChat.Id == groupChatId );
        if (groupChat == null)
            throw new ArgumentException($"Группа с id {groupChatId} не существует!");
        if (!groupChat.Members.Exists(m => m.Id == memberId)) 
            throw new ArgumentException($"Пользователь с id {memberId} не состоит в группе с id {groupChatId}");
        if (groupChat.Messages == null)
            throw new ArgumentException($"В группе с id {groupChatId} нет сообщений!");
        if (lastReaded == null)
        {
            member.LastReadMessagesInGroupChat.Add(groupChat.Messages.LastOrDefault());
            return null;
        }
        var index = member.LastReadMessagesInGroupChat.IndexOf(lastReaded);
        member.LastReadMessagesInGroupChat[index] = groupChat.Messages.LastOrDefault();
        var messages = _dbContext.Messages.Include(m => m.Sender)
            .Where(m => ( m.SendDateTime < lastReaded.SendDateTime && m.GroupChat.Id == groupChatId))
            .ToList();
        messages.Sort((x, y) => x.SendDateTime.CompareTo(y.SendDateTime));
        return messages;
    }
    
    public IEnumerable<Message> GetNewMessagesFromGroupChat(int groupChatId, int memberId)
         {
             var groupChat = _dbContext.GroupChats
                 .Include(g => g.Messages).FirstOrDefault(g => g.Id == groupChatId);
             var member = _dbContext.Users.Include(u => u.LastReadMessagesInGroupChat)
                 .FirstOrDefault(u => u.Id == memberId);
             var lastReaded = member.LastReadMessagesInGroupChat.FirstOrDefault(m => m.GroupChat.Id == groupChatId );
             if (groupChat == null)
                 throw new ArgumentException($"Группа с id {groupChatId} не существует!");
             if (!groupChat.Members.Exists(m => m.Id == memberId))
                 throw new ArgumentException($"Пользователь с id {memberId} не состоит в группе с id {groupChatId}");
             if (groupChat.Messages == null)
                 throw new ArgumentException($"В группе с id {groupChatId} нет сообщений!");
             if (lastReaded == null)
             {
                 member.LastReadMessagesInGroupChat.Add(groupChat.Messages.LastOrDefault());
                 return groupChat.Messages;
             }
             var index = member.LastReadMessagesInGroupChat.IndexOf(lastReaded);
             member.LastReadMessagesInGroupChat[index] = groupChat.Messages.LastOrDefault();
             var messages = _dbContext.Messages.Include(m => m.Sender)
                 .Where(m => ( m.SendDateTime > lastReaded.SendDateTime && m.GroupChat.Id == groupChatId))
                 .ToList();
             messages.Sort((x, y) => x.SendDateTime.CompareTo(y.SendDateTime));
             return messages;
             
          }
}