using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entity
{
    [Serializable]
    public class ConfigEntity
    {
        /// <summary>
        /// 键
        /// </summary>
        public string FKey { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string FValue { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string FRemark { get; set; }
    }
}
