using Common.Entity.Business;
using Common.Misc;
using Common.Misc.SQL;
using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimerService.Task
{
    public class CleanTask : IJob
    {
        private static readonly object _lock = new object();
        public void Execute()
        {
            lock (_lock)
            {
                try
                {
                    var list = MySqlHelper.Query<CustomerTemplateInfoEntity>("", "select * from Tcustomertemplateinfo where FNeedDelete=true and FPhoto !='' and FExpireDate <now()").ToList();
                    //var splitExp = new char[] { ';' };
                    //var imgList = item.FPhoto.Split(splitExp, StringSplitOptions.RemoveEmptyEntries);
                    var path = ConfigurationManager.AppSettings["path"];
                    foreach (var item in list)
                    {
                        var dirPath = Path.Combine(path, item.FUId);
                        if (Directory.Exists(dirPath))
                        {
                            DirectoryInfo dir = new DirectoryInfo(dirPath);
                            dir.Delete(true);

                            LogsManager.Info($"[清理上传图片]FUID:{item.FUId}.");
                            Thread.Sleep(1000);
                        }
                    }


                }
                catch (Exception ex)
                {
                    ex.Error("清理任务异常");
                }
            }

        }
    }
}
