using System.Web.Mvc;
using Sample.Command;

namespace Sample.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var cmd = new TestCommand("nameA", "pwdB");
            CommandBus.Send(cmd);
            return View();
        }
    }
}