using Common.Entity.Business;
using Common.Interface.BusinessInterface;
using Common.Interface.BusinessInterface.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace BLL.Business
{
    public class CustomerTemplateInfoService : ICustomerTemplateInfoService
    {
        public CustomerTemplateInfoService()
        {

        }

        [Dependency]
        public ICustomerTemplateInfoRepository _CustomerTemplateInfoRepository { set; get; }

        public bool Insert(CustomerTemplateInfoEntity m)
        {
            return _CustomerTemplateInfoRepository.Insert(m) > 0;
        }
    }
}
