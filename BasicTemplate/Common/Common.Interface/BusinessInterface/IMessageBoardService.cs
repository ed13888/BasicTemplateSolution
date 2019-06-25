using Common.Entity.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interface.BusinessInterface
{
    public interface IMessageBoardService
    {
        bool Insert(MessageBoardEntity m);

        IList<MessageBoardEntity> GetList(string strWhere = "");
    }
}
