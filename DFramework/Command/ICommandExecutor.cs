using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework
{
    /// <summary>
    /// 命令执行器
    /// </summary>
    public interface ICommandExecutor<TCommand> : ICommandExecutor where TCommand : ICommand
    {
        Task ExecuteAsync(TCommand cmd);
    }

    public interface ICommandExecutor { }
}
