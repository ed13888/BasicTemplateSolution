using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace AdminCode
{
    public class BootStrapper
    {
        /// <summary>
        /// 获取容器-注册依赖关系
        /// </summary>
        /// <returns></returns>
        public static IUnityContainer Initialise()
        {
            var container = BulidUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }

        /// <summary>
        /// 加载容器
        /// </summary>
        /// <returns></returns>
        private static IUnityContainer BulidUnityContainer()
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        }

        /// <summary>
        /// 实施依赖注入
        /// </summary>
        /// <param name="container"></param>
        private static void RegisterTypes(IUnityContainer container)
        {
            ////依赖关系可以选择代码形式，也可以用配置文件的形式
            UnityConfigurationSection config = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);
            config.Configure(container);

            //类型的配置容器注册
            //container.RegisterType<IUserRepository, UserRepository>();
            //container.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());//通过生命周期实现了单例模式

            //已有对象实例的配置容器注册,也为单例
            //UserService userService = new UserService(); 
            //container.RegisterInstance<IUserService>(userService);

            //container.RegisterType<IUserBll, UserBll>();
            //container.RegisterType<IUserDal, UserDal>();
        }
    }
}