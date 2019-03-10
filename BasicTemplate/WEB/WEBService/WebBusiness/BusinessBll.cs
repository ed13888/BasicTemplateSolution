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
        public static bool CreateTemplate(Controller controller, TemplateEntity entity)
        {
            var val = CommonClass.TemplateService.Insert(entity);
            return true;
        }
    }
}
