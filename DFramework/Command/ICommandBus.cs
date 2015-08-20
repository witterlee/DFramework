using System.Threading.Tasks;

namespace DFramework
{
    /// <summary>
    /// 命令总线
    /// </summary>
    public interface ICommandBus
    {
        Task<ICommand> SendAsync<TCommand>(TCommand cmd) where TCommand : class, ICommand;
    }
}
