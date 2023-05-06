using Blazored.LocalStorage;
using Blazored.SessionStorage;

namespace webApiMessenger.BalzorClient.Services;

public class JwtService
{
    private readonly ILocalStorageService _localStorage;
    private readonly ISessionStorageService _sessionStorage;

    public JwtService(ILocalStorageService localStorage, ISessionStorageService sessionStorage)
    {
        _localStorage = localStorage;
        _sessionStorage = sessionStorage;
    }

    public async Task<string?> GetJwtToken()
    {
        string? jwt = null;
        if (await _localStorage.ContainKeyAsync("jwt"))
        {
            jwt = await _localStorage.GetItemAsStringAsync("jwt");
        }
        else
        {
            jwt = await _sessionStorage.GetItemAsStringAsync("jwt");
        }

        return jwt;
    }
}