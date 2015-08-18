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
            await CommandBus.SendAsync(cmd);

            return View();
        }

    }
}