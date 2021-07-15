using Aircraft.Tasks.Internal.Public.Extensions;
using Aircraft.Tasks.Worker.Consumers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Aircraft.Tasks.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                     .AddTaskServices()
                     .AddMassTransit(x =>
                     {
                         x.AddConsumer<AirCraftTaskNextDueConsumer>();
                         x.UsingRabbitMq((context, cfg) =>
                         {
                             cfg.Host("rabbitmq");
                             cfg.ConfigureEndpoints(context);
                         });
                     })
                    .AddHostedService<Worker>();
                });
    }
}
