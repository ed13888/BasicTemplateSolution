using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc.Logs
{
    /// <summary>
    /// 自定义log4net实例。主要用来替换CustomLogger类写日志的方法。CustomLogger经常漏日志
    /// </summary>
    class CustomLog4
    {
        private static Dictionary<string, ILog> Logers = new Dictionary<string, ILog>();

        /// <summary>
        /// 获取log实例
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ILog GetLogger(string name)
        {
            if (Logers.ContainsKey(name))
                return Logers[name];
            else
            {
                lock (Logers)
                {
                    if (Logers.ContainsKey(name)) return Logers[name];
                    else
                    {
                        var newLoger = CreateLogerInstance(name);
                        Logers.Add(name, newLoger);
                        return newLoger;
                    }
                }
            }
        }

        /// <summary>
        /// 创建Log实例
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static ILog CreateLogerInstance(string name)
        {
            try
            {
                // Pattern Layout
                PatternLayout layout = new PatternLayout("%date{yyyy-MM-dd HH:mm:ss} %-5level[%L] %message% %F%newline");
                // Level Filter
                LevelMatchFilter filter = new LevelMatchFilter();
                filter.LevelToMatch = Level.All;
                filter.ActivateOptions();
                // File Appender
                RollingFileAppender appender = new RollingFileAppender();
                // 目录
                appender.File = $"logs\\{name}\\";
                // 立即写入磁盘
                appender.ImmediateFlush = true;
                // true：追加到文件；false：覆盖文件
                appender.AppendToFile = true;
                // 新的日期或者文件大小达到上限，新建一个文件
                appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
                // 文件大小达到上限，新建文件时，文件编号放到文件后缀前面
                appender.PreserveLogFileNameExtension = true;
                // 时间模式
                appender.DatePattern = $"yyyyMMdd'.txt'";
                // 最小锁定模型以允许多个进程可以写入同一个文件
                appender.LockingModel = new FileAppender.MinimalLock();
                appender.Name = $"{name}Appender";
                appender.AddFilter(filter);
                appender.Layout = layout;
                appender.ActivateOptions();
                // 文件大小上限
                appender.MaximumFileSize = "100MB";
                // 设置无限备份=-1 ，最大备份数为30
                appender.MaxSizeRollBackups = 30;
                appender.StaticLogFileName = false;
                //create instance
                string repositoryName = $"{name}Repository";
                ILoggerRepository repository = LoggerManager.CreateRepository(repositoryName);
                BasicConfigurator.Configure(repository, appender);
                //After the log instance initialization, we can get the instance from the LogManager by the special log instance name. Then you can start your logging trip.
                ILog logger = LogManager.GetLogger(repositoryName, name);
                return logger;
            }
            catch (Exception ex)
            {
                LogsManager.Error($"自定义日志，创建Log实例，错误：{ex.Message},详细错误：{ex.StackTrace}");
                // 自定义日志失败，那就用默认的                              
                return log4net.LogManager.GetLogger(name);
            }

        }
    }
}
