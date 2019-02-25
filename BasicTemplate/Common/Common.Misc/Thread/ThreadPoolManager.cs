using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Misc
{
    /// <summary>自定义线程池管理类</summary>
    public class ThreadPoolManager : IDisposable
    {
        private static bool RunStatus = true;
        private static bool _isDispose = false;
        /// <summary>最小线程数</summary>
        private static int MinThread = 10;
        /// <summary>最大线程数</summary>
        private static int MaxThread = 50;
        /// <summary>线程池</summary>
        private static ConcurrentBag<ThreadPoolManager.ThreadPoolTask> _pools = new ConcurrentBag<ThreadPoolManager.ThreadPoolTask>();
        /// <summary>线程状态</summary>
        private static List<ThreadState> threadStatusList = new List<ThreadState>()
        {
          ThreadState.Stopped,
          ThreadState.WaitSleepJoin,
          ThreadState.Suspended
        };
        /// <summary>任务队列</summary>
        private static ConcurrentQueue<ThreadPoolManager.ThreadPoolEntity> TaskQueue = new ConcurrentQueue<ThreadPoolManager.ThreadPoolEntity>();
        private static Thread CheckStatusThread;

        static ThreadPoolManager()
        {
            ThreadPoolManager.RunStatus = true;
            if (ThreadPoolManager.CheckStatusThread != null)
            {
                ThreadPoolManager.CheckStatusThread.Abort();
                ThreadPoolManager.CheckStatusThread = (Thread)null;
            }
            ThreadPoolManager.CheckStatusThread = new Thread(new ThreadStart(ThreadPoolManager.AutoCheck));
            ThreadPoolManager.CheckStatusThread.Start();
            ThreadPoolManager.Init();
        }

        /// <summary>自动检查线程</summary>
        private static void AutoCheck()
        {
            while (ThreadPoolManager.RunStatus && !ThreadPoolManager._isDispose)
            {
                lock (ThreadPoolManager._pools)
                {
                    int count = ThreadPoolManager._pools.Count;
                    if (count > ThreadPoolManager.MinThread)
                    {
                        int num = 0;
                        List<ThreadPoolManager.ThreadPoolTask> threadPoolTaskList = new List<ThreadPoolManager.ThreadPoolTask>();
                        ThreadPoolManager.ThreadPoolTask result;
                        while (ThreadPoolManager._pools.TryTake(out result))
                        {
                            if (!result.IsRun && result.SleepTime >= TimeSpan.FromMinutes(30.0) && ThreadPoolManager.threadStatusList.Contains(result.Task.ThreadState))
                            {
                                try
                                {
                                    result.Task.DisableComObjectEagerCleanup();
                                    result.Task.Abort();
                                }
                                catch
                                {
                                }
                                result.Task = (Thread)null;
                            }
                            else
                                threadPoolTaskList.Add(result);
                            ++num;
                            if (num >= count)
                                break;
                        }
                        foreach (ThreadPoolManager.ThreadPoolTask threadPoolTask in threadPoolTaskList)
                            ThreadPoolManager._pools.Add(threadPoolTask);
                    }
                    if (ThreadPoolManager._pools.Count<ThreadPoolManager.ThreadPoolTask>((Func<ThreadPoolManager.ThreadPoolTask, bool>)(a => !a.IsRun)) == 0 && count < ThreadPoolManager.MaxThread)
                        ThreadPoolManager.CreateTask();
                    //LogsMrg.Logger.Debug((object)string.Format("自定义线程池中当前线程数[{0}]", (object)ThreadPoolManager._pools.Count));
                }
                Thread.Sleep(30000);
            }
        }

        /// <summary>初始化</summary>
        private static void Init()
        {
            lock (ThreadPoolManager._pools)
            {
                while (true)
                {
                    if (ThreadPoolManager.ThreadCount < ThreadPoolManager.MinThread)
                        ThreadPoolManager.CreateTask();
                    else
                        break;
                }
            }
        }

        private static void CreateTask()
        {
            ThreadPoolManager.ThreadPoolTask threadPoolTask = new ThreadPoolManager.ThreadPoolTask();
            threadPoolTask.IsRun = true;
            threadPoolTask.SleepTime = TimeSpan.Zero;
            Thread thread = new Thread(new ParameterizedThreadStart(ThreadPoolManager.AutoRunning));
            thread.IsBackground = true;
            thread.Name = string.Format("ThreadPoolManager{0}", (object)ThreadPoolManager._pools.Count);
            threadPoolTask.Task = thread;
            thread.Start((object)threadPoolTask);
            ThreadPoolManager._pools.Add(threadPoolTask);
        }

        /// <summary>设置线程数</summary>
        /// <param name="minThread"></param>
        /// <param name="maxThread"></param>
        public static void SetThreadCount(int minThread, int maxThread)
        {
            if (minThread <= 0)
                minThread = ThreadPoolManager.MinThread;
            if (maxThread <= 0)
                maxThread = ThreadPoolManager.MaxThread;
            if (maxThread < minThread)
                maxThread = minThread;
            ThreadPoolManager.MinThread = minThread;
            ThreadPoolManager.MaxThread = maxThread;
            ThreadPoolManager.Init();
        }

        /// <summary>线程数</summary>
        public static int ThreadCount
        {
            get
            {
                lock (ThreadPoolManager._pools)
                    return ThreadPoolManager._pools.Count;
            }
        }

        /// <summary>自动运行任务</summary>
        private static void AutoRunning(object state)
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(100.0);
            ThreadPoolManager.ThreadPoolTask threadPoolTask = state as ThreadPoolManager.ThreadPoolTask;
            while (ThreadPoolManager.RunStatus)
            {
                if (ThreadPoolManager._isDispose)
                    break;
                TimeSpan sleepTime;
                try
                {
                    if (ThreadPoolManager.TaskQueue.Any<ThreadPoolManager.ThreadPoolEntity>())
                    {
                        threadPoolTask.SleepTime = TimeSpan.Zero;
                        threadPoolTask.IsRun = true;
                        ThreadPoolManager.ThreadPoolEntity result;
                        if (ThreadPoolManager.TaskQueue.TryDequeue(out result))
                        {
                            Action<object> action = result.Action;
                            if (action != null)
                                action(result.State);
                        }
                    }
                    else
                    {
                        threadPoolTask.IsRun = false;
                        sleepTime = threadPoolTask.SleepTime;
                        sleepTime.Add(timeSpan);
                        Thread.Sleep(timeSpan);
                    }
                }
                catch (Exception ex)
                {
                    ex.Error("从队列中获取数据处理时发生异常");
                    threadPoolTask.IsRun = false;
                    sleepTime = threadPoolTask.SleepTime;
                    sleepTime.Add(timeSpan);
                    Thread.Sleep(timeSpan);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public static void Run(Action action)
        {
            ThreadPoolManager.TaskQueue.Enqueue(new ThreadPoolManager.ThreadPoolEntity()
            {
                Action = (Action<object>)(o => action()),
                State = (object)null
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="state"></param>
        public static void Run(Action<object> action, object state)
        {
            ThreadPoolManager.TaskQueue.Enqueue(new ThreadPoolManager.ThreadPoolEntity()
            {
                Action = action,
                State = state
            });
        }

        /// <summary>释放资源</summary>
        public void Dispose()
        {
            ThreadPoolManager._isDispose = true;
            bool flag;
            do
            {
                ThreadPoolManager.ThreadPoolTask result;
                flag = ThreadPoolManager._pools.TryTake(out result);
                if (flag)
                {
                    try
                    {
                        result.Task.DisableComObjectEagerCleanup();
                        result.Task.Abort();
                    }
                    catch
                    {
                    }
                    result.Task = (Thread)null;
                }
            }
            while (flag);
        }

        /// <summary>停止</summary>
        public static void Stop()
        {
            ThreadPoolManager._isDispose = true;
            ThreadPoolManager.RunStatus = false;
            Thread.Sleep(TimeSpan.FromSeconds(1.0));
            bool flag;
            do
            {
                ThreadPoolManager.ThreadPoolTask result;
                flag = ThreadPoolManager._pools.TryTake(out result);
                if (flag)
                {
                    try
                    {
                        result.Task.DisableComObjectEagerCleanup();
                        result.Task.Abort();
                    }
                    catch
                    {
                    }
                    result.Task = (Thread)null;
                }
            }
            while (flag);
        }

        /// <summary>线程池实体类</summary>
        private class ThreadPoolEntity
        {
            /// <summary>运行方法</summary>
            public Action<object> Action { get; set; }

            /// <summary>参数</summary>
            public object State { get; set; }
        }

        private class ThreadPoolTask
        {
            public Thread Task { get; set; }

            public bool IsRun { get; set; }

            public TimeSpan SleepTime { get; set; }
        }
    }
}
