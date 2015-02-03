using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFramework
{
    /// <summary>
    /// 命令执行器
    /// </summary>
    public interface ICommandExecutor<TCommand> : ICommandExecutor where TCommand : ICommand
    {
        void Execute(TCommand cmd);
    }

    public interface ICommandExecutor { }
}
