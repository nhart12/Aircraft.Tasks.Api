using System.Threading.Tasks;
using Aircraft.Tasks.Core.Contracts.Requests;
using Aircraft.Tasks.Core.Contracts.Responses;

namespace Aircraft.Tasks.Core.Services
{
    public interface IAirCraftTaskQueueService
    {
        Task<TaskDueListResponseDto> CalculateDueDatesAsync(TaskDueListRequestDto request);
    }
}
