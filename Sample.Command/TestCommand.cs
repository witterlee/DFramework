using DFramework;

namespace Sample.Command
{
    public class TestCommand:DFramework.Command
    {
        public TestCommand(string name, string password)
        {
            this.Name = name;
            this.Password = password;
        }

        public string Name { get; set; }
        public string Password { get; set; }
    }
    public class TestHasReturnValueCommand : Command<int>
    {
        public TestHasReturnValueCommand(string name, string password)
        {
            this.Name = name;
            this.Password = password;
        }

        public string Name { get; set; }
        public string Password { get; set; }
    }
}
