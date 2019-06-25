using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entity.Business
{
    /// <summary>
    /// 表白墙
    /// </summary>
    public class MessageBoardEntity
    {
        public int FId { get; set; } = 0;
        public string FMessageFrom { get; set; }
        public string FMail { get; set; }
        public string FContent { get; set; }
        public string FRemark { get; set; }
        public DateTime FCreateTime { get; set; }
        public string FProvince { get; set; }
        public string FCity { get; set; }
        public string FIp { get; set; }
    }
}
