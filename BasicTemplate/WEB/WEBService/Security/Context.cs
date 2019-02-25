using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBService.Security
{
    [Serializable]
    public class Context
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public int GradeId { get; set; }

        public string GradeName { get; set; }

        public int CompanyId { get; set; }

        public Role Role { get; set; }

        public int Status { get; set; }

        public int LoginStatus { get; set; }

        public int AuthorityID { get; set; }

        public int AuthorityGradeID { get; set; }

        public int ParentID { get; set; }

        public string Ip { get; set; }
        /// <summary>
        /// 是否代理
        /// </summary>
        public bool IsAgent { get; set; }

        /// <summary>
        /// 是否为API登录
        /// </summary>
        public bool IsAPI { get; set; }
        /// <summary>
        /// 是否要强制修改密码
        /// </summary>
        public bool ForcesChangePWD { get; set; }
        /// <summary>
        /// 是否测试账号
        /// </summary>
        public bool IsTestAccount { get; set; }

        /// <summary>
        /// 用户登录的Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 登录时的GUID
        /// </summary>
        public string GuidKey { get; set; }

        ///// <summary>
        ///// 最后请求时间
        ///// </summary>
        //public DateTime LastRequestTime { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime? LoginTime { get; set; }
        /// <summary>
        /// 是否推广用户
        /// </summary>
        public bool IsGeneralize { get; set; }
        /// <summary>
        /// 登录SessionID
        /// </summary>
        public string LoginSessionID { get; set; }
        /// <summary>
        /// 是否公司客服账号
        /// </summary>
        public bool IsCustomerServiceAccount { get; set; }

        public string HomeView { get; set; }
        /// <summary>
        /// 当前玩的游戏ID
        /// </summary>
        public int CurrentPlayGameId { get; set; }

        public int OldPlayGameId { get; set; }
    }
    public enum Role
    {
        User,
        Proxy,
        Administrator,
        Guest
    }

    [Serializable]
    public class UserToken
    {
        public int UserID { get; set; }
        public string Token { get; set; }
    }
}
