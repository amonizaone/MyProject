using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;

namespace MyApi.Scheduler
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
       Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostContext, services) =>
           {


               //services.AddSingleton<IJobFactory, JobFactory>();
               //services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
               //services.AddHostedService<QuartzHostedService>();


               // Add the required Quartz.NET services
               services.AddQuartz(q =>
              {
                   // Use a Scoped container to create jobs. I'll touch on this later
                   q.UseMicrosoftDependencyInjectionScopedJobFactory();

                  RegisterJobs.InitialJob(q, hostContext.Configuration);
              });

               // Add the Quartz.NET hosted service

               services.AddQuartzHostedService(
                  q => q.WaitForJobsToComplete = true);

               // other config
           });
    }

}

