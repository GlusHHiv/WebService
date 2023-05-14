namespace webApiMessenger.Domain.Entities;

/// <summary>
/// Пользователь
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Ник пользователя
    /// </summary>
    public string Nick { get; set; }
    
    /// <summary>
    /// Адресс эл. почты
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Логин
    /// </summary>
    public string Login { get; set; }
    
    /// <summary>
    /// Не хэшированный пароль
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Возраст
    /// </summary>
    public int Age { get; set; }
    
    /// <summary>
    /// Друзья пользователя
    /// </summary>
    public List<User> Friends { get; set; }

    /// <summary>
    /// Дата и время регистрации
    /// </summary>
    public DateTime RegisterDateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// Групп чаты в которых участвует пользователь
    /// </summary>
    public List<GroupChat> GroupChats { get; set; }

    /// <summary>
    /// Диалоги в которых участвует пользователь
    /// </summary>
    public List<GroupChat> Dialogues { get; set; }

    /// <summary>
    /// Последнее прочитанное сообщения в группе
    /// </summary>
    public List<Message> LastReadMessagesInGroupChat { get; set; }

    public List<Message> SendMessages { get; set; }
}