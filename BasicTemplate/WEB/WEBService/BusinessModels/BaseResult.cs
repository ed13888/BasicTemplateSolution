using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBService.BusinessModels
{
    /// <summary>
    /// 通用返回对象
    /// </summary>
    public class BaseResult
    {
        public bool Status { get; set; }
        public string Info { get; set; }
        public APICodeEnum Code { get; set; }

        public BaseResult()
        {
            Status = false;
            Info = "参数不能为空";
        }

        public BaseResult(bool status, string info)
        {
            Status = status;
            Info = info;
        }

        public BaseResult(bool status, string info, APICodeEnum code)
        {
            Status = status;
            Info = info;
            Code = code;
        }
    }
}
