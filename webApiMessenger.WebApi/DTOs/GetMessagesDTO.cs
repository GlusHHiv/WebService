namespace webApiMessenger.WebApi.DTOs;

public class GetMessagesDTO
{
    public IEnumerable<MessageDTO> OldMessages { get; set; }
    public IEnumerable<MessageDTO> NewMessages { get; set; }
}