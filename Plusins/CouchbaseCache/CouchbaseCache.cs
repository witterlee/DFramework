using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Couchbase.Configuration;
using Couchbase.Core;
using Couchbase.Configuration.Client;

namespace DFramework.CouchbaseCache
{
    public class CouchbaseCache : ICache
    {
        private readonly ICluster _cluster;
        private readonly IBucket _bucket;

        public CouchbaseCache(ClientConfiguration clientConfig, string bucketName)
        {
            this._cluster = new Cluster(clientConfig);
            this._bucket = this._cluster.OpenBucket(bucketName);
        }

        public object Get(string key)
        {
            var result = this._bucket.Get<object>(key);

            if (result.Success)
                return result.Value;
            else
                return null;
        }

        public bool TryGet(string key, out object value)
        {
            var result = this._bucket.Get<object>(key);

            if (result.Success)
                value = result.Value;
            else
                value = null;

            return result.Success;
        }

        public T Get<T>(string key)
        {
            var result = this._bucket.Get<T>(key);

            if (result.Success)
                return result.Value;
            else
                return default(T);
        }

        public bool TryGet<T>(string key, out T value)
        {
            var result = this._bucket.Get<T>(key);

            if (result.Success)
                value = result.Value;
            else
                value = default(T);

            return result.Success;
        }

        public void Add<T>(string key, T value)
        {
            this._bucket.Upsert<T>(key, value);
        }

        public void Add<T>(string key, T value, DateTime absoluteExpiration)
        {
            var timespan = absoluteExpiration - DateTime.Now;
            if (timespan.TotalSeconds > 0)
                this._bucket.Upsert<T>(key, value, timespan);
        }

        public void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            if (slidingExpiration.TotalSeconds > 0)
                this._bucket.Upsert<T>(key, value, slidingExpiration);
        }

        public void Remove(string key)
        {
            this._bucket.Remove(key);
        } 
    }
}
