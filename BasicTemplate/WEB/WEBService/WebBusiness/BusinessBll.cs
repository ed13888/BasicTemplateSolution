using Common.Entity.Business;
using Common.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WEBService.WebBusiness
{
    public class BusinessBll
    {
        public static IList<TemplateEntity> GetTemplateList(Controller controller, string strWhere = "")
        {
            return CommonClass.TemplateService.GetList(strWhere);
        }
        public static TemplateEntity GetTemplateById(Controller controller, int id)
        {
            return CommonClass.TemplateService.GetById(id);
        }
        public static bool IncreaceTemplateCheckCount(Controller controller, int id)
        {
            return CommonClass.TemplateService.IncreaceCheckCount(id);
        }


        public static bool CreateTemplate(Controller controller, CustomerTemplateInfoEntity entity)
        {
            var val = CommonClass.CustomerTemplateInfoService.Insert(entity);
            return val;
        }
        public static CustomerTemplateInfoEntity GetCustomerTemplateById(Controller controller, string fuid)
        {
            var m = CommonClass.CustomerTemplateInfoRepository.GetByFuid(fuid);
            return m;
        }

        public static IList<MessageBoardEntity> GetMessageBoardList(Controller controller, string strWhere = "")
        {
            return CommonClass.MessageBoardService.GetList(strWhere);
        }
        public static bool CreateMessageBoard(Controller controller, MessageBoardEntity entity)
        {
            var val = CommonClass.MessageBoardService.Insert(entity);
            return val;
        }




    }
}
