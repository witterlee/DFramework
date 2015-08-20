using System;
using System.Collections.Generic;
using System.Reflection;
using DFramework.DynamicReflection;

namespace DFramework
{
    public interface ICommandExecutorContainer
    {
        //ICommandExecutor<TCommand> FindExecutor<TCommand>() where TCommand : ICommand;
        Tuple<Proc<object, ICommand>, ICommandExecutor> FindExecutor(Type commandType);

        void RegisterExecutor(Type executorType);

        void RegisterExecutors(Assembly assemblyToScan);

        void RegisterExecutors(IEnumerable<Assembly> assembliesToScan);

    }
}
