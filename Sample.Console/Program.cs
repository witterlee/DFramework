using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DFramework;
using DFramework.Autofac;
using DFramework.Log4net;
using DFramework.Memcached;
using DFramework.RabbitCommandBus;
using RabbitMQ.Client;
using Sample.Command;

namespace Sample.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory { Uri = "amqp://rabbit:rabbit@10.0.0.200/test2", AutomaticRecoveryEnabled = true };

            DEnvironment.Initialize()
                        .UseAutofac()
                        .UseDefaultJsonSerializer()
                        .UseMemcached("10.0.0.200")
                        .UseLog4net()
                        .UseRabbitCommandBus(connectionFactory, RegisterTypeCode(), GetAllAssembly());

            int i = 1000, j = 10;

            Stopwatch sw = Stopwatch.StartNew();
            var taskList = new List<Task>();
            var bus = IoC.Resolve<ICommandBus>();

            while (j-- > 0)
            {
                i = 10000;
                while (i-- > 0)
                {
                    var cmd = new TestCommand("nameA", "pwdB");
                    taskList.Add(bus.SendAsync(cmd));
                }

                Task.WhenAll(taskList).Wait();
            }
            sw.Stop();
            System.Console.WriteLine(sw.ElapsedMilliseconds);
            System.Console.Read();
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

        private static Dictionary<int, Type> RegisterTypeCode()
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
