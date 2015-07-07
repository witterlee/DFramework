using System;
using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Core;

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
        public void Add<T>(string key, T value, DateTime absoluteExpiration)
        {
            var timespan = absoluteExpiration - DateTime.Now;
            if (timespan.TotalSeconds > 0)
                this._bucket.Upsert(key, value, timespan);
        }

        public void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            if (slidingExpiration.TotalSeconds > 0)
                this._bucket.Upsert(key, value, slidingExpiration);
        }

        public void Remove(string key)
        {
            this._bucket.Remove(key);
        } 
    }
}
