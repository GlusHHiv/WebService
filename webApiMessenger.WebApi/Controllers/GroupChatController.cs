using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webApiMessenger.Application.services;
using webApiMessenger.WebApi.DTOs;

namespace webApiMessenger.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class GroupChatController : Controller
{
    private GroupService _groupService;

    public GroupChatController(GroupService groupService)
    {
        _groupService = groupService;
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
    public IEnumerable<GroupChatDTO> GetGroupChats()
    {
        var groupChats = _groupService.GetGroupChats();
        
        // Ручками
        // var groupChatDTOs = new List<GroupChatDTO>();
        // foreach (var groupChat in groupChats)
        // {
        //     var members = groupChat.Members
        //         .Select(groupChatMember => 
        //             new MemberGroupChatDTO
        //             {
        //                 Id = groupChatMember.Id, 
        //                 Nick = groupChatMember.Nick
        //             }).ToList();
        //
        //     groupChatDTOs.Add(new GroupChatDTO
        //     {
        //         Id = groupChat.Id,
        //         Members = members
        //     });
        // }
        //var groupChatDTOs = groupChats.Select(g => g.Adapt<GroupChatDTO>());
        var groupChatDTOs = groupChats.Adapt<List<GroupChatDTO>>();
        return groupChatDTOs;
    }
}