using System.Net.Http.Json;
using webApiMessenger.Shared.DTOs;

namespace webApiMessenger.BalzorClient.Services;

public class UserService
{
    private HttpClientFactory _clientFactory;

    public UserService(HttpClientFactory httpClientFactory)
    {
        _clientFactory = httpClientFactory;
    }
    
    public async Task<UserPublicDTO?> Me()
    {
        var client = await _clientFactory.CreateHttpClientAsync();
        return await client.GetFromJsonAsync<UserPublicDTO>("User/Me");
    }

    public async Task<ICollection<UserWithoutFriendsDTO>?> GetFriends()
    {
        var client = await _clientFactory.CreateHttpClientAsync();
        return await client.GetFromJsonAsync<ICollection<UserWithoutFriendsDTO>>("User/GetFriends");
    }
    
    public async Task<ICollection<UserWithoutFriendsDTO>?> GetUsers()
    {
        var client = await _clientFactory.CreateHttpClientAsync();
        return await client.GetFromJsonAsync<ICollection<UserWithoutFriendsDTO>>("User/GetUsers");
    }
    
    public async Task AddFriend(int id)
    {
        var client = await _clientFactory.CreateHttpClientAsync();
        var response = await client.PostAsJsonAsync("User/AddFriend", new AddFriendDTO
        {
            UserId = id
        });
        response.EnsureSuccessStatusCode();
    }
    
    public async Task<int> CreateDialogue(int id)
    {
        
        var client = await _clientFactory.CreateHttpClientAsync();
        var response =  await client.PostAsJsonAsync("GroupChat/CreateDialogue", new AddFriendDTO
        {
            UserId = id
        });
        response.EnsureSuccessStatusCode();
        var strId = await response.Content.ReadAsStringAsync();
        return int.Parse(strId);
        
    }

    public async Task CreateGroupChat(IEnumerable<int> userIds)
    {
        var client = await _clientFactory.CreateHttpClientAsync();
        var response = await client.PostAsJsonAsync("GroupChat/CreateGroup", userIds);
        response.EnsureSuccessStatusCode();
    }
}