using Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interface.AccountInterface
{
    public interface IAccountService
    {
        TAccount AccountLogin(string userName, string passWord, string validateCode, string ipAddress, int companyId, ref int status);
    }
}
