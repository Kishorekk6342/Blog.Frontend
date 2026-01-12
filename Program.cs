using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blog.Frontend;
using Blog.Frontend.Services;
using MudBlazor.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();



builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri("https://localhost:7200/")
    });


builder.Services.AddScoped<PostService>(); 
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthStateService>();

builder.Services.AddMudServices();


await builder.Build().RunAsync();
