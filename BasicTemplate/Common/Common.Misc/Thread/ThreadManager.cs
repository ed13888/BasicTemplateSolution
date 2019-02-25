using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Misc
{
    public class ThreadManager
    {
        private static bool RunStatus = true;
        private static CancellationTokenSource cts = new CancellationTokenSource();
        private static object lockObj = new object();
        /// <summary>默认休眠时间（毫秒）</summary>
        private static int DefaultWatiTime = 20;
        /// <summary>一次休眠的时间（毫秒）</summary>
        private static int OneWatiTime = 500;
        /// <summary>系统所有线程集合</summary>
        private static ConcurrentBag<ThreadManager.ThreadClass> ThreadList = new ConcurrentBag<ThreadManager.ThreadClass>();

        /// <summary>添加工作线程</summary>
        /// <param name="action">工作方法</param>
        /// <param name="state">参数</param>
        /// <param name="sleepTime">休眠时间（毫秒）</param>
        public static void AddThread(Action<object> action, object state, double sleepTime = 0.0)
        {
            ThreadManager.Add(new ThreadManager.ThreadClass()
            {
                Action = action,
                State = state,
                RunStatus = true,
                SleepTime = sleepTime == 0.0 ? ThreadManager.DefaultWatiTime : (int)sleepTime
            });
        }

        /// <summary>添加工作线程</summary>
        /// <param name="action">工作方法</param>
        /// <param name="sleepTime">休眠时间（毫秒）</param>
        public static void AddThread(Action action, double sleepTime = 0.0)
        {
            ThreadManager.Add(new ThreadManager.ThreadClass()
            {
                Action = (Action<object>)(obj => action()),
                State = (object)null,
                RunStatus = true,
                SleepTime = sleepTime == 0.0 ? ThreadManager.DefaultWatiTime : (int)sleepTime
            });
        }

        private static void Add(ThreadManager.ThreadClass taskParam)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(ThreadManager.Running));
            thread.IsBackground = true;
            taskParam.WorkTask = thread;
            thread.Start((object)taskParam);
            lock (ThreadManager.lockObj)
                ThreadManager.ThreadList.Add(taskParam);
        }

        private static void Running(object state)
        {
            ThreadManager.ThreadClass task = state as ThreadManager.ThreadClass;
            int num = task.SleepTime;
            int millisecondsTimeout = ThreadManager.DefaultWatiTime;
            if (task.SleepTime > ThreadManager.DefaultWatiTime)
                millisecondsTimeout = task.SleepTime > ThreadManager.OneWatiTime ? ThreadManager.OneWatiTime : task.SleepTime;
            while (ThreadManager.RunStatus)
            {
                if (ThreadManager.cts.Token.IsCancellationRequested)
                {
                    task.RunStatus = false;
                    break;
                }
                task.RunStatus = true;
                if (task.SleepTime > 0)
                {
                    if (num >= task.SleepTime)
                    {
                        ThreadManager.RunAction(task);
                        num = 0;
                    }
                    else
                        num += millisecondsTimeout;
                }
                else
                    ThreadManager.RunAction(task);
                task.RunStatus = false;
                Thread.Sleep(millisecondsTimeout);
            }
        }

        private static void RunAction(ThreadManager.ThreadClass task)
        {
            try
            {
                task.Action(task.State);
            }
            catch (Exception ex)
            {
                ex.Error("线程执行异常");
            }
        }

        /// <summary>停止线程</summary>
        public static void StopThread()
        {
            ThreadManager.RunStatus = false;
            ThreadManager.cts.Cancel();
            if (!ThreadManager.ThreadList.Any<ThreadManager.ThreadClass>())
                return;
            Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
            List<ThreadState> threadStateList = new List<ThreadState>()
      {
        ThreadState.Stopped,
        ThreadState.WaitSleepJoin,
        ThreadState.Suspended
      };
            do
            {
                lock (ThreadManager.lockObj)
                {
                    ThreadManager.ThreadClass result;
                    if (ThreadManager.ThreadList.TryTake(out result))
                    {
                        if (!result.RunStatus && threadStateList.Contains(result.WorkTask.ThreadState))
                        {
                            result.WorkTask.DisableComObjectEagerCleanup();
                            result.WorkTask.Abort();
                            result.WorkTask = (Thread)null;
                        }
                        else
                            ThreadManager.ThreadList.Add(result);
                    }
                }
            }
            while (ThreadManager.ThreadList.Count != 0);
            //LogsMrg.Logger.Info((object)"线程结束完毕");
        }

        private class ThreadClass
        {
            public Thread WorkTask { get; set; }

            public bool RunStatus { get; set; }

            public Action<object> Action { get; set; }

            public object State { get; set; }

            public int SleepTime { get; set; }
        }
    }
}
