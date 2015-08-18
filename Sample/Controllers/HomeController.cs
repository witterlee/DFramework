using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sample.Command;

namespace Sample.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var cmd = new TestCommand("nameA", "pwdB");
            var result = await CommandBus.SendAsync(cmd);

            Console.WriteLine(result.Status);

            var cmd2 = new TestHasReturnValueCommand("nameA", "pwdB");
            var result2 = await CommandBus.SendAsync(cmd2);

            Console.WriteLine(result2.Status);
            return View(); 
        }

        [HttpGet]
        public async Task<ActionResult> IndexWithReturnValue()
        {
            var cmd = new TestCommand("nameA", "pwdB");
            await CommandBus.SendAsync(cmd);

           
            return View();
        }
    }
}