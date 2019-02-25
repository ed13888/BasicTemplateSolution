using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    public partial class FeatureHelpers
    {
        public static string GetEncryptionCode()
        {
            return "$u#e7$4g7asdrs*ds&7=";
        }

        /// <summary>
        /// 是否API接口（前后端分离）
        /// </summary>
        public static bool IsApi => ConfigurationManager.AppSettings["IsApi"].Value<bool>();
        /// <summary>
        /// 是否APP接口（原生APP）
        /// </summary>
        public static bool IsApp => ConfigurationManager.AppSettings["IsApp"].Value<bool>();
    }
}
