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
        private readonly ConcurrentDictionary<Guid, TaskCompletionSource<CommandResult>> _commandTaskDic;
        private bool _isRequestStop = false;


        public DefaultCommandBus(ICommandExecutorContainer executorContainer)
        {
            Check.Argument.IsNotNull(executorContainer, "executorContainer");

            this._executorContainer = executorContainer;
            this._commandQueue = new BlockingCollection<ICommand>();
            this._commandTaskDic = new ConcurrentDictionary<Guid, TaskCompletionSource<CommandResult>>();

        }

        public void Start()
        {
            var worker = new Thread(() =>
            {
                while (!_isRequestStop)
                {
                    ICommand cmd;
                    if (this._commandQueue.TryTake(out cmd))
                    {
                        Task.Factory.StartNew(() =>
                        {
                            TaskCompletionSource<CommandResult> tcs;
                            if (_commandTaskDic.TryGetValue(cmd.Id, out tcs))
                            {
                                var delegete = this._executorContainer.FindExecutor(cmd.GetType());

                                if (delegete == null)
                                    tcs.SetResult(new CommandResult(cmd, CommandStatus.Fail, "not found command " + cmd.GetType().Name + "'s executor"));
                                else
                                {
                                    try
                                    {
                                        delegete.Item1.DynamicInvoke(delegete.Item2, cmd);

                                        tcs.SetResult(new CommandResult(cmd, CommandStatus.Success));
                                        Log.Debug("execute cmd success,cmdId={0},cmdStatus={1}");

                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error("execute command error", ex);
                                        tcs.SetResult(new CommandResult(cmd, CommandStatus.Fail, "not found command " + cmd.GetType().Name + "'s executor"));
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
                    var tcs = new TaskCompletionSource<CommandResult>();
                    if (_commandTaskDic.TryAdd(cmd.Id, tcs))
                    {
                        return tcs.Task;
                    }
                }

                return Task.FromResult(new CommandResult(cmd, CommandStatus.Fail, "send command to queue exception"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new CommandResult(cmd, CommandStatus.Fail, ex.Message));
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
