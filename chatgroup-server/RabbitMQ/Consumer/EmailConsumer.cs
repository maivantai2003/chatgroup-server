
using chatgroup_server.Helpers;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using chatgroup_server.RabbitMQ.Models;
using chatgroup_server.RabbitMQ.Services;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace chatgroup_server.RabbitMQ.Consumer
{
    public class EmailConsumer : BackgroundService
    {
        private readonly string _queueName = "email_queue";
        private readonly IServiceProvider _serviceProvider;
        
        public EmailConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection=await RabbitMQConnectionFactory.GetConnection();
            using var channel = await connection.CreateChannelAsync();
            var properties = new BasicProperties()
            {
                Persistent = true,
            };
            await channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync +=async (model, ea) =>
            {
                var scope=_serviceProvider.CreateScope();
                var sendGmailService=scope.ServiceProvider.GetService<ISendGmailService>();
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var email = JsonConvert.DeserializeObject<EmailMessageModel>(body);
                var semaphone = new SemaphoreSlim(3);
                var tasks = email.ToEmails.Select(async to =>
                {
                    await semaphone.WaitAsync();
                    try
                    {
                        var gmail = new Gmail()
                        {
                            Body = email.Body,
                            Name=email.Name,
                            Subject=email.Subject,
                            ToGmail=to,
                        };
                        await sendGmailService.SendGmailAsync(gmail);
                    }
                    catch (Exception ex) {
                        Console.WriteLine($"[Email Error] {to}: {ex.Message}");
                    }
                    finally
                    {
                        semaphone.Release();
                    }
                });
                await Task.WhenAll(tasks);
            };
            await channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);
            //await Task.CompletedTask;
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
