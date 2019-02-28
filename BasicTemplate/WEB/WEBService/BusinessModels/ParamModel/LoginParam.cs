using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBService.BusinessModels.ParamModel
{
    [Serializable]
    public class LoginData
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string ValidateCode { get; set; }
        /// <summary>
        /// Google验证码
        /// </summary>
        public string GoogleCode { get; set; } = "";
        /// <summary>
        /// 账户id(限额用户id)/用户获取谷歌验证码缓存的key
        /// </summary>
        public int AccountFid { get; set; } = 0;

        public string OnlyFlag { get; set; }

        public string ClientFlag { get; set; }

    }
}
