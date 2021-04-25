using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using System;

namespace Disk.Api
{
    public static class Program
    {
        private const string logo = @"
    ██████╗ ██╗███████╗██╗  ██╗     █████╗ ██████╗ ██╗
    ██╔══██╗██║██╔════╝██║ ██╔╝    ██╔══██╗██╔══██╗██║
    ██║  ██║██║███████╗█████╔╝     ███████║██████╔╝██║
    ██║  ██║██║╚════██║██╔═██╗     ██╔══██║██╔═══╝ ██║
    ██████╔╝██║███████║██║  ██╗    ██║  ██║██║     ██║
    ╚═════╝ ╚═╝╚══════╝╚═╝  ╚═╝    ╚═╝  ╚═╝╚═╝     ╚═╝                                        
";

        public static void Main(string[] args)
        {
            Console.WriteLine(logo);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
