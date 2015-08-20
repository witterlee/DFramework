using System.Threading.Tasks;

namespace DFramework
{
    /// <summary>
    /// 命令总线
    /// </summary>
    public interface ICommandBus
    {
        Task<CommandResult> SendAsync<TCommand>(TCommand cmd) where TCommand : ICommand;
    }
}
