using System.Collections.Generic;

namespace Aircraft.Tasks.Core.Contracts.Responses
{
    public class TaskDueListResponseDto
    {
        public int AirCraftId { get; set; }
        public IEnumerable<TaskDueDto> Tasks { get; set; }
    }
}
