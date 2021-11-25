using System.Threading.Tasks;
using Hamster.UseCases.Stocks.Queries.GetFundamental;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hamster.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class StockController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("{ticker}")]
        public Task<FundamentalDto> Fundamental(string ticker)
        {
            return _mediator.Send(new GetFundamentalQuery{Ticker = ticker});
        }
    }
}