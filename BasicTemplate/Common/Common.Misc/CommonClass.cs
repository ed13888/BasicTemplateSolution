using Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    public static class CommonClass
    {
        #region <Service>
        #endregion

        #region <Repository>
        public static ISystemConfigRepository SystemConfigRepository => null;// ObjectFactory.GetInstance<ISystemConfigRepository>();
        #endregion
    }
}
