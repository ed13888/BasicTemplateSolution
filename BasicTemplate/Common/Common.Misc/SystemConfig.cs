using Common.Entity;
using Common.Misc.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Misc
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public static class SystemConfig
    {
        private static Dictionary<string, string> CacheData = new Dictionary<string, string>();
        private static Thread thread;

        static SystemConfig()
        {
            if (thread != null)
            {
                try
                {
                    thread.Abort();
                    thread.DisableComObjectEagerCleanup();
                }
                finally
                {
                    thread = null;
                }
            }
            RefreshData(true);
            thread = new Thread(new ThreadStart(AutoRefreshData));
            thread.IsBackground = false;
            thread.Start();
        }

        /// <summary>
        /// 刷新系统配置数据
        /// </summary>
        private static void AutoRefreshData()
        {
            int sleepTime = 15;
            Thread.Sleep(TimeSpan.FromSeconds(sleepTime));
            while (true)
            {
                try
                {
                    RefreshData(false);
                }
                catch (Exception ex)
                {
                    ex.Error("刷新系统配置数据异常");
                }
                Thread.Sleep(TimeSpan.FromSeconds(sleepTime));
            }
        }

        private static void RefreshData(bool isFirst)
        {
            IEnumerable<ConfigEntity> configRows = isFirst ? GetSystemConfigs() : ConfigRows;
            lock (CacheData)
            {
                foreach (var item in configRows)
                {
                    var key = item.FKey.ToLower();
                    var value = item.FValue;
                    CacheData[key] = value;
                }
            }
        }
        public static IEnumerable<ConfigEntity> ConfigRows
        {
            get
            {
                var list = CacheHelper.Get<List<ConfigEntity>>(CacheKeys.SystemConfigCacheKeys);
                if (list == null)
                {
                    list = GetSystemConfigs();
                    CacheHelper.Set(CacheKeys.SystemConfigCacheKeys, list);
                }
                return list;
            }
        }

        private static List<ConfigEntity> GetSystemConfigs()
        {
            return MySqlHelper.Query<ConfigEntity>("", "select * from SystemConfig").ToList();
        }

        /// <summary>
        /// 获取配置的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigValue(string key)
        {
            if (CacheData.ContainsKey(key.ToLower()))
            {
                return CacheData[key.ToLower()];
            }
            return "";
        }

        /// <summary>
        /// Redis配置
        /// </summary>
        public static string RedisConfig
        {
            get
            {
                var configRows = GetSystemConfigs();
                var configRow = configRows.FirstOrDefault(a => a.FKey.Equals(SystemConfigKeys.RedisConfig));
                return configRow != null ? configRow.FValue : "";
            }
        }

        /// <summary>
        /// 消息队列链接字符串
        /// </summary>
        public static string RabbitMQConnection
        {
            get
            {
                return GetConfigValue(SystemConfigKeys.RabbitMQConnection);
            }
        }


        /// <summary>
        /// 是否开发环境
        /// </summary>
        public static bool IsDevelopment => GetConfigValue(SystemConfigKeys.IsDevelopment).Value<bool>();

        /// <summary>
        /// 余额、账变服务地址
        /// </summary>
        public static string BalanceServiceUrl
        {
            get
            {
                return GetConfigValue(SystemConfigKeys.BalanceServiceUrl);
            }
        }

    }
}
