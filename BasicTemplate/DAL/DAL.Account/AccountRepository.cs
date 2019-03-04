using Common.Entity;
using Common.Interface.AccountInterface.DAL;
using Common.Misc.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Account
{
    public class AccountRepository : IAccountRepository
    {
        public AccountRepository()
        {

        }


        public TAccount AccountLogin(string userName, string passWord, string validateCode, string ipAddress, int companyId, ref int status)
        {
            status = 0;
            var t = MySqlHelper.FirstOrDefault<TAccount>(databaseName: "DB",
                storedProcName: "select * from TAccount where FAccount=@FUserName and FLogonPass=@FLogonPass",
                param: new { @FUserName = userName, @FLogonPass = passWord },
                commandType: CommandType.Text);
            if (t != null) status = 0;
            return t;
        }
    }
}
