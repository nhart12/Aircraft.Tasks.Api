using System;
using FluentValidation;

namespace Aircraft.Tasks.Core.Contracts
{
    public class TaskDto
    {
        public int ItemNumber { get; set; }
        public string Description { get; set; }
        public DateTime LogDate { get; set; }
        public int? LogHours { get; set; }
        public int? IntervalMonths { get; set; }
        public int? IntervalHours { get; set; }
    }

    public class TaskDtoValidator: AbstractValidator<TaskDto>
    {
        public TaskDtoValidator()
        {
            RuleFor(x => x.ItemNumber).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.LogDate).NotEmpty();
        }
    }
}
