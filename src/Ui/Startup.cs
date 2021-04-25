using AspNetCore.Proxy;

using Disk.Ui.Authentication;
using Disk.Ui.Configuration;
using Disk.Ui.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Net.Http;
using System.Net.Http.Headers;

namespace Disk.Ui
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddHttpClient("api-client", config =>
            {
                config.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true
                };

            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                UseCookies = false,
                CookieContainer = new System.Net.CookieContainer(0)
            });
            services.AddHttpContextAccessor();

            services.AddTransient<RestClient>();
            services.AddTransient<IObjectSerializer, ObjectSerializer>();

            services.AddTransient<AuthenticationService>();
            services.AddTransient<DirectoryService>();
            services.AddTransient<FileService>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "api";
            })
           .AddScheme<ApiAuthenticationOptions, ApiAuthenticationHandler>("api", op => { });

            services.ConfigureHealthChecks(this.configuration);

            services.ConfigureLogging(this.configuration);

            services.AddProxies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<RedirectMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHealthCheck();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
