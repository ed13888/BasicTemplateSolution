using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Common.Misc
{
    public partial class FeatureHelper
    {
        public static string GetEncryptionCode()
        {
            return "$u#e7$4g7asdrs*ds&7=";
        }

        /// <summary>
        /// 服务器组
        /// </summary>
        public const string WebSiteGroupID = "OfficialCash";

        /// <summary>
        /// 是否API接口（前后端分离）
        /// </summary>
        public static bool IsApi => ConfigurationManager.AppSettings["IsApi"].Value<bool>();
        /// <summary>
        /// 是否APP接口（原生APP）
        /// </summary>
        public static bool IsApp => ConfigurationManager.AppSettings["IsApp"].Value<bool>();

        /// <summary>
        /// Redis “写”链接池链接数 
        /// </summary>
        public static int MaxWritePoolSize
        {
            get
            {
                XElement xml = null;// FeatureItem.Get("RedisConfig").Value;
                //return Convert.ToInt32(xml.Element("RedisConfig").Element("MaxWritePoolSize").Value);
                return 100;
            }
        }
        /// <summary>
        /// Redis “读”链接池链接数 
        /// </summary>
        public static int MaxReadPoolSize
        {
            get
            {
                XElement xml = null;//FeatureItem.Get("RedisConfig").Value;
                //return Convert.ToInt32(xml.Element("RedisConfig").Element("MaxReadPoolSize").Value);
                return 100;
            }
        }

        public static string DB_Main { get; set; } = "DB_Main";

        public static string DB_ReadOnly { get; set; } = "DB_ReadOnly";

    }
}
