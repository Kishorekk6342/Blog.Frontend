using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Blog.Frontend;
using Blog.Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Root components
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 🔑 HttpClient – MUST point to backend API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://blog-backend-a5sf.onrender.com")
});

// MudBlazor
builder.Services.AddMudServices();

// Application services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<UserService>();

await builder.Build().RunAsync();
