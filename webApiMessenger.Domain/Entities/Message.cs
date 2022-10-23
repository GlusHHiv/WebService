namespace webApiMessenger.Domain.Entities;

/// <summary>
/// Сообщение  
/// </summary>
public class Message : BaseEntity
{
    /// <summary>
    /// Время отправки сообщения
    /// </summary>
    public DateTime SendDateTime { get; set; }
    
    /// <summary>
    /// Прочитано или нет?
    /// </summary>
    public bool IsRead { get; set; }
    
    /// <summary>
    /// Пользователь который отправил сообщение
    /// </summary>
    public User Sender { get; set; }
    
    /// <summary>
    /// В каком чате было отправленно сообщение
    /// </summary>
    public GroupChat GroupChat { get; set; }
}