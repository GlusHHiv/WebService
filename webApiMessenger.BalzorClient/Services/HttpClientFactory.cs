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
    private HttpClient? _client = null;

    public HttpClientFactory(IOptions<ServerSettings> options, ILocalStorageService LocalStorage, ISessionStorageService SessionStorage)
    {
        _serverSettings = options.Value ?? throw new ArgumentNullException(nameof(options));
        _localStorage = LocalStorage ?? throw new ArgumentNullException(nameof(LocalStorage));
        _sessionStorage = SessionStorage ?? throw new ArgumentNullException(nameof(SessionStorage));
    }

    public async Task<HttpClient> CreateHttpClientAsync(string? token = null)
    {
        if (_client != null) return _client;

        var jwt = token;
        if (await _localStorage.ContainKeyAsync("jwt"))
        {
            jwt ??= await _localStorage.GetItemAsStringAsync("jwt");
        }
        else
        {
            jwt ??= await _sessionStorage.GetItemAsStringAsync("jwt");
        }

        _client ??= new HttpClient();
        _client.BaseAddress = _serverSettings.Uri;
        if (jwt != null)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }


        return _client;
    }
}