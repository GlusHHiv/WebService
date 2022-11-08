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
    
    public IEnumerable<Message> GetMessagesFromGroupChat(int groupChatId)
    {
        return _dbContext.Messages.Include(m => m.Sender).Where(m => m.GroupChat.Id == groupChatId).ToList();
    }
}