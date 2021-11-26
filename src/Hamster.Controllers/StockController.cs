using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Hamster.UseCases.Stocks.Queries.GetFundamental;

namespace Hamster.Controllers
{
    /// <summary>
    /// API получения сведений по акциям
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class StockController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Dependency injection
        /// </summary>
        public StockController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Получить базовый набор фундаментальных показателей акции
        /// </summary>
        /// <param name="ticker">Тикер акции на бирже</param>
        /// <param name="cancellationToken">Cancelation token</param>
        /// <returns>Основные фундаментальные показатели акции</returns>
        [HttpGet("{ticker}")]
        public async Task<FundamentalDto> Fundamental(string ticker, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetFundamentalQuery{Ticker = ticker}, cancellationToken);
        }
    }
}