using System;

namespace Jira_ya.NotificationWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read RabbitMQ HostName from appsettings.json
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var hostName = config["RabbitMq:HostName"] ?? "localhost";
            var consumer = new NotificationConsumer(hostName);
            consumer.Start();
        }
    }
}
