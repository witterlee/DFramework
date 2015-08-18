using System;

namespace DFramework
{
    /// <summary>
    /// 带返回值的命令
    /// </summary>
    public class Command<T> : Command
    {
        public T ReturnValue { get; set; }
    }
    /// <summary>
    /// 命令
    /// </summary>
    public class Command : ICommand
    {
        public Command()
        {
            this.Id = Guid.NewGuid();
        }
        public Guid Id { get; private set; }
    }
    /// <summary>
    /// 命令接口
    /// </summary>
    public interface ICommand
    {
        Guid Id { get; }

    }

    public enum CommandStatus
    {
        Pending = 0,
        Fail = 1,
        Success = 2
    }

    public class CommandResult
    {
        public CommandResult(ICommand cmd, CommandStatus status, string message="")
        {
            this.Cmd = cmd;
            this.Status = status;
            this.Message = message;
        }

        public ICommand Cmd { get; private set; }

        public CommandStatus Status { get; private set; }
        public string Message { get; private set; }
    }
}
