using Common.Enums;
using Common.Misc.SQL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Misc
{
    /// <summary>
    /// 监控类
    /// </summary>
    public static class MonitorUtil
    {
        private static int ProcessorCount = 0;
        private static PerformanceCounter CpuCounter;
        private static PerformanceCounter MemoryCounter;
        private static Thread thread = null;

        /// <summary>
        /// 开始监控
        /// </summary>
        public static void Start()
        {
            ProcessorCount = Environment.ProcessorCount;
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
            thread = new Thread(new ThreadStart(Monitor));
            thread.IsBackground = false;
            thread.Start();
        }

        private static void InitMonitor()
        {
            try
            {
                Process CurrentProcess = Process.GetCurrentProcess();
                string serviceName = CurrentProcess.ProcessName;
                string instance1 = GetInstanceName("Process", "ID Process", CurrentProcess);
                if (instance1 != null)
                {
                    PerformanceCounter cpucounter = new PerformanceCounter("Process", "% Processor Time", instance1);
                    if (cpucounter != null)
                    {
                        cpucounter.NextValue();
                        System.Threading.Thread.Sleep(200); //等200ms(是测出能换取下个样本的最小时间间隔)，让后系统获取下一个样本
                        CpuCounter = cpucounter;
                        LogsManager.Info($"[{serviceName}]生成CPU监控成功--[{instance1}]");
                    }
                    else
                    {
                        LogsManager.Error($"[{serviceName}]生成CPU监控失败--[{instance1}]");
                    }

                    PerformanceCounter memoryCounter = new PerformanceCounter("Process", "Working Set - Private", instance1);
                    if (memoryCounter != null)
                    {
                        memoryCounter.NextValue();
                        System.Threading.Thread.Sleep(200); //等200ms(是测出能换取下个样本的最小时间间隔)，让后系统获取下一个样本
                        MemoryCounter = memoryCounter;
                        LogsManager.Info($"[{serviceName}]生成内存监控成功--[{instance1}]");
                    }
                    else
                    {
                        LogsManager.Error($"[{serviceName}]生成内存监控失败--[{instance1}]");
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Error("初始化监控异常");
            }
        }

        private static string GetInstanceName(string categoryName, string counterName, Process p)
        {
            try
            {
                PerformanceCounterCategory processcounter = new PerformanceCounterCategory(categoryName);
                string[] instances = processcounter.GetInstanceNames();
                foreach (string instance in instances)
                {
                    PerformanceCounter counter = new PerformanceCounter(categoryName, counterName, instance);
                    if (counter.NextValue() == p.Id)
                    {
                        return instance;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        private static void Monitor()
        {
            int sleepTime = 30;
            InitMonitor();
            while (true)
            {
                try
                {
                    sleepTime = 30;
                    string cpu = Cpu;
                    string memory = Memory;
                    if (string.IsNullOrWhiteSpace(cpu) || string.IsNullOrWhiteSpace(memory))
                    {
                        continue;
                    }
                    int threadCount = 0;
                    string processName = "";
                    string fileName = "";
                    int connectionsCount = WCFMonitorData.Instance.CurrentConnections;
                    Process CurrentProcess = Process.GetCurrentProcess();
                    if (CurrentProcess != null)
                    {
                        threadCount = CurrentProcess.Threads.Count;
                        processName = CurrentProcess.ProcessName;
                        fileName = CurrentProcess.MainModule.FileName;
                    }
                    LogType.Monitor.WriterLog($"进程名称：[{processName}]，当前CPU：[{cpu}%]，当前内存：[{memory}MB]，当前线程数：[{threadCount}]，连接数：[{connectionsCount}]，路径：[{fileName}]");

                    StringBuilder sbSql = new StringBuilder();
                    sbSql.Append("INSERT INTO TMonitorData(FProcessName, FCpu, FMemory, FFhreadCount, FConnections, FCreateTime) ");
                    sbSql.Append("VALUES(@processName,@cpu,@memory,@threadCount,@connectionsCount,@createTime) ");
                    var param = new
                    {
                        processName = processName,
                        cpu = decimal.Round(decimal.Parse(cpu), 2),
                        memory = decimal.Round(decimal.Parse(memory), 2),
                        threadCount = threadCount,
                        connectionsCount = connectionsCount,
                        createTime = DateTime.Now
                    };
                    SqlHelper.Execute(sbSql.ToString(), param, System.Data.CommandType.Text);
                }
                catch (Exception ex)
                {
                    sleepTime = 60;
                    ex.Error("服务监控异常");
                }
                Thread.Sleep(TimeSpan.FromSeconds(sleepTime));
            }
        }

        /// <summary>
        /// 当前CPU
        /// </summary>
        public static string Cpu
        {
            get
            {
                if (CpuCounter != null)
                {
                    return (CpuCounter.NextValue() / ProcessorCount).ToString("F");
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 当前内存
        /// </summary>
        public static string Memory
        {
            get
            {
                if (MemoryCounter != null)
                {
                    return (MemoryCounter.NextValue() / (1024 * 1024)).ToString("0.#");
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 当前内存
        /// </summary>
        public static int ThreadCount
        {
            get
            {
                Process CurrentProcess = Process.GetCurrentProcess();
                if (CurrentProcess != null)
                {
                    return CurrentProcess.Threads.Count;
                }
                return 0;
            }
        }
    }
}
