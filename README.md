#DFramework    
a command bus framework,this framework have three plugins

IOC     --> Autofac plugin   
Cache   --> Couchbase cache   
Log     --> Log4net   

----------


##Use Case
----------
####application start
```csharp
 DEnvironment.Initialize()
 .UseAutofac()
 .UseCouchbaseCache(clientConfig, "datastore")
 .UseLog4net()
 .UseDefaultCommandBus(GetAllAssembly())
 .Start();
```

####controller

```csharp
[HttpGet]
public ActionResult Index()
{
    var cmd = new TestCommand("nameA", "pwdB");
    this.CommandBus.Send(cmd);
    return View();
}
```


####command
```csharp
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
```

####command-executor
```csharp
 public class SampleCommandExecutor : ICommandExecutor<TestCommand>
    {
        public Task ExecuteAsync(TestCommand cmd)
        {
            return Task.Factory.StartNew(() =>
             {
                 Console.WriteLine("TestCommand Executed.");
             });
        }
    }
```
