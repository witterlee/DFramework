using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework
{
    /// <summary>
    /// 命令总线
    /// </summary>
    public interface ICommandBus
    {
        void Send<TCommand>(TCommand cmd) where TCommand : ICommand;
        Task SendAsync<TCommand>(TCommand cmd) where TCommand : ICommand;
    }
}
