using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace backend.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        public IMediator _mediator { get; set; }
    }
}
