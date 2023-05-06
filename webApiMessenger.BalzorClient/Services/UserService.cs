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

    public async Task<IEnumerable<UserWithoutFriendsDTO>?> GetFriends()
    {
        var client = await _clientFactory.CreateHttpClientAsync();
        return await client.GetFromJsonAsync<IEnumerable<UserWithoutFriendsDTO>>("User/GetFriends");
    }
    
    public async Task<IEnumerable<UserWithoutFriendsDTO>?> GetUsers()
    {
        var client = await _clientFactory.CreateHttpClientAsync();
        return await client.GetFromJsonAsync<IEnumerable<UserWithoutFriendsDTO>>("User/GetUsers");
    }
    
    public async Task<string?> AddFriend(int id)
    {
        var client = await _clientFactory.CreateHttpClientAsync();
        var response = await client.PostAsJsonAsync("User/AddFriend", id);
        return await response.Content.ReadAsStringAsync();
    }
    
}