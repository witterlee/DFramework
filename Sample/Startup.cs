using Microsoft.Owin;
using Owin;
using Sample;

[assembly: OwinStartup(typeof(Startup))]
namespace Sample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           
        }
    }
}
