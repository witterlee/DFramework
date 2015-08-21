namespace DFramework
{
    /// <summary>
    /// 命令执行器
    /// </summary>
    public interface ICommandExecutor<in TCommand> : ICommandExecutor where TCommand : ICommand
    {
        void Execute(TCommand cmd);
    }

    public interface ICommandExecutor
    {
    }
}

