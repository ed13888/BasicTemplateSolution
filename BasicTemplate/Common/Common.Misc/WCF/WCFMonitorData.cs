using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Misc
{
    /// <summary>
    /// WCF监控
    /// </summary>
    public class WCFMonitorData
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static volatile WCFMonitorData _instance = null;
        private static volatile object lockhelper = new object();
        /// <summary>
        /// 实体
        /// </summary>
        public static WCFMonitorData Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockhelper)
                    {
                        if (_instance == null)
                        {
                            _instance = new WCFMonitorData();
                        }
                    }
                }

                return _instance;
            }
        }

        private int _CurrentConnections = 0;
        /// <summary>
        /// 当前连接数
        /// </summary>
        public int CurrentConnections => _CurrentConnections;

        public void Add()
        {
            Interlocked.Increment(ref _CurrentConnections);
        }

        public void Reduce()
        {
            Interlocked.Decrement(ref _CurrentConnections);
        }
    }
}
