using Couchbase.Configuration.Client;

namespace DFramework.CouchbaseCache
{
    public static class DFrameworkEnterpriseCacheExtension
    {
        /// <summary>
        ///  CouchBase cache plusins
        /// </summary>
        /// <param name="framework"></param>
        /// <returns>FCFramework</returns>
        public static DEnvironment UseCouchbaseCache(this DEnvironment framework, ClientConfiguration clientConfig, string cacheBucketName)
        {
            IoC.Register<ICache>(new CouchbaseCache(clientConfig, cacheBucketName));

            return framework;
        }
    }
}
