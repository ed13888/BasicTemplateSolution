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
    public class CustomerTemplateInfoRepository : ICustomerTemplateInfoRepository
    {
        public CustomerTemplateInfoRepository()
        {

        }

        public CustomerTemplateInfoEntity GetByFuid(string fuid)
        {
            var sql = $@" select c.*,t.FId as FTemplateId,FName,FImgUrl,FCheckCount,FSentenceCount,FTemplateUrl from TCustomerTemplateInfo c 
                        inner join TTemplate t on c.FTemplateId =t.FId and FUId=@FUId limit 0,1";
            var m = MySqlHelper.Query<CustomerTemplateInfoEntity, TemplateEntity>(null, sql, (tempinfo, temp) =>
            {
                tempinfo.Template = temp;
                return tempinfo;
            }, "FTemplateId",
            new { @FUId = fuid }).FirstOrDefault();
            return m;
        }

        public int Insert(CustomerTemplateInfoEntity m)
        {
            var param = new
            {
                @FUId = m.FUId ?? Guid.NewGuid().ToString("N"),
                @FTitle = m.FTitle,
                @FMusic = m.FMusic,
                @FPhoto = m.FPhoto,
                @FDescription = m.FDescription,
                @FProducer = m.FProducer,
                @FP1 = m.FP1,
                @FP2 = m.FP2,
                @FP3 = m.FP3,
                @FP4 = m.FP4,
                @FP5 = m.FP5,
                @FP6 = m.FP6,
                @FP7 = m.FP7,
                @FP8 = m.FP8,
                @FP9 = m.FP9,
                @FP10 = m.FP10,
                @FExpireDate = DateTime.Now.AddHours(6),
                @FNeedDelete = m.FNeedDelete
            };


            var sql = @"INSERT INTO `TCustomerTemplateInfo`(`FUId`,`FTitle`,`FMusic`,`FPhoto`,`FDescription`,`FProducer`,`FP1`,`FP2`,`FP3`,`FP4`,`FP5`,`FP6`,`FP7`,`FP8`,`FP9`,`FP10`,`FExpireDate`,`FNeedDelete`)
            VALUES(@FUId,@FTitle,@FMusic,@FPhoto,@FDescription,@FProducer,@FP1,@FP2,@FP3,@FP4,@FP5,@FP6,@FP7,@FP8,@FP9,@FP10,@FExpireDate,@FNeedDelete);";

            var val = MySqlHelper.Execute(sql, param);
            return val;
        }
    }
}
