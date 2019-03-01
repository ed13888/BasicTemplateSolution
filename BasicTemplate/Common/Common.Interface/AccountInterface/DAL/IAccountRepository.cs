using Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interface.AccountInterface.DAL
{
    public interface IAccountRepository
    {
        TAccount AccountLogin(string userName, string passWord, string validateCode, string ipAddress, int companyId, ref int status);
    }
}
