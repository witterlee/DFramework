using System;
using System.Collections.Generic;
using System.Reflection;
using DFramework.Utilities;

namespace DFramework.RabbitCommandBus
{
    public static class DFrameworkRabbitCommandBusExtension
    {
        public static DEnvironment UseRabbitCommandBus(this DEnvironment framework, Dictionary<int, Type> commandTypeMapping, Assembly[] assemblies, int queueCount = 3)
        {
            Check.Argument.IsNotNull(assemblies, "assemblies");

            var commandExecutorContainer = new CommandExecutorContainer();

            commandExecutorContainer.RegisterExecutors(assemblies);
            var commandBus = new CommandBus(commandExecutorContainer, commandTypeMapping, queueCount);
            IoC.Register<ICommandBus>(commandBus);

            return framework;
        }
    }
}
