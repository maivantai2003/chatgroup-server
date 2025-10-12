
using chatgroup_server.Helpers;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using chatgroup_server.RabbitMQ.Interfaces;
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
        private readonly string QueueName = "email_queue";
        private readonly IServiceProvider _serviceProvider;
        private IRabbitMQConnection _connection;
        private IChannel? _channel;
        private readonly ILogger<EmailConsumer> _logger;
        public EmailConsumer(IRabbitMQConnection connection,IServiceProvider serviceProvider, ILogger<EmailConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _connection = connection;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //if (_channel == null)
            //    return;

            //var consumer = new AsyncEventingBasicConsumer(_channel);
            var conn = await _connection.GetConnectionAsync();
            _channel = await conn.CreateChannelAsync();

            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false);
            await _channel.BasicQosAsync(0, 3, false);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            var semaphore = new SemaphoreSlim(3);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                using var scope = _serviceProvider.CreateScope();
                var sendGmailService = scope.ServiceProvider.GetRequiredService<ISendGmailService>();

                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var email = JsonConvert.DeserializeObject<EmailMessageModel>(body);

                    if (email == null)
                    {
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
                        return;
                    }

                    
                    var tasks = email.ToEmails.Select(async to =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            var gmail = new Gmail
                            {
                                Body = email.Body,
                                Name = email.Name,
                                Subject = email.Subject,
                                ToGmail = to
                            };
                            await sendGmailService.SendGmailAsync(gmail);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[Email Error] {to}: {ex.Message}");
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    });

                    await Task.WhenAll(tasks);
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[EmailConsumer Error]");
                    Console.WriteLine($"[RabbitMQ Consumer Error] {ex.Message}");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await _channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer);

            // giữ tiến trình chạy
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    await Task.Delay(1000, stoppingToken);
            //}
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null)
                await _channel.CloseAsync();

            await base.StopAsync(cancellationToken);
        }
    }
}
