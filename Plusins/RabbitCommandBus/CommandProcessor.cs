using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Content;

namespace DFramework.RabbitCommandBus
{
    public class CommandProcessor
    {
        #region ctor
        public CommandProcessor(CommandBus commandBus, CommandTypeMapping commandTypeMapping, ICommandExecutorContainer executorContainer, int queueCount)
        {
            StartCommandConsumer(queueCount, commandTypeMapping, executorContainer, commandBus);
        }
        #endregion

        #region Private Methods
        private void StartCommandConsumer(int queueCount, CommandTypeMapping commandTypeMapping, ICommandExecutorContainer executorContainer, CommandBus commandBus)
        {
            var durable = true;
            while (queueCount-- > 0)
            {
                //绑定发送队列
                var channel = RabbitMqChannelManager.GetChannel();
                var routeKey = Constants.COMMAND_ROUTE_KEY_PREFIX + queueCount.ToString();
                var queueName = Constants.COMMAND_QUEUE + queueCount.ToString();
                channel.ExchangeDeclare(Constants.EXCHANGE, ExchangeType.Direct, durable, false, null);
                channel.QueueDeclare(queueName, durable, false, false, null);
                channel.QueueBind(queueName, Constants.EXCHANGE, routeKey);

                var consumer = new CommandConsumer(channel, commandTypeMapping, executorContainer, commandBus);
                channel.BasicQos(0, 100, false);
                channel.BasicConsume(queueName, false, consumer);
            }
        }

        #endregion

        #region Consumer

        #region Command Consumer
        internal class CommandConsumer : DefaultBasicConsumer
        {
            private readonly CommandTypeMapping commandTypeMapping;
            private readonly ICommandExecutorContainer executorContainer;
            private readonly CommandBus _commandBus; 
            private readonly IModel publishChannel;
            public CommandConsumer(IModel model, CommandTypeMapping commandTypeMapping, ICommandExecutorContainer executorContainer, CommandBus commandBus)
                : base(model)
            {
                this.commandTypeMapping = commandTypeMapping;
                this.executorContainer = executorContainer;
                this._commandBus = commandBus;
                this.publishChannel = RabbitMqChannelManager.GetChannel();
            }

            public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered,
                string exchange, string routingKey, IBasicProperties properties, byte[] body)
            {
                var commandTypeString = properties.Type;
                int commandTypeCode;
                var message = Encoding.UTF8.GetString(body);

                if (int.TryParse(commandTypeString, out commandTypeCode))
                {
                    var cmdType = this.commandTypeMapping.GetTypeByCode(commandTypeCode);

                    if (cmdType != null)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            ICommand cmd = IoC.Resolve<IJsonSerializer>().Deserialize(message, cmdType) as ICommand;
                            var delegete = this.executorContainer.FindExecutor(cmdType);
                            try
                            {
                                delegete.Item1.Invoke(delegete.Item2, cmd);
                                cmd.Status = CommandStatus.Success;
                                ReplyCommandResult(cmd, commandTypeCode, properties.ReplyTo);
                                Model.BasicAck(deliveryTag, false);
                                //Log.Debug("execute cmd success,cmdId={0},cmdStatus={1}", cmd.Id, CommandStatus.Success.ToString("G"));
                            }
                            catch (Exception ex)
                            {
                                //Log.Error("execute command error,cmdId=" + cmd.Id, ex);
                                Model.BasicNack(deliveryTag, false, true);
                            }
                        });
                    }

                }
            }

            private void ReplyCommandResult(ICommand processedCommand, int commandTypeCode,string replyTo)
            {
                //如果不是本机注册的CommandTask，再广播结果消息
                if (!this._commandBus.InternalCompleteTaskByResult(processedCommand))
                {
                    var messageBody = IoC.Resolve<IJsonSerializer>().Serialize(processedCommand);
                    var bytes = Encoding.UTF8.GetBytes(messageBody);
                    var build = new BytesMessageBuilder(publishChannel);
                    build.WriteBytes(bytes);
                    var contentHeader = ((IBasicProperties)build.GetContentHeader());

                    contentHeader.DeliveryMode = 1;
                    contentHeader.Expiration = "60000";
                    contentHeader.Type = commandTypeCode.ToString();
                    publishChannel.BasicPublish(Constants.EXCHANGE, replyTo, contentHeader, build.GetContentBody());
                }
            }
        }
        #endregion
        #endregion
    }
}
