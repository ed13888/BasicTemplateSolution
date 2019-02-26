using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Common.Misc
{
    /// <summary>
    /// 服务配置帮助类
    /// </summary>
    public static class ServiceConfigHelper
    {
        private static string RootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
        private static XElement Root;

        static ServiceConfigHelper()
        {
            if (File.Exists(ConfigPath) == false)
            {
                Root = new XElement("Root");
                Root.Save(ConfigPath);
            }
            else
            {
                Root = XElement.Load(ConfigPath);
            }
        }

        private static string ConfigPath
        {
            get
            {
                return Path.Combine(RootPath, "ServiceConfig.xml"); ;
            }
        }

        /// <summary>
        /// 获取配置节点值
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static string GetConfig(string nodeName)
        {
            if (Root.Element(nodeName) != null)
            {
                return Root.Element(nodeName).Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllConfig()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (Root.Elements().Any())
            {
                foreach (var item in Root.Elements())
                {
                    dic[item.Name.LocalName] = item.Value;
                }
            }
            return dic;
        }

        /// <summary>
        /// 设置节点配置值
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="value"></param>
        public static void SetConfig(string nodeName, string value)
        {
            if (Root.Element(nodeName) != null)
            {
                Root.Element(nodeName).Value = value;
            }
            else
            {
                Root.Add(new XElement(nodeName, value));
            }
            Root.Save(ConfigPath);
        }
    }
}
