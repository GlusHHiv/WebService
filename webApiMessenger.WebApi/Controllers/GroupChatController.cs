using Microsoft.AspNetCore.Mvc;
using webApiMessenger.Application.services;
using webApiMessenger.Domain;
using webApiMessenger.Domain.Entities;
using webApiMessenger.WebApi.DTOs;

namespace webApiMessenger.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class GroupChatController : Controller
{
    private GroupService _groupService;

    public GroupChatController(IDbContext dbContext)
    {
        _groupService = new GroupService(dbContext);
    }

    [HttpPost]
    public void CreateGroup(int user1id, int user2id)
    {
        _groupService.CreateGroupChat(user1id, user2id);
    }
    
    [HttpPost]
    public void AddMember(int groupChatid, int user1id)
    {
        _groupService.AddMember(groupChatid, user1id);
    }
    
    [HttpDelete]
    public void DeleteMember(int groupChatId, int removeUserId)
    {
        _groupService.DeleteMember(groupChatId, removeUserId);
    }


    [HttpGet]
    public IEnumerable<GroupChat> GetGroupChats()
    {
        return _groupService.GetGroupChats();
    }
}