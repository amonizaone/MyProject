using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Quartz.Spi;
using System;
using System.Threading.Tasks;

namespace MyApi.Scheduler
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

            // Grab the Scheduler instance from the Factory
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            // and start it off
            await scheduler.Start();


            CreateHostBuilder(args).Build().Run();

            Console.WriteLine("Press any key to close the application");
            Console.ReadKey();
        }


        private class ConsoleLogProvider : ILogProvider
        {
            public Logger GetLogger(string name)
            {
                return (level, func, exception, parameters) =>
                {
                    if (level >= LogLevel.Info && func != null)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                    }
                    return true;
                };
            }

            public IDisposable OpenNestedContext(string message)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
            {
                throw new NotImplementedException();
            }
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

