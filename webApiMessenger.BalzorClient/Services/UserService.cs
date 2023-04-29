using System.Net.Http.Json;
using webApiMessenger.Shared.DTOs;

namespace webApiMessenger.BalzorClient.Services;

public class UserService
{
    private HttpClient _client;

    public UserService(HttpClientFactory httpClientFactory)
    {
        var task = httpClientFactory.CreateHttpClientAsync();
    }
    
    public async Task<UserPublicDTO?> Me()
    {
        return await _client.GetFromJsonAsync<UserPublicDTO>("User/Me");
    }

    public async Task<IEnumerable<UserWithoutFriendsDTO>?> GetFriends()
    {
        return await _client.GetFromJsonAsync<IEnumerable<UserWithoutFriendsDTO>>("User/GetFriends");
    }
}