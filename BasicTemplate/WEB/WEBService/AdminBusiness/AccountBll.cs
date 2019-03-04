using Common.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WEBService.BusinessModels;
using WEBService.BusinessModels.ParamModel;
using WEBService.Security;

namespace WEBService.AdminBusiness
{
    public class AccountBll
    {

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="skin"></param>
        /// <param name="isMobile">是否为手机版</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static LoginResult Login(Controller controller, bool isMobile, LoginData data)
        {
            int status = 0;
            var user = CommonClass.AccountService.AccountLogin(data.UserName, data.Password, data.ValidateCode, "", 0, ref status);

            Context context = new Context();
            context.Id = user.FAccountID;
            //context.GradeId = gradeId;
            context.UserName = user.FAccount;
            //context.GradeName = GradeName;
            //context.CompanyId = IsCustomerServiceAccount ? skin.CompanyID : row.Element("fcompanyid").Value<int>();
            context.Role = user.FRole.StringToEnum<Role>();
            //context.ParentID = row.Element("fparentid") == null || string.IsNullOrWhiteSpace(row.Element("fparentid").Value) ? -1 : Int32.Parse(row.Element("fparentid").Value);
            //context.ForcesChangePWD = row.Element("fforceschangepwd") == null ? false : bool.Parse(row.Element("fforceschangepwd").Value);
            //context.IsAgent = isAgent;
            //context.Ip = ipAddress;
            //context.IsCustomerServiceAccount = IsCustomerServiceAccount;
            //context.Status = status;
            //context.LoginTime = DateTime.Now;

            //bool isBranchCompany = context.AuthorityGradeID == FeatureHelpers.BranchCompany && !IsCustomerServiceAccount;

            if (Authentication.Login(context))
            {
                //var key = $"{CacheKeys.ErrorPassword}_{userId}";
                //CacheHelper.Remove(key);
                //info = $"登录日志：用户编号={Authentication.Id}，用户名称={Authentication.UserName}，等级名称={Authentication.GradeName}，所属公司={Authentication.CompanyID}，登录时间={DateTime.Now:yyyy-MM-dd HH:mm:ss}，登录IP={ipAddress}";
                //CustomLogger.WriterLog(LXYD.WebGame.Util.EnumHelpers.LogType.Login, info);
                //CommonAction.WriteLoginLog(1, userId, "", context, skin);
                var url = "/Admin/Index";
                return new LoginResult { Status = true, Info = "", Url = url };
            }


            return new LoginResult { Status = false, Info = "", Code = APICodeEnum.ConnectedCodeError };
        }
    }
}
