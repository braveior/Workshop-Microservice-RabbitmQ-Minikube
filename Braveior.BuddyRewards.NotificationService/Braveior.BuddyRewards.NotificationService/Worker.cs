using MailKit.Net.Smtp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Braveior.BuddyRewards.NotificationService
{
    public class Worker : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;

        public Worker()
        {

        }
        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory
            { 
                HostName = "rabbit-mq-service-service",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            // create connection
            _connection = factory.CreateConnection();

            // create channel
            _channel = _connection.CreateModel();

            _channel.QueueDeclare("buddyrewards.rating", false, false, false, null);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                // received message
                var content = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
                // handle the received message
                HandleMessage(content);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("buddyrewards.rating", false, consumer);
        }
        private void HandleMessage(string content)
        {
            // we just print this message 
            var rating = JsonConvert.DeserializeObject<RatingDTO>(content);
            SendEmail(rating);
        }
        private void SendEmail(RatingDTO rating)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Braveior", "braveior@outlook.com"));
            message.To.Add(new MailboxAddress("Sreehari Parameswaran", "sreehari.p@outlook.com"));
            message.Subject = $"New Rating submitted for {rating.RatedFor}";

            message.Body = new TextPart("plain")
            {
                Text = $"{rating.RatedFor} got a new rating {rating.Star} star from {rating.RatedBy}"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp-mail.outlook.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(GetEnvironmentVariable("email"), GetEnvironmentVariable("emailpassword"));

                client.Send(message);
                client.Disconnect(true);
            }
        }
        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name.ToLower()) ?? Environment.GetEnvironmentVariable(name.ToUpper());
        }
    }
}
