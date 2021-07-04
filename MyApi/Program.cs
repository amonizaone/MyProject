using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, c) =>
            {
                var env = hostingContext.HostingEnvironment;
                c.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            })
            .ConfigureWebHostDefaults(c =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                c.UseContentRoot(Directory.GetCurrentDirectory());
                //c.UseKestrel();
                //c.UseUrls("http://localhost:5000/", "http://10.10.24.140:5000/");
                c.UseUrls("http://localhost:5000/");
                //c.UseSerilog();
                c.UseStartup<Startup>();
            });
    }
}
