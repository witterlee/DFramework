using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Sample.Controllers
{
    public class BaseController : Controller
    {

        protected ICommandBus CommandBus
        {
            get { return IoC.Resolve<ICommandBus>(); }
        }

    }
}