using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Blog.Frontend;
using Blog.Frontend.Services;
using Blazored.LocalStorage;
using System.Net.Http.Headers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Root components
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 🔑 API URL (dev + production)
var apiUrl = builder.HostEnvironment.IsDevelopment()
    ? "https://localhost:7200/"
    : "https://blog-backend-a5sf.onrender.com/";

// LocalStorage
builder.Services.AddBlazoredLocalStorage();

// HttpClient with token (minimal fix)
builder.Services.AddScoped(sp =>
{
    var http = new HttpClient
    {
        BaseAddress = new Uri(apiUrl)
    };

    var localStorage = sp.GetRequiredService<ILocalStorageService>();

    string? token = null;

    try
    {
        token = localStorage.GetItemAsync<string>("token")
            .AsTask()
            .GetAwaiter()
            .GetResult();
    }
    catch
    {
        // prevent crash in dev
    }

    if (!string.IsNullOrEmpty(token))
    {
        http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    return http;
});

// MudBlazor
builder.Services.AddMudServices();

// Application services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<UserService>();

await builder.Build().RunAsync();