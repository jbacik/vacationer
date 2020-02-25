using System.Threading.Tasks;
using backend.Handlers.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly ILogger<UsersController> _logger;
        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(int id){
            _logger.LogInformation("Getting user - {0}", id);
            var result = await _mediator.Send(new UserDetail.Query(){ Id = id});
            if (result == null) return NotFound();

            return Ok(result);
        }
    }
}