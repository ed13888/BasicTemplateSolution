using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    /// <summary>延迟加载基类，定义唯一实例</summary>
    /// <typeparam name="T"></typeparam>
    public class LazyServiceBase<T> where T : class, new()
    {
        /// <summary>唯一实例</summary>
        private static readonly Lazy<T> instance = new Lazy<T>((Func<T>)(() => new T()), true);

        /// <summary>获取唯一实例</summary>
        public static T Instance
        {
            get
            {
                return LazyServiceBase<T>.instance.Value;
            }
        }
    }
}
