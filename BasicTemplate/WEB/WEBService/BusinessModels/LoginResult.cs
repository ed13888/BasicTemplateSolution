using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBService.BusinessModels
{
    public class LoginResult : BaseResult
    {
        public int ErrorCount { get; set; }
        public string LoginSessionID { get; set; }
        public int CompanyID { get; set; }
        public bool ForcesChangePWD { get; set; }
        public string HomeView { get; set; }
        public AccountData Data { get; set; }
        public string Url { get; set; } = "";
        public APICodeEnum Code { get; set; }
    }

    public class AccountData
    {

        public string AccountName { get; set; }
        public string UserName { get; set; }
        public string GradeName { get; set; }
        public string Handicap { get; set; }
        public string CreditBalance { get; set; }
        public string CreditTitle { get; set; }
        /// <summary>
        /// 是否代理会员
        /// </summary>
        public bool IsAgent { get; set; }
        /// <summary>
        /// 是否显示分红
        /// </summary>
        public bool IsShowDividend { get; set; }
        /// <summary>
        /// 是否显示日工资
        /// </summary>
        public bool IsShowDayRate { get; set; }
        public bool IsShowOddLimit { get; set; }
        /// <summary>
        /// 是否开放体育竞技
        /// </summary>
        public dynamic IsOpenSports { get; set; }
        /// <summary>
        /// 是否显示用户资料
        /// </summary>
        public bool IsShowUserInfo { get; set; }

    }
}
