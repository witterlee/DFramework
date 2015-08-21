using System.Reflection;
using DFramework.Utilities;

namespace DFramework
{
    public static class DFrameworkCommandBusExtension
    {
        public static DEnvironment UseDefaultCommandBus(this DEnvironment framework, params Assembly[] assemblies)
        {
            Check.Argument.IsNotNull(assemblies, "assemblies");

            var commandExecutorContainer = new CommandExecutorContainer();

            commandExecutorContainer.RegisterExecutors(assemblies);
            var commandBus = new DefaultCommandBus(commandExecutorContainer); 
            IoC.Register<ICommandBus>(commandBus);

            return framework;
        }
    }
}
