namespace DFramework
{
    public class CommandResult 
    {
        public CommandResult(ICommand cmd, CommandStatus status, string message = "")
        {
            this.Cmd = cmd;
            this.Status = status;
            this.Message = message;
        }

        public ICommand Cmd { get; private set; }

        public CommandStatus Status { get; private set; }
        public string Message { get; private set; }
    }
}