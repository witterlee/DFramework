using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using RabbitMQ.Client;

namespace DFramework.RabbitCommandBus
{
    public class RabbitMqChannelManager
    {
        private static readonly Dictionary<IConnection, List<IModel>> _channelPool;
        private static readonly object _locker = new object();
        private static readonly Timer _timer = new Timer();
        private const int MAX_CONNECTION_CHANNEL_PERCENT = 5;
        private const int INTERVAL = 10;

        static RabbitMqChannelManager()
        {
            _channelPool = new Dictionary<IConnection, List<IModel>>();
            _timer = new Timer(INTERVAL);
            _timer.Elapsed += (s, e) => CollectionIdleChannel();
        }

        public static IModel GetChannel()
        {
            IModel channel = null;

            if (!_timer.Enabled)
                _timer.Start();

            var connectionStatistics = new Dictionary<IConnection, int>();

            try
            {
                lock (_locker)
                {
                    _channelPool.ForEach(c =>
                    {
                        var nullCount = c.Value.Count(ch => ch == null);
                        connectionStatistics.Add(c.Key, nullCount);
                    });

                    var sortList = connectionStatistics.OrderByDescending(d => d.Value);

                    if (sortList.Any())
                    {
                        //取目前Channel最少的连接(每个Channel都静态所以，只有在使用完，才会释放掉)
                        var firstConn = sortList.First();

                        //如果该连接有空余Channel
                        if (firstConn.Value < MAX_CONNECTION_CHANNEL_PERCENT)
                        {
                            channel = firstConn.Key.CreateModel();
                        }
                    }


                    if (channel == null)
                    {
                        var newConn = IoC.Resolve<IConnectionFactory>().CreateConnection();
                        channel = newConn.CreateModel();
                        var cList = new List<IModel> { channel };
                        _channelPool.Add(newConn, cList);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetChannel Exception", ex);
            }

            return channel;
        }

        /// <summary>
        /// 清理闲置的Channel
        /// </summary>
        private static void CollectionIdleChannel()
        {
            lock (_locker)
            {
                var nullConnectionKeys = new List<IConnection>();
                _channelPool.ForEach(c =>
                {
                    var nullChannelRefrences = c.Value.Where(ch => ch == null);

                    nullChannelRefrences.ForEach(chKey =>
                    {
                        c.Value.Remove(chKey);
                    });

                    if (c.Value.Count == 0) nullConnectionKeys.Add(c.Key);
                });
                //移除空闲的连接
                nullConnectionKeys.ForEach(nc =>
                {
                    _channelPool.Remove(nc);
                    try { nc.Close(); }
                    catch { }
                });
            }
        }
    }
}
