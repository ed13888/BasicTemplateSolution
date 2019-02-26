using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interface.WcfInterface
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IWCFInterface
    {
        [OperationContract]
        string GetCache(string key);

        [OperationContract]
        List<string> GetAllKeys();

        [OperationContract(IsOneWay = true)]
        void Clear();

        [OperationContract(IsOneWay = true)]
        void Remove(string key);

        [OperationContract(IsOneWay = true)]
        void RemoveAll(string pattern);

        [OperationContract(IsOneWay = true)]
        void RemoveKeys(List<string> keys);

        [OperationContract]
        List<string> SearchKeys(string pattern);

        [OperationContract]
        bool ContainsKey(string key);

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool TestConnect();
    }
}
