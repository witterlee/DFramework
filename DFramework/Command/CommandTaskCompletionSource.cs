using System;
using System.Threading.Tasks;

namespace DFramework
{
    public class CommandTaskCompletionSource
    {
        public CommandTaskCompletionSource(ICommand command)
        {
            this.CompletionSource = new TaskCompletionSource<ICommand>();
            this.Command = command;
        }

        public TaskCompletionSource<ICommand> CompletionSource { get; set; }
        public ICommand Command { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
