using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    /// <summary>
    /// Quartz.NET 任务调度中心
    /// </summary>
    public class SchedulerUtil
    {
        private static IScheduler scheduler;
        private static object lockHelper = new object();
        public static IScheduler Current
        {
            get
            {
                if (scheduler == null)
                {
                    lock (lockHelper)
                    {
                        if (scheduler == null)
                        {
                            ISchedulerFactory factory = new StdSchedulerFactory();
                            scheduler = factory.GetScheduler();
                            scheduler.Start();
                        }
                    }
                }
                return scheduler;
            }
        }

        /// <summary>
        /// 创建定时任务
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="runTime"></param>
        public static void CreateJob<T>(TimeSpan sp, DateTimeOffset runTime) where T : IJob
        {
            string className = typeof(T).Name.Replace("Job", "");
            string jobName = $"{className}_Job";
            string groupName = typeof(T).Name;
            string triggerName = $"{className}_Trigger";

            JobKey jobKey = new JobKey(jobName, groupName);
            TriggerKey triggerKey = new TriggerKey(triggerName, groupName);

            if (!Current.CheckExists(jobKey) || !Current.CheckExists(triggerKey))
            {
                var job = JobBuilder.Create<T>().WithIdentity(jobName, groupName).Build();
                var trigger = TriggerBuilder.Create().WithIdentity(triggerName, groupName).WithSimpleSchedule(x => x.WithInterval(sp).RepeatForever()).StartAt(runTime).Build();
                Current.ScheduleJob(job, trigger);
            }
        }

        /// <summary>
        /// 创建执行一次的任务
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="runTime"></param>
        public static void CreateOnceJob<T>(TimeSpan sp, DateTimeOffset runTime) where T : IJob
        {
            string className = typeof(T).Name.Replace("Job", "");
            string jobName = $"{className}_Job";
            string groupName = typeof(T).Name;
            string triggerName = $"{className}_Trigger";

            JobKey jobKey = new JobKey(jobName, groupName);
            TriggerKey triggerKey = new TriggerKey(triggerName, groupName);

            if (!Current.CheckExists(jobKey) || !Current.CheckExists(triggerKey))
            {
                var job = JobBuilder.Create<T>().WithIdentity(jobName, groupName).Build();
                //WithRepeatCount(0) 重复执行的次数，因为加入任务的时候马上执行了，所以不需要重复，否则会多一次。  
                var trigger = TriggerBuilder.Create().WithIdentity(triggerName, groupName).WithSimpleSchedule(x => x.WithInterval(sp).WithRepeatCount(0)).StartAt(runTime).Build();
                Current.ScheduleJob(job, trigger);
            }
        }


        /// <summary>
        /// 创建定时任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cronExp"></param>
        /// <param name="runTime"></param>
        public static void CreateJob<T>(string cronExp, DateTimeOffset runTime) where T : IJob
        {
            string className = typeof(T).Name.Replace("Job", "");
            string jobName = $"{className}_Job";
            string groupName = typeof(T).Name;
            string triggerName = $"{className}_Trigger";

            JobKey jobKey = new JobKey(jobName, groupName);
            TriggerKey triggerKey = new TriggerKey(triggerName, groupName);

            if (!Current.CheckExists(jobKey) || !Current.CheckExists(triggerKey))
            {
                //DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTime.Now);
                IJobDetail job = JobBuilder.Create<T>().WithIdentity(jobName, groupName).Build();
                ITrigger trigger = TriggerBuilder.Create().WithIdentity(triggerName, groupName).WithCronSchedule(cronExp).StartAt(runTime).Build();
                Current.ScheduleJob(job, trigger);
            }
        }
    }
}
