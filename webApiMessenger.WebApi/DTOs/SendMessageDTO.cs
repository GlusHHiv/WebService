﻿namespace webApiMessenger.WebApi.DTOs;

public class SendMessageDTO
{
    public int GroupChatId { get; set; }
    public int SenderId { get; set; }
    public string Text { get; set; }
}