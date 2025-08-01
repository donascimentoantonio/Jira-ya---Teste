using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Jira_ya.Application.MessageBus;

namespace Jira_ya.Infrastructure.MessageBus
{
    public class RabbitMqPublisher : IMessageBusPublisher
    {
        private readonly string _hostName;
        private const string DefaultHostName = "localhost";
        public RabbitMqPublisher(IConfiguration configuration)
        {
            _hostName = configuration["RabbitMq:HostName"] ?? DefaultHostName;
        }

        public Task PublishAsync(string queue, object message)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            const bool Durable = false;
            const bool Exclusive = false;
            const bool AutoDelete = false;
            channel.QueueDeclare(queue, durable: Durable, exclusive: Exclusive, autoDelete: AutoDelete, arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: body);
            return Task.CompletedTask;
        }
    }
}
