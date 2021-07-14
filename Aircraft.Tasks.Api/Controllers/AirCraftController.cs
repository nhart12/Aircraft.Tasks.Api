using System;
using System.Threading.Tasks;
using Aircraft.Tasks.Core.Contracts.Requests;
using Aircraft.Tasks.Core.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Aircraft.Tasks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirCraftController : ControllerBase
    {
        private readonly IAirCraftTaskQueueService airCraftTaskService;

        public AirCraftController(IAirCraftTaskQueueService airCraftTaskService)
        {
            this.airCraftTaskService = airCraftTaskService ?? throw new ArgumentNullException(nameof(airCraftTaskService));
        }

        [HttpPost("{id}/duelist")]
        public async Task<IActionResult> GetDueList(int id, [FromBody] TaskDueListRequestDto requestDto)
        {
            requestDto.AirCraftId = id;
            var result = await airCraftTaskService.CalculateDueDatesAsync(requestDto);
            if (result == null) { return NotFound(); }
            return Ok(result);
        }
    }
}
