using Common.Entity;
using Common.Interface.AccountInterface.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Account
{
    public class AccountRepository : IAccountRepository
    {
        public TAccount AccountLogin(string userName, string passWord, string validateCode, string ipAddress, int companyId, ref int status)
        {
            status = 0;
            return null;
        }
    }
}
