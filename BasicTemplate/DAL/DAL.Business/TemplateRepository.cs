using Common.Entity.Business;
using Common.Interface.BusinessInterface.DAL;
using Common.Misc.SQL;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Business
{
    public class TemplateRepository : ITemplateRepository
    {
        public TemplateRepository()
        {

        }
        public int Insert(TemplateEntity m)
        {
            var param = new DynamicParameters();
            param.Add("@FUId", Guid.NewGuid().ToString("N"), DbType.String);
            param.Add("@FTitle", m.FTitle, DbType.String);
            param.Add("@FMusic", m.FMusic, DbType.String);
            param.Add("@FPhoto", m.FPhoto, DbType.String);
            param.Add("@FDescription", m.FDescription, DbType.String);
            param.Add("@FProducer", m.FProducer, DbType.String);
            param.Add("@FP1", m.FP1, DbType.String);
            param.Add("@FP2", m.FP2, DbType.String);
            param.Add("@FP3", m.FP3, DbType.String);
            param.Add("@FP4", m.FP4, DbType.String);
            param.Add("@FP5", m.FP5, DbType.String);
            param.Add("@FP6", m.FP6, DbType.String);
            param.Add("@FP7", m.FP7, DbType.String);
            param.Add("@FP8", m.FP8, DbType.String);
            param.Add("@FP9", m.FP9, DbType.String);
            param.Add("@FP10", m.FP10, DbType.String);
            var sql = @"INSERT INTO `ttemplate`(`FUId`,`FTitle`,`FMusic`,`FPhoto`,`FDescription`,`FProducer`,`FP1`,`FP2`,`FP3`,`FP4`,`FP5`,`FP6`,`FP7`,`FP8`,`FP9`,`FP10`)
            VALUES(@FUId,@FTitle,@FMusic,@FPhoto,@FDescription,@FProducer,@FP1,@FP2,@FP3,@FP4,@FP5,@FP6,@FP7,@FP8,@FP9,@FP10);";

            var val = MySqlHelper.Execute(sql, param);
            return val;
        }
    }
}
