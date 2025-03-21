using RabbitMQ.Client;
using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using BusinessLayer.Interface;

namespace BusinessLayer.Service
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly string _hostName;
        private readonly string _queueName;
        private readonly string _userName;
        private readonly string _password;

        public RabbitMQService(IConfiguration configuration)
        {
            var rabbitConfig = configuration.GetSection("RabbitMQ");

            _hostName = rabbitConfig["Host"] ?? "localhost";  // Default fallback
            _queueName = rabbitConfig["QueueName"] ?? "defaultQueue";
            _userName = rabbitConfig["UserName"] ?? "guest";
            _password= rabbitConfig["Password"] ?? "guest";
        }

        public void PublishMessage(string message)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _password
                };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);

                    Console.WriteLine($" [x] Sent '{message}' to queue '{_queueName}'");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error publishing message: {ex.Message}");
            }
        }
    }
}
