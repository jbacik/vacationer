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
        public async Task<IActionResult> GetUserVacationTimes(int userId)
        {
            
        }
    }
}