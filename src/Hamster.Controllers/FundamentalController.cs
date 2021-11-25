using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hamster.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class FundamentalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FundamentalController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public Task<string> Resources()
        {
            return Task.FromResult("Hello!");
        }
    }
}