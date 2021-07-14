using System.Collections.Generic;
using FluentValidation;

namespace Aircraft.Tasks.Core.Contracts.Requests
{
    public class TaskDueListRequestDto
    {
        public int AirCraftId { get; set; }
        public IEnumerable<TaskDto> Tasks { get; set; }
    }

    public class TaskDueListRequestDtoValidator: AbstractValidator<TaskDueListRequestDto>
    {
        public TaskDueListRequestDtoValidator()
        {
            RuleForEach(x => x.Tasks)
                .SetValidator(new TaskDtoValidator());
        }
    }
}
