using Blazor.WebAssembly.Server.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace Blazor.WebAssembly.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddResponseCompression(opt => {
                opt.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UsePathBase("/blazorwebassembly");
            app.UseResponseCompression();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/blazorwebassembly"), app =>
            //{
            //    app.UseBlazorFrameworkFiles("/blazorwebassembly");
            //    app.UseStaticFiles();

            //    app.UseStaticFiles("/blazorwebassembly");
            //    app.UseRouting();
            //    app.UseEndpoints(endpoints =>
            //    {
            //        endpoints.MapControllers();
            //        endpoints.MapHub<ChatHub>("blazorwebassembly/chathub");
            //        endpoints.MapFallbackToFile("blazorwebassembly/{*path:nonfile}", "blazorwebassembly/index.html");
            //    });
            //});

            app.UseBlazorFrameworkFiles("/blazorwebassembly");
            app.UseStaticFiles();

            app.UseStaticFiles("/blazorwebassembly");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/blazorwebassembly/chathub");
                endpoints.MapFallbackToFile("blazorwebassembly/{*path:nonfile}", "blazorwebassembly/index.html");
            });
        }
    }
}
