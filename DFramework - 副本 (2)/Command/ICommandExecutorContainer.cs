using System;
using System.Collections.Generic;
using System.Reflection;

namespace DFramework
{
    public interface ICommandExecutorContainer
    {
        ICommandExecutor<TCommand> FindExecutor<TCommand>() where TCommand : ICommand;

        void RegisterExecutor(Type executorType);

        void RegisterExecutors(Assembly assemblyToScan);

        void RegisterExecutors(IEnumerable<Assembly> assembliesToScan);

    }
}
