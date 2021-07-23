using Blazor.WebAssembly.Client.Auth;
using Blazor.WebAssembly.Client.Auth.Authentication;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.WebAssembly.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            // 添加本行代码
            builder.Services.AddBootstrapBlazor();
            builder.Services
                .AddBlazoredLocalStorage();
            builder.Services.AddScoped<DiyBlazorStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<DiyBlazorStateProvider>());
            builder.Services.AddAuthorizationCore(c => {
                c.AddPolicy("default", a => a.RequireAuthenticatedUser());
                c.DefaultPolicy = c.GetPolicy("default");
            });

            builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            await builder.Build().RunAsync();
        }
    }
}
