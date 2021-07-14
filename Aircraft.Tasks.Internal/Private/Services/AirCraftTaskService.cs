using System;
using System.Collections.Generic;
using System.Linq;
using Aircraft.Tasks.Core.Contracts;
using Aircraft.Tasks.Core.Contracts.Requests;
using Aircraft.Tasks.Core.Contracts.Responses;
using Aircraft.Tasks.Core.Services;
using Aircraft.Tasks.Internal.Private.Repositories;
using Aircraft.Tasks.Internal.Private.Repositories.DbModels;

namespace Aircraft.Tasks.Internal.Private.Services
{
    internal class AirCraftTaskService: IAirCraftTaskService
    {
        private readonly IAirCraftUtilizationRepository airCraftUtilizationRepository;
        private readonly IDateTimeProvider dateTimeProvider;
        public AirCraftTaskService(IDateTimeProvider dateTimeProvider, IAirCraftUtilizationRepository airCraftUtilizationRepository)
        {
            this.dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            this.airCraftUtilizationRepository = airCraftUtilizationRepository ?? throw new ArgumentNullException(nameof(airCraftUtilizationRepository));
        }

        public TaskDueListResponseDto CalculateDueDates(TaskDueListRequestDto request)
        {
            var aircraftUtilization = airCraftUtilizationRepository.GetAirCraftUtilization(request.AirCraftId);
            if(aircraftUtilization == null) { return null; }

            var tasksDue = new List<TaskDueDto>();
            foreach(var task in request.Tasks)
            {
                tasksDue.Add(CaclulateDueDateForTask(task, aircraftUtilization));
            }
            return new TaskDueListResponseDto()
            {
                AirCraftId = request.AirCraftId,
                Tasks = tasksDue.OrderByDescending(x => x.NextDue.HasValue)
                                .ThenBy(x => x.NextDue)
                                .ThenBy(x => x.Description)
            };
        }

        private TaskDueDto CaclulateDueDateForTask(TaskDto task, AirCraftUtilization airCraftUtilization)
        {
            var intervalMonthsNextDue = GetIntervalMonthsNextDue(task);
            var intervalHoursNextDue = GetIntervalHoursNextDue(task, airCraftUtilization);

            return new TaskDueDto()
            {
                Description = task.Description,
                LogDate = task.LogDate,
                IntervalHours = task.IntervalHours,
                IntervalMonths = task.IntervalMonths,
                ItemNumber = task.ItemNumber,
                LogHours = task.LogHours,
                NextDue = GetNextDueDate(intervalMonthsNextDue, intervalHoursNextDue)
            };
        }

        private DateTime? GetNextDueDate(DateTime? intervalMonthsNextDue, DateTime? intervalHoursNextDue)
        {

            if (!intervalMonthsNextDue.HasValue && !intervalHoursNextDue.HasValue)
            {
                //if neither have a value return null
               return null;
            }
            else if (!intervalHoursNextDue.HasValue)
            {
                // if hours has no value, return months
                return intervalMonthsNextDue.Value;
            }
            else if (!intervalMonthsNextDue.HasValue)
            {
                // if months have no value, return hours
                return intervalHoursNextDue.Value;
            }
            else
            {
                return DateTime.Compare(intervalMonthsNextDue.Value, intervalHoursNextDue.Value) < 0 ? intervalMonthsNextDue.Value : intervalHoursNextDue.Value;
            }
        }

        private DateTime? GetIntervalMonthsNextDue(TaskDto task)
        {
            return task.IntervalMonths.HasValue? task.LogDate.AddMonths(task.IntervalMonths.Value) : null;
        }

        private DateTime? GetIntervalHoursNextDue(TaskDto task, AirCraftUtilization airCraftUtilization)
        {
            var today = dateTimeProvider.UtcNow();//new DateTime(2018, 6, 19);
            var daysRemainingByHoursInterval = GetDaysRemainingByHoursInterval(task, airCraftUtilization);
            return daysRemainingByHoursInterval.HasValue ? today.AddDays(daysRemainingByHoursInterval.Value): null;
        }

        private int? GetDaysRemainingByHoursInterval(TaskDto task, AirCraftUtilization airCraftUtilization)
        {
            if(!task.IntervalHours.HasValue || !task.LogHours.HasValue)
            {
                return null;
            }
            return (int?)((int)(task.LogHours.Value + task.IntervalHours.Value - airCraftUtilization.CurrentHours) / airCraftUtilization.DailyHours);
        }
    }
}
