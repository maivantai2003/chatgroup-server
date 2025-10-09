
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
        private IChannel? _channel;
        public EmailConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    var connection=await RabbitMQConnectionFactory.GetConnectionAsync();
        //    using var channel = await connection.CreateChannelAsync();
        //    var properties = new BasicProperties()
        //    {
        //        Persistent = true,
        //    };
        //    await channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        //    var consumer = new AsyncEventingBasicConsumer(channel);
        //    consumer.ReceivedAsync +=async (model, ea) =>
        //    {
        //        var scope=_serviceProvider.CreateScope();
        //        var sendGmailService=scope.ServiceProvider.GetService<ISendGmailService>();
        //        var body = Encoding.UTF8.GetString(ea.Body.ToArray());
        //        var email = JsonConvert.DeserializeObject<EmailMessageModel>(body);
        //        var semaphone = new SemaphoreSlim(3);
        //        var tasks = email.ToEmails.Select(async to =>
        //        {
        //            await semaphone.WaitAsync();
        //            try
        //            {
        //                var gmail = new Gmail()
        //                {
        //                    Body = email.Body,
        //                    Name=email.Name,
        //                    Subject=email.Subject,
        //                    ToGmail=to,
        //                };
        //                await sendGmailService.SendGmailAsync(gmail);
        //            }
        //            catch (Exception ex) {
        //                Console.WriteLine($"[Email Error] {to}: {ex.Message}");
        //            }
        //            finally
        //            {
        //                semaphone.Release();
        //            }
        //        });
        //        await Task.WhenAll(tasks);
        //    };
        //    await channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);
        //    //await Task.CompletedTask;
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        await Task.Delay(1000, stoppingToken);
        //    }
        //}
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var connection = await RabbitMQConnectionFactory.GetConnectionAsync();
            _channel = await connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            await _channel.BasicQosAsync(0, 3, false); // Giới hạn tối đa 3 message xử lý cùng lúc

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_channel == null)
                return;

            var consumer = new AsyncEventingBasicConsumer(_channel);
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

                    var semaphore = new SemaphoreSlim(3);
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
                    Console.WriteLine($"[RabbitMQ Consumer Error] {ex.Message}");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: _queueName,
                autoAck: false,
                consumer: consumer);

            // giữ tiến trình chạy
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null)
                await _channel.CloseAsync();

            await base.StopAsync(cancellationToken);
        }
    }
}
