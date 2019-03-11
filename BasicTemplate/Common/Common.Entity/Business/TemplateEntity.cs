using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entity.Business
{
    public class TemplateEntity
    {
        public int FId { get; set; } = 0;
        public string FName { get; set; }
        public string FImgUrl { get; set; }
        public int FCheckCount { get; set; }
        public int FSentenceCount { get; set; }
    }
}
