using System.Threading.Tasks;

namespace DFramework
{
    /// <summary>
    /// 命令总线
    /// </summary>
    public interface ICommandBus
    {
        void Start();
        Task<CommandResult> SendAsync<TCommand>(TCommand cmd) where TCommand : ICommand;
        void Stop();
    }
}
