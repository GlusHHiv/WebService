using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webApiMessenger.Application.services;
using webApiMessenger.Shared.DTOs;

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
    public void CreateGroup(int user2id)
    {
        var user1id = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type == "Id").Value);
        _groupService.CreateGroupChat(user1id, user2id);
    }
    
    [HttpPost]
    public async Task AddMember(int groupChatid, int user1id)
    {
        await _groupService.AddMember(groupChatid, user1id);
    }
    
    [HttpDelete]
    public async Task DeleteMember(int groupChatId, int removeUserId)
    {
        await _groupService.DeleteMember(groupChatId, removeUserId);
    }

    [HttpDelete]
    public async Task DeleteMeFormChat(int groupChatId)
    {
        var removeUserId = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type == "Id").Value);
        await _groupService.DeleteMember(groupChatId, removeUserId);
    }

    [HttpGet]
    public async Task<IEnumerable<GroupChatDTO>> GetGroupChats()
    {
        var groupChats = await _groupService.GetGroupChats();
        
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