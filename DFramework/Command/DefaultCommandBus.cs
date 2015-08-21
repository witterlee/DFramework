using System;
using System.Collections.Concurrent;
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
                                {
                                    cmd.Status = CommandStatus.Fail;
                                    cmd.Message = "not found command " + cmd.GetType().Name + "'s executor";
                                    tcs.CompletionSource.SetResult(cmd);
                                }
                                else
                                {
                                    try
                                    {
                                        delegete.Item1.Invoke(delegete.Item2, cmd);
                                        cmd.Status = CommandStatus.Success;
                                        tcs.CompletionSource.SetResult(cmd);
                                        Log.Debug("execute cmd success,cmdId={0},cmdStatus={1}");

                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Debug("execute command error", ex);

                                        cmd.Status = CommandStatus.Fail;
                                        cmd.Message = ex.Message;
                                        tcs.CompletionSource.SetResult(cmd);
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


        public virtual Task SendAsync<TCommand>(TCommand cmd) where TCommand : class ,ICommand
        {
            try
            {
                if (_commandQueue.TryAdd(cmd))
                {
                    var tcs = new CommandTaskCompletionSource(cmd);
                    if (_commandTaskDic.TryAdd(cmd.Id, tcs))
                    {
                        return tcs.CompletionSource.Task;
                    }
                }
                cmd.Status = CommandStatus.Fail;
                cmd.Message = "send command to queue exception";
            }
            catch (Exception ex)
            {
                cmd.Status = CommandStatus.Fail;
                cmd.Message = ex.Message;
            }

            return Task.FromResult(1);
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
