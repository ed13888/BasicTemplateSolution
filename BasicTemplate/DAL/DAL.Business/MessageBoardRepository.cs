using Common.Entity.Business;
using Common.Interface.BusinessInterface.DAL;
using Common.Misc.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Business
{
    public class MessageBoardRepository : IMessageBoardRepository
    {
        public IList<MessageBoardEntity> GetList(string strWhere = "")
        {
            var sql = "select * from TMessageBoard";
            var list = MySqlHelper.Query<MessageBoardEntity>(null, sql).ToList();
            return list;
        }

        public int Insert(MessageBoardEntity m)
        {
            var param = new
            {
                @FMessageFrom = m.FMessageFrom,
                @FMail = m.FMail,
                @FContent = m.FContent,
                @FProvince = m.FProvince,
                @FCity = m.FCity,
                @FCreateTime = DateTime.Now,
                @FRemark = m.FRemark,
                @FIp = m.FIp,
            };


            var sql = @"INSERT INTO `TMessageBoard`(`FMessageFrom`,`FMail`,`FContent`,`FProvince`,`FCity`,`FCreateTime`,`FRemark`,`FIp`)
            VALUES(@FMessageFrom,@FMail,@FContent,@FProvince,@FCity,@FCreateTime,@FRemark,@FIp);";

            var val = MySqlHelper.Execute(sql, param);
            return val;
        }
    }
}
