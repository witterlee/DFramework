using System.Web.Mvc;
using DFramework;

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