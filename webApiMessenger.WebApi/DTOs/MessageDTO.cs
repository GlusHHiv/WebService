namespace webApiMessenger.WebApi.DTOs;

public class MessageDTO
{
    public int Id { get; set; }
    public string SenderNick { get; set; }
    public string Text { get; set; }
    public DateTime SendDateTime { get; set; }
}