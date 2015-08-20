using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var cmd = new TestHasReturnValueCommand("nameA", "pwdB");
            var result =await CommandBus.SendAsync(cmd);

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> IndexLoop()
        {
            Stopwatch sw = Stopwatch.StartNew();

            int i = 1000, j = 10;

            var taskList = new List<Task>();

            while (j-- > 0)
            {
                i = 10000;
                while (i-- > 0)
                {
                    var cmd = new TestCommand("nameA", "pwdB");
                    taskList.Add(CommandBus.SendAsync(cmd));
                }

                await Task.WhenAll(taskList);
            }
            sw.Stop();
            ViewBag.Time = sw.ElapsedMilliseconds / 1000;
            return View();
        }
    }
}