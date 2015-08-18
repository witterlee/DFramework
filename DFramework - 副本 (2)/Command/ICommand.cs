using System;

namespace DFramework
{
    /// <summary>
    /// 待返回值的命令
    /// </summary>
    public class Command<T> : ICommand
    {
        public Command()
        {
            this.Id = Guid.NewGuid().Shrink();
        }
        public string Id { get; private set; }
        public T CommandResult { get; set; }
    }
    /// <summary>
    /// 命令
    /// </summary>
    public class Command : ICommand
    {
        public Command()
        {
            this.Id = Guid.NewGuid().Shrink();
        }
        public string Id { get; private set; }
    }
    /// <summary>
    /// 命令接口
    /// </summary>
    public interface ICommand
    {
        string Id { get; }
    }
}
