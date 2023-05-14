using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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

    public async Task<Message> AddMessage(int groupChatId, int senderId, string messageText)
    {
        if (string.IsNullOrEmpty(messageText) || 
            string.IsNullOrWhiteSpace(messageText)) throw new ArgumentException("Текст сообщения не может быть пустым");
        
        var groupChat =  await _dbContext.GroupChats.Include(g => g.Members).FirstOrDefaultAsync(g => g.Id == groupChatId);
        if (groupChat == null) throw new ArgumentException($"Группа с id {groupChatId} не существует!");

        var sender = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == senderId);
        if (sender == null) throw new ArgumentException($"Пользователь с id {senderId} не существует!");

        if (!groupChat.Members.Exists(m => m.Id == senderId)) 
            throw new ArgumentException($"Пользователь с id {senderId} не состоит в группе с id {groupChatId}");

        var message = new Message
        {
            Sender = sender,
            GroupChat = groupChat,
            Text = messageText
        };
        await _dbContext.Messages.AddAsync(message);
        await _dbContext.SaveChangesAsync();

        return message;
    }
    
    
    public async Task<IEnumerable<Message>> GetOldMessagesFromGroupChat(int groupChatId, int memberId)
    {
        var groupChat = await _dbContext.GroupChats
            .Include(g => g.Messages).FirstOrDefaultAsync(g => g.Id == groupChatId);
        var member = await _dbContext.Users.Include(u => u.LastReadMessagesInGroupChat)
            .Include(u => u.GroupChats).FirstOrDefaultAsync(u => u.Id == memberId);
        if (groupChat == null)
            throw new ArgumentException($"Группа с id {groupChatId} не существует!");
        if (member == null)
            throw new ArgumentException($"В группе с id {groupChatId} нет gjkmpjdfntkz!");
        var lastReaded =  member.LastReadMessagesInGroupChat.FirstOrDefault(m => m.GroupChat.Id == groupChatId );


        if (groupChat.Messages.Count == 0)
            return groupChat.Messages;

        if (lastReaded == null)
        {
            return new List<Message>();
        }

        var messages = _dbContext.Messages.Include(m => m.Sender)
            .Where(m => ( m.SendDateTime <= lastReaded.SendDateTime && m.GroupChat.Id == groupChatId))
            .ToList();
        
        messages.Sort((x, y) => x.SendDateTime.CompareTo(y.SendDateTime));
        return messages;
    }
    public async Task<IEnumerable<Message>> GetNewMessagesFromGroupChat(int groupChatId, int memberId)
     {
         var groupChat = await _dbContext.GroupChats
             .Include(g => g.Messages)
             .FirstOrDefaultAsync(g => g.Id == groupChatId);
         
         var member = await _dbContext.Users
             .Include(u => u.LastReadMessagesInGroupChat)
             .FirstOrDefaultAsync(u => u.Id == memberId);
         
         var lastReaded = member.LastReadMessagesInGroupChat
             .FirstOrDefault(m => m.GroupChat.Id == groupChatId);
         
         if (groupChat == null)
             throw new ArgumentException($"Группа с id {groupChatId} не существует!");

         if (groupChat.Messages.Count == 0) 
             return groupChat.Messages;

         if (lastReaded == null)
         {
             member.LastReadMessagesInGroupChat.Add(groupChat.Messages.LastOrDefault());
             await _dbContext.SaveChangesAsync();
             return groupChat.Messages;
         }
         
         var index = member.LastReadMessagesInGroupChat.IndexOf(lastReaded);
         member.LastReadMessagesInGroupChat[index] = groupChat.Messages.LastOrDefault();
         await _dbContext.SaveChangesAsync();
         
         var messages = _dbContext.Messages.Include(m => m.Sender)
             .Where(m => DateTime.Compare(m.SendDateTime, lastReaded.SendDateTime) > 0 && m.GroupChat.Id == groupChatId)
             .ToList();
         messages.Sort((x, y) => x.SendDateTime.CompareTo(y.SendDateTime));
         return messages;
         
      }
}