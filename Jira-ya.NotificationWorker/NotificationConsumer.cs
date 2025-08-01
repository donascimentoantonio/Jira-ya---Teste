using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Jira_ya.NotificationWorker
{
    public class NotificationConsumer
    {

        private readonly string _hostName;
        private readonly string _queue = "user-notifications";

        public NotificationConsumer(string hostName)
        {
            _hostName = hostName;
        }

        public void Start()
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: _queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var notification = JsonSerializer.Deserialize<NotificationMessage>(message);
                Console.WriteLine($"[x] Notificação para usuário {notification.UserId}: {notification.Message}");
            };
            channel.BasicConsume(queue: _queue, autoAck: true, consumer: consumer);

            Console.WriteLine("[x] Aguardando notificações. Pressione [enter] para sair.");
            Console.ReadLine();
        }
    }

    public class NotificationMessage
    {
        public Guid UserId { get; set; }
        public string Message { get; set; }
    }
}
