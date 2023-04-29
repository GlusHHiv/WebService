using System.Net.Http;
using System.Net.Http.Json;

namespace webApiMessenger.BalzorClient.Services;

public class SignInOrUpService
{
    private readonly HttpClientFactory _httpClientFactory;

    public SignInOrUpService(HttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string?> SignIn(string login, string password)
    {
        var client = await _httpClientFactory.CreateHttpClientAsync();
        var response = await client.PostAsJsonAsync("User/Login", new
        {
            login,
            password
        });
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return null;
    }

    public async Task<string?> SignUp()
    {
        throw new NotImplementedException();
    }
}