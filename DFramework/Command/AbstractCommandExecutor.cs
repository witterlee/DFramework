using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFramework
{
    public abstract class AbstractCommandExecutor<TCommand> : ICommandExecutor<TCommand> where TCommand : ICommand
    {
        public abstract void Execute(TCommand cmd);
    }
}
