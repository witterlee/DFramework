

using DFramework.Plusins.Memcached;

namespace DFramework.Memcached
{
    public static class DFrameworkMemcachedExtension
    {
        /// <summary>
        ///  CouchBase cache plusins
        /// </summary>
        /// <param name="framework"></param>
        /// <returns>FCFramework</returns>
        public static DEnvironment UseMemcached(this DEnvironment framework, string memcacheServerIp, string zone = "", string ocsUser = "", string ocsPassword = "")
        {
            IoC.Register<ICache>(new Memcache(memcacheServerIp, zone, ocsUser, ocsPassword));

            return framework;
        }
    }
}
