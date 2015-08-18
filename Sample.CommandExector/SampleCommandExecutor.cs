using System;
using System.Threading.Tasks;
using DFramework;
using Sample.Command;

namespace Sample.CommandExector
{
    public class SampleCommandExecutor : ICommandExecutor<TestCommand>,
        ICommandExecutor<TestHasReturnValueCommand>
    { 

        public async Task ExecuteAsync(TestCommand cmd)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("TestCommand Executed.");
            });
            await task;
        }

        public async Task ExecuteAsync(TestHasReturnValueCommand cmd)
        {

            var task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("TestHasReturnValueCommand Executed.");
                cmd.CommandResult = 1;
            });
            await task;
        }
    }
}
