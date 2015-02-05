using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework
{
    public abstract class AbstractCommandExecutor<TCommand> : ICommandExecutor<TCommand> where TCommand : ICommand
    {
        public abstract Task ExecuteAsync(TCommand cmd); 
    }
}
