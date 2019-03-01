using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entity
{
    /// <summary>
    /// 用户基础信息表
    /// </summary>
    public class TAccount
    {
        /// <summary>
        /// ID
        /// </summary>
        public int FID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string FAccount { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string FLogonPass { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string FRole { get; set; }

        /// <summary>
        /// 安全密码
        /// </summary>
        public string FInsurePass { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string FNickname { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime FRegisterDate { get; set; }

        /// <summary>
        /// 登陆次数
        /// </summary>
        public int FLogonTimes { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public Nullable<DateTime> FLastLogonDate { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        public string FLastLogonIP { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string FEmail { get; set; }

        /// <summary>
        /// 用户状态0停用、1启用、2暂停
        /// </summary>
        public int FStatus { get; set; }

        /// <summary>
        /// 上级用户
        /// </summary>
        public int FParentID { get; set; }

        /// <summary>
        /// 等级标识
        /// </summary>
        public int FGradeID { get; set; }

        /// <summary>
        /// 根标识-所属公司标识
        /// </summary>
        public int FCompanyID { get; set; }

        /// <summary>
        /// 是否直属公司
        /// </summary>
        public bool FIsAffiliate { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public Nullable<DateTime> FLastUpdateDate { get; set; }
        /// <summary>
        /// 用户余额
        /// </summary>
        public decimal FBalance { get; set; }
        /// <summary>
        /// 用户层级
        /// </summary>
        public int FHierarchy { get; set; }
        /// <summary>
        /// 注册IP
        /// </summary>
        public string FRegisterIP { get; set; }

        /// <summary>
        /// 所属总代
        /// </summary>
        public int FTotalGeneration { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public int OnlineStatus { get; set; }

        /// <summary>
        /// 代理类型（1：公司号，2：直属号）
        /// </summary>
        public int FAgentType { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string FRealName { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string FPhoneNo { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string FQQ { get; set; }
        /// <summary>
        /// 取款密码
        /// </summary>
        public string FWithdrawalPass { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string FWeChat { get; set; }
        /// <summary>
        /// 是否有取款功能
        /// </summary>
        public bool FIsWithdraw { get; set; }

        /// <summary>
        /// 注册方式
        /// </summary>
        public string FRegistration { get; set; }

        /// <summary>
        /// 注册地址
        /// </summary>
        public string FRegistrUrl { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public int FAccountID { get; set; }
        /// <summary>
        /// 存款笔数
        /// </summary>
        public int FDepositCount { get; set; }
        /// <summary>
        /// 存款金额
        /// </summary>
        public decimal FDepositAmount { get; set; }
        /// <summary>
        /// 最大存款金额
        /// </summary>
        public decimal FMaxDepositAmount { get; set; }
        /// <summary>
        /// 取款笔数
        /// </summary>
        public int FDrawingCount { get; set; }
        /// <summary>
        /// 取款金额
        /// </summary>
        public decimal FDrawingAmount { get; set; }
        /// <summary>
        /// 是否锁住
        /// </summary>
        public bool FIsLock { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public int FCurrencyID { get; set; }
        /// <summary>
        /// 所属总代 名称
        /// </summary>
        public string FTotalGenerations { get; set; }
        /// <summary>
        /// 给用户注册日志新增-  操作人
        /// </summary>
        public string FOperator { get; set; }

    }
}
