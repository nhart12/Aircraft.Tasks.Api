using Aircraft.Tasks.Core.Contracts.Requests;
using Aircraft.Tasks.Core.Contracts.Responses;

namespace Aircraft.Tasks.Core.Services
{
    public interface IAirCraftTaskService
    {
        TaskDueListResponseDto CalculateDueDates(TaskDueListRequestDto request);
    }
}
