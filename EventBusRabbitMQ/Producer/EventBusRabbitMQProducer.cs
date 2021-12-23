using EventBusRabbitMQ.Events.Interfaces;
using Microsoft.Extensions.Logging;
using Polly.Retry;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace EventBusRabbitMQ.Producer
{
   public class EventBusRabbitMQProducer
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<EventBusRabbitMQProducer> _logger;
        private readonly int _retryCount;

        public EventBusRabbitMQProducer(IRabbitMQPersistentConnection persistentConnection, ILogger<EventBusRabbitMQProducer> logger, int retryCount)
        {
            _persistentConnection = persistentConnection;
            _logger = logger;
            _retryCount = retryCount;
        }
        public void Publis(string queueName, IEvent @event)
        {
            if(!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>().Or<SocketException>().WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                _logger.LogWarning(ex,"Could not publish event : " +
                    "{EventId} after {Timeout}s ({ExceptionMassage})"
                    ,@event.RequestId, $"{time.TotalSeconds:n1}" );


            });
            using (var channel = _persistentConnection.CreateModel())
            {

                channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.DeliveryMode = 2;

                    channel.ConfirmSelect();
                    channel.BasicPublish(
                        exchange:"",
                        routingKey:queueName,
                        mandatory:true,
                        basicProperties:properties,
                        body:body);
                    channel.WaitForConfirmsOrDie();
                    channel.BasicAcks += (sender, EventArgs) =>
                    {
                        Console.WriteLine("Sent RabbitMQ");
                    };



                }
                

                ) ;
            }
        }
    }
}
