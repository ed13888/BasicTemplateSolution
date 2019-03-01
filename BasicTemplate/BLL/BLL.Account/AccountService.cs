using Common.Entity;
using Common.Interface.AccountInterface;
using Common.Interface.AccountInterface.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Account
{
    public class AccountService : IAccountService
    {
        private IAccountRepository AccountRepository { set; get; }

        public TAccount AccountLogin(string userName, string passWord, string validateCode, string ipAddress, int companyId, ref int status)
        {
            return AccountRepository.AccountLogin(userName, passWord, validateCode, ipAddress, companyId, ref status);
        }
    }
}
