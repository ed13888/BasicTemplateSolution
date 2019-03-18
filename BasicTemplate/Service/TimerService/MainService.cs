using Common.Entity.Business;
using Common.Misc;
using FluentScheduler;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TimerService
{
    public class MainService : WCFServiceFactory
    {
        public override void Start()
        {
            try
            {
                lock (lockRoot)
                {
                    //Thread.Sleep(20000);
                    #region 加载配置
                    //BsonSerializer.RegisterSerializationProvider(new CustomSerializationProvider()); //MongoDB配置
                    var configDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
                    XmlConfigurator.Configure(new FileInfo(Path.Combine(configDir, "log4net.config")));
                    //StructureMapHelpers.Initialize(Path.Combine(configDir, "StructureMap.config"));
                    //CacheHelper.Configure();
                    #endregion

                    #region 加载任务列表
                    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config\TimingTask.xml");//读取配置文件中的任务
                    var xmlStr = File.ReadAllText(path);
                    var xmls = XElement.Parse(xmlStr);
                    var taskList = (from x in xmls.Elements("Task")
                                    select new TaskEntity
                                    {
                                        TaskTitle = x.Element("TaskTitle").Value,
                                        TaskFullName = x.Element("TaskFullName").Value,
                                        TaskIntervals = x.Element("TaskIntervals").Value.Value<int>(),
                                        TaskStatus = x.Element("TaskStatus").Value.Value<bool>()
                                    }).ToList();
                    var timingTask = new TimingTask();
                    foreach (var task in taskList)
                    {
                        if (string.IsNullOrEmpty(task.TaskFullName))
                        {
                            continue;
                        }
                        if (!task.TaskStatus)
                        {
                            LogsManager.Info($"[{task.TaskTitle}]定时任务未启动");
                            continue;
                        }
                        var taskType = Type.GetType(task.TaskFullName);
                        if (taskType == null)
                        {
                            LogsManager.Error($"[{task.TaskTitle}]定时任务获取实例为空");
                            continue;
                        }
                        var taskjob = Activator.CreateInstance(taskType) as IJob;
                        timingTask.Schedule(() => taskjob.Execute()).ToRunNow().AndEvery(task.TaskIntervals).Seconds();
                        LogsManager.Info($"[{task.TaskFullName}]定时任务启动成功");
                    }
                    JobManager.Initialize(timingTask);


                    #endregion

                    LogsManager.Info("------------------------服务启动成功----------------------");
                }
            }
            catch (Exception ex)
            {
                ex.Error("系统启动异常");
            }
        }

        public override void Stop()
        {
            try
            {
                lock (lockRoot)
                {
                    JobManager.StopAndBlock();
                    base.Close();
                    LogsManager.Info("------------------------服务关闭成功----------------------");
                }
            }
            catch (Exception ex)
            {
                ex.Error("系统关闭异常");
            }
        }
    }
}
