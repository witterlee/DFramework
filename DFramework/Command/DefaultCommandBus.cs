using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Threading;
using System.Threading.Tasks;
using DFramework.Utilities;

namespace DFramework
{
    /// <summary>
    /// 命令总线
    /// </summary>
    public class DefaultCommandBus : ICommandBus, IDisposable
    {
        private readonly ICommandExecutorContainer _executorContainer;
        private readonly BlockingCollection<ICommand> _commandQueue;
        private readonly ConcurrentDictionary<Guid, CommandTaskCompletionSource> _commandTaskDic;
        private Thread worker;

        public DefaultCommandBus(ICommandExecutorContainer executorContainer)
        {
            Check.Argument.IsNotNull(executorContainer, "executorContainer");

            this._executorContainer = executorContainer;
            this._commandQueue = new BlockingCollection<ICommand>();
            this._commandTaskDic = new ConcurrentDictionary<Guid, CommandTaskCompletionSource>();

            worker = new Thread(() =>
            {
                while (true)
                {
                    ICommand cmd;
                    if (this._commandQueue.TryTake(out cmd))
                    {
                        Task.Factory.StartNew(() =>
                        {
                            CommandTaskCompletionSource tcs;
                            if (_commandTaskDic.TryGetValue(cmd.Id, out tcs))
                            {
                                var delegete = this._executorContainer.FindExecutor(cmd.GetType());

                                if (delegete == null)
                                    tcs.CompletionSource.SetResult(new CommandResult(cmd, CommandStatus.Fail, "not found command " + cmd.GetType().Name + "'s executor"));
                                else
                                {
                                    try
                                    {
                                        delegete.Item1.DynamicInvoke(delegete.Item2, cmd);

                                        tcs.CompletionSource.SetResult(new CommandResult(cmd, CommandStatus.Success));
                                        Log.Debug("execute cmd success,cmdId={0},cmdStatus={1}");

                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error("execute command error", ex);
                                        tcs.CompletionSource.SetResult(new CommandResult(cmd, CommandStatus.Fail, "not found command " + cmd.GetType().Name + "'s executor"));
                                    }

                                    _commandTaskDic.TryRemove(cmd.Id, out tcs);
                                }
                            }

                        });
                    }
                }
            });

            worker.Start(); 
        }
         

        public virtual Task<CommandResult> SendAsync<TCommand>(TCommand cmd) where TCommand : ICommand
        {
            try
            {
                if (_commandQueue.TryAdd(cmd))
                {
                    var tcs = new CommandTaskCompletionSource();
                    if (_commandTaskDic.TryAdd(cmd.Id, tcs))
                    {
                        return tcs.CompletionSource.Task;
                    }
                }

                return Task.FromResult(new CommandResult(cmd, CommandStatus.Fail, "send command to queue exception"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new CommandResult(cmd, CommandStatus.Fail, ex.Message));
            }
        }
          
        public void Dispose()
        {
            if (worker != null && worker.IsAlive)
            {
                worker.Abort();
            }
        }
    }
}
