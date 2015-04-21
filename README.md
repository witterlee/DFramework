#DFramework    

[![Join the chat at https://gitter.im/weitaolee/DFramework](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/weitaolee/DFramework?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
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
 .Start();
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

```csharp
[HttpGet]
public async Task<ActionResult> Index()
{
    var cmd = new TestReturnResultCommand("nameA", "pwdB");
    await this.CommandBus.SendAsync(cmd);
    var reuslt=cmd.CommandResult;
    return View();
}
```


####command
```
 public class HasReturnValueCommand<T> : DFramework.Command
 {
     public T CommandResult { get; set; }
 }
```
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
public class TestReturnResultCommand:HasReturnValueCommand<T>
{
    public TestReturnResultCommand(string name, string password)
    {
        this.Name = name;
        this.Password = password;
    }

    public string Name { get; set; }
    public string Password { get; set; }
    public T CommandResult { get; set; }
}
```

####command-executor
```csharp
 public class SampleCommandExecutor : ICommandExecutor<TestCommand>,
                                      ICommandExecutor<TestReturnResultCommand>
    {
        public Task ExecuteAsync(TestCommand cmd)
        {
            return Task.Factory.StartNew(() =>
             {
                 Console.WriteLine("TestCommand Executed.");
             });
        }

        public Task ExecuteAsync(TestReturnResultCommand cmd)
        {
            return Task.Factory.StartNew(() =>
             {
                 Console.WriteLine("TestCommand Executed.");
                 cmd.CommandResult=default
             });
        }
    }
```
