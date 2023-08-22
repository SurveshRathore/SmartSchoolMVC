using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ModelLayer.Model
{
    public class ConsumerProcessingEmail
    {
        public void StartListening()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "email_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    // Parse message and send email
                    string[] lines = message.Split('\n');
                    string recipient = lines[0].Substring(4);
                    string subject = lines[1].Substring(9);
                    string body2 = lines[2].Substring(6);

                    SendEmail(recipient, subject, body2);

                    channel.BasicAck(ea.DeliveryTag, false);
                };

                channel.BasicConsume(queue: "New_Test_queue", autoAck: false, consumer: consumer);

                Console.WriteLine("Email worker is listening for requests.");
                Console.ReadLine(); // Keep the console application running
            }
        }

        private void SendEmail(string recipient, string subject, string body)
        {
            using (var smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential("surveshrathore98@gmail.com", "eondshuodywiynas");
                smtpClient.EnableSsl = true;

                using (var message = new MailMessage("surveshrathore98@gmail.com", recipient, subject, body))
                {
                    smtpClient.Send(message);
                }
            }
        }
    }
}