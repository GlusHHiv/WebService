using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using webApiMessenger.BalzorClient;
using webApiMessenger.BalzorClient.Options;
using webApiMessenger.BalzorClient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddOptions<ServerSettings>()
    .Bind(builder.Configuration.GetSection(ServerSettings.ConfigName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddBlazoredSessionStorageAsSingleton();
builder.Services.AddBlazoredLocalStorageAsSingleton();

builder.Services.AddSingleton<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddSingleton<HttpClientFactory>();
builder.Services.AddSingleton<SignInOrUpService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<JwtService>();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
