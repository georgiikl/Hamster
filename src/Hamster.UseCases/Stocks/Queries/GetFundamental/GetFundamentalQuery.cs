using MediatR;

namespace Hamster.UseCases.Stocks.Queries.GetFundamental
{
    public class GetFundamentalQuery : IRequest<FundamentalDto>
    {
        public string Ticker { get; set; }
    }
}