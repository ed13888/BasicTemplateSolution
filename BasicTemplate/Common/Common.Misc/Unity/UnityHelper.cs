using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Common.Misc
{
    public class UnityHelper
    {
        /// <summary>唯一实例</summary>
        private static IUnityContainer container = null;

        /// <summary>获取唯一实例</summary>
        public static IUnityContainer Container
        {
            get
            {
                return container;
            }
        }

        /// <summary>
        /// 获取容器-注册依赖关系
        /// </summary>
        /// <returns></returns>
        public static IUnityContainer Initialize()
        {
            container = BulidUnityContainer();
            return container;
        }

        /// <summary>
        /// 加载容器
        /// </summary>
        /// <returns></returns>
        private static IUnityContainer BulidUnityContainer()
        {
            var _container = new UnityContainer();
            RegisterTypes(_container);
            return _container;
        }

        /// <summary>
        /// 实施依赖注入
        /// </summary>
        /// <param name="container"></param>
        private static void RegisterTypes(IUnityContainer _container)
        {
            ////依赖关系可以选择代码形式，也可以用配置文件的形式
            UnityConfigurationSection config = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);
            config.Configure(_container);
        }
    }
}
