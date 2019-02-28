using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBService.BusinessModels
{
    /// <summary>
    /// API 返回状态code枚举
    /// </summary>
    public enum APICodeEnum
    {
        /// <summary>
        /// 默认值
        /// </summary>
        [Description("默认值")]
        Default = 0,

        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 1,
        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Failure = 3,

        /// <summary>
        /// 用户无权限
        /// </summary>
        [Description("用户无权限")]
        NoPermission = 404,

        [Description("正在维护")]
        Maintain = 999,

        /// <summary>
        /// 未登录
        /// </summary>
        [Description("未登录")]
        NotLogin = 1001,

        /// <summary>
        /// 未指定调用方法
        /// </summary>
        [Description("未指定调用方法")]
        EmptyMethod = 1002,

        /// <summary>
        /// 未指定调用方法
        /// </summary>
        [Description("指定的方法未找到")]
        UnFindMethod = 1003,

        /// <summary>
        /// 执行方法出现异常
        /// </summary>
        [Description("执行方法出现异常")]
        MethodException = 1004,

        /// <summary>
        /// 请求参数为空
        /// </summary>
        [Description("请求参数为空")]
        NullParameter = 1005,

        /// <summary>
        /// 请求方法中相关设置为空（日工资/分红）
        /// </summary>
        [Description("请求方法中相关设置为空（日工资/分红）")]
        EmptySetting = 1006,

        [Description("账户停用")]
        AccountDisabled = 1007,

        [Description("第三方账户绑定")]
        OtherAccountTransit = 1008,

        [Description("账户为空")]
        AccountEmpty = 1009,

        [Description("密码为空")]
        PasswordEmpty = 1010,

        [Description("Token为空")]
        TokenEmpty = 1011,

        [Description("验证码为空")]
        ValidateCodeEmpty = 1012,

        [Description("验证码错误")]
        ValidateCodeError = 1013,

        [Description("验证码过期")]
        ValidateCodeExpire = 1014,

        [Description("IP在黑名单")]
        IPInBlacklist = 1015,

        [Description("账户不存在")]
        AccountNotExist = 1016,

        [Description("密码错误")]
        PasswordError = 1017,

        [Description("登录异常")]
        LoginError = 1018,

        [Description("等级错误")]
        GradeError = 1019,

        [Description("登录限制")]
        LoginLimit = 1020,

        [Description("域名错误")]
        DomainError = 1021,

        [Description("首页注册关闭")]
        HomeRegisterClose = 1022,

        [Description("注册频繁")]
        RegisterFrequently = 1023,

        [Description("账户已存在")]
        AccountAlreadyExist = 1024,

        [Description("注册超过限制")]
        RegisterExceedLimit = 1025,

        [Description("注册异常")]
        RegisterException = 1026,

        [Description("谷歌验证码为空")]
        GoogleValidateCodeEmpty = 1027,

        [Description("谷歌验证码错误")]
        GoogleValidateCodeError = 1028,

        [Description("通信服务异常")]
        ConnectedCodeError = 1029,

        [Description("IP限制")]
        IPLimit = 1030,

        [Description("多点登录")]
        MultiLogin = 1031,
    }
}
