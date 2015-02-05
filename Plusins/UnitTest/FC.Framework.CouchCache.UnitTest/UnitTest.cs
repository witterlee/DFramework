using System;
using Xunit;
using DFramework;
using DFramework.Autofac;
using DFramework.CouchbaseCache;
using Couchbase;
using System.Collections.Generic;
using System.Linq;
using Couchbase.Configuration.Client;

namespace FC.Framework.CouchCache.UnitTest
{
    public class UnitTest
    {
        static bool Inited = false;
        static object _lock = new object();
        public UnitTest()
        {
            if (!Inited)
            {
                lock (_lock)
                {
                    if (!Inited)
                    {
                        var configuration = new ClientConfiguration
                        {
                            Servers = new List<Uri>  
                            {
                               new Uri("http://192.168.0.100:8091")
                            }
                        };
                        DEnvironment.Initialize().UseAutofac().UseCouchbaseCache(configuration, "default");
                        Inited = true;
                    }
                }
            }
        }

        [Fact]
        public void TestStoreModel()
        {
            var model = new TestModel { Name = "weitaolee", Sex = "male", Age = 25 };
            var key = Guid.NewGuid().Shrink();

            Cache.Add<TestModel>(key, model, TimeSpan.FromSeconds(30));

            var cacheModel = Cache.Get<TestModel>(key);

            Assert.NotEqual(cacheModel, null);
            Assert.Equal(cacheModel.Name, model.Name);
            Assert.Equal(cacheModel.Sex, model.Sex);
            Assert.Equal(cacheModel.Age, model.Age);


            Cache.Remove(key);

            cacheModel = Cache.Get<TestModel>(key);

            Assert.Equal(cacheModel, null);
        }

        [Fact]
        public void TestStoreListModel()
        {
            var model = new TestModel { Name = "weitaolee", Sex = "male", Age = 25 };
            var key = Guid.NewGuid().Shrink();

            var list = new List<TestModel>();
            list.Add(model);

            Cache.Add(key, list, TimeSpan.FromSeconds(30));

            var cacheModelList = Cache.Get<IEnumerable<TestModel>>(key);

            Assert.NotNull(cacheModelList);
            Assert.True(cacheModelList.Count() > 0);

            Cache.Remove(key);
        }


        [Fact]
        public void TestSimpleValue()
        {
            var key = Guid.NewGuid().Shrink();

            Cache.Add<int>(key, 2, TimeSpan.FromSeconds(30));

            var cacheModel = Convert.ToInt32(Cache.Get(key));

            Assert.NotEqual(Cache.Get(key), null);
            Assert.Equal(cacheModel, 2);

            Cache.Remove(key);
        }

        private class TestModel
        {
            public string Name { get; set; }
            public string Sex { get; set; }
            public int Age { get; set; }
        }
    }
}