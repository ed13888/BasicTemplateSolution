using Common.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceService
{
    public class TaskService : LazyServiceBase<TaskService>
    {
        public void Start()
        {
            //定时器，在线支付重新统计今日入款金额
            System.Timers.Timer t = new System.Timers.Timer()
            {
                Interval = 1000,
                AutoReset = true,
                Enabled = true
            };
            t.Elapsed += new System.Timers.ElapsedEventHandler(TimerElapsed);

            //ThreadManager.AddThread(AutoLoadUntreatedOrders, TimeSpan.FromSeconds(20).TotalMilliseconds);
            //ThreadManager.AddThread(AutoLoadNoPayoutPeriod, TimeSpan.FromMinutes(1).TotalMilliseconds);
            //ThreadManager.AddThread(AutoCancellHalfHourData, TimeSpan.FromMinutes(3).TotalMilliseconds);
            //ThreadManager.AddThread(AutoBackCashPeriod, TimeSpan.FromMinutes(1).TotalMilliseconds);
            //ThreadManager.AddThread(AutoUpdateBankCardsCurrentAmount, TimeSpan.FromSeconds(20).TotalMilliseconds);
        }

        private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var executeTime = DateTime.Parse("02:01");
            int minute = e.SignalTime.Minute;
            int second = e.SignalTime.Second;
            int millsecond = e.SignalTime.Millisecond;
        }
    }
}
