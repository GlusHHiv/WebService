using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.Extensions.Options;
using webApiMessenger.BalzorClient.Options;

namespace webApiMessenger.BalzorClient.Services;

public class HttpClientFactory
{
    private readonly ServerSettings _serverSettings;
    private readonly ILocalStorageService _localStorage;
    private readonly ISessionStorageService _sessionStorage;

    public HttpClientFactory(IOptions<ServerSettings> options, ILocalStorageService LocalStorage, ISessionStorageService SessionStorage)
    {
        _serverSettings = options.Value ?? throw new ArgumentNullException(nameof(options));
        _localStorage = LocalStorage ?? throw new ArgumentNullException(nameof(LocalStorage));
        _sessionStorage = SessionStorage ?? throw new ArgumentNullException(nameof(SessionStorage));
    }

    public async Task<HttpClient> CreateHttpClientAsync(string? token = null)
    {
        var jwt = token;
        if (await _localStorage.ContainKeyAsync("jwt"))
        {
            jwt ??= await _localStorage.GetItemAsStringAsync("jwt");
        }
        else
        {
            jwt ??= await _sessionStorage.GetItemAsStringAsync("jwt");
        }

        var client = new HttpClient();
        client.BaseAddress = _serverSettings.Uri;
        if (jwt != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        return client;
    }

    public HttpClient CreateHttpClient(string? token = null)
    {
        var jwt = token;
        if (_localStorage.ContainKeyAsync("jwt").GetAwaiter().GetResult())
        {
            jwt ??= _localStorage.GetItemAsStringAsync("jwt").GetAwaiter().GetResult();
        }
        else
        {
            jwt ??= _sessionStorage.GetItemAsStringAsync("jwt").GetAwaiter().GetResult();
        }

        var client = new HttpClient();
        client.BaseAddress = _serverSettings.Uri;
        if (jwt != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        return client;
    }
}