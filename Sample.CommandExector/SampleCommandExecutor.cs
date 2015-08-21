using System.Threading.Tasks;
using DFramework;
using Sample.Command;

namespace Sample.CommandExector
{
    public class SampleCommandExecutor : ICommandExecutor<TestCommand>,
        ICommandExecutor<TestHasReturnValueCommand>
    {
        public async void Execute(TestCommand cmd)
        {
            var task = Task.Factory.StartNew(() =>
            {
                //Console.WriteLine("TestCommand Executed.");
            });
            await task;
        }

        public async void Execute(TestHasReturnValueCommand cmd)
        {
            var task = Task.Factory.StartNew(() =>
            {
               // Console.WriteLine("TestHasReturnValueCommand Executed.");
                cmd.ReturnValue = 1000;
            });
            await task;
        }
    }
}
