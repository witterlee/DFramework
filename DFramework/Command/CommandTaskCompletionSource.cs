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
            this.CompletionSource = new TaskCompletionSource<ICommand>();
        }

        public TaskCompletionSource<ICommand> CompletionSource { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
