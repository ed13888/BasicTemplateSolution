using Common.Entity.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interface.BusinessInterface
{
    public interface ICustomerTemplateInfoService
    {
        bool Insert(CustomerTemplateInfoEntity m);
        CustomerTemplateInfoEntity GetByFuid(string fuid);
    }
}
