using chatgroup_server.Messaging.Consumers;
using MassTransit;

namespace chatgroup_server.Messaging.Configurations
{
    public static class MassTransitConfig
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<BirthdayConsumer>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(
                        configuration["RabbitMQ:Host"] ?? "rabbitmq",
                        "/",
                        h =>
                        {
                            h.Username(configuration["RabbitMQ:Username"] ?? "admin");
                            h.Password(configuration["RabbitMQ:Password"] ?? "admin");
                        });
                    cfg.UseMessageRetry(r =>
                        r.Interval(3, TimeSpan.FromSeconds(5)));
                    cfg.ConfigureEndpoints(ctx);
                });
            });
            return services;
        }
    }
}
