using System.Threading.Tasks;
using backend.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
    public class VacationTimesController : BaseApiController
    {
        private readonly ILogger<VacationTimesController> _logger;
        public VacationTimesController(IMediator mediator, ILogger<VacationTimesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserVacationTimes(Handlers.VacationTimes.List.Query query)
        {
            var result = await _mediator.Send(query);
            if (result == null) return BadRequest("Unable to find vacation times for the given user");   
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserVacationTime(Handlers.VacationTimes.Create.Command command)
        {
            var result = await _mediator.Send(command);
            if (result <= 0) return BadRequest("Unable to create vacation time for the given user");
            return Ok(result);
        }
    }
}