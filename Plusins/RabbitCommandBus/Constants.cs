namespace DFramework.RabbitCommandBus
{
    public class Constants
    { 
        public const string EXCHANGE = "DFramework.RabbitCommandBus";
        public const string COMMAND_QUEUE = "DFramework.RabbitCommandBus.CommandQueue";
        public const string COMMAND_ROUTE_KEY_PREFIX = "Command"; 

        #region Private Methods
        public int GetQueueCountSetting()
        {
            try
            {
                return IoC.Resolve<int>("DFramework.RabbitCommandBus.CommandQueue.Count");
            }
            catch
            {
                //默认支持3台Server（即3条command队列和3条result队列），在以后扩容时，可对这个数量进行调整
                return 3;
            }
        }
        #endregion
    }
}
