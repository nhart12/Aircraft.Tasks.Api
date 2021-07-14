using Aircraft.Tasks.Core.Services;
using Aircraft.Tasks.Internal.Private.Repositories;
using Aircraft.Tasks.Internal.Private.Services;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Aircraft.Tasks.Core.Contracts.Requests;

namespace Aircraft.Tasks.Internal.Public.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTaskServices(this IServiceCollection services)
        {
            return services
                    .AddSingleton<IAirCraftTaskService, AirCraftTaskService>()
                    .AddScoped<IAirCraftTaskQueueService, AirCraftTaskQueueService>()
                    .AddSingleton<IDateTimeProvider, DateTimeProvider>()
                    .AddSingleton<IAirCraftUtilizationRepository, AirCraftUtilizationRepository>()
                    .AddMassTransit(x =>
                    {
                        x.AddConsumer<AirCraftTaskNextDueConsumer>();
                        x.AddRequestClient<TaskDueListRequestDto>();
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("rabbitmq");
                            cfg.ConfigureEndpoints(context);
                        });
                    })
                    .AddMassTransitHostedService(true);
                ;
        }
    }
}
