namespace webApiMessenger.WebApi.DTOs;

public class GroupChatDTO
{
    public int Id { get; set; }
    public List<MemberGroupChatDTO> Members { get; set; }
}

public class MemberGroupChatDTO
{
    public int Id { get; set; }
    public string Nick { get; set; }
    public string Email { get; set; }
}