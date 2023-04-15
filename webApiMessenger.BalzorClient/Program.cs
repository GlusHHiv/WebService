using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using webApiMessenger.BalzorClient;
using webApiMessenger.BalzorClient.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddOptions<ServerSettings>()
    .Bind(builder.Configuration.GetSection(ServerSettings.ConfigName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
