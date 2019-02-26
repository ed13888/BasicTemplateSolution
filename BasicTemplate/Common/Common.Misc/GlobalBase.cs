﻿using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    /// <summary>
    /// 全局静态类
    /// </summary>
    public static class GlobalBase
    {
        /// <summary>
        /// 初始化程序需要的配置
        /// </summary>
        public static void Init(bool isDeleteLogs = true)
        {
            string configDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
            XmlConfigurator.Configure(new FileInfo(Path.Combine(configDir, "log4net.config")));
            //StructureMapHelpers.Initialize(Path.Combine(configDir, "StructureMap.config"));
            CacheHelper.Configure();
            if (isDeleteLogs)
            {
                FileHelper.DeleteLogs();
            }
            RabbitMQHelper.Init(SystemConfig.RabbitMQConnection);

            //var selfGameRabbitMqConfig = SystemConfig.SelfGameRabbitMQConfig;

            //if (!string.IsNullOrWhiteSpace(selfGameRabbitMqConfig))
            //{
            //    RabbitMQHelper.Init(selfGameRabbitMqConfig, SelfGameConfig.SelfGameRabbitMQInstance);
            //}
            /// 暂时关闭日志写入ES
            //Log4netAppenderHelper.SetEsConnection(SystemConfig.EalasticsearchConnection);
        }
    }
}