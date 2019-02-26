using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TimerService
{
    public partial class CommonService : ServiceBase
    {
        private readonly MainService main = new MainService();
        public CommonService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            main.Start();
        }

        protected override void OnStop()
        {
            main.Stop();
        }
    }
}
