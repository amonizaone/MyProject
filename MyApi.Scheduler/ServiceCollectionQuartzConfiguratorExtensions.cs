using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace MyApi.Scheduler
{
    public static class ServiceCollectionQuartzConfiguratorExtensions
    {

        public static void AddJobAndTrigger<T>(
            this IServiceCollectionQuartzConfigurator service,
            IConfiguration configuration = null,
            string cron = null
            ) where T : IJob
        {

            // Use the name of the IJob as the appsettings.json key
            // เรียกใช้งาน job จาก appsettings.json key
            string jobName = typeof(T).Name;

            // Try and load the schedule from configuration
            // ทดลองและโหลดข้อมูลจากการตั้งค่า
            var configKey = $"Quartz:{jobName}";
            string cronSchedule = null;

            if (configuration != null)
                cronSchedule = configuration[configKey];

             
            if (string.IsNullOrEmpty(cronSchedule) && cron == null)
            {
                throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {configKey}");
            }


            // register job as before
            // หลังจากนั้นก็ลงทะเบียน job
            JobKey jobKey = new JobKey(jobName);
            service.AddJob<T>(opts => opts.WithIdentity(jobKey));

            service.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity($"{jobName}-trigger")
                .WithCronSchedule(cronSchedule ?? cron) 
            );


        }
    }
}
