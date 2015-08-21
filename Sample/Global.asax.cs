using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Couchbase.Configuration.Client;
using Couchbase.Configuration.Client.Providers;
using DFramework;
using DFramework.Autofac;
using DFramework.Log4net;
using DFramework.Memcached;
using DFramework.RabbitCommandBus;
using RabbitMQ.Client;
using Sample.Command;

namespace Sample
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var configSection =
                (CouchbaseClientSection)ConfigurationManager.GetSection("couchbaseClients/couchbaseCache");

            var clientConfig = new ClientConfiguration(configSection);


            DEnvironment.Initialize()
                        .UseAutofac()
                        .UseDefaultJsonSerializer()
                        .UseMemcached("10.0.0.200")
                        .UseLog4net()
                        .UseRabbitCommandBus(RegisterTypeCode(), GetAllAssembly(),3);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        #region 获取程序集
        private static Assembly[] GetAllAssembly()
        {
            List<Assembly> assemlies = new List<Assembly>();

            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            Directory.GetFiles(basePath, "*.dll", SearchOption.AllDirectories)
                     .Where(file => file.IndexOf(".dll", StringComparison.OrdinalIgnoreCase) > 0 &&
                                    file.IndexOf("Command", StringComparison.OrdinalIgnoreCase) > 0)
                     .ForEach(dll =>
                     {
                         assemlies.Add(Assembly.LoadFrom(dll));
                     });

            return assemlies.ToArray();
        }
        #endregion

        #region Register Type Code

        private Dictionary<int, Type> RegisterTypeCode()
        {
            var factory = new ConnectionFactory { Uri = "amqp://rabbit:rabbit@10.0.0.200/test2", AutomaticRecoveryEnabled = true };
            IoC.Register<IConnectionFactory>(factory);
            var dic = new Dictionary<int, Type>();
            dic.Add(100, typeof(TestCommand));
            dic.Add(101, typeof(TestHasReturnValueCommand));

            return dic;
        }

        #endregion
    }
}
