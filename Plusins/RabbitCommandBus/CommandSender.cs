using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Content;

namespace DFramework.RabbitCommandBus
{
    public class CommandSender
    {
        private readonly IModel _channel;
        private readonly CommandTypeMapping commandTypeMapping;
        private readonly int _queueCount;
        private string _replyQueueName;
        public CommandSender(CommandBus commandBus, CommandTypeMapping commandTypeMapping, ICommandExecutorContainer executorContainer, int queueCount)
        {
            _channel = RabbitMqChannelManager.GetChannel();
            this.commandTypeMapping = commandTypeMapping;
            this._queueCount = queueCount;
            this.RegisterSendQueue(queueCount);
            this.StartCommandResultConsumer(commandBus, commandTypeMapping, executorContainer);
        }

        public bool Send(ICommand command)
        {
            var sendResult = false;
            var routeKey = Constants.COMMAND_ROUTE_KEY_PREFIX + Math.Abs(command.Id.GetHashCode()) % this._queueCount;
            var commandTypeCode = commandTypeMapping.GetTypeCode(command);
            if (commandTypeCode != -1)
            {
                try
                {
                    var messageBody = IoC.Resolve<IJsonSerializer>().Serialize(command);
                    var bytes = Encoding.UTF8.GetBytes(messageBody);
                    var build = new BytesMessageBuilder(_channel);
                    build.WriteBytes(bytes);
                    var contentHeader = ((IBasicProperties)build.GetContentHeader());

                    contentHeader.DeliveryMode = 2;
                    contentHeader.ReplyTo = this._replyQueueName;
                    contentHeader.Type = commandTypeCode.ToString();
                    _channel.BasicPublish(Constants.EXCHANGE, routeKey, contentHeader, build.GetContentBody());
                    sendResult = true;
                }
                catch (Exception ex)
                {
                    Log.Error("send command message to queue error", ex);
                }
            }

            return sendResult;
        }

        #region Private Method

        private void RegisterSendQueue(int queueCount)
        {
            var durable = true;
            while (queueCount-- > 0)
            {
                var routeKey = Constants.COMMAND_ROUTE_KEY_PREFIX + queueCount;
                var queueName = Constants.COMMAND_QUEUE + queueCount;
                _channel.ExchangeDeclare(Constants.EXCHANGE, ExchangeType.Direct, durable, false, null);
                _channel.QueueDeclare(queueName, durable, false, false, null);
                _channel.QueueBind(queueName, Constants.EXCHANGE, routeKey);
            }
        }
        private void StartCommandResultConsumer(CommandBus commandBus, CommandTypeMapping commandTypeMapping, ICommandExecutorContainer executorContainer)
        {
            var durable = true;

            //绑定结果队列
            var channel = RabbitMqChannelManager.GetChannel();

            channel.ExchangeDeclare(Constants.EXCHANGE, ExchangeType.Direct, durable, false, null);
            string queueName = channel.QueueDeclare();
            channel.QueueBind(queueName, Constants.EXCHANGE, queueName);

            var consumer = new CommandResultConsumer(channel, commandTypeMapping, executorContainer, commandBus);
            channel.BasicQos(0, 100, false);
            channel.BasicConsume(queueName, true, consumer);

            this._replyQueueName = queueName;
        }


        #endregion

        #region Command Result Consumer
        internal class CommandResultConsumer : DefaultBasicConsumer
        {
            private readonly CommandTypeMapping _commandTypeMapping;
            private readonly ICommandExecutorContainer _executorContainer;
            private readonly CommandBus _commandBus;
            public CommandResultConsumer(IModel model, CommandTypeMapping commandTypeMapping, ICommandExecutorContainer executorContainer, CommandBus commandBus)
                : base(model)
            {
                this._commandTypeMapping = commandTypeMapping;
                this._executorContainer = executorContainer;
                this._commandBus = commandBus;
            }

            public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered,
                string exchange, string routingKey, IBasicProperties properties, byte[] body)
            {
                var commandTypeString = properties.Type;
                int commandTypeCode;
                var message = Encoding.UTF8.GetString(body);

                if (int.TryParse(commandTypeString, out commandTypeCode))
                {
                    var cmdType = this._commandTypeMapping.GetTypeByCode(commandTypeCode);
                    if (cmdType != null)
                    {
                        var cmd = IoC.Resolve<IJsonSerializer>().Deserialize(message, cmdType) as ICommand;
                        _commandBus.DistributedCompleteTaskByResult(cmd);
                    }

                }
            }
        }
        #endregion
    }
}
