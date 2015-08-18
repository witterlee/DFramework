using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DFramework.DynamicReflection;
using DFramework.Utilities;

namespace DFramework
{
    /// <summary>
    /// command executor container
    /// </summary>
    internal class CommandExecutorContainer : ICommandExecutorContainer
    {
        private readonly object _writeLock = new object();
        private readonly Dictionary<Type, Tuple<Delegate, ICommandExecutor>> _DelegateForCommand = new Dictionary<Type, Tuple<Delegate, ICommandExecutor>>();

        //public ICommandExecutor<TCommand> FindExecutor<TCommand>() where TCommand : ICommand
        //{
        //    var executor = default(ICommandExecutor);

        //    this._executorForCommand.TryGetValue(typeof(TCommand), out executor);

        //    return executor as ICommandExecutor<TCommand>;
        //}

        public Tuple<Delegate, ICommandExecutor> FindExecutor(Type commandType)
        {
            var executor = default(Tuple<Delegate, ICommandExecutor>);

            this._DelegateForCommand.TryGetValue(commandType, out executor);

            return executor;
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

                    foreach (var method in executorInstance.GetType().GetMethods())
                    {
                        if (method.Name == "Execute")
                        {
                            var @params = method.GetParameters();

                            if (@params.Length == 1)
                            {
                                var paramsType = @params.First().ParameterType;
                                foreach (var execType in executorTypes)
                                {
                                    if (execType == paramsType)
                                    {
                                        var del = Dynamic<object>.Instance.Procedure.Explicit<ICommand>.CreateDelegate(method);

                                        this._DelegateForCommand[execType] = new Tuple<Delegate, ICommandExecutor>(del,
                                            executorInstance);
                                    }
                                    //this._executorForCommand[execType] = executorInstance;
                                }
                            }
                        }
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
