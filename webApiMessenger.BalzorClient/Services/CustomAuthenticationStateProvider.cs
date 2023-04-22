using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace webApiMessenger.BalzorClient.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private readonly ISessionStorageService _sessionStorageService;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorageService, ISessionStorageService sessionStorageService)
    {
        _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
        _sessionStorageService = sessionStorageService ?? throw new ArgumentNullException(nameof(sessionStorageService));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var jwt = await _localStorageService.GetItemAsStringAsync("jwt");
        if (jwt == null)
        {
            jwt = await _sessionStorageService.GetItemAsStringAsync("jwt");
        }
        var claimIdentity = new ClaimsIdentity();

        if (jwt != null)
        {
            claimIdentity = new ClaimsIdentity(ParseClaimsFromJwt(jwt), "jwt");
        }

        var userClaimPrincipal = new ClaimsPrincipal(claimIdentity);
        var state = new AuthenticationState(userClaimPrincipal);
        NotifyAuthenticationStateChanged(Task.FromResult(state));
        return state;
    }

    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}