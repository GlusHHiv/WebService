namespace webApiMessenger.Domain.Entities;

/// <summary>
/// Сообщение  
/// </summary>
public class Message : BaseEntity
{
    /// <summary>
    /// Время отправки сообщения
    /// </summary>
    public DateTime SendDateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// Пользователь который отправил сообщение
    /// </summary>
    public User Sender { get; set; }
    
    /// <summary>
    /// В каком чате было отправленно сообщение
    /// </summary>
    public GroupChat GroupChat { get; set; }

    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Text { get; set; }
    
    public List<User> ReadedBy { get; set; }
}