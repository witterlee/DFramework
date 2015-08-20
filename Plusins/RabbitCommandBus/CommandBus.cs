using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using DFramework.Utilities;
using Timer = System.Timers.Timer;

namespace DFramework.RabbitCommandBus
{
    /// <summary>
    /// 命令总线
    /// </summary>
    public class CommandBus : ICommandBus
    { 
        private readonly CommandSender _commandSender;
        private readonly CommandProcessor _commandProcessor;
        private Timer _timeoutCommandCheckerTimer;
        private readonly ConcurrentDictionary<Guid, CommandTaskCompletionSource> _commandTaskDic;

        public CommandBus(ICommandExecutorContainer executorContainer, Dictionary<int, Type> commangTypeCodeMapping, int queryCount)
        {
            var commandTypeMapping = new CommandTypeMapping(commangTypeCodeMapping);
            Check.Argument.IsNotNull(executorContainer, "executorContainer");

            //StartCommandCheckerTimer();//回头要启用

            this._commandTaskDic = new ConcurrentDictionary<Guid, CommandTaskCompletionSource>();
            this._commandProcessor = new CommandProcessor(this, commandTypeMapping, executorContainer, queryCount);
            this._commandSender = new CommandSender(this, commandTypeMapping, executorContainer, queryCount);
        }

        public virtual Task<CommandResult> SendAsync<TCommand>(TCommand cmd) where TCommand : ICommand
        {
            try
            {
                var tcs = new CommandTaskCompletionSource();

                if (this.RegisterCommandTask(cmd.Id, tcs) && this._commandSender.Send(cmd))
                {
                    return tcs.CompletionSource.Task;
                }

                return Task.FromResult(new CommandResult(cmd, CommandStatus.Fail, "send command to queue exception"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new CommandResult(cmd, CommandStatus.Fail, ex.Message));
            }
        }

        internal bool InternalCompleteTaskByResult(CommandResult result)
        {
            return false;
            CommandTaskCompletionSource completionSource;
            var isContainCommandTask = _commandTaskDic.TryRemove(result.Cmd.Id, out completionSource);

            if (isContainCommandTask)
            {
                completionSource.CompletionSource.SetResult(result);
            }

            return isContainCommandTask;
        }
        internal bool DistributedCompleteTaskByResult(CommandResult result)
        {
            CommandTaskCompletionSource completionSource;
            var isContainCommandTask = _commandTaskDic.TryRemove(result.Cmd.Id, out completionSource);

            if (isContainCommandTask)
            {
                completionSource.CompletionSource.SetResult(result);
            }

            return isContainCommandTask;
        } 

        #region Private Method
        private bool RegisterCommandTask(Guid id, CommandTaskCompletionSource completionSource)
        {
            return this._commandTaskDic.TryAdd(id, completionSource);
        }
        private void StartCommandCheckerTimer()
        {
            this._timeoutCommandCheckerTimer = new Timer(500);
            this._timeoutCommandCheckerTimer.Elapsed += (s, e) =>
            {
                if (_commandTaskDic.Count > 0)
                {
                    var now = DateTime.Now;
                    _commandTaskDic.ForEach(ct =>
                    {
                        if (ct.Value.Timestamp.AddSeconds(15) > now)
                        {
                            CommandTaskCompletionSource ctcs;
                            this._commandTaskDic.TryRemove(ct.Key, out ctcs);
                        }
                    });
                }
            };
            this._timeoutCommandCheckerTimer.Start();
        }
        #endregion
    }
}
