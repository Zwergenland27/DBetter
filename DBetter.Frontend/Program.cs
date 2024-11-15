using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DBetter.Frontend;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddLocalization();

builder.Services.AddScoped<AuthenticationHandler>();
builder.Services.AddHttpClient(AuthenticationService.HttpClientName,
    client => client.BaseAddress = new Uri("https://localhost:44308"));
builder.Services.AddHttpClient("DBetter.Api", client => client.BaseAddress = new Uri("https://localhost:44308"))
    .AddHttpMessageHandler<AuthenticationHandler>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();