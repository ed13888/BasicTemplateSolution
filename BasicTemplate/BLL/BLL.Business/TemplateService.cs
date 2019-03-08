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
    public class TemplateService : ITemplateService
    {
        public TemplateService()
        {

        }

        [Dependency]
        public ITemplateRepository _TemplateRepository { set; get; }

        public bool Insert(TemplateEntity m)
        {
            return _TemplateRepository.Insert(m) > 0;
        }
    }
}
