namespace webApiMessenger.WebApi.DTOs;

public class RegistrationUserDTO
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
}