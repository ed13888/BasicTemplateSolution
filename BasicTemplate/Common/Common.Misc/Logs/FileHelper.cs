using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Misc
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public static class FileHelper
    {
        private static bool IsRun = false;
        private static List<string> folders = new List<string>();
        private static int fileSaveDays = 30;
        private static Timer timer;

        static FileHelper()
        {
            if (ConfigurationManager.AppSettings["FileSaveDays"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["FileSaveDays"].Value<string>()))
            {
                fileSaveDays = ConfigurationManager.AppSettings["FileSaveDays"].Value<int>();
            }
            timer = new Timer(new TimerCallback(AutoDelete), null, TimeSpan.FromSeconds(1), TimeSpan.FromHours(1));
        }

        /// <summary>
        /// 删除log4日志，默认保留30天
        /// </summary>
        public static void DeleteLogs()
        {
            string logs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            AddDeleteFolder(logs);
        }

        /// <summary>
        /// 添加删除文件夹，默认保留30天
        /// </summary>
        /// <param name="folder">文件夹全路径</param>
        public static void AddDeleteFolder(string folder)
        {
            lock (folders)
            {
                folders.Add(folder);
            }
        }
        /// <summary>
        /// 自动删除
        /// </summary>
        /// <param name="state"></param>
        private static void AutoDelete(object state)
        {
            if (IsRun == false)
            {
                IsRun = true;
                string[] temp = null;
                lock (folders)
                {
                    temp = new string[folders.Count];
                    folders.CopyTo(temp, 0);
                }
                if (temp != null)
                {
                    foreach (var item in temp)
                    {
                        DeleteFiles(item);
                    }
                }
                IsRun = false;
            }
        }

        private static void DeleteFiles(string folder)
        {
            if (!string.IsNullOrWhiteSpace(folder))
            {
                if (Directory.Exists(folder))
                {
                    DirectoryInfo di = new DirectoryInfo(folder);
                    if (di.GetFileSystemInfos().Any() == false)
                    {
                        try
                        {
                            di.Delete();
                        }
                        catch (Exception ex)
                        {
                            ex.Error("删除文件夹异常");
                        }
                    }
                    else
                    {
                        var files = di.GetFileSystemInfos().Where(a => (DateTime.Now - a.CreationTime).Days > fileSaveDays);
                        foreach (var file in files)
                        {
                            if (File.GetAttributes(file.FullName).HasFlag(FileAttributes.Directory))
                            {
                                DeleteFiles(file.FullName);
                            }
                            else
                            {
                                if ((DateTime.Now - file.CreationTime).Days > fileSaveDays)
                                {
                                    try
                                    {
                                        if (file.Exists)
                                        {
                                            file.Delete();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.Error("删除文件异常");
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
