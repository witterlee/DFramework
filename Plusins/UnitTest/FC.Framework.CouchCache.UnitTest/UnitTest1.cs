using System;
using Xunit;
using FC.Framework;
using FC.Framework.CouchbaseCache;
using FC.Framework.Autofac;
using System.Collections.Generic;
using System.Linq;

namespace FC.Framework.CouchCache.UnitTest
{
    public class UnitTest1
    {
        static bool Inited = false;
        public UnitTest1()
        {
            if (!Inited)
            {
                FCFramework.Initialize().UseAutofac().UseCouchbaseCache();
                Inited = true;
            }
        }

        [Fact]
        public void TestStoreModel()
        {
            var model = new TestModel { Name = "weitaolee", Sex = "male", Age = 25 };
            var key = Guid.NewGuid().Shrink();

            Cache.Add<TestModel>(key, model);

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

            Cache.Add(key, list);

            var cacheModelList = Cache.Get<IEnumerable<TestModel>>(key);

            Assert.NotNull(cacheModelList);
            Assert.True(cacheModelList.Count() > 0);

            Cache.Remove(key);
        }


        [Fact]
        public void TestSimpleValue()
        {
            var key = Guid.NewGuid().Shrink();

            Cache.Add<int>(key, 2);

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