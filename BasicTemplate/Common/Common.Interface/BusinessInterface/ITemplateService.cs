using Common.Entity.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interface.BusinessInterface
{
    public interface ITemplateService
    {
        bool Insert(TemplateEntity m);
    }
}
