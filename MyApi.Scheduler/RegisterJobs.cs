using Microsoft.Extensions.Configuration;
using MyApi.Scheduler.Jobs;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApi.Scheduler
{
    public static class RegisterJobs
    { 
        public static void InitialJob(IServiceCollectionQuartzConfigurator service, IConfiguration configuration) {
 
            // Create a "key" for the job
            //var jobKey = new JobKey("HelloWorldJob");

            // Register the job with the DI container
            //service.AddJob<HelloWorldJob>(opts => opts.WithIdentity(jobKey));


            // Create a trigger for the job
            //service.AddTrigger(opts => opts
            //    .ForJob(jobKey) // link to the HelloWorldJob
            //    .WithIdentity("HelloWorldJob-trigger") // give the trigger a unique name
            //    .WithCronSchedule("0/5 * * * * ?")); // run every 5 seconds

            service.AddJobAndTrigger<HelloWorldJob>(cron: "0/5 * * * * ?");
        }


    }
}
