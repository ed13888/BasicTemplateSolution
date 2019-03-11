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
        public static bool CreateTemplate(Controller controller, CustomerTemplateInfoEntity entity)
        {
            var val = CommonClass.CustomerTemplateInfoRepository.Insert(entity);
            return true;
        }
        public static CustomerTemplateInfoEntity GetCustomerTemplateById(Controller controller, string fuid)
        {
            var m = CommonClass.CustomerTemplateInfoRepository.GetByFuid(fuid);
            return m;
        }
    }
}
