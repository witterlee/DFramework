using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework
{
    public class CommandTaskCompletionSource
    {
        public CommandTaskCompletionSource()
        {
            CompletionSource = new TaskCompletionSource<CommandResult>();
        }
        public TaskCompletionSource<CommandResult> CompletionSource { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
