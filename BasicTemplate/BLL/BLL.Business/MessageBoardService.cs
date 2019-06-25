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
    public class MessageBoardService : IMessageBoardService
    {
        public MessageBoardService()
        {

        }

        [Dependency]
        public IMessageBoardRepository _MessageBoardRepository { set; get; }

        public IList<MessageBoardEntity> GetList(string strWhere = "")
        {
            return _MessageBoardRepository.GetList(strWhere);
        }

        public bool Insert(MessageBoardEntity m)
        {
            return _MessageBoardRepository.Insert(m) > 0;
        }
    }
}
