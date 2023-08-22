using RabbitMQ.Client;
using System;
using System.Text;

namespace ModelLayer.Model
{
    public class EmailSender
    {
        public void SendEmailRequest(string recipient, string subject, string body)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" }; // RabbitMQ server address

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "New_Test_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                string message = $"To: {recipient}\nSubject: {subject}\nBody: {body}";
                var bodyBytes = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "email_queue", basicProperties: null, body: bodyBytes);
                Console.WriteLine("Email request sent.");
            }
        }
    }
}
