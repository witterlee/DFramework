using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DFramework.Utilities;

namespace DFramework
{
    /// <summary>
    /// command executor container
    /// </summary>
    public class CommandExecutorContainer : ICommandExecutorContainer
    {
        private readonly object _writeLock = new object();
        private readonly Dictionary<Type, ICommandExecutor> _executorForCommand = new Dictionary<Type, ICommandExecutor>();


        public ICommandExecutor<TCommand> FindExecutor<TCommand>() where TCommand : ICommand
        {
            var executor = default(ICommandExecutor);

            this._executorForCommand.TryGetValue(typeof(TCommand), out executor);

            return executor as ICommandExecutor<TCommand>;
        }

        public void RegisterExecutor(Type executorType)
        {
            Check.Argument.IsNotNull(executorType, "executorType");

            if (!executorType.IsClass || executorType.IsAbstract || executorType.IsGenericType || executorType.IsInterface) return;

            var executorTypes = TypeUtil.GetGenericArgumentTypes(executorType, typeof(ICommandExecutor<>));

            if (executorTypes != null && executorTypes.Any())
                lock (this._writeLock)
                {
                    var executorInstance = (ICommandExecutor)Activator.CreateInstance(executorType);
                    foreach (var execType in executorTypes)
                    {
                        this._executorForCommand[execType] = executorInstance;
                    }
                }
        }

        public void RegisterExecutors(Assembly assemblyToScan)
        {
            Check.Argument.IsNotNull(assemblyToScan, "assemblyToScan");

            foreach (Type type in assemblyToScan.GetTypes())
            {
                RegisterExecutor(type);
            }
        }

        public void RegisterExecutors(IEnumerable<Assembly> assembliesToScan)
        {
            Check.Argument.IsNotNull(assembliesToScan, "assembliesToScan");

            foreach (var assemblyToScan in assembliesToScan)
            {
                foreach (Type type in assemblyToScan.GetTypes())
                {
                    RegisterExecutor(type);
                }
            }

        }
    }
}
