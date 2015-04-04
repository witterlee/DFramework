using System;
using Xunit;
using DFramework;
using System.Collections.Generic;
using System.Linq;
using DFramework.Utilities;
using System.Text;

namespace DFramework.CouchCache.UnitTest
{
    public class UnitTest
    {

        [Fact]
        public void TestUnixTimestamp()
        {
            var date = DateTime.Now;
            var timestamp2 = date.ToUnixTimestamp();
            var resolveLocalDate2 = timestamp2.ToLocalDateTime();
            var resolveUTCDate2 = timestamp2.ToUtcDateTime();
            Assert.Equal(date.Date, resolveLocalDate2.Date);
            Assert.Equal(date.Hour, resolveLocalDate2.Hour);
            Assert.Equal(date.Minute, resolveLocalDate2.Minute);
            Assert.Equal(date.Second, resolveLocalDate2.Second);
            Assert.NotEqual(date, resolveUTCDate2);
            Assert.NotEqual(resolveLocalDate2, resolveUTCDate2);
        }

        [Fact]
        public void TestDecimalAndDoubleFixed()
        {
            var dc1 = 12.123456;
            var dc2 = 12.1234465;


            var db1 = 12.123456;
            var db2 = 12.1234465;

            Assert.Equal(dc1.ToFixed(4), dc2.ToFixed(4));
            Assert.Equal(db1.ToFixed(4), db2.ToFixed(4));
        }


        [Fact]
        public void TestKetamaNodeHash()
        {
            var list = new Dictionary<string, int>();

            var nodes = new List<string> { "q0", "q1", "q2", "q3", "q4", "q5", "q6", "q7", "q8", "q9" };
            var ms = new List<string> { "CNT_BTC", "CNT_LTC", "CNT_IFC", "CNT_NXT", "CNT_DOGE", "BTC_LTC", "BTC_IFC", "BTC_DOGE", "BTC_NXT" };
            var KetamaNode = new KetamaNodeLocator(nodes, 500);

            var d = new Random();
            for (int i = 0; i < 10000; i++)
            {
                var key = ms[d.Next(9)] + d.Next(100);
                var value = KetamaNode.GetPrimary(key.ToString());

                if (list.ContainsKey(value))
                {
                    list[value] += 1;
                }
                else
                {
                    list[value] = 1;
                }
            }

            //foreach (var key in list.Keys)
            //{
            //    var s = key + ":" + (list[key] / 10000.0D).ToString("P");
            //    Console.WriteLine(s);
            //}
        }
    }
}