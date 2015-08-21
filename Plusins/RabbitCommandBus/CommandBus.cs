using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using DFramework.Utilities;

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

        public virtual Task SendAsync<TCommand>(TCommand cmd) where TCommand : class, ICommand
        {
            try
            {
                var tcs = new CommandTaskCompletionSource(cmd);

                if (this.RegisterCommandTask(cmd.Id, tcs) && this._commandSender.Send(cmd))
                {
                    return tcs.CompletionSource.Task;
                }
                cmd.Status = CommandStatus.Fail;
                cmd.Message = "send command to queue exception";
                return Task.FromResult(cmd as ICommand);
            }
            catch (Exception ex)
            {
                cmd.Status = CommandStatus.Fail;
                cmd.Message = ex.Message;
                return Task.FromResult(cmd as ICommand);
            }
        }

        internal bool InternalCompleteTaskByResult(ICommand processdCommand)
        { 
            CommandTaskCompletionSource completionSource;
            var isContainCommandTask = _commandTaskDic.TryRemove(processdCommand.Id, out completionSource);

            if (isContainCommandTask)
            {
                CommandCopyHelper.Copy(processdCommand, completionSource.Command);
                completionSource.CompletionSource.SetResult(processdCommand);
            }

            return isContainCommandTask;
        }
        internal bool DistributedCompleteTaskByResult(ICommand processdCommand)
        {
            CommandTaskCompletionSource completionSource;
            var isContainCommandTask = _commandTaskDic.TryRemove(processdCommand.Id, out completionSource);

            if (isContainCommandTask)
            {
                CommandCopyHelper.Copy(processdCommand, completionSource.Command);
                completionSource.CompletionSource.SetResult(processdCommand);
            }

            return isContainCommandTask;
        }

        #region Private Method
        private bool RegisterCommandTask(Guid id, CommandTaskCompletionSource completionSource)
        {
            var task = completionSource;
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
