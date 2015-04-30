using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework;
using Sample.Command;

namespace Sample.CommandExector
{
    public class SampleCommandExecutor : ICommandExecutor<TestCommand>,
                                         ICommandExecutor<TestHasReturnValueCommand>
    {
        public Task ExecuteAsync(TestCommand cmd)
        {
            return Task.Factory.StartNew(() =>
             {
                 Console.WriteLine("TestCommand Executed.");
             });
        }

        public async Task ExecuteAsync(TestHasReturnValueCommand cmd)
        {
          await Task.Factory.StartNew(() =>
          {
              Console.WriteLine("TestHasReturnValueCommand Executed.");
              cmd.CommandResult = 1;
          });
        }
    }
}
