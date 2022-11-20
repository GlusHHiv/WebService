namespace webApiMessenger.WebApi.DTOs;

public class GetMessagesDTO
{
   
    public IEnumerable<MessageDTO> OldDTO { get; set; }
    public string Separator {get; set; } = "Новые Сообщения";
    public IEnumerable<MessageDTO> NewDTO { get; set; }
}