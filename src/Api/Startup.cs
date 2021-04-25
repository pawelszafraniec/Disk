using Disk.Api.Data;

using Disk.Api.Configuration;
using Disk.Common;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Disk.Api
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.ConfigureLogging(this.configuration);
            services.ConfigureDbContext(this.configuration);
            services.ConfigureHealthChecks(this.configuration);
            services.ConfigureSwagger();
            services.ConfigureAuthentication(this.configuration);
            services.AddMvc();

            services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddTransient<IContentTypeProvider, FileExtensionContentTypeProvider>();
        }


        public void Configure(IApplicationBuilder app)
        {
            if (this.environment.IsDevelopment() || this.environment.IsLocal())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerDocs();

            app.UseHealthCheck();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
