using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
            this.CommandBus.Send(cmd);
            return View();
        }
    }
}