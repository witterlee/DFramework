#DFramework    
a command bus framework,this framework have three plugins

IOC     --> Autofac plugin   
Cache   --> Couchbase cache   
Cache   --> Memcached/ali OCS cache  
Log     --> Log4net   

----------


##Use Case
----------
####application start
#####Couchbase cache
```csharp
 DEnvironment.Initialize()
 .UseAutofac()
 .UseCouchbaseCache(clientConfig, "datastore")
 .UseLog4net()
 .UseDefaultCommandBus(GetAllAssembly())
 .UseDefaultJsonSerialaizer();
```
#####Memcached
```csharp

 //IPAddress newaddress = Dns.GetHostByName("xxxxxxxx.ocs.aliyuncs.com").AddressList.FirstOrDefault();
 DEnvironment.Initialize()
 .UseAutofac()
 .UseMemcached("memcached server") //not support memcached cluster now, will support later
 //.UseMemcached(newaddress.ToString(),ocsUser: "xxx", ocsPassword: "xxx")
 .UseLog4net()
 .UseDefaultCommandBus(GetAllAssembly())
 .UseDefaultJsonSerialaizer();
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

```csharp
[HttpGet]
public async Task<ActionResult> Index()
{
    var cmd = new TestCommand("nameA", "pwdB");
    await this.CommandBus.SendAsync(cmd);
    var reuslt=cmd.CommandResult;
    return View();
}
```

```csharp
[HttpGet]
public async Task<ActionResult> Index()
{
    var cmd = new TestHasReturnValueCommand("nameA", "pwdB");
    await this.CommandBus.SendAsync(cmd);
    var reuslt=cmd.CommandResult;
    Console.WriteLine(result.ToString());
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

```csharp
public class TestHasReturnValueCommand:Command<int>
{
    public TestReturnResultCommand(string name, string password)
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

        public async Task ExecuteAsync(TestReturnResultCommand cmd)
        {
             await Task.Factory.StartNew(() =>
             {
                 Console.WriteLine("TestHasReturnValueCommand Executed.");
                 cmd.CommandResult=1;
             });
        }
    }
```
