using Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interface
{
    public interface ISystemConfigRepository
    {
        /// <summary>
        /// 新增WCF注册地址
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int InsertWcfRegister(TWcfRegister model);

        /// <summary>
        /// 修改WCF注册地址
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateWcfRegister(TWcfRegister model);

        /// <summary>
        /// 删除WCF注册地址
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int DeleteWcfRegister(TWcfRegister model);

        /// <summary>
        /// 获取WCF注册集合
        /// </summary>
        /// <returns></returns>
        List<TWcfRegister> GetWcfRegisterList();
    }
}
