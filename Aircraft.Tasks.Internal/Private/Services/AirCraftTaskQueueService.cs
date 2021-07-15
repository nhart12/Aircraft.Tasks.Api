using System;
using System.Threading.Tasks;
using Aircraft.Tasks.Core.Contracts.Requests;
using Aircraft.Tasks.Core.Contracts.Responses;
using Aircraft.Tasks.Core.Services;
using Aircraft.Tasks.Internal.Private.Models;
using MassTransit;

namespace Aircraft.Tasks.Internal.Private.Services
{
    internal class AirCraftTaskQueueService : IAirCraftTaskQueueService
    {
        private readonly IRequestClient<TaskDueListRequestDto> serviceBusRequestClient;
        public AirCraftTaskQueueService(IRequestClient<TaskDueListRequestDto> serviceBusRequestClient)
        {
            this.serviceBusRequestClient = serviceBusRequestClient ?? throw new ArgumentNullException(nameof(serviceBusRequestClient));
        }

        public async Task<TaskDueListResponseDto> CalculateDueDatesAsync(TaskDueListRequestDto request)
        {
            //for now am just waiting the response directly. in the future it'd be good to fire and forget possibly?
            var response = await serviceBusRequestClient.GetResponse<TaskDueListResponseDto, AircraftNotFound>(request);
            if (response.Is(out Response<TaskDueListResponseDto> taskResponse))
            {
                return taskResponse.Message;
            }
            return null;
        }
    }
}
