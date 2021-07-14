using System;
namespace Aircraft.Tasks.Core.Contracts.Responses
{
    public class TaskDueDto: TaskDto
    {
        public DateTime? NextDue { get; set; }
    }
}
