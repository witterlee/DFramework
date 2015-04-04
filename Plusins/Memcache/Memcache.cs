using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;

namespace DFramework.Plusins.Memcached
{
    public class Memcache : ICache
    {
        private readonly MemcachedClient _memClient;

        public Memcache(string memcacheServer, string zone = "", string ocsUser = "", string ocsPassword = "")
        {
            MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();

            IPAddress newaddress = IPAddress.Parse(memcacheServer);
            IPEndPoint ipEndPoint = new IPEndPoint(newaddress, 11211);

            
            memConfig.Servers.Add(ipEndPoint);
            memConfig.Protocol = MemcachedProtocol.Binary;
            if (!string.IsNullOrEmpty(ocsUser) && !string.IsNullOrEmpty(ocsPassword))
            {
                memConfig.Authentication.Type = typeof(PlainTextAuthenticator);
                memConfig.Authentication.Parameters["zone"] = zone;
                memConfig.Authentication.Parameters["userName"] = "username";
                memConfig.Authentication.Parameters["password"] = "password";
            }
            memConfig.SocketPool.MinPoolSize = 5;
            memConfig.SocketPool.MaxPoolSize = 200;
            this._memClient = new MemcachedClient(memConfig);
        }

        public object Get(string key)
        {
            return this._memClient.Get<object>(key);
        }

        public bool TryGet(string key, out object value)
        {
            var result = false;
            try
            {
                value = this._memClient.Get<object>(key);
                result = true;
            }
            catch (Exception ex)
            {
                value = null;
            }

            return result;
        }

        public T Get<T>(string key)
        {
            return this._memClient.Get<T>(key);
        }

        public bool TryGet<T>(string key, out T value)
        {
            var result = false;
            try
            {
                value = this._memClient.Get<T>(key);
                result = true;
            }
            catch (Exception ex)
            {
                value = default(T);
            }

            return result;
        }
        public void Add<T>(string key, T value, DateTime absoluteExpiration)
        {
            var notExpirated = absoluteExpiration > DateTime.Now;
            if (notExpirated)
                this._memClient.Store(StoreMode.Set, key, value, absoluteExpiration);
        }

        public void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            if (slidingExpiration.TotalSeconds > 0)
                this._memClient.Store(StoreMode.Set, key, value, slidingExpiration);
        }

        public void Remove(string key)
        {
            this._memClient.Remove(key);
        }
    }
}