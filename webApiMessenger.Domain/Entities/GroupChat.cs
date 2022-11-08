namespace webApiMessenger.Domain.Entities;

/// <summary>
/// Беседа между пользователя, в случаи двух пользователей -- приватный чат
/// </summary>
public class GroupChat : BaseEntity
{
    /// <summary>
    /// Участиники
    /// </summary>
    public List<User> Members { get; set; }

    /// <summary>
    /// Сообщения в группе
    /// </summary>
    public List<Message> Messages { get; set; }
}