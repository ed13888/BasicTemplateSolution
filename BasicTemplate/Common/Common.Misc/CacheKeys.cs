using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    /// <summary>
    /// 缓存KEY
    /// </summary>
    public static class CacheKeys
    {
        private static HashSet<string> hasCompressKeys = new HashSet<string>();

        /// <summary>
        /// 判断是否需要压缩
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool HasNeedCompress(string key)
        {
            bool flag = false;
            foreach (var item in hasCompressKeys)
            {
                if (key.Contains(item))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        static CacheKeys()
        {
            hasCompressKeys.Add(QrcodeRedisKey);
            hasCompressKeys.Add(SubordinateAccounts);
            hasCompressKeys.Add(DirectlySubAccounts);
            hasCompressKeys.Add(CompanyActivesListId);
        }

        public static string WebSiteGroupID = FeatureHelper.WebSiteGroupID;
        /// <summary>
        /// 系统设置
        /// </summary>
        public static string SystemConfigCacheKeys = "SystemConfigCacheKeys_3.4";
        /// <summary>
        /// 域名皮肤
        /// </summary>
        public static string Skin = WebSiteGroupID + "_Skin_3.4";
        /// <summary>
        /// 最新期数
        /// </summary>
        public static string NewPeriodListId = WebSiteGroupID + "_NewPeriodListId_3.4";
        /// <summary>
        /// 游戏分类
        /// </summary>
        public static string GameCategoryList = WebSiteGroupID + "_GameCategoryList_3.4";
        /// <summary>
        /// 匹配游戏
        /// </summary>
        public static string GameMatchList = WebSiteGroupID + "_GameMatchList";
        /// <summary>
        /// 游戏
        /// </summary>
        public static string GameListId_R13_3_3 = WebSiteGroupID + "_GameListId_R13_3_3";
        /// <summary>
        /// 玩法
        /// </summary>
        public static string GamePlayListId = WebSiteGroupID + "_GamePlayList_3.4";
        /// <summary>
        /// 玩法项
        /// </summary>
        public static string GamePlayItemListId = WebSiteGroupID + "_GamePlayItemList_3.4";
        /// <summary>
        /// 用户投注金额
        /// </summary>
        public static string UserBetAmount = WebSiteGroupID + "_UserBetAmount";
        /// <summary>
        /// 公司赔率
        /// </summary>
        public static string CompanyDefaultOdds = WebSiteGroupID + "_CompanyDefaultOdds";
        /// <summary>
        /// 机器人入局设置
        /// </summary>
        public static string CompanyRobotSettings = WebSiteGroupID + "_CompanyRobotSettings";
        /// <summary>
        /// 机器人设置改变
        /// </summary>
        public static string CompanyRobotSettingChange = WebSiteGroupID + "CompanyRobotSettingChange";
        /// <summary>
        /// 机器人列表
        /// </summary>
        public static string CompanyRobot = WebSiteGroupID + "_CompanyRobot";
        /// <summary>
        /// 一级代理与公司赔率差值
        /// </summary>
        public static string AgentDefaultOdds = WebSiteGroupID + "_AgentDefaultOdds";
        /// <summary>
        /// 改变的赔率
        /// </summary>
        public static string ChangeOddsList = WebSiteGroupID + "_ChangeOddsList";
        /// <summary>
        /// 公司设置
        /// </summary>
        public static string CompanySysConfigKey = WebSiteGroupID + "_CompanySysConfigKey_3.4";
        /// <summary>
        /// 公司皮肤
        /// </summary>
        public static string CompanySkinListId = WebSiteGroupID + "_CompanySkinListId";
        /// <summary>
        /// 用户配置
        /// </summary>
        public static string AccountConfigKey = WebSiteGroupID + "_AccountConfig";
        /// <summary>
        /// 用户禁用游戏玩法
        /// </summary>
        public static string UserDisabledGamePlay = WebSiteGroupID + "_UserDisabledGamePlay_3.4";
        /// <summary>
        /// 用户信息
        /// </summary>
        public static string UserInformation = WebSiteGroupID + "_UserInformation";
        /// <summary>
        /// 是否已派彩
        /// </summary>
        public static string PayoutKey = WebSiteGroupID + "_PayoutKey";
        /// <summary>
        /// 代理返点
        /// </summary>
        public static string AgentRebateKey = WebSiteGroupID + "_AgentRebateKey";
        /// <summary>
        /// 推广
        /// </summary>
        public static string GeneralizeId = WebSiteGroupID + "_GeneralizeId";
        /// <summary>
        /// 中奖注单缓存是否已经写入完毕
        /// </summary>
        public static string WinOrdersFinish = WebSiteGroupID + "_WinOrdersFinish";
        /// <summary>
        /// 二维码
        /// </summary>
        public static string QrcodeRedisKey = WebSiteGroupID + "_Qrcode";
        /// <summary>
        /// 公司首页图片
        /// </summary>
        public static string CompanyHomeImageListId = WebSiteGroupID + "_CompanyHomeImageList_3.4";
        /// <summary>
        /// 投注中奖排行
        /// </summary>
        public static string RanKingConfigRedisKey = WebSiteGroupID + "_CompanyRanKingConfig";
        /// <summary>
        /// 排行
        /// </summary>
        public static string RanKingRedisKey = WebSiteGroupID + "_CompanyRanKing_3.4";
        /// <summary>
        /// 用户排行配置
        /// </summary>
        public static string RanKingRedisTypeKey = WebSiteGroupID + "_CompanyRanKingType";
        /// <summary>
        /// 公司活动
        /// </summary>
        public static string CompanyActivesListId = WebSiteGroupID + "_CompanyActivesListId_3.4";
        /// <summary>
        /// 用户上级
        /// </summary>
        public static string SupAccounts = WebSiteGroupID + "_SupAccounts_3.4";
        /// <summary>
        /// 用户直属下级
        /// </summary>
        public static string DirectlySubAccounts = WebSiteGroupID + "_DirectlySubAccounts_3.4";
        /// <summary>
        /// 用户下级
        /// </summary>
        public static string SubordinateAccounts = WebSiteGroupID + "_SubordinateAccounts_3.4";
        /// <summary>
        /// 所有公司
        /// </summary>
        public static string AllCompanyList = WebSiteGroupID + "_AllCompanyList";
        /// <summary>
        /// 限额
        /// </summary>
        public static string AccountLimitListId = WebSiteGroupID + "_AccountLimit_V2";
        /// <summary>
        /// 公司游戏
        /// </summary>
        public static string CompanyGameListId = WebSiteGroupID + "_CompanyGameListId_3.4";
        /// <summary>
        /// 错误密码
        /// </summary>
        public static string ErrorPassword = WebSiteGroupID + "_ErrorPassword";
        /// <summary>
        /// 公司设置
        /// </summary>
        public static string CompanyBasicSettings = WebSiteGroupID + "_CompanyBasicSettings";
        /// <summary>
        /// 游戏玩法分类
        /// </summary>
        public static string GamePlayClassification = WebSiteGroupID + "_GamePlayClassification";
        /// <summary>
        /// 未处理的出入款数据
        /// </summary>
        public static string UnhandledEntrySectionDic = WebSiteGroupID + "_UnhandledEntrySectionDic";
        /// <summary>
        /// 开奖结果是否异常
        /// </summary>
        public static string CheckPeriodResultList = WebSiteGroupID + "_CheckPeriodResultList";
        /// <summary>
        /// 是否系统维护
        /// </summary>
        public static string IsSysMaintain = WebSiteGroupID + "_IsSysMaintain";
        /// <summary>
        /// 是否维护
        /// </summary>
        public static string IsMaintain = WebSiteGroupID + "_IsMaintain";
        /// <summary>
        /// 自动降赔记录
        /// </summary>
        public static string CountRiskDownRecord = WebSiteGroupID + "_CountRiskDownRecord ";
        /// <summary>
        /// 被踢的用户集合
        /// </summary>
        public static string KickAccountList = WebSiteGroupID + "_KickAccountList";
        /// <summary>
        /// 游戏开奖结果
        /// </summary>
        public static string GameResult = WebSiteGroupID + "_GameResult";
        /// <summary>
        /// 未读信息
        /// </summary>
        public static string MessageReminded = WebSiteGroupID + "_MessageReminded";

        /// <summary>
        /// 生成序列号时的全局锁key.
        /// </summary>
        public static string WaterNumberLock = WebSiteGroupID + "_WaterNumberLock";

        /// <summary>
        /// 生成序列号的程序集标识KEY.
        /// </summary>
        public static string WaterNumberProId = WebSiteGroupID + "_WaterNumberProId";
        /// <summary>
        /// 锁定出款记录的key
        /// </summary>
        public static string EncashmentLockRecordId = WebSiteGroupID + "_EncashmentLockRecordId";
        /// <summary>
        /// 用户入款订单号缓存
        /// </summary>
        public static string RechargeOrderNumber = "RechargeOrderNumber";
        /// <summary>
        /// 公司在线人数
        /// </summary>
        public static string CompanyOnlineCount = "CompanyOnlineCount";
        /// <summary>
        /// 公司在线用户列表
        /// </summary>
        public static string CompanyOnlineUserList = "CompanyOnlineUserList";

        /// <summary>
        /// 公司在线用户
        /// </summary>
        public static string CompanyOnlineUser = WebSiteGroupID + "_CompanyOnlineUser";

        /// <summary>
        /// 公司在线用户
        /// </summary>
        public static string CompanyGameOnlineUSer = WebSiteGroupID + "_CompanyGameOnlineUSer";

        /// <summary>
        /// 公司最高在线人数
        /// </summary>
        public static string CompanyMaxOnlineUser = WebSiteGroupID + "_CompanyMaxOnlineUser";

        /// <summary>
        /// 公司某游戏在线人数
        /// </summary>
        public static string CompanyGameOnlineCount = "CompanyGameOnlineCount";
        /// <summary>
        /// 用户历史访问地址
        /// </summary>
        public static string AccountHistoryUrl = "AccountHistoryUrl";

        /// <summary>
        /// 用户微信头像
        /// </summary>
        public static string AccountWeChatHeadImg = nameof(AccountWeChatHeadImg);

        /// <summary>
        /// 用户出款配置缓存
        /// </summary>
        public static string EncashmentChargeConfig = "EncashmentChargeConfig";

        /// <summary>
        /// 用户自动入款配置
        /// </summary>
        public static string AccountBank = "AccountBank";
        /// <summary>
        /// 公司新闻
        /// </summary>
        public static string CompanyNewsListId = WebSiteGroupID + "_CompanyNewsListId_3.4";
        /// <summary>
        /// 猜你喜欢
        /// </summary>
        public static string GuessLikeRedisKey = WebSiteGroupID + "_GuessLike_3.4";
        /// <summary>
        /// 第三方用户登录缓存key
        /// </summary>
        public static string OtherAccountLogin = nameof(OtherAccountLogin);
        /// <summary>
        /// 出码排行
        /// </summary>
        public static string TopRunners = WebSiteGroupID + "_TopRunners";
        /// <summary>
        /// 遗漏排行
        /// </summary>
        public static string ForgetNumberData = WebSiteGroupID + "_ForgetNumberData";
        /// <summary>
        /// 路单
        /// </summary>
        public static string WayBill = WebSiteGroupID + "_WayBill";
        /// <summary>
        /// 跑马灯公告
        /// </summary>
        public static string CompanyNoticeListId = WebSiteGroupID + "_CompanyNoticeListId_3_4";
        /// <summary>
        /// 获取用户短信的未提醒条数
        /// </summary>
        public static string NotRemindedCount = WebSiteGroupID + "_NotRemindedCount";
        /// <summary>
        /// 获取公司游戏资讯信息
        /// </summary>
        public static string CompanyGameInfo = WebSiteGroupID + "_CompanyGameInfo";
        /// <summary>
        /// 契约分红配置
        /// </summary>
        public static string ContractConfigs = "ContractConfigs";
        /// <summary>
        /// 日工资配置
        /// </summary>
        public static string DayWageConfigs = "DayWageConfigs";
        /// <summary>
        /// 佣金设置
        /// </summary>
        public static string CommissionConfigs = "CommissionConfigs";
        /// <summary>
        /// 赔率穿盘提示
        /// </summary>
        public static string DefaultOddsWearPlateTips = "DefaultOddsWearPlateTips";
        /// <summary>
        /// 赔率系数设置
        /// </summary>
        public static string OddsCoefficientConfig = "OddsCoefficientConfig";
        /// <summary>
        /// 皇朝下注信息
        /// </summary>
        public static string RoyalBetInfo = "RoyalBetInfo";
        /// <summary>
        /// APP皇朝下注信息
        /// </summary>
        public static string AppBetInfo = "AppBetInfo";
        /// <summary>
        /// VR扣款讯息
        /// </summary>
        public static string VRDeductMoneyNotifyId = "VRDeductMoneyNotifyId";
        /// <summary>
        /// 系统银行卡
        /// </summary>
        public static string SystemBanks = $"SystemBanks";
        /// <summary>
        /// API 存取款订单号
        /// </summary>
        public static string ApiDepositWithdrawOrderNumber = WebSiteGroupID + "_ApiDepositWithdrawOrderNumber";
        /// <summary>
        /// 公司银行管理
        /// </summary>
        public static string CompanyBankCard = WebSiteGroupID + "_CompanyBankCard";
        /// <summary>
        /// 总公司下发子公司公告
        /// </summary>
        public static string CompanyNotice = WebSiteGroupID + "_SysCompanyNotice";
        #region Redis全局锁
        /// <summary>
        /// 生成追号、取消追号，SetNX全局锁
        /// </summary>
        public static string NXTrackOrder = "NXTrackOrderInsertOrCancel";
        /// <summary>
        /// 追号订单生成或者取消，追号key前缀
        /// </summary>
        public static string TrackDetailsIdIC = "TrackDetailsIdIC";
        /// <summary>
        /// 生成注单，防止太频繁，用户key前缀
        /// </summary>
        public static string AddOrderOften = "AddOrderOften";
        #endregion

        /// <summary>
        /// 四方支付接入系统接口的所有信息
        /// </summary>
        public static string PayInterfaceInfo = "PayInterfaceInfo";

        /// <summary>
        /// 订单对应的支付商户
        /// </summary>
        public static string PayOrderNumberMerchant = "PayOrderNumberMerchant";
        /// <summary>
        /// 总公司
        /// </summary>
        public static string HeadCompanyMaxWins = "HeadCompanyMaxWins";
        /// <summary>
        /// 单个活动缓存
        /// </summary>
        public static string RebateActivitiesEntity = "RebateActivitiesEntity";
        /// <summary>
        /// 红包活动缓存
        /// </summary>
        public static string RedBagActiy = "RedBagActiy";

        /// <summary>
        /// 人工存取款
        /// </summary>
        public static string ManualAccessAudit = "ManualAccessAudit_";

        /// <summary>
        /// 游戏视频缓存Key
        /// </summary>
        public static string GameVideo = "GameVideo";

        /// <summary>
        /// 三方游戏缓存Key
        /// </summary>
        public static string ThirdPartyGameConfig = "ThirdPartyGameConfig";
        /// <summary>
        /// 三方游戏金流类型
        /// </summary>
        public static string ThirdPartyGameCashFlow = "ThirdPartyGameCashFlow";
        /// <summary>
        /// 三方游戏公司后缀
        /// </summary>
        public static string ThirdPartyGameCompanySuffix = "ThirdPartyGameCompanySuffix";
        /// <summary>
        /// 三方游戏系统游戏信息
        /// </summary>
        public static string ThirdPartyGameSysGame = "ThirdPartyGameSysGame";
        /// <summary>
        /// VR期数缓存
        /// </summary>
        public static string VRPeriod = "VRPeriod";
        /// <summary>
        /// 前台获取单挑设置缓存
        /// </summary>
        public static string PlayMoneyLimit = "PlayMoneyLimit_{0}_{1}";

        /// <summary>
        /// WCF注册中心
        /// </summary>
        public static string WCFRegisterCenter = "WCFRegisterCenter_R13.3.4";
        /// <summary>
        /// 三方游戏AFB足球代理缓存KEY
        /// </summary>
        public static string ThirdPartyGameAFBAgent = "ThirdPartyGameAFBAgent";
        /// <summary>
        /// 三方游戏WM体育代理缓存KEY
        /// </summary>
        public static string ThirdPartyGameWMAgent = "ThirdPartyGameWMAgent";
        /// <summary>
        /// 三方游戏厂商缓存KEY
        /// </summary>
        public static string ThirdPartyGameHalls = "ThirdPartyGameHalls";
        /// <summary>
        /// 三方游戏类别缓存KEY
        /// </summary>
        public static string ThirdPartyGameCategory = "ThirdPartyGameCategory";
        /// <summary>
        /// 三方游戏公司启用缓存KEY
        /// </summary>
        public static string ThirdPartyGameCompanyEnabled = "CompanyEnabledThirdPartyGame";
        /// <summary>
        /// 电子游戏列表
        /// </summary>
        public static string GetElectronicGameList = "ElectronicGameList";

        /// <summary>
        /// 棋牌游戏列表
        /// </summary>
        public static string GetChessCardGameList = "ChessCardGameList";

        /// <summary>
        /// Play'nGo用户名缓存KEY
        /// </summary>
        public static string PlaynGoUserName = "PlaynGoUserName";

        /// <summary>
        /// 已经接过水的期数
        /// </summary>
        public static string CollectionResult = "CollectionResult";
        /// <summary>
        /// VR玩法数据
        /// </summary>
        public static string VRGamePlayItem = "VRGamePlayItem";

        /// <summary>
        /// VR代理商缓存Key
        /// </summary>
        public static string VRMerchantId = "VRMerchantId";

        /// <summary>
        /// 公司活动（红包活动专用）
        /// </summary>
        public static string CompanyRedBagActivesListId = WebSiteGroupID + "_CompanyRedBagActivesListId_3.4";

        /// <summary>
        /// 防封Url
        /// </summary>
        public static string TDoorPlankUrlName = "TDoorPlankUrl_";

        /// <summary>
        /// 获取客服系统数据
        /// </summary>
        public static string GetCustomerSystemKey = "GetCustomerSystemKey_{0}_{1}";

        /// <summary>
        /// 获取自营彩利润率设置
        /// </summary>
        public static string GetTSelfGamblingSetting = "GetTSelfGamblingSetting_{0}_{1}";

        /// <summary>
        /// VG用户缓存
        /// </summary>
        public static string VGCacheUserKey = "VGGameUserCacheKey";

        /// <summary>
        /// VG游戏列表缓存
        /// </summary>
        public static string VGGamelistCacheUserKey = "VGGamelistCacheUserKey";

        /// <summary>
        /// 第三方游戏OBTY代理Token 缓存
        /// </summary>
        public static string ThirdPartyGameOrbitxToken = "ThirdPartyGameOrbitxToken";
        /// <summary>
        /// 体育游戏参数设置缓存设置
        /// </summary>
        public static string SportsGameParamConfig = "SportsGameParamConfig";

        /// <summary>
        /// 游戏入口配置缓存
        /// </summary>
        public static string GameEntranceConfig = "GameEntranceConfig";

        /// <summary>
        /// 余额转第三方平台缓存
        /// {0}公司{1}用户
        /// </summary>
        public static string AccountBalanceToTransfer = "AccountBalancePlatform_{0}_{1}";



    }
}
