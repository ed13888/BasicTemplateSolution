using Common.Entity.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interface.BusinessInterface.DAL
{
    public interface ICustomerTemplateInfoRepository
    {
        int Insert(CustomerTemplateInfoEntity m);
        CustomerTemplateInfoEntity GetByFuid(string fuid);

    }
}
