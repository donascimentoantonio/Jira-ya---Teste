using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Jira_ya.NotificationWorker
{
    public class NotificationConsumer
    {

        private readonly string _hostName;
        private const string QueueName = "user-notifications";

        public NotificationConsumer(string hostName)
        {
            _hostName = hostName;
        }

        public void Start()
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            const bool Durable = false;
            const bool Exclusive = false;
            const bool AutoDelete = false;
            channel.QueueDeclare(queue: QueueName, durable: Durable, exclusive: Exclusive, autoDelete: AutoDelete, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Mensagem recebida: {message}");
            };
            channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);

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
