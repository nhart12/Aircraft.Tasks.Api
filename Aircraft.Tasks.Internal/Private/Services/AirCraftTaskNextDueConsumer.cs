using System;
using System.Threading.Tasks;
using Aircraft.Tasks.Core.Contracts.Requests;
using Aircraft.Tasks.Core.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Aircraft.Tasks.Internal.Private.Services
{
    internal class AirCraftTaskNextDueConsumer : IConsumer<TaskDueListRequestDto>
    {
        private readonly ILogger<AirCraftTaskNextDueConsumer> logger;
        private readonly IAirCraftTaskService airCraftTaskService;
        private readonly IBus serviceBus;

        public AirCraftTaskNextDueConsumer(ILogger<AirCraftTaskNextDueConsumer> logger, IAirCraftTaskService airCraftTaskService, IBus serviceBus)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.airCraftTaskService = airCraftTaskService ?? throw new ArgumentNullException(nameof(airCraftTaskService));
            this.serviceBus = serviceBus ?? throw new ArgumentNullException(nameof(serviceBus));
        }

        public async Task Consume(ConsumeContext<TaskDueListRequestDto> context)
        {
            logger.LogInformation($"Processing task list for aircraft id: {context.Message.AirCraftId}");
            var result = airCraftTaskService.CalculateDueDates(context.Message);
            await context.RespondAsync(result);
        }
    }
}
